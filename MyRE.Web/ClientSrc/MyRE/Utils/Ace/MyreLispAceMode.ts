import brace = require("brace");
import 'brace/mode/lisp'
import { DeviceInfo, AttributeInfo, CommandInfo, CapabilityInfo } from "MyRE/Api/Models";

interface Mode {
    new(): Mode;
}

const TextModeRules: Mode = brace.acequire('ace/mode/text_highlight_rules').TextHighlightRules;
const TextMode: Mode = brace.acequire('ace/mode/text').Mode;
const TokenIterator: any = brace.acequire("ace/token_iterator").TokenIterator;

const controlKeywords = ["if", "else"];
const operatorKeywords = ["eq", "neq", "and", "or"];
const constantLanguage = ["null", "nil"];
const supportFunctions = ["def", "defn", "defun", "defev", "get", "t"];

const mergeKeywordList = (l: string[]): string => l.join('|');

interface ICompletion {
    value: string;
    meta: string;
    score: number;
}

export class MyreLispCompletions {
    private devices: DeviceInfo[];

    private baseLanguageCompletions: ICompletion[];
    private deviceCompletions: ICompletion[];

    constructor(devices: DeviceInfo[]) {
        this.devices = devices;

        this.baseLanguageCompletions = [];
        this.deviceCompletions = [];

        controlKeywords.map((s: string): ICompletion => ({ value: s, meta: "control", score: 1 })).forEach(c => this.baseLanguageCompletions.push(c));
        operatorKeywords.map((s: string): ICompletion => ({ value: s, meta: "operator", score: 1 })).forEach(c => this.baseLanguageCompletions.push(c));
        constantLanguage.map((s: string): ICompletion => ({ value: s, meta: "constant", score: 1 })).forEach(c => this.baseLanguageCompletions.push(c));
        supportFunctions.map((s: string): ICompletion => ({ value: s, meta: "builtin", score: 1 })).forEach(c => this.baseLanguageCompletions.push(c));

        [...new Set(devices.map(item => item.DeviceId))]
            .map(deviceId => devices.find(d => d.DeviceId === deviceId))
            .map(d => d ? ({ value: d.DisplayName, meta: 'device', score: 1 }) : undefined)
            .forEach(d => {
                if (d) {
                    this.deviceCompletions.push(d);
                }
            });
    }

    private isGetterOnReference(session: brace.IEditSession, pos: brace.Position) {
        var iter: brace.TokenIterator = new TokenIterator(session, pos.row, pos.column);

        //(window as any).iteratorThing = () => new TokenIterator(session, pos.row, pos.column);

        var token = iter.getCurrentToken();        

        var previousToken = iter.stepBackward();
        while (previousToken.value === " ") previousToken = iter.stepBackward();
        
        var thirdToken = iter.stepBackward();
        while (thirdToken.value === " ") thirdToken = iter.stepBackward();

        if (previousToken.type === 'reference' && thirdToken.value === 'get') {
            var displayName = previousToken.value.slice(1, -1);

            return this.devices.find(d => d.DisplayName === displayName);
        }
        return undefined;
    }

    private mapAttributes = (ai: AttributeInfo[], completions: ICompletion[]) => ai.map(a => ({ value: a.Name, meta: a.Type, score: 1 })).forEach(a => completions.push(a));
    private mapCommands = (ci: CommandInfo[], completions: ICompletion[]) => ci.map(c => ({ value: c.Name, meta: "command", score: 1 })).forEach(a => completions.push(a));
    private mapCapabilities = (ci: CapabilityInfo[], completions: ICompletion[]) => ci.map(c => {
        this.mapAttributes(c.Attributes, completions);
        this.mapCommands(c.Commands, completions);
    });

    private getDeviceProperties(device: DeviceInfo): ICompletion[] {
        var props: ICompletion[] = [];

        this.mapAttributes(device.Attributes, props);
        this.mapCommands(device.Commands, props);
        this.mapCapabilities(device.Capabilities, props);

        var completions: ICompletion[] = [];
        props.forEach(p => {
            if (!completions.find(c => p.value === c.value)) {
                completions.push(p);
            }
        });

        return completions;
    }

    private isDeviceIdentifier(session: brace.IEditSession, pos: brace.Position) {
        var iter: brace.TokenIterator = new TokenIterator(session, pos.row, pos.column);

        var token = iter.getCurrentToken();

        if (token.type !== "string") {
            return false;
        }

        var previousToken = iter.stepBackward();
        while (previousToken.value === " ") previousToken = iter.stepBackward();

        if (previousToken.value === "dev") {
            return true;
        }

        return false;
    }
    
    public getCompletions(editor: brace.Editor, session: brace.IEditSession, pos: brace.Position, prefix: string, callback: (a: any, completions: ICompletion[]) => void): void {
        var token = session.getTokenAt(pos.row, pos.column);

        const sendResults = (results: ICompletion[]) => {
            console.debug({ token, prefix, pos, session, editor, results });
            return callback(null, results);
        }
        
        if (this.isDeviceIdentifier(session, pos)) {
            return sendResults(this.deviceCompletions);
        }

        var parentDevice = this.isGetterOnReference(session, pos);
        if (parentDevice) {
            return sendResults(this.getDeviceProperties(parentDevice));
        }

        return sendResults(this.baseLanguageCompletions);
    }
}

export class MyreLispHighlightRules extends TextModeRules {
    [x: string]: any;

    constructor() {
        super();

        var keywordMapper = this.createKeywordMapper({
            "keyword.control": mergeKeywordList(controlKeywords),
            "keyword.operator": mergeKeywordList(operatorKeywords),
            "constant.language": mergeKeywordList(constantLanguage),
            "support.function": mergeKeywordList(supportFunctions)
        }, "identifier", true);

        this.$rules =
            {
                "start": [
                    {
                        token: "comment",
                        regex: ";;.*$"
                    },
                    //{
                    //    token: "comment",
                    //    regex: "#|.?|#"
                    //},
                    {
                        token: ["storage.type.function-type.myre", "text", "entity.name.function.myre"],
                        regex: "(?:\\b(?:(defun|defn|defmethod|defmacro|defev))\\b)(\\s+)((?:\\w|\\-|\\!|\\?)*)"
                    },
                    {
                        token: ["punctuation.definition.constant.character.myre", "constant.character.myre"],
                        regex: "(#)((?:\\w|[\\\\+-=<>'\"&#])+)"
                    },
                    {
                        token: ["punctuation.definition.variable.myre", "variable.other.global.myre", "punctuation.definition.variable.myre"],
                        regex: "(\\*)(\\S*)(\\*)"
                    },
                    {
                        token: "constant.numeric", // hex
                        regex: "0[xX][0-9a-fA-F]+(?:L|l|UL|ul|u|U|F|f|ll|LL|ull|ULL)?\\b"
                    },
                    {
                        token: "constant.numeric", // float
                        regex: "[+-]?\\d+(?:(?:\\.\\d*)?(?:[eE][+-]?\\d+)?)?(?:L|l|UL|ul|u|U|F|f|ll|LL|ull|ULL)?\\b"
                    },
                    {
                        token: keywordMapper,
                        regex: "[a-zA-Z_$][a-zA-Z0-9_$]*\\b"
                    },
                    {
                        token: "string",
                        regex: '"(?=.)',
                        next: "qqstring"
                    }
                ],
                "qqstring": [
                    {
                        token: "constant.character.escape.myre",
                        regex: "\\\\."
                    },
                    {
                        token: "string",
                        regex: '[^"\\\\]+'
                    }, {
                        token: "string",
                        regex: "\\\\$",
                        next: "qqstring"
                    }, {
                        token: "string",
                        regex: '"|$',
                        next: "start"
                    }
                ]
            };
    }
}

export default class MyreLispAceMode extends TextMode {
    [x: string]: any;

    public HighlightRules = MyreLispHighlightRules;
    public $behaviour: any;

    constructor() {
        super();

        this.$behaviour = this.$defaultBehaviour;
    }
}
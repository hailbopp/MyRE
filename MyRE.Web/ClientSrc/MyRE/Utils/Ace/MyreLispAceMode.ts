import brace = require("brace");
import 'brace/mode/lisp'
import { DeviceInfo } from "MyRE/Api/Models";

interface Mode {
    new(): Mode;
}

const TextModeRules: Mode = brace.acequire('ace/mode/text_highlight_rules').TextHighlightRules;
const TextMode: Mode = brace.acequire('ace/mode/text').Mode;

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

    constructor() {
        this.devices = [];

        this.baseLanguageCompletions = [];

        controlKeywords.map((s: string): ICompletion => ({ value: s, meta: "control", score: 1 })).forEach(c => this.baseLanguageCompletions.push(c));
        operatorKeywords.map((s: string): ICompletion => ({ value: s, meta: "operator", score: 1 })).forEach(c => this.baseLanguageCompletions.push(c));
        constantLanguage.map((s: string): ICompletion => ({ value: s, meta: "constant", score: 1 })).forEach(c => this.baseLanguageCompletions.push(c));
        supportFunctions.map((s: string): ICompletion => ({ value: s, meta: "builtin", score: 1 })).forEach(c => this.baseLanguageCompletions.push(c));
    }

    public getCompletions(editor: brace.Editor, session: brace.IEditSession, pos: brace.Position, prefix: string, callback: (a: any, completions: ICompletion[])=>void): void {
        callback(null, this.baseLanguageCompletions);
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
                        regex: "(?:\\b(?:(defun|defmethod|defmacro))\\b)(\\s+)((?:\\w|\\-|\\!|\\?)*)"
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
                    },
                    {
                        token: "reference",
                        regex: '`(?=.)',
                        next: "refstring"
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
                ],
                "refstring": [
                    {
                        token: "constant.character.escape.myre",
                        regex: "\\\\."
                    },
                    {
                        token: "reference",
                        regex: '[^`\\\\]+'
                    }, {
                        token: "reference",
                        regex: "\\\\$",
                        next: "refstring"
                    }, {
                        token: "reference",
                        regex: '`|$',
                        next: "start"
                    }
                ]
            };
    }
}

export default class MyreLispAceMode extends TextMode {
    [x: string]: any;

    public HighlightRules = MyreLispHighlightRules;
    public $completer: MyreLispCompletions = new MyreLispCompletions();
    public $behaviour: any;

    constructor() {
        super();

        this.$behaviour = this.$defaultBehaviour;
        this.$completer;
    }
}
import { Parjs, LoudParser } from 'parjs';
import { Identifier, DeviceLiteral, InitialExpression, Literal, Expression } from 'MyRE/Utils/Models/Expression';
import { MemberAccessExpression } from 'MyRE/Utils/Models/MemberAccessExpression';
import { OnStatement } from 'MyRE/Utils/Models/StatementTypes';
import { Statement, CoreStatement, TopLevelStatement } from 'MyRE/Utils/Models/Statement';
import { Block } from 'MyRE/Utils/Models/Block';
import { Trace } from 'parjs/dist/lib/internal/reply';

// TODO: This does not work very well. If we want a DSL later, we can add it in.

const whitespace = Parjs.whitespaces;
const separator = whitespace.mustBeNonEmpty().q;
const isWhitespace = (c: string) => whitespace.parse(c).kind === 'OK';

const letter = Parjs.letter;
const digit = Parjs.digit;
const identifierChar = Parjs.anyCharOf("_$").or(letter).or(digit);

const identifier = letter.then(identifierChar.many()).str.map<Identifier>(i => ({
    kind: 'IDENTIFIER',
    name: i,
}));

const deviceLiteralQuote = Parjs.string("`");
const deviceLiteralStringChar = Parjs.noCharOf('`');
const deviceLiteralString = deviceLiteralStringChar.many().str;
const deviceLiteral = deviceLiteralString.between(deviceLiteralQuote, deviceLiteralQuote).str.map<DeviceLiteral>(s => ({
    kind: 'LITERAL_DEVICE',
    name: s,
}));

const literal = Parjs.any(
    deviceLiteral
).cast<Literal>();

const initialExpression = Parjs.any(
    identifier,
    literal,
).cast<InitialExpression>();

const memberAccessOperator = Parjs.string('.');
let _memberAccessExpression: LoudParser<any>;
const memberAccessExpression = Parjs.late(() => _memberAccessExpression);

const expression = Parjs.any(
    initialExpression,
    memberAccessExpression,
).cast<Expression>();

_memberAccessExpression = initialExpression.then(memberAccessOperator.q).then(identifier).map<MemberAccessExpression>(([expr, ident]) => ({
    kind: 'EXPR_MEMBER_ACCESS',
    parent: expr,
    property: ident,
}));

let _block: LoudParser<Block>;
const block = Parjs.late(() => _block);

const onToken = Parjs.string("on");

const onStatement = onToken.then(separator).q.then(expression).then(separator).then(block).map<OnStatement>(([expr, block]) => ({
    kind: 'ON_STATEMENT',
    target: expr,
    block: block,
}));

const endOfStatement = Parjs.anyCharOf(';').q;
const coreStatement = Parjs.any(
    expression,
).cast<CoreStatement>();
const topLevelStatement = Parjs.any(
    onStatement,
).cast<TopLevelStatement>();
const statement = coreStatement;

const leftBrace = Parjs.string("{");
const rightBrace = Parjs.string("}");
_block = (statement.between(whitespace, whitespace)).many().between(leftBrace, rightBrace).isolate;

let project = topLevelStatement.many();

class DslParser {
    private parser = project;
    
    public parse = (input: string) => this.parser.parse(input);
    public visualizer = (trace: Trace) => {
        return Parjs.visualizer(trace)
    }
}

export default DslParser;

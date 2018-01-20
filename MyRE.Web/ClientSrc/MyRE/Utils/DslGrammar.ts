export default {
    RegExpID: 'RE::',
    Style: {
        comment: 'comment',
        atom: 'constant',
        keyword: 'keyword',
        this: 'keyword',
        builtin: 'support',
        operator: 'operator',
        identifier: 'identifier',
        property: 'constant.support',
        number: 'constant.numeric',
        string: 'string',
        regex: 'string.regexp'
    },
    Lex: {
        comment: {
            type: 'comment',
            tokens: [
                [
                    '//',
                    null
                ],
                [
                    '/*',
                    '*/'
                ]
            ]
        },
        identifier: 'RE::/[_A-Za-z$][_A-Za-z0-9$]*/',
        this: 'RE::/this\\b/',
        property: 'RE::/[_A-Za-z$][_A-Za-z0-9$]*/',
        number: [
            'RE::/\\d*\\.\\d+(e[\\+\\-]?\\d+)?/',
            'RE::/\\d+\\.\\d*/',
            'RE::/\\.\\d+/',
            'RE::/0x[0-9a-fA-F]+L?/',
            'RE::/0b[01]+L?/',
            'RE::/0o[0-7]+L?/',
            'RE::/[1-9]\\d*(e[\\+\\-]?\\d+)?L?/',
            'RE::/0(?![\\dx])/'
        ],
        string: {
            type: 'escaped-block',
            escape: '\\',
            tokens: [
                'RE::/([\'"])/',
                1
            ]
        },
        regex: {
            type: 'escaped-block',
            escape: '\\',
            tokens: [
                '/',
                'RE::#/[gimy]{0,4}#'
            ]
        },
        operator: {
            tokens: [
                '+',
                '-',
                '++',
                '--',
                '%',
                '>>',
                '<<',
                '>>>',
                '*',
                '/',
                '^',
                '|',
                '&',
                '!',
                '~',
                '>',
                '<',
                '<=',
                '>=',
                '!=',
                '!==',
                '=',
                '==',
                '===',
                '+=',
                '-=',
                '%=',
                '>>=',
                '>>>=',
                '<<=',
                '*=',
                '/=',
                '|=',
                '&='
            ]
        },
        delimiter: {
            tokens: [
                '(',
                ')',
                '[',
                ']',
                '{',
                '}',
                ',',
                '=',
                ';',
                '?',
                ':',
                '+=',
                '-=',
                '*=',
                '/=',
                '%=',
                '&=',
                '|=',
                '^=',
                '++',
                '--',
                '>>=',
                '<<='
            ]
        },
        atom: {
            autocomplete: true,
            tokens: [
                'true',
                'false',
                'null',
                'undefined',
                'NaN',
                'Infinity'
            ]
        },
        keyword: {
            autocomplete: true,
            tokens: [
                'on',
                'if',
                'while',
                'with',
                'else',
                'do',
                'try',
                'finally',
                'return',
                'break',
                'continue',
                'new',
                'delete',
                'throw',
                'var',
                'const',
                'let',
                'function',
                'catch',
                'void',
                'for',
                'switch',
                'case',
                'default',
                'class',
                'import',
                'yield',
                'in',
                'typeof',
                'instanceof'
            ]
        },
        builtin: {
            autocomplete: true,
            tokens: [
                'Object',
                'Function',
                'Array',
                'String',
                'Date',
                'Number',
                'RegExp',
                'Math',
                'Exception',
                'setTimeout',
                'setInterval',
                'parseInt',
                'parseFloat',
                'isFinite',
                'isNan',
                'alert',
                'prompt',
                'console',
                'window',
                'global',
                'this'
            ]
        }
    },
    Syntax: {
        dot_property: {
            sequence: [
                '.',
                'property'
            ]
        },
        js: 'comment | number | string | regex | keyword | operator | atom | ((\'}\' | \')\' | this | builtin | identifier | dot_property) dot_property*)'
    },
    Parser: [
        [
            'js'
        ]
    ]
}
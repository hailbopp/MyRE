{
    const stringify = (s) =>
    	s.map(c => c[1]).join('').trim();
}

program = expr*
expr =  sexp / atom
    
sexp
	= functionDefExpr
    / eventHandlerExpr
   	/ lambdaExpr
    / variableDefExpr
    / getPropertyExpr
    / _ "(" _ atoms:atom+ _ ")" _ { return { type: 'S-EXPR', func: atoms[0], args: atoms.slice(1) }; }
	/ _ v:listLiteral _ { return v; }

atom
	= _ a:sexp _ { return a; }
    / _ a:integer _ { return a; }
    / _ a:float _ { return a; }
    / _ a:string _ { return a; }
    / _ a:symbol _ { return a; }
    / _ a:reference _ { return a; }
    / _ a:comment _ { return a; }
    
variableDefExpr = _ "(" _ "def" _ name:symbol _ value:expr _ ")" _ { return { type: 'VARIABLE_DEFINITION', name, value }; }

functionDefToken = "defun" / "defn"
functionDefExpr = _ "(" _ functionDefToken _ name:symbol _ args:argumentList _ block:expr+ _ ")" _ { return { type: 'NAMED_FUNCTION_DEFINITION', name: name, args: args, body: block }; }
eventHandlerExpr = _ "(" _ "defev" _ name:eventHandlerName _ event:expr _ handler:expr _ ")" _ { return { type: 'EVENT_HANDLER_DEFINITION', name: name, event: event, body: handler }; }
lambdaExpr = _ "(" _ "lambda" _ args:argumentList _ block:expr+ _ ")" _ { return { type: 'ANONYMOUS_FUNCTION_DEFINITION', args: args, body: block }; }
getPropertyExpr = _ "(" _ "get" _ objectName:expr _ propertyName:expr _ ")" _ { return { type: 'GET_PROPERTY_DEFINITION', object: objectName, property: propertyName }; }
    
argumentList = _ "(" _ atoms:atom* _ ")" { return { type: 'ARGUMENT_LIST', value: atoms }; }
eventHandlerName = symbol / string
    
lineComment = ";;" s:(!EOL sourcechar)* EOL { return { type: 'COMMENT', contents: stringify(s) }; }
blockComment = "#|" s:(!"|#" sourcechar)* "|#" { return { type: 'COMMENT', contents: stringify(s) }; }
comment = lineComment / blockComment

emptyList = _ "[" _ "]" { return { type: 'LITERAL_LIST', value: [] }; }
atomList = _ "[" _ atoms:atom+ _ "]" { return { type: 'LITERAL_LIST', value: atoms }; }
listLiteral = emptyList / atomList

boolean = 't' / 'nil'
booleanLiteral = b:boolean _ { let s = stringify(b); return { type: 'LITERAL_BOOL', value: b === 't' ? true : false }; }
    
integer	= i:([0-9]+) { return { type: 'LITERAL_INT', value: parseInt(i.join(''), 10) } }    
float = ipart:([0-9]+) '.' fpart:([0-9]+) { return { type: 'LITERAL_FLOAT', value: parseFloat(stringify(ipart) + '.' + stringify(fpart)) }; }
string = '"' d:(!'"' sourcechar)* '"' _ { return { type: 'LITERAL_STRING', value: stringify(d) }; }
symbol = s:(symbolChar+) { return { type: 'LITERAL_SYMBOL', value: stringify(s) }; }
reference = '`' r:(!'`' sourcechar)* '`' _ { return { type: 'LITERAL_REFERENCE', value: stringify(r) }; }

paren = "(" / ")"
delimiter = paren / _
EOF	= !.
EOL	= "\n" / EOF
__ = [\n, ]
_ = (__)*
invalidSymbolChar = [()",'`;#|\\ ]
symbolChar = (!invalidSymbolChar .)
sourcechar = .
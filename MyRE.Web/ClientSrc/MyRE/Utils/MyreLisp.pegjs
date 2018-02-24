{
    const stringify = (s) =>
    	s.map(c => c[1]).join('').trim();
}

program = expr*
expr =  sexp / atom
    
sexp
	= _ "(" _ atoms:atom+ _ ")" _ { 
			//return { type: 'S-EXPR', func: atoms[0], args: atoms.slice(1) }; 
			return atoms;
		}
	/ _ v:listLiteral _ { return v; }

atom
	= _ a:sexp _ { return a; }
    / _ a:integer _ { return a; }
    / _ a:float _ { return a; }
    / _ a:string _ { return a; }
    / _ a:symbol _ { return a; }
    / _ a:reference _ { return a; }
    / _ a:comment _ { return a; }
    
lineComment = ";;" s:(!EOL sourcechar)* EOL { 
	//return { type: 'COMMENT', contents: stringify(s) }; 
	return ["comment", stringify(s)];
}

blockComment = "#|" s:(!"|#" sourcechar)* "|#" { 
	//return { type: 'COMMENT', contents: stringify(s) }; 
	return ["comment", stringify(s)];
}

comment = lineComment / blockComment

emptyList = _ "[" _ "]" { 
	//return { type: 'LITERAL_LIST', value: [] }; 
	return [];
}

atomList = _ "[" _ atoms:atom+ _ "]" { 
	//return { type: 'LITERAL_LIST', value: atoms }; 
	return atoms;
}

listLiteral = emptyList / atomList

boolean = 't' / 'nil'
booleanLiteral = b:boolean _ { 
	let s = stringify(b); 
	//return s === 't' ? true : false; 
	return {
		role: "bool",
		value: s === 't' ? true : false
	};
}
    
integer	= i:([0-9]+) { 
	//return parseInt(i.join(''), 10); 
	return {
		role: "num",
		value: parseInt(i.join(''), 10)
	};
}    

float = ipart:([0-9]+) '.' fpart:([0-9]+) { 
	//return parseFloat(stringify(ipart) + '.' + stringify(fpart)); 
	return {
		role: "num",
		value: parseFloat(stringify(ipart) + '.' + stringify(fpart))
	};
}

string = '"' d:(!'"' sourcechar)* '"' _ { 
	//return "str:" + stringify(d); 
	return {
		role: "str",
		value: stringify(d)
	};
}

symbol = s:(symbolChar+) { 
	//return "sym:" + stringify(s); 
	return {
		role: "sym",
		value: stringify(s)
	};
}

reference = '`' r:(!'`' sourcechar)* '`' _ { 
	//return "ref:" + stringify(r); 
	return {
		role: "ref",
		value: stringify(r)
	};
}

paren = "(" / ")"
delimiter = paren / _
EOF	= !.
EOL	= "\n" / EOF
__ = [\n, ]
_ = (__)*
invalidSymbolChar = [()",'`;#|\\ ]
symbolChar = (!invalidSymbolChar .)
sourcechar = .
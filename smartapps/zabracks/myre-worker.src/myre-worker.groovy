def getNAMESPACE() { return "zabracks" }
def getPARENT_NAME() { return "MyRE" }
def getAPP_NAME() { return "${PARENT_NAME}-worker" }
def getVERSION() { return "0.0.1" }

definition(
    name: APP_NAME,
    namespace: NAMESPACE,
    author: "Drew Worthey",
    description: "MyRE worker app. Do not install directly.",
    parent: "zabracks:${PARENT_NAME}",
    category: "Convenience",
    singleInstance: false
)

preferences {
    page(name: "pageInit")
}

def pageInit() {
    return dynamicPage(name: "pageInit", title: "", uninstall: !!state.inited) {
        if(parent && parent.isInstalled()) {
            section() {
                paragraph "This is the worker app for the ${state.name} project."
            }
        } else {
            section() {
                paragraph "You cannot install this SmartApp directly. You must install the ${PARENT_NAME} app first."
            }
        }
    }
}

def getAllDevices() {
    return parent.getManagedDevices()
}

def getDeviceById(deviceId) {
    return parent.getDeviceStatusById(deviceId)
}

///// Implementation of MyreLisp intepreter
// I'm adapting much of MAL (https://github.com/kanaka/mal/blob/master/groovy) for this.
// Due to limitations of the SmartThings API, we're forced to do things in a hacky way.

/// Types
// Since ST doesn't allow class definitions, we're going to do everything with functions and maps...
// for better or for worse.
Map Symbol(name) {
    [ __cls: "SYMBOL", value: name ]
}
def symbol_Q(o) {
    return o instanceof Map && o.get("__cls", "") == "SYMBOL"
}

int Symbol__compare(a, b) {
    a['value'] <=> b['value']
}

Map Atom(value) {
    [ __cls: 'ATOM', value: value ]
}
def atom_Q(o) {
    return o instanceof Map && o.get("__cls", "") == "ATOM"
}

Map Function(_EVAL, _ast, _env, _params) {
    [
        __cls: 'FUNC',
        EVAL: _EVAL,
        ast: _ast,
        env: _env,
        params: _params,
        ismacro: false
    ]
}
def function_Q(o) {
    return o instanceof Map && o.get("__cls", "") == "FUNC"
}
def Function__call(func, args) {
    def new_env = Env(func['env'], func['params'], args)
    return EVAL(func['ast'], new_env)
}

Map MyreLispException(String msg) {
    [ __cls: "EXCEPTION", message: msg ]
}

def string_Q(o) {
    return o instanceof String && (o.size() == 0 || o[0] != "\u029e")
}

def keyword_Q(o) {
    return o instanceof String && o.size() > 0 && o[0] == "\u029e"
}
def keyword(o) {
    this.&keyword_Q(o) ? o : ("\u029e" + o)
}

def list_Q(o) {
    //return (o instanceof List || o instanceof Object[]) &&
    return o instanceof List && !o.hasProperty("isvector")
}

def vector(o) {
    def v = o.collect()
    v.metaClass.isvector = true
    v
}
def vector_Q(o) {
    return o instanceof List && o.hasProperty("isvector") && o.isvector
}
def hash_map(lst) {
    def m = [:]
    assoc_BANG(m, lst)
}
def assoc_BANG(m, kvs) {
    for (int i=0; i<kvs.size(); i+=2) {
        m[kvs[i]] = kvs[i+1];
    }
    return m
}
def dissoc_BANG(m, ks) {
    for (int i=0; i<ks.size(); i++) {
        m.remove(ks[i])
    }
    return m
}
def hash_map_Q(o) {
    return o instanceof Map
}

def sequential_Q(o) {
    return types.list_Q(o) || types.vector_Q(o)
}

/// Env
Map Env(Map outer_env = null, binds = [], exprs = []) {
    def result = [:]
    result['outer'] = outer_env
    result['data'] = [:]
    for (int i=0; i<binds.size; i++) {
        if (binds[i].value == "&") {
            result['data'][binds[i+1].value] = (exprs.size() > i) ? exprs[i..-1] : []
            break
        } else {
            result['data'][binds[i].value] = exprs[i]
        }
    }
}
def Env__set(Map env, Map keySymbol, val) {
    if(keySymbol['__cls'] == "SYMBOL") {
        return MyreLispException("Expected symbol")
    }
    env['data'][keySymbol['value']] = val
}
def Env__find(Map env, Map keySymbol) {
    if(keySymbol['__cls'] == "SYMBOL") {
        return MyreLispException("Expected symbol")
    }
    if(env['data'].containsKey(keySymbol['value'])) {
        env
    } else if(env['outer'] != null) {
        Env__find(env['outer'])
    } else {
        null
    }
}
def Env__get(Map env, Map keySymbol) {
    def e = Env__find(env, keySymbol)
    if(e == null) {
        return MyreLispException("'${keySymbol['value']}' not found.")
    } else {
        e['data'].get(keySymbol['value'])
    }
}

/// Printer
def print_list(lst, sep, Boolean print_readably) {
    return lst.collect{ e -> pr_str(e, print_readably) }.join(sep)
}
def pr_str(exp, Boolean print_readably) {
    def _r = print_readably
    switch (exp) {
        case { list_Q(exp) }:
            def lst = exp.collect { pr_str(it, _r) }
            return "(${lst.join(" ")})"
        case { vector_Q(exp) }:
            def lst = exp.collect { pr_str(it, _r) }
            return "[${lst.join(" ")}]"
        case { symbol_Q(exp) }:
            return exp['value']
        case { atom_Q(exp) }:
            return "(atom ${exp['value']})"
        case Map:
            def lst = []
            exp.each { k,v -> lst.add(pr_str(k,_r)); lst.add(pr_str(v,_r)) }
            return "{${lst.join(" ")}}"
        case String:
            if (keyword_Q(exp)) {
                return ":" + exp.drop(1)
            } else if (print_readably) {
                //return "\"${StringEscapeUtils.escapeJava(exp)}\""
                return exp
            } else {
                return exp
            }
        case null:
            return 'nil'
        default:
            return exp.toString()
    }
}

/// Reader
Map Reader(tokens) {
    [ __cls: 'READER', tokens: tokens, position: 0 ]
}
def Reader__peek(Map reader) {
    if (reader['position'] >= reader['tokens'].size) {
        return null
    } else {
        return reader['tokens'][reader['position']]
    }
}
def Reader__next(reader) {
    if (reader['position'] >= reader['tokens'].size) {
        null
    } else {
        reader['tokens'][reader['position']]
        reader['position'] = reader['position'] + 1
    }
}
def tokenizer(String str) {
    def mask = /[\s,]*(~@|[\[\]{}()'`~^@]|"(?:\\.|[^\\"])*"|;.*|[^\s\[\]{}('"`,;)]*)/
    def m = str =~ mask

    def tokens = []
    for(item in m) {
        String token = item[1]
        if (token != null &&
                !(token == "") &&
                !(token[0] == ';')) {
            tokens.add(token)
        }
    }
    return tokens
}
def read_atom() {
    def mask = /(^-?[0-9]+$)|(^-?[0-9][0-9.]*$)|(^nil$)|(^true$)|(^false$)|^"(.*)"$|:(.*)|(^[^"]*$)/
    def m = (token =~ mask)[0]

    if (m[1] != null) {
        Integer.parseInt(m[1])
    } else if (m[3] != null) {
        null
    } else if (m[4] != null) {
        true
    } else if (m[5] != null) {
        false
    } else if (m[6] != null) {
        //StringEscapeUtils.unescapeJava(m.group(6))
        m[6]
    } else if (m[7] != null) {
        "\u029e" + m[7]
    } else if (m[8] != null) {
        Symbol(m[8])
    } else {
        return MyreLispException("unrecognized '${m.group(0)}'")
    }
}
def read_list(Map rdr, char start, char end) {
    String token = Reader__next(rdr)
    def lst = []
    if (token.charAt(0) != start) {
        return MyreLispException("expected '${start}'")
    }

    while ((token = Reader__peek(rdr)) != null && token.charAt(0) != end) {
        lst.add(read_form(rdr))
    }

    if (token == null) {
        return MyreLispException("expected '${end}', got EOF")
    }
    Reader__next(rdr)

    return lst
}
def read_vector(Map rdr) {
    def lst = read_list(rdr, '[' as char, ']' as char)
    return vector(lst)
}

def read_hash_map(Map rdr) {
    def lst = read_list(rdr, '{' as char, '}' as char)
    return hash_map(lst)
}

def read_form(Map rdr) {
    def token = Reader__peek(rdr)
    switch (token) {
    // reader macros/transforms
        case "'":
            Reader__next(rdr)
            return [Symbol("quote"), read_form(rdr)]
        case '`':
            Reader__next(rdr)
            return [Symbol("quasiquote"), read_form(rdr)]
        case '~':
            Reader__next(rdr)
            return [Symbol("unquote"), read_form(rdr)]
        case '~@':
            Reader__next(rdr)
            return [Symbol("splice-unquote"), read_form(rdr)]
        case '^':
            Reader__next(rdr)
            def meta = read_form(rdr);
            return [Symbol("with-meta"), read_form(rdr), meta]
        case '@':
            Reader__next(rdr)
            return [Symbol("deref"), read_form(rdr)]

    // list
        case ')': return MyreLispException("unexpected ')'")
        case '(': return read_list(rdr, '(' as char, ')' as char)

    // vector
        case ']': return MyreLispException("unexpected ']'")
        case '[': return read_vector(rdr)

    // hash-map
        case '}': return MyreLispException("unexpected '}'")
        case '{': return read_hash_map(rdr)

    // atom
        default: return read_atom(rdr)
    }
}

def read_str(String str) {
    def tokens = tokenizer(str)
    if (tokens.size() == 0) {
        return null
    }
    //println "tokens ${tokens}"
    def rdr = Reader(tokens)
    read_form(rdr)
}

/// Core
def do_concat(args) {
    args.inject([], { a, b -> a + (b as List) })
}
def do_nth(args) {
    if (args[0].size() <= args[1]) {
        return MyreLispException("nth: index out of range")
    }
    args[0][args[1]]
}
def do_apply(args) {
    def start_args = args.drop(1).take(args.size()-2) as List
    args[0](start_args + (args.last() as List))
}

def do_swap_BANG(args) {
    def atm = args[0]
    def f = args[1]
    atm['value'] = f([atm.value] + (args.drop(2) as List))
}

def do_conj(args) {
    if (types.list_Q(args[0])) {
        args.drop(1).inject(args[0], { a, b -> [b] + a })
    } else {
        vector(args.drop(1).inject(args[0], { a, b -> a + [b] }))
    }
}
def do_seq(args) {
    def obj = args[0]
    switch (obj) {
        case { list_Q(obj) }:
            return obj.size() == 0 ? null : obj
        case { vector_Q(obj) }:
            return obj.size() == 0 ? null : obj.clone()
        case { string_Q(obj) }:
            return obj.size() == 0 ? null : obj.collect{ it.toString() }
        case null:
            return null
        default:
            return MyreLispException("seq: called on non-sequence")
    }
}

def tryGetProperty(o, propName) {
    try {
        o."${propName}"
    } catch (Exception e) {
        null
    }
}

def tryGetMethod(o, methodName) {
    try {
        o.&"${methodName}"
    } catch (e) {
        null
    }
}

def getMember(o, memberName) {
    if(o instanceof Map) {
        o[memberName]
    } else {
        def m = tryGetProperty(o, memberName)
        if(m != null) {
            return m
        }
        m = tryGetMethod(o, memberName)
        if(m != null) {
            return m
        }
        o."${memberName}"
    }
}

def ns = [
        "=": { a -> a[0]==a[1]},
        "throw": { a -> return MyreLispException(a[0]) },

        "nil?": { a -> a[0] == null },
        "true?": { a -> a[0] == true },
        "false?": { a -> a[0] == false },
        "string?": { a -> string_Q(a[0]) },
        "symbol": { a -> Symbol(a[0]) },
        "symbol?": { a -> a[0] instanceof Map && a[0]['__cls'] == "SYMBOL"},
        "keyword": { a -> keyword(a[0]) },
        "keyword?": { a -> keyword_Q(a[0]) },
        "number?": { a -> a[0] instanceof Integer },
        "fn?": { a -> (a[0] instanceof Map && a[0]['__cls'] == "FUNC" && !a[0]['ismacro']) ||
                a[0] instanceof Closure },
        "macro?": { a -> a[0] instanceof Map && a[0]["__cls"] == "FUNC" && a[0]['ismacro'] },

        "pr-str": this.&do_pr_str,
        "str": this.&do_str,
        "prn": this.&do_prn,
        "println": this.&do_println,
        "read-string": this.&read_str,
        //"readline": { a -> System.console().readLine(a[0]) },
        "slurp": { a ->
            //new File(a[0]).text
            ""
        },

        "<":  { a -> a[0]<a[1]},
        "<=": { a -> a[0]<=a[1]},
        ">":  { a -> a[0]>a[1]},
        ">=": { a -> a[0]>=a[1]},
        "+":  { a -> a[0]+a[1]},
        "-":  { a -> a[0]-a[1]},
        "*":  { a -> a[0]*a[1]},
        "/":  { a -> a[0]/a[1]},  // /
        "time-ms": { a -> System.currentTimeMillis() },

        "list": { a -> a },
        "list?": { a -> list_Q(a[0]) },
        "vector": { a -> vector(a) },
        "vector?": { a -> vector_Q(a[0]) },
        "hash-map": { a -> hash_map(a) },
        "map?": { a -> hash_map_Q(a[0]) },
        "assoc": { a -> assoc_BANG(types.copy(a[0]), a.drop(1)) },
        "dissoc": { a -> dissoc_BANG(types.copy(a[0]), a.drop(1)) },
        "get": { a ->
            if(a[0] instanceof Map) {
                a[0] == null ? null : a[0][a[1]]
            } else {
                getMember(a[0], a[1])
            }
        },
        "contains?": { a -> a[0].containsKey(a[1]) },
        "keys": { a -> a[0].keySet() as List },
        "vals": { a -> a[0].values() as List },

        "sequential?": { a -> this.&sequential_Q(a[0]) },
        "cons": { a -> [a[0]] + (a[1] as List) },
        "concat": this.&do_concat,
        "nth": this.&do_nth,
        "first": { a -> a[0] == null || a[0].size() == 0 ? null : a[0][0] },
        "rest": { a -> a[0] == null ? [] as List : a[0].drop(1) },
        "empty?": { a -> a[0] == null || a[0].size() == 0 },
        "count": { a -> a[0] == null ? 0 : a[0].size() },
        "apply": this.&do_apply,
        "map": { a -> a[1].collect { x -> a[0].call([x]) } },

        "conj": this.&do_conj,
        "seq": this.&do_seq,

        "meta": { a -> a[0].hasProperty("meta") ? a[0].getProperties().meta : null },
        "with-meta": { a -> def b = types.copy(a[0]); b.getMetaClass().meta = a[1]; b },
        "atom": { a -> Atom(a[0]) },
        "atom?": { a -> a[0] instanceof Map && a[0]["__cls"] == "ATOM"},
        "deref": { a -> a[0].value },
        "reset!": { a -> a[0].value = a[1] },
        "swap!": this.&do_swap_BANG,

        "dev": { a ->
            def deviceId = a[0]
            def device = parent.getDeviceById(deviceId)

            device
        },
]

/// REPL
def READ(str) {
    read_str(str)
}
def macro_Q(ast, env) {
    if (list_Q(ast) &&
            ast.size() > 0 &&
            ast[0]['__cls'] == "SYMBOL" &&
            Env__find(env, ast[0])) {
        def obj = Env__get(env, ast[0])
        if (obj instanceof Map && obj['__cls'] == "FUNC" && obj['ismacro']) {
            return true
        }
    }
    return false
}
def macroexpand(ast, env) {
    while (macro_Q(ast, env)) {
        def mac = Env__get(env, ast[0])
        ast = mac(ast.drop(1))
    }
    return ast
}
def pair_Q(ast) {
    return sequential_Q(ast) && ast.size() > 0
}
def quasiquote(ast) {
    if (! pair_Q(ast)) {
        [Symbol("quote"), ast]
    } else if (ast[0] != null &&
            ast[0]["__cls"] == "SYMBOL" &&
            ast[0]["value"] == "unquote") {
        ast[1]
    } else if (pair_Q(ast[0]) && ast[0][0]['__cls'] == "SYMBOL" &&
            ast[0][0]['value'] == "splice-unquote") {
        [Symbol("concat"), ast[0][1], quasiquote(ast.drop(1))]
    } else {
        [Symbol("cons"), quasiquote(ast[0]), quasiquote(ast.drop(1))]
    }
}
def eval_ast(ast, env) {
    switch(ast) {
        case List:
            return vector_Q(ast) ?
                    vector(ast.collect { EVAL(it,env) }) :
                    ast.collect { EVAL(it,env) }
        case Map:
            if(ast.get('__cls', "HASHMAP") == "SYMBOL") {
                return Env__get(env, ast)
            } else {
                def new_hm = [:]
                ast.each { k,v ->
                    new_hm[EVAL(k, env)] = EVAL(v, env)
                }
                return new_hm
            }
        default:
            return ast
    }
}
def EVAL(ast, env) {
    while (true) {
        //println("EVAL: ${printer.pr_str(ast,true)}")
        if (! list_Q(ast)) return eval_ast(ast, env)

        ast = macroexpand(ast, env)
        if (! list_Q(ast)) return eval_ast(ast, env)
        if (ast.size() == 0) return ast

        switch (ast[0]) {
            case { symbol_Q(it) && it['value'] == "def!" }:
                return Env__set(env, ast[1], EVAL(ast[2], env))
            case { symbol_Q(it) && it['value'] == "let*" }:
                def let_env = Env(env)
                for (int i=0; i < ast[1].size(); i += 2) {
                    Env__set(let_env, ast[1][i], EVAL(ast[1][i+1], let_env))
                }
                env = let_env
                ast = ast[2]
                break // TCO
            case { symbol_Q(it) && it['value'] == "subev" }:
                def devicesList = EVAL(ast[1], env)
                def eventName = EVAL(ast[2], env)
                def handler = ast[3]

                subscribe(devicesList, eventName, { e ->
                    Function__call(handler, [e])
                })
            case { symbol_Q(it) && it['value'] == "quote" }:
                return ast[1]
            case { symbol_Q(it) && it['value'] == "quasiquote" }:
                ast = quasiquote(ast[1])
                break // TCO
            case { symbol_Q(it) && it['value'] == "defmacro!" }:
                def f = EVAL(ast[2], env)
                f.ismacro = true
                return env.set(ast[1], f)
            case { symbol_Q(it) && it['value'] == "macroexpand" }:
                return macroexpand(ast[1], env)
            case { symbol_Q(it) && it['value'] == "try*" }:
                def result = EVAL(ast[1], env)
                if(result instanceof Map && result.get("__cls", "") == "EXCEPTION") {
                    if (ast.size() > 2 &&
                            symbol_Q(ast[2][0]) &&
                            ast[2][0]['value'] == "catch*") {
                        def e = null
                        e = result['message']
                        return EVAL(ast[2][2], Env(env, [ast[2][1]], [e]))
                    } else {
                        throw exc
                    }
                }
            case { symbol_Q(it) && it['value'] == "do" }:
                ast.size() > 2 ? eval_ast(ast[1..-2], env) : null
                ast = ast[-1]
                break // TCO
            case { symbol_Q(it) && it['value'] == "if" }:
                def cond = EVAL(ast[1], env)
                if (cond == false || cond == null) {
                    if (ast.size > 3) {
                        ast = ast[3]
                        break // TCO
                    } else {
                        return null
                    }
                } else {
                    ast = ast[2]
                    break // TCO
                }
            case { symbol_Q(it) && it['value'] == "fn*" }:
                return Function(EVAL, ast[2], env, ast[1])
            default:
                def el = eval_ast(ast, env)
                def f = el[0]
                def args = el.drop(1)
                if (function_Q(f)) {
                    env = Env(f['env'], f['params'], args)
                    ast = f['ast']
                    break // TCO
                } else {
                    return Function__call(f, args)
                }
        }
    }
}

def PRINT(exp) {
    pr_str(exp, true)
}

def REP(str) {
    PRINT(EVAL(READ(str), state.env))
}

def getInterpreter() {
    state.env = state.env?: Env()


    ns.each { k,v ->
        Env__set(state.env, Symbol(k), v)
    }
    Env__set(repl_env, Symbol("eval"), { a -> EVAL(a[0], state.env) })
    Env__set(repl_env, Symbol("*ARGV*"), [])

    // core.mal: defined using mal itself
    REP("(def! *host-language* \"groovy-smartthings\")")
    REP("(def! not (fn* (a) (if a false true)))")
    //REP("(def! load-file (fn* (f) (eval (read-string (str \"(do \" (slurp f) \")\")))))")
    REP("(defmacro! cond (fn* (& xs) (if (> (count xs) 0) (list 'if (first xs) (if (> (count xs) 1) (nth xs 1) (throw \"odd number of forms to cond\")) (cons 'cond (rest (rest xs)))))))");
    REP("(def! *gensym-counter* (atom 0))");
    REP("(def! gensym (fn* [] (symbol (str \"G__\" (swap! *gensym-counter* (fn* [x] (+ 1 x)))))))");
    REP("(defmacro! or (fn* (& xs) (if (empty? xs) nil (if (= 1 (count xs)) (first xs) (let* (condvar (gensym)) `(let* (~condvar ~(first xs)) (if ~condvar ~condvar (or ~@(rest xs)))))))))");

    this.&REP
}

def interpret(str) {
    def interpreter = getInterpreter()
    interpreter(str)
}
///// END interpreter implementation

def getSummary() {
    [
        appId: app.id,
        projectId: state.projectId,
        name: state.name,
        description: state.desc,
        source: state.src
    ]
}


Boolean setup(projectId, name, description, source) {
    if(projectId && name && description && source) {
        state.projectId = projectId
        state.name = name
        state.desc = description
        state.src = source

        state.inited = true

        return true
    } else {
        return false
    }
}

Boolean updateProject(name, description, source) {
    if(name && description && source) {
        unsubscribe()

        state.name = name
        state.desc = description
        state.src = source

        interpret(state.src)
    }
}

def installed() {
    state.created = now()
    state.modified = now()
    state.active = true
    state.subscriptions = state.subscriptions?: [:]

    interpret(state.src)

    return true
}
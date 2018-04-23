## Language Syntax
The basic syntax of MyreLisp is very similar to other languages of the Lisp
family. If you have used Common Lisp, Scheme, Emacs Lisp, or Clojure, you will
likely likely be right at home.

### Basic Syntax Elements
MyreLisp is built from several types of atoms.

#### Comments
```lisp
;; Comments are single-line. There are not currently multiline or block comments.
;; Begin a comment using `;;`. The comment will continue until the end of the line.

;; This is a comment that takes up the entire line.
(def varName 1)    ;; This is a comment that begins after the variable definition expression.
(def otherVar      ;; Comments can appear inside of expressions.
    2)
```

#### Numbers
```lisp
(def integerVar 100)                           ;; You can assign numbers to variables.
(def negativeNumber -20)                       ;; Negative numbers work as you might expect.

(def closeToPi 3.14159)                        ;; Floating point literals work just like integers.
(def closeToNegativePi (* -1 closeToPi))       ;; Integers and floats generally work together without fuss.
```

#### Strings
```lisp
"This is a string"
(def stringThing "This is how you assign a string to a variable.")
(+ "Hello" "concatenation!")                    ;; "Helloconcatenation!"
(* "Hello! " 3)                                 ;; "Hello! Hello! Hello! "
```

#### Booleans
```lisp
;; It is worth noting that in MyreLisp, in order to stay true to the underlying
;; Groovy implementation, nil != false.
(= nil false)           ;; false
(= false false)         ;; true
(= true true)           ;; true
(= true false)          ;; false
(= 1 true)              ;; false
(= 0 false)             ;; false
(= 0 nil)               ;; false
```


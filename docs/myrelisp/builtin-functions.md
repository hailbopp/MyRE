## Built-in Functions
The following are built-in functionality of MyreLisp.

### General

#### =
Checks for equality between arguments.
#### + - * /
#### < <= > >=
#### throw
#### nil?
#### true?
#### false?
#### string? symbol? keyword? number? fn? macro?
#### keyword symbol list vector hash-map
#### str / pr-str
Converts an expression to its string representation.
#### prn / println
Prints a string to the output stream.
#### time-ms
Returns the current time in milliseconds.
#### assoc/dissoc/get/contains?/keys/vals (hashmap operations)
#### sequential?
#### cons
Inserts an element at the beginning of a list.
#### concat
Concatenate two lists.
#### nth
Get the item at a specified index from a list.
#### first
Return the first item in a list.
#### rest
Return a list without its first element.
#### empty?
Returns true if a provided list is empty.
#### count
Returns the length of a list
#### apply
Call a function with arguments
#### filter
Filter a list's elements using a predicate function
#### map
Apply a function to each element in a list, returning the new list.
#### conj
Conjoin items.
#### seq
Create a sequence from the argument.
#### meta/with-meta
#### atom/atom?
#### def
Declare a symbol with a value within the current scope.
#### let
Declare a symbol within a new scope.
#### try/catch
#### do
#### if
#### fn [argument list] [function body]
Declare a new function.

### Macros
#### quote
#### quasiquote
#### defmacro
#### macroexpand

### SmartThings
#### set-instance
Sets the active instance.
#### get-devices
Returns a list of all device references.
#### using-dev [symbol] [device reference, label, or ID] [block body]
Gets the current device state and stores it in [symbol], then executes
the body.
#### dev [label or ID]
Returns a device reference for a given device label or ID.
#### dev-attr [device reference] [command name]
Returns the current state of an attribute on the device.
#### dev-cmd [device reference(s)] [command name] [argument list]
Execute the named device command, providing any necessary parameters.

### Utility Functions
#### [placeholder]

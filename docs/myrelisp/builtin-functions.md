## Built-in Functions
The following are built-in functionality of MyreLisp.

### Operator Functions
#### =
Checks for equality between arguments.
#### + - * /
#### < <= > >=

### Symbols
#### def
Declare a symbol with a value within the current scope.
#### let
Declare a symbol within a new scope.

### Control Flow
#### if
#### do
#### fn [argument list] [function body]
Declare a new function.
#### apply
Call a function with arguments

### Expressions
#### keyword symbol 
#### atom/atom?

### Lists and Collection Operations
#### list vector
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
#### filter
Filter a list's elements using a predicate function
#### map
Apply a function to each element in a list, returning the new list.
#### conj
Conjoin items.
#### seq
Create a sequence from the argument.

### Maps
#### hash-map
#### assoc/dissoc/get/contains?/keys/vals

### Type Checking
#### nil? true? false?
#### string? symbol? keyword? number? fn? macro?
#### sequential?

### Output
#### str / pr-str
Converts an expression to its string representation.
#### prn / println
Prints a string to the output stream.

### Exception Handling
#### try/catch
#### throw

### Macros
#### quote
#### quasiquote
#### defmacro
#### macroexpand
#### meta/with-meta

### SmartThings
#### get-devices
Returns a list of all device references.

#### devref [label or ID]
Returns a device reference for a given device label or ID. Device references can be passed to other functions without
making a call to get attributes or commands.
#### devstate@ [label or ID]
Returns the current state of a given device, including all attribute states, capabilities, and available commands.

#### dev-attr [devstate] [command name]
Returns the current state of an attribute on the device.
#### dev-cmd [devref or devstate] [command name] [argument vector]
Execute the named device command, providing any necessary parameters.

### Time
#### time-ms
Returns the number of milliseconds since 1970-01-01.
#### utc-date
Returns a vector of the form [year month day]

### Utility Functions
#### to-json / from-json
#### 

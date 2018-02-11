import brace = require("brace");
import 'brace/mode/lisp'

interface Mode {
    new(): Mode;
}

const LispMode: Mode = brace.acequire('ace/mode/lisp').Mode;

export default class MyreLispMode extends LispMode {
    constructor() {
        super();
        // Your code goes here
    }
}
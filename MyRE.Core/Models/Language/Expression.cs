using System;
using System.Collections.Generic;
using System.Text;

namespace MyRE.Core.Models.Language
{
    public class BaseGrammarElement
    {
        public string Type { get; set; }
    }
    
    public class SExpression : BaseGrammarElement
    {
        public string Func { get; set; }
        public IEnumerable<BaseGrammarElement> Args {get;set;}
    }

    public class VariableDefinitionExpr : BaseGrammarElement
    {
        public string Name { get; set; }
        public BaseGrammarElement Value { get; set; } 
    }

    public class ArgumentList : BaseGrammarElement
    {
        public IEnumerable<BaseGrammarElement> Value { get; set; }
    }

    public class FunctionDefinitionExpr : BaseGrammarElement
    {
        public string Name { get; set; }
        public ArgumentList Args { get; set; }
        public IEnumerable<BaseGrammarElement> Body { get; set; }
    }

    public class EventHandlerExpr : BaseGrammarElement
    {
        public string Name { get; set; }
        public string Event { get; set; }
        public BaseGrammarElement Body { get; set; }
    }

    public class LambdaExpr : BaseGrammarElement
    {
        public ArgumentList Args { get; set; }
        public IEnumerable<BaseGrammarElement> Body { get; set; }
    }

    public class GetPropertyExpr : BaseGrammarElement
    {
        public BaseGrammarElement Object { get; set; }
        public BaseGrammarElement Property { get; set; }
    }

    public class CommentExpr : BaseGrammarElement
    {
        public string Contents { get; set; }
    }

    public class ListLiteral : BaseGrammarElement
    {
        public IEnumerable<BaseGrammarElement> Value { get; set; }
    }

    public class BooleanLiteral : BaseGrammarElement
    {
        public bool Value { get; set; }
    }

    public class IntegerLiteral : BaseGrammarElement
    {
        public int Value { get; set; }
    }

    public class FloatLiteral : BaseGrammarElement
    {
        public decimal Value { get; set; }
    }

    public class StringLiteral : BaseGrammarElement
    {
        public string Value { get; set; }
    }

    public class SymbolLiteral : BaseGrammarElement
    {
        public string Value { get; set; }
    }

    public class ReferenceLiteral : BaseGrammarElement
    {
        public string Value { get; set; }
    }
}

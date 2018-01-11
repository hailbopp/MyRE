using System.Collections.Generic;

namespace MyRE.Core.Models
{
    public class InvocationExpression : Expression
    {
        public string FunctionName { get; set; }
        public List<FunctionParameter> Parameters { get; set; }
    }
}
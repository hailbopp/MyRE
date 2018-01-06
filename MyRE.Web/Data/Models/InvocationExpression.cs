using System.Collections.Generic;

namespace MyRE.Web.Data.Models
{
    public class InvocationExpression : Expression
    {
        public string FunctionName { get; set; }
        public List<FunctionParameter> Parameters { get; set; }
    }
}
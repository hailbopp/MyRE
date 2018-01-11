using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyRE.Core.Models
{
    public class InvocationExpression : Expression
    {
        [MaxLength(64)]
        public string FunctionName { get; set; }
        public List<FunctionParameter> Parameters { get; set; }
    }
}
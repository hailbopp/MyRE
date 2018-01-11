using MyRE.Core.Models;

namespace MyRE.Core.Models
{
    public class ActionStatement : Statement
    {
        public Expression ExpressionToEvaluate { get; set; }
    }
}
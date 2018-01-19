using System;

namespace MyRE.Core.Models.Data
{
    public class ActionStatement : Statement
    {
        public Guid ExpressionToEvaluateId { get; set; }
        public Expression ExpressionToEvaluate { get; set; }
    }
}
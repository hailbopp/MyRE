namespace MyRE.Core.Models
{
    public class VariableAssignmentStatement : Statement
    {
        public VariableNameExpression VariableNameExpression { get; set; }
        public Expression Value { get; set; }
    }
}
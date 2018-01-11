namespace MyRE.Core.Models
{
    public class VariableAssignmentStatement : Statement
    {
        public string VariableName { get; set; }
        public Expression Value { get; set; }
    }
}
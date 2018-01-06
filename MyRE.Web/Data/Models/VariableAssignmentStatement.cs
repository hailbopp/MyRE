namespace MyRE.Web.Data.Models
{
    public class VariableAssignmentStatement : Statement
    {
        public string VariableName { get; set; }
        public Expression Value { get; set; }
    }
}
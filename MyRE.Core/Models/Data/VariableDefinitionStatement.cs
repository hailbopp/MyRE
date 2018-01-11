namespace MyRE.Core.Models.Data
{
    public class VariableDefinitionStatement : Statement
    {
        public DataType VariableType { get; set; }
        public VariableNameExpression VariableNameExpression { get; set; }
    }
}
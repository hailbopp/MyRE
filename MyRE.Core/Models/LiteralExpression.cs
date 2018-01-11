namespace MyRE.Core.Models
{
    public class LiteralExpression : Expression
    {
        public DataType ValueType { get; set; }

        public string Value { get; set; }
    }
}
namespace MyRE.Web.Data.Models
{
    public class LiteralExpression : Expression
    {
        public enum LiteralType
        {
            Boolean = 1,
            Integer = 2,
            Decimal = 3,
            String = 4,
            Byte = 5,
        }

        public LiteralType ValueType { get; set; }

        public string Value { get; set; }
    }
}
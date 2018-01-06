namespace MyRE.Web.Data.Models
{
    public class FunctionParameter
    {
        public long FunctionParameterId { get; set; }
        public int Position { get; set; }
        public Expression Value { get; set; }
    }
}
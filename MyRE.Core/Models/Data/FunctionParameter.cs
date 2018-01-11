namespace MyRE.Core.Models.Data
{
    public class FunctionParameter
    {
        public long FunctionParameterId { get; set; }
        public int Position { get; set; }
        public Expression Value { get; set; }
    }
}
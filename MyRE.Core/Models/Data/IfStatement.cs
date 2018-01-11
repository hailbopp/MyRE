namespace MyRE.Core.Models.Data
{
    public class IfStatement : Statement
    {
        public Expression Condition { get; set; }
        public Block Block { get; set; }
    }
}
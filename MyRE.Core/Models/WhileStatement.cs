namespace MyRE.Core.Models
{
    public class WhileStatement : Statement
    {
        public Expression Condition { get; set; }
        public Block Block { get; set; }
    }
}
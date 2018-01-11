namespace MyRE.Core.Models.Data
{
    public class WhileStatement : Statement
    {
        public Expression Condition { get; set; }
        public Block Block { get; set; }
    }
}
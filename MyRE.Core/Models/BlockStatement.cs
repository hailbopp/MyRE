namespace MyRE.Core.Models
{
    public class BlockStatement
    {
        public long BlockStatementId { get; set; }
        public int Position { get; set; }
        public Statement Statement { get; set; }
    }
}
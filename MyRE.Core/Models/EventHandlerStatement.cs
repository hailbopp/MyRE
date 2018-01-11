namespace MyRE.Core.Models
{
    public class EventHandlerStatement : Statement
    {
        public string Event { get; set; }
        public Block Block { get; set; }
    }
}
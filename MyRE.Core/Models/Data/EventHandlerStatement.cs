using System.ComponentModel.DataAnnotations;

namespace MyRE.Core.Models.Data
{
    public class EventHandlerStatement : Statement
    {
        [MaxLength(128)]
        public string Event { get; set; }
        public Block Block { get; set; }
    }
}
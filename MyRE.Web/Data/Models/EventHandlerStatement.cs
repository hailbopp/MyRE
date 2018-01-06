using System.Collections.Generic;

namespace MyRE.Web.Data.Models
{
    public class EventHandlerStatement : Statement
    {
        public string Event { get; set; }
        public List<BlockStatement> Block { get; set; }
    }
}
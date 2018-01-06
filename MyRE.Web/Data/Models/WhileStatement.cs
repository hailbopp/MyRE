using System.Collections.Generic;

namespace MyRE.Web.Data.Models
{
    public class WhileStatement : Statement
    {
        public Expression Condition { get; set; }
        public List<BlockStatement> Block { get; set; }
    }
}
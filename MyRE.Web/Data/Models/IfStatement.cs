using System.Collections.Generic;

namespace MyRE.Web.Data.Models
{
    public class IfStatement : Statement
    {
        public Expression Condition { get; set; }
        public List<BlockStatement> Block { get; set; }
    }
}
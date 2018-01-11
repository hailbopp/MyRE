using System.Collections.Generic;
using MyRE.Core.Models;

namespace MyRE.Core.Models
{
    public class IfStatement : Statement
    {
        public Expression Condition { get; set; }
        public Block Block { get; set; }
    }
}
using System.Collections.Generic;
using MyRE.Core.Models;

namespace MyRE.Core.Models
{
    public class Block
    {
        public long BlockId { get; set; }
        public List<BlockStatement> Statements { get; set; }
    }
}
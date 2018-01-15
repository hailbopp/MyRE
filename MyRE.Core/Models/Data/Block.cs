using System;
using System.Collections.Generic;

namespace MyRE.Core.Models.Data
{
    public class Block
    {
        public Guid BlockId { get; set; }
        public List<BlockStatement> Statements { get; set; }
    }
}
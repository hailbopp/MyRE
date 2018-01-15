using System;

namespace MyRE.Core.Models.Data
{
    public class BlockStatement
    {
        public Guid BlockStatementId { get; set; }
        public int Position { get; set; }
        public Statement Statement { get; set; }
    }
}
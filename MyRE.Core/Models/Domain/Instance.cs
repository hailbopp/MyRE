using System;

namespace MyRE.Core.Models.Domain
{
    public class Instance
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid AccountId { get; set; }
    }
}

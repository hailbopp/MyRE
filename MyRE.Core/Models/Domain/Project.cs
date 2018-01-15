using System;

namespace MyRE.Core.Models.Domain
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid InstanceId { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace MyRE.Core.Models.Domain
{
    public class Project
    {
        public Guid ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid InstanceId { get; set; }

        public ProjectSource Source { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRE.Core.Models.Data
{
    public class Project
    {
        public Guid ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Guid ParentInstanceId { get; set; }

        [ForeignKey("ParentInstanceId")]
        public AppInstance ParentInstance { get; set; }
        
        public ProjectSource Source { get; set; }
    }
}

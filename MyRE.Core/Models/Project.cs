using System.Collections.Generic;

namespace MyRE.Core.Models
{
    public class Project
    {
        public long ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public AppInstance ParentInstance { get; set; }

        public List<Routine> Routines { get; set; }
    }
}

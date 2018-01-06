using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRE.Web.Data.Models
{
    public class Project
    {
        public long ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public AppInstance ParentInstance { get; set; }
    }
}

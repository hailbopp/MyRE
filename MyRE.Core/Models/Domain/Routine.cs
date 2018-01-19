using System;
using System.Collections.Generic;
using System.Text;

namespace MyRE.Core.Models.Domain
{
    public class Routine
    {
        public Guid RoutineId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public Guid ProjectId { get; set; }
        public Guid BlockId { get; set; }
        public string ExecutionMethod { get; set; }
    }
}

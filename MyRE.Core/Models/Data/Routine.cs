using System;
using System.ComponentModel.DataAnnotations;

namespace MyRE.Core.Models.Data
{
    public class Routine
    {
        public Guid RoutineId { get; set; }

        [MaxLength(1024)]
        public string Name { get; set; }

        [MaxLength(4096)]
        public string Description { get; set; }
        public Project Project { get; set; }
        public RoutineExecutionMethod ExecutionMethod { get; set; }
        
        public Block Block { get; set; }

        public enum RoutineExecutionMethod
        {
            EntryPoint = 1,
            Invoked = 2,
        }
    }
}
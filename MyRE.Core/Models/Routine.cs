namespace MyRE.Core.Models
{
    public class Routine
    {
        public long RoutineId { get; set; }
        public string Name { get; set; }
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
namespace MyRE.Core.Models.Domain
{
    public class Project
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long InstanceId { get; set; }
    }
}

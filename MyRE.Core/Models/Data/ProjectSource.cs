using System;

namespace MyRE.Core.Models.Data
{
    public class ProjectSource
    {
        public Guid ProjectSourceId { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        public string Source { get; set; }
        public string ExpressionTree { get; set; }
    }
}
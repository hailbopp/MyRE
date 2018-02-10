using System;
using System.Collections.Generic;
using MyRE.Core.Models.Language;

namespace MyRE.Core.Models.Domain
{

    public class ProjectSource
    {
        public Guid ProjectSourceId { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public Guid ProjectId { get; set; }

        public string Source { get; set; }

        public List<Object> ExpressionTree { get; set; }
    }
}
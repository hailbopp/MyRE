﻿using System;

namespace MyRE.Core.Models.Domain
{
    public class Project
    {
        public Guid ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid InstanceId { get; set; }

        public string Source { get; set; }
        public string ExpressionTree { get; set; }
    }
}

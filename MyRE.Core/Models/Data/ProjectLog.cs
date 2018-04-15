using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace MyRE.Core.Models.Data
{
    public class ProjectLog
    {
        public Guid ProjectLogId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public Guid ProjectId { get; set; }
        public Dictionary<string, object> LogData { get; set; }
    }
}

using System;

namespace MyRE.SmartApp.Api.Client.Models
{
    public class ChildSmartApp
    {
        public string AppId { get; set; }
        public string ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string Source { get; set; }
    }
}
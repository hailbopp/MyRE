using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MyRE.Core.Models.Language;
using Newtonsoft.Json;

namespace MyRE.Core.Models.Data
{
    public class ProjectSource
    {
        public Guid ProjectSourceId { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public Guid ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        public string Source { get; set; }
        public string ExpressionTree { get; private set; }

        [NotMapped]
        public List<Object> ParsedExpressionTree {
            get => JsonConvert.DeserializeObject<List<Object>>(ExpressionTree);
            set => ExpressionTree = JsonConvert.SerializeObject(value);
        }
    }
}
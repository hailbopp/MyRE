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
        public Project Project { get; set; }

        public string Source { get; set; }
        public string ExpressionTree { get; set; }

        [NotMapped]
        public List<BaseGrammarElement> ParsedExpressionTree {
            get => JsonConvert.DeserializeObject<List<BaseGrammarElement>>(ExpressionTree);
            set => ExpressionTree = JsonConvert.SerializeObject(value);
        }
    }
}
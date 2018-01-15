using System;
using System.ComponentModel.DataAnnotations;

namespace MyRE.Core.Models.Data
{
    public abstract class Statement
    {
        public Guid StatementId { get; set; }

        [MaxLength(32)]
        public string Discriminator { get; set; }
    }
}
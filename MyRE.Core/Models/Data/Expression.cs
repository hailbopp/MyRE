using System;
using System.ComponentModel.DataAnnotations;

namespace MyRE.Core.Models.Data
{
    public abstract class Expression
    {
        public Guid ExpressionId { get; set; }

        [MaxLength(32)]
        public string Discriminator { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace MyRE.Core.Models.Data
{
    public abstract class Expression
    {
        public long ExpressionId { get; set; }

        [MaxLength(32)]
        public string Discriminator { get; set; }
    }
}
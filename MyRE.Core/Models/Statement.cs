using System.ComponentModel.DataAnnotations;

namespace MyRE.Core.Models
{
    public abstract class Statement
    {
        public long StatementId { get; set; }

        [MaxLength(32)]
        public string Discriminator { get; set; }
    }
}
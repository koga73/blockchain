using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Q.DB.DBO.Struct
{
    internal class TransactionInput
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int id { get; set; }

        [Required]
        [MaxLength(64)]
        public string OfTransaction { get; set; }

        [Required]
        [MaxLength(64)]
        public string TransactionHash { get; set; }

        [Required]
        public int OutputIndex { get; set; }
    }
}

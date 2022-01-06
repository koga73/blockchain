using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Q.DB.DBO.Struct
{
    internal class TransactionOutput
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int id { get; set; }

        [Required]
        [MaxLength(64)]
        public string OfTransaction { get; set; }

        [Required]
        [MaxLength(64)]
        public string Address { get; set; }

        [Required]
        public float Amount { get; set; }

        [Required]
        public int Index { get; set; }
    }
}

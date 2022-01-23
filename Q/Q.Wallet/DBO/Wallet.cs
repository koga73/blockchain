using System.ComponentModel.DataAnnotations;

namespace Q.Wallet.DBO
{
    internal class Wallet
    {
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }

        [Key]
        [MaxLength(128)]
        public string PublicKey { get; set; }

        [Required]
        [MaxLength(64)]
        public string Alias { get; set; }

        [Required]
        public float Balance { get; set; }
    }
}

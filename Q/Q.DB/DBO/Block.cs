using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Q.DB.DBO
{
    internal class Block
    {
        [Key]
        [Required]
        [MaxLength(64)]
        public string Hash { get; set; }

        [Required]
        [MaxLength(64)]
        public string MerkleRoot { get; set; }

        [Required]
        [MaxLength(64)]
        public string PreviousBlockHash { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public int Version { get; set; }

        [Required]
        [MaxLength(64)]
        public string Nonce { get; set; }

        [Required]
        public int Height { get; set; }

        [Required]
        public int Difficulty { get; set; }
    }
}

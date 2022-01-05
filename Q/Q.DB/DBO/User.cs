using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Q.DB.DBO
{
    internal class User
    {
        [Key]
        [MaxLength(128)]
        public string PublicKey { get; set; }
        
        [Required]
        [MaxLength(64)]
        public string Alias { get; set; }

        [Required]
        public int DataIndex { get; set; }

        [Required]
        [MaxLength(64)]
        public string BlockHash { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }
    }
}

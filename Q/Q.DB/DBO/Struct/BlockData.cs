using System;
using System.ComponentModel.DataAnnotations;

namespace Q.DB.DBO.Struct
{
    internal class BlockData
    {
        [Required]
        [MaxLength(64)]
        public string BlockHash { get; set; }

        [Required]
        public int DataIndex { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace Q.DB.DBO.Struct
{
    internal class BlockData
    {
        //Hash of this data
        [Key]
        [MaxLength(64)]
        public string Hash { get; set; }

        //Signature relating to hash of this data
        [Required]
        public string Signature { get; set; }

        //Which block is this data part of?
        [Required]
        [MaxLength(64)]
        public string BlockHash { get; set; }

        //Which index in the block data is this item?
        [Required]
        public int DataIndex { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }
    }
}

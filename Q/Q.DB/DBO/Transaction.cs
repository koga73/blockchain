using System;
using System.ComponentModel.DataAnnotations;

using Q.DB.DBO.Struct;

namespace Q.DB.DBO
{
    internal class Transaction : BlockData
    {
        [Key]
        [MaxLength(128)]
        public string Hash { get; set; }
    }
}

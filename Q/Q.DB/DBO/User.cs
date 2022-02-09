using System.ComponentModel.DataAnnotations;

using Q.DB.DBO.Struct;

namespace Q.DB.DBO
{
    internal class User : BlockData
    {
        [MaxLength(128)]
        public string PublicKey { get; set; }
        
        [Required]
        [MaxLength(64)]
        public string Alias { get; set; }
    }
}

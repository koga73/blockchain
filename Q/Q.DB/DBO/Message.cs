using System.ComponentModel.DataAnnotations;

using Q.DB.DBO.Struct;

namespace Q.DB.DBO
{
    internal class Message : BlockData
    {
        [MaxLength(128)]
        public string PublicKey { get; set; }
        
        [Required]
        [MaxLength(1024)]
        public string Data { get; set; }
    }
}

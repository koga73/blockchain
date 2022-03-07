using System;
using System.Collections.Generic;
using System.Text;

namespace Q.Data.Models.Struct
{
    public abstract class BlockData
    {
        public DateTime Timestamp { get; set; }
        public abstract string Hash { get; }
        public string Signature { get; set; }

        public BlockData()
        {
            Timestamp = DateTime.Now;
        }

        override public string ToString()
        {
            return $"{{ Timestamp:{Timestamp.Ticks} \"Hash\":\"{Hash}\" }}";
        }
    }
}

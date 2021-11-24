using System;
using System.Collections.Generic;
using System.Text;

namespace Q.Models
{
    public abstract class BlockDataBase
    {
        public DateTime Timestamp;
        public abstract string Hash { get; }
        public string Signature;

        public BlockDataBase()
        {
            Timestamp = DateTime.Now;
        }

        override public string ToString()
        {
            return $"{{ Timestamp:{Timestamp.Ticks} \"Hash\":\"{Hash}\" }}";
        }
    }
}

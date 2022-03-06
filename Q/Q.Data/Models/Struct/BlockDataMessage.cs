using System;
using System.Collections.Generic;
using System.Text;
using Q.Data.Common;

namespace Q.Data.Models.Struct
{
    public class BlockDataMessage : BlockData
    {
        public string PublicKey { get; set; }
        public string Data { get; set; }
        override public string Hash
        {
            get
            {
                string state = $"{Timestamp.Ticks}-{PublicKey}-{Data}";
                return Crypto.ComputeHash(state);
            }
        }

        override public string ToString()
        {
            return $"{{ Timestamp:{Timestamp.Ticks}, \"Hash\":\"{Hash}\", \"PublicKey\":\"{PublicKey}\", \"Data\":\"{Data}\"}}";
        }
    }
}

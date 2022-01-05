using System;
using System.Collections.Generic;
using System.Text;
using Q.Data.Common;

namespace Q.Data.Models.Struct
{
    public class BlockDataRegistration : BlockData
    {
        public string Alias;
        public string PublicKey;
        override public string Hash
        {
            get
            {
                string state = $"{Timestamp.Ticks}-{Alias}-{PublicKey}";
                return Crypto.ComputeHash(state);
            }
        }

        override public string ToString()
        {
            return $"{{ \"Timestamp\":{Timestamp.Ticks}, \"Hash\":\"{Hash}\", \"Alias\":\"{Alias}\", \"PublicKey\":\"{PublicKey}\", \"Signature\":\"{Signature}\" }}";
        }
    }
}

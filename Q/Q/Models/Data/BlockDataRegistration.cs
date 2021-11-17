using System;
using System.Collections.Generic;
using System.Text;
using Q.Common;

namespace Q.Models
{
    public class BlockDataRegistration : BlockDataBase
    {
        public string Alias;
        public string PublicKey;
        override public string Hash
        {
            get
            {
                string state = $"{Timestamp.Ticks}-{Alias}-{PublicKey}";
                return Utils.ComputeHash(state);
            }
        }

        override public string ToString()
        {
            return $"{{ \"Timestamp\":{Timestamp.Ticks} \"Hash\":\"{Hash}\" \"Alias\":\"{Alias}\" \"PublicKey\":\"{PublicKey}\" }}";
        }
    }
}

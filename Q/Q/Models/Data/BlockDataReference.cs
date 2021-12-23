using System;
using System.Collections.Generic;
using System.Text;
using Q.Common;

namespace Q.Models.Data
{
    public class BlockDataReference : BlockDataBase
    {
        public string From;
        public string To;
        public string Description;
        public string DataHash;
        override public string Hash
        {
            get
            {
                string state = $"{Timestamp.Ticks}-{From}-{To}-{DataHash}-{Description}";
                return Crypto.ComputeHash(state);
            }
        }

        override public string ToString()
        {
            return $"{{ Timestamp:{Timestamp.Ticks}, \"Hash\":\"{Hash}\", \"From\":\"{From}\", \"To\":\"{To}\", \"DataHash\":\"{DataHash}\", \"Description\":\"{Description}\" }}";
        }
    }
}

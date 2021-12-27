using Q.Data.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Q.Data.Models.Struct
{
    public class TransactionInput
    {
        public string Address;

        public string Hash
        {
            get
            {
                string state = $"{Address}";
                return Crypto.ComputeHash(state);
            }
        }

        override public string ToString()
        {
            return $"{{ \"Hash\":\"{Hash}\", \"Address\":{Address} }}";
        }
    }
}

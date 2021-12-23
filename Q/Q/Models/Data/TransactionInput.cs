using Q.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Q.Models.Data
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

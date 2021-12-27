using Q.Data.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Q.Data.Models.Struct
{
    public class TransactionOutput
    {
        public string Address;
        public float Amount;
        public string Hash
        {
            get
            {
                string state = $"{Address}-{Amount}";
                return Crypto.ComputeHash(state);
            }
        }

        override public string ToString()
        {
            return $"{{ \"Address\":{Address}, \"Amount\":{Amount} }}";
        }
    }
}

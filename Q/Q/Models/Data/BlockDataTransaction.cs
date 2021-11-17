using System;
using System.Collections.Generic;
using System.Text;
using Q.Common;

namespace Q.Models
{
    public class BlockDataTransaction : BlockDataBase
    {
        public float Amount;
        public string From;
        public string To;
        override public string Hash
        {
            get
            {
                string state = $"{Timestamp.Ticks}-{Amount}-{From}-{To}";
                return Utils.ComputeHash(state);
            }
        }

        override public string ToString()
        {
            return $"{{ \"Timestamp\":{Timestamp.Ticks} \"Hash\":\"{Hash}\" \"Amount\":{Amount} \"From\":\"{From}\" \"To\":\"{To}\" }}";
        }
    }
}

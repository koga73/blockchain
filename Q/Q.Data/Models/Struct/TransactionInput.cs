

namespace Q.Data.Models.Struct
{
    public class TransactionInput
    {
        public string TransactionHash;
        public int OutputIndex;

        override public string ToString()
        {
            return $"{{ \"TransactionHash\":\"{TransactionHash}\", \"OutputIndex\":{OutputIndex} }}";
        }
    }
}

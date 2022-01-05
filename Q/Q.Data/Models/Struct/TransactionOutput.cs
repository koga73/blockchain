
namespace Q.Data.Models.Struct
{
    public class TransactionOutput
    {
        public string Address;
        public float Amount;

        override public string ToString()
        {
            return $"{{ \"Address\":{Address}, \"Amount\":{Amount} }}";
        }
    }
}

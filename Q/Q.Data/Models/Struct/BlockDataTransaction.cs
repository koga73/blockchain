using Q.Data.Common;

namespace Q.Data.Models.Struct
{
    public class BlockDataTransaction : BlockData
    {
        public List<TransactionInput> TxIn = new List<TransactionInput>();
        public List<TransactionOutput> TxOut = new List<TransactionOutput>();
        override public string Hash
        {
            get
            {
                string state = $"{Timestamp.Ticks}{TxIn.Aggregate("", (acc, x) => acc + "-" + x.TransactionHash + "-" + x.OutputIndex)}{TxOut.Aggregate("", (acc, x) => acc + "-" + x.Address + "-" + x.Amount)}";
                return Crypto.ComputeHash(state);
            }
        }

        override public string ToString()
        {
            return $"{{ \"Timestamp\":{Timestamp.Ticks}, \"Hash\":\"{Hash}\", \"TxIn\":[{TxIn.Aggregate("", (acc, x) => acc + x.ToString() + ",")}], \"TxOut\":[{TxOut.Aggregate("", (acc, x) => acc + x.ToString() + ",")}] }}";
        }
    }
}

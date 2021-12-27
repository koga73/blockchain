using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Q.Data.Common;

namespace Q.Data.Models.Struct
{
    public class BlockDataTransaction : BlockDataBase
    {
        public List<TransactionInput> TxIn = new List<TransactionInput>();
        public List<TransactionOutput> TxOut = new List<TransactionOutput>();
        override public string Hash
        {
            get
            {
                //TODO: ToString each TxIn / TxOut
                string state = $"{Timestamp.Ticks}{TxIn.Aggregate("", (acc, x) => acc + "-" + x.Hash)}{TxOut.Aggregate("", (acc, x) => acc + "-" + x.Hash)}";
                return Crypto.ComputeHash(state);
            }
        }

        override public string ToString()
        {
            return $"{{ \"Timestamp\":{Timestamp.Ticks}, \"Hash\":\"{Hash}\", \"TxIn\":[{TxIn.Aggregate("", (acc, x) => acc + x.ToString() + ",")}], \"TxOut\":[{TxOut.Aggregate("", (acc, x) => acc + x.ToString() + ",")}] }}";
        }
    }
}

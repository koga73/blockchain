using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.DB
{
    public class TransactionRepository
    {
        public static void Add(Data.Models.Struct.BlockDataTransaction transaction, int dataIndex, string blockHash)
        {
            using (Context db = new Context())
            {
                //Add transaction
                db.Add(new DBO.Transaction()
                {
                    Hash = transaction.Hash,
                    Timestamp = transaction.Timestamp,
                    DataIndex = dataIndex,
                    BlockHash = blockHash
                });

                //Add inputs
                foreach (Data.Models.Struct.TransactionInput input in transaction.TxIn)
                {
                    db.Add(new DBO.Struct.TransactionInput()
                    {
                        OfTransaction = transaction.Hash,
                        TransactionHash = input.TransactionHash,
                        OutputIndex = input.OutputIndex
                    });
                }

                //Add outputs
                foreach (Data.Models.Struct.TransactionOutput output in transaction.TxOut)
                {
                    db.Add(new DBO.Struct.TransactionOutput()
                    {
                        OfTransaction = transaction.Hash,
                        Address = output.Address,
                        Amount = output.Amount
                    });
                }

                db.SaveChanges();
            }
        }

        public static void Clear()
        {
            using (Context db = new Context())
            {
                db.Transactions.RemoveRange(from row in db.Transactions select row);
                db.SaveChanges();
            }
        }
    }
}

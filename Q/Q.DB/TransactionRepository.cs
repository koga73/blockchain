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
                    BlockHash = blockHash,
                    Signature = transaction.Signature
                });

                //Add inputs
                for (int i = 0; i < transaction.TxIn.Count; i++)
                {
                    Data.Models.Struct.TransactionInput input = transaction.TxIn[i];
                    db.Add(new DBO.Struct.TransactionInput()
                    {
                        OfTransaction = transaction.Hash,
                        TransactionHash = input.TransactionHash,
                        OutputIndex = input.OutputIndex,
                        Index = i
                    });
                }

                //Add outputs
                for (int i = 0; i < transaction.TxOut.Count; i++)
                {
                    Data.Models.Struct.TransactionOutput output = transaction.TxOut[i];
                    db.Add(new DBO.Struct.TransactionOutput()
                    {
                        OfTransaction = transaction.Hash,
                        Address = output.Address,
                        Amount = output.Amount,
                        Index = i
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

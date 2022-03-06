using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Q.Data.Models.Struct;

namespace Q.DB
{
    public class MessageRepository
    {
        public static void Add(BlockDataMessage message, int dataIndex, string blockHash)
        {
            using (Context db = new Context())
            {
                db.Add(new DBO.Message()
                {
                    PublicKey = message.PublicKey,
                    Data = message.Data,
                    Timestamp = message.Timestamp,
                    DataIndex = dataIndex,
                    BlockHash = blockHash,
                    Hash = message.Hash,
                    Signature = message.Signature
                });
                db.SaveChanges();
            }
        }

        public static void Clear()
        {
            using (Context db = new Context())
            {
                db.Messages.RemoveRange(from row in db.Messages select row);
                db.SaveChanges();
            }
        }
    }
}

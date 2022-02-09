using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Q.Data.Models.Struct;

namespace Q.DB
{
    public class UserRepository
    {
        public static void Add(BlockDataRegistration user, int dataIndex, string blockHash)
        {
            using (Context db = new Context())
            {
                db.Add(new DBO.User()
                {
                    Alias = user.Alias.ToLower(),
                    PublicKey = user.PublicKey,
                    Timestamp = user.Timestamp,
                    DataIndex = dataIndex,
                    BlockHash = blockHash,
                    Hash = user.Hash,
                    Signature = user.Signature
                });
                db.SaveChanges();
            }
        }

        public static BlockDataRegistration GetUserByAlias(string alias)
        {
            alias = alias.ToLower();

            using (Context db = new Context())
            {
                DBO.User user = (from row in db.Users where row.Alias == alias select row).FirstOrDefault();
                if (user != null)
                {
                    return new BlockDataRegistration()
                    {
                        Alias = user.Alias,
                        PublicKey = user.PublicKey
                    };
                }
                return null;
            }
        }

        public static BlockDataRegistration GetUserByKey(string publicKey)
        {
            using (Context db = new Context())
            {
                DBO.User user = (from row in db.Users where row.PublicKey == publicKey select row).FirstOrDefault();
                if (user != null)
                {
                    return new BlockDataRegistration()
                    {
                        Alias = user.Alias,
                        PublicKey = user.PublicKey
                    };
                }
                return null;
            }
        }

        public static void Clear()
        {
            using (Context db = new Context())
            {
                db.Users.RemoveRange(from row in db.Users select row);
                db.SaveChanges();
            }
        }
    }
}

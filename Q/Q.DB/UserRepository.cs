using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.DB
{
    public class UserRepository
    {
        public static void Add(Data.Models.Struct.BlockDataRegistration user, int dataIndex, string blockHash)
        {
            using (Context db = new Context())
            {
                db.Add(new DBO.User()
                {
                    Alias = user.Alias.ToLower(),
                    PublicKey = user.PublicKey,
                    Timestamp = user.Timestamp,
                    DataIndex = dataIndex,
                    BlockHash = blockHash
                });
                db.SaveChanges();
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

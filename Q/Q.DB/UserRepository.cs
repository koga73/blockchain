using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.DB
{
    public class UserRepository
    {
        public static void Add(Data.Models.User user, Data.Models.Block blockReference)
        {
            using (Context db = new Context())
            {
                db.Add(new DBO.User()
                {
                    Alias = user.Alias.ToLower(),
                    PublicKey = user.PublicKeyString,
                    BlockHash = blockReference.Hash
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.DB
{
    public class UserRepository
    {
        public static void Add(Data.Models.User user)
        {
            using (Context db = new Context())
            {
                db.Add(new DBO.User()
                {
                    Alias = user.Alias,
                    PublicKey = user.PublicKeyString
                });
                db.SaveChanges();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.Wallet
{
    public class WalletRepository
    {
        public static void Add(string publicKey, string? alias = null, string? name = null)
        {
            using (Context db = new Context())
            {
                db.Add(new DBO.Wallet()
                {
                    PublicKey = publicKey,
                    Alias = !String.IsNullOrEmpty(alias) ? alias : null,
                    Name = !String.IsNullOrEmpty(name) ? name : !String.IsNullOrEmpty(alias) ? alias : "default",
                    Balance = 0,
                });
                db.SaveChanges();
            }
        }

        public static void Clear()
        {
            using (Context db = new Context())
            {
                db.Wallets.RemoveRange(from row in db.Wallets select row);
                db.SaveChanges();
            }
        }
    }
}

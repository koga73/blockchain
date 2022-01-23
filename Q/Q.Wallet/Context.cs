using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Q.Wallet
{
    internal class Context : DbContext
    {
        public string DbPath { get; set; }

        public DbSet<DBO.Wallet> Wallets { get; set; }

        public Context()
        {
            DbPath = "./Q-wallet.db";
        }

        static Context()
        {
            using (Context ctx = new Context())
            {
                ctx.Database.EnsureCreated();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={DbPath}");
        }
    }
}

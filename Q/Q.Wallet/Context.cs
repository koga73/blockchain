using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.IO;

using Q.Data.Common;

namespace Q.Wallet
{
    internal class Context : DbContext
    {
        const string DB_FILE_NAME = "Q-wallet.db";

        public string DbPath { get; set; }

        public DbSet<DBO.Wallet> Wallets { get; set; }

        public Context()
        {
            DbPath = Path.Join(Paths.ApplicationPath, DB_FILE_NAME);
        }

        static Context()
        {
            using (Context ctx = new Context())
            {
                try
                {
                    ctx.Database.EnsureCreated();
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={DbPath}");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Q.DB
{
    internal class Context : DbContext
    {
        public string DbPath { get; set; }

        public DbSet<DBO.User> Users { get; set; }
        public DbSet<DBO.Block> Blocks { get; set; }
        public DbSet<DBO.Transaction> Transactions { get; set; }
        public DbSet<DBO.Struct.TransactionInput> TransactionInputs { get; set; }
        public DbSet<DBO.Struct.TransactionOutput> TransactionOutputs { get; set; }

        public Context()
        {
            DbPath = "./Q.db";
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

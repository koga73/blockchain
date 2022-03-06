using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.IO;

using Q.Data.Common;

namespace Q.DB
{
    internal class Context : DbContext
    {
        const string DB_FILE_NAME = "Q.db";

        public string DbPath { get; set; }

        public DbSet<DBO.Block> Blocks { get; set; }
        public DbSet<DBO.User> Users { get; set; }
        public DbSet<DBO.Transaction> Transactions { get; set; }
        public DbSet<DBO.Struct.TransactionInput> TransactionInputs { get; set; }
        public DbSet<DBO.Struct.TransactionOutput> TransactionOutputs { get; set; }
        public DbSet<DBO.Message> Messages { get; set; }

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
                } catch (Exception ex)
                {
                    Logger.Info(ex.Message);
                    Environment.Exit(1);
                }
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={DbPath}");
        }
    }
}

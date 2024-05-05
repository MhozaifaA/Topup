using Meteors.AspNetCore.Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topup.Infrastructure.Models;

namespace Topup.Infrastructure.Databases.SqlServer
{
    //quick simple db api not required authorize 
    public class BalanceDbContext : DbContext
    {
        public BalanceDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            //by call this I added indexing of DateCreating for per on order by.
            // query filter on DateCreated
            base.OnModelCreating(builder);

            builder.Entity<UserBalance>().HasIndex(x => x.UserNO)
             .IsUnique();


            builder.Entity<TransactionBalance>().HasIndex(x => x.TransactionReference)
                .IsUnique();

            //builder.Entity<TransactionBalance>().HasIndex(x => x.SourceReference)
            //.HasFilter("[SourceReference] IS NOT NULL");

        }

        public DbSet<UserBalance> Users { get; set; }
        public DbSet<TransactionBalance> Transactions { get; set; }

    }
}

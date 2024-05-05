using Meteors.AspNetCore.Infrastructure.EntityFramework.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Topup.Infrastructure.Models;

namespace Topup.Infrastructure.Databases.SqlServer
{
    public class TopupDbContext : MrIdentityDbContext<Account, Guid>
    {
        public TopupDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            ///TODO Conventions to restrected

            //by call this I added indexing of DateCreating for per on order by.
            // query filter on DateCreated
            base.OnModelCreating(builder);

            //change the shcma and Name for Identity Tables -- Default came as dbo.AspNet...  which is I dont like it
            builder.Entity<Account>().ToTable("Users", "app").
                HasIndex(x=>x.UserNO).IsUnique();

            builder.Entity<IdentityRole<Guid>>().ToTable("Roles", "app");
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims", "app");
            builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles", "app");
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins", "app");
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims", "app");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens", "app");


            builder.Entity<Beneficiary>().HasIndex(x => 
            new { x.UserId , x.Name })
              .IsUnique();

            builder.Entity<Transaction>().HasIndex(x=>x.TransactionReference)
                .IsUnique();

            builder.Entity<Transaction>().HasIndex(x => x.SourceReference)
            .HasFilter("[SourceReference] IS NOT NULL");

            builder.Entity<Transaction>()
                  .HasOne(t => t.Beneficiary)
                  .WithMany(u => u.Transactions)
                  .HasForeignKey(t => t.BeneficiaryId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Transaction>()
                 .HasOne(t => t.User)
                 .WithMany(u => u.Transactions)
                 .HasForeignKey(t => t.UserId)
                 .OnDelete(DeleteBehavior.Restrict);

            builder.ModelCreatingSeed();
        }


       

        public DbSet<Beneficiary> Beneficiaries { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<UserOption> UserOptions { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionStatusHistory> TransactionStatusHistory { get; set; }


    }
}

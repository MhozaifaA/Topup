using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topup.Infrastructure.Models;

namespace Topup.Infrastructure.Databases.SqlServer
{
    public static class TopupDataSeed
    {
        /// <summary>
        /// <see cref="TopupDataSeed"/>
        /// </summary>
        /// <param name="builder"></param>
        public static void ModelCreatingSeed(this ModelBuilder builder)
        {

            // (AED 5, AED 10, AED 20, AED 30, AED 50, AED 75, AED 100)
            int[] ops = [5, 10, 20, 30, 50, 75, 100];
            foreach (var i in ops)
                builder.Entity<Option>().HasData(new Option {
                    Id = Guid.NewGuid(),
                    Name = $"AED {i}" });
        }

        public static async Task<TopupDbContext> AccountsSeedAsync(this TopupDbContext context,
        IServiceProvider provider) //local scoped
        {
            var userManager = provider.GetRequiredService<UserManager<Account>>();
            if (userManager is null) //not registered
                return context;

            List<Account> accounts = [new Account() {
                Id = new Guid("{4FA7A59C-88AF-45EE-B7DB-D7D814C59DEB}"),//const  id for test
                Name = "Huzaifa Aseel",
                UserName = "huzaifa",
                Email = "h@h.h",
                UserNO = "012345" //#012345
            },

            new Account() {
                Id = new Guid("{E3CE42A6-3C24-4236-B3D3-B4F0C90FC0EA}"),//const  id for test
                Name = "Tester",
                UserName = "tester",
                Email = "t@t.t",
                UserNO = "543210" //#543210
            },

            ];

            foreach (var account in accounts)
            {
                await CreateUser(userManager, account);
            }


            return context;
        }

        private static async Task CreateUser(UserManager<Account> userManager, Account account)
        {

            var one = await userManager.FindByNameAsync(account.UserName);
            if (one is null)
                await userManager.CreateAsync(account, account.UserName); //password  = username
        }
    }
}

using Meteors.AspNetCore.Common.AuxiliaryImplemental.Classes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Topup.Infrastructure.Models;

namespace Topup.Infrastructure.Databases.SqlServer
{
    public static class BalanceDataSeed
    {

        public static async Task<BalanceDbContext> UsersSeedAsync(this BalanceDbContext context,
       IServiceProvider provider)
        {

            var d = DateTimeOffset.Now.AddMinutes(-5);
            List<UserBalance> users = [new UserBalance() {

                AvailableBalance = 600,
                CurrentBalance = 600, //default
                UserNO = "012345", //#012345   huzaifa
                TransactionBalances = [
                new TransactionBalance() {
                    Date = d.AddMinutes(2),
                    Amount = + 800,
                    Status = TransactionStatus.Completed,
                    TransactionReference = Generator.RandomString(10),
                },
                new () {
                    Date = d.AddMinutes(4),
                    Amount = - 200,
                    Status = TransactionStatus.Completed,
                    TransactionReference = Generator.RandomString(10),
                },

                ]
            },

            new UserBalance() {

                CurrentBalance = 0, //default
                AvailableBalance = 0, //default
                UserNO = "543210" //#543210   tester
            },
            ];

            foreach (var user in users)
            {
                var isexist = await context.Users.AnyAsync(b => b.UserNO == user.UserNO);
                if (!isexist)
                    await context.AddAsync(user);
            }
          
            await context.SaveChangesAsync();

            return context;
        }
    }
}

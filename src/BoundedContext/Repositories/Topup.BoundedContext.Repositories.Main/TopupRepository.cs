using Meteors;
using Meteors.AspNetCore.Common.AuxiliaryImplemental.Classes;
using Meteors.AspNetCore.Service.BoundedContext;
using Meteors.OperationContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topup.BoundedContext.DataTransferObjects;
using Topup.Core;
using Topup.Domain.Implementation;
using Topup.Infrastructure.Databases.SqlServer;
using Topup.Infrastructure.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Topup.BoundedContext.Repositories.Main
{
    [AutoService]
    public class TopupRepository : MrRepository<TopupDbContext, Guid, Transaction>, ITopupRepository
    {
        private readonly AppRoleOptions appRoles;

        public TopupRepository(TopupDbContext context, IOptionsMonitor<AppRoleOptions> appRoles) : base(context)
        {
            this.appRoles = appRoles.CurrentValue;
        }

        /*
        1- check roles (status failed not enter)
        2- insert pending transaction -  call debit api
        3- update srouce transaction from debit api and check if proccing/pending , if not update transaction topup to failed
        4- api prducer will be consumer in topup if proccing will consume in topup .. check ..
        5- then call confirm debit and update trancation topup to complated


        Note: to check Concurrency  roles lock by db column update to release or by queue broker,
        for quick i will solve by adding reatelimit on api call per user
         */

        public async Task<OperationResult<List<TransactionDto>>> GetByUserMonth()
        {

            Guid? userId = Context.CurrentUserId;

            if (!userId.HasValue)//|| userId != dto.UserId
                return ("", Statuses.Forbidden);


            var user = await _query<Account>().FirstOrDefaultAsync(x => x.Id == userId);
            if (user is null)
                return ("An account has been deleted", Statuses.Forbidden);


            DateTimeOffset currentDate = DateTimeOffset.Now;

            int currentYear = currentDate.Year;
            int currentMonth = currentDate.Month;


            var res = await Query.Where(x => x.UserId == userId && x.Status != TransactionStatus.Failed &&
            x.Date.Year == currentYear && x.Date.Month == currentMonth)
                .Select(TransactionDto.Selector).ToListAsync();
            return (res, null);
        }


        public async Task<OperationResult<TransactionDto>> Topup(TopupDto dto)
        {

            Guid? userId = Context.CurrentUserId;

            if (!userId.HasValue)//|| userId != dto.UserId
                return ("", Statuses.Forbidden);


            var user = await _query<Account>().FirstOrDefaultAsync(x => x.Id == userId 
            && x.Beneficiaries.Any(b=>b.Id == dto.BeneficiaryId));
            if (user is null)
                return ("An account has been deleted or Beneficiary not exist", Statuses.Forbidden);
            //check 

           

            DateTimeOffset currentDate = DateTimeOffset.Now;

            int currentYear = currentDate.Year;
            int currentMonth = currentDate.Month;

            //calculate in database .. need more conecte to db but keep clear from retrive data in local even if local fast to calculate

            //var s = await Query.Where(x=>x.UserId == userId &&  x.Status != TransactionStatus.Failed &&
            //x.Date.Year == currentYear && x.Date.Month == currentMonth).SumAsync(x=>x.Amount);
            //number of records  = 3000/5(less topup)   600 records
            var transactions = await Query.Where(x => x.UserId == userId && x.Status != TransactionStatus.Failed &&
            x.Date.Year == currentYear && x.Date.Month == currentMonth)
                .Select(TransactionDto.Selector).ToListAsync();

            /*
              The user can top up multiple beneficiaries but is limited to a total of AED 3,000 per month 
                     for all beneficiaries. Note:  (is not 30Days) so  calendar month , and withour fee 3,000
          */
            var totalPerMonth = transactions.Sum(x => x.Amount);
            if (totalPerMonth + dto.Amount > appRoles.TotalUserPerMonth)
                return ($"limited to a total of AED {appRoles.TotalUserPerMonth} per month", Statuses.Failed);

            /*
             **If a user is not verified, they can top up a maximum of AED 1,000 per calendar month per 
                beneficiary for security reasons.

                **If a user is verified, they can top up a maximum of AED 500 per calendar month per 
                beneficiary.

             */
            var allBeneficiaryTran = transactions.Where(x => x.BeneficiaryId == dto.BeneficiaryId).Sum(x => x.Amount);

            var limitedbe = user.HasVerified ? appRoles.VerifiedPerMonthBeneficiary : appRoles.NotVerifiedPerMonthBeneficiary;

            if (allBeneficiaryTran + dto.Amount > limitedbe)
                return ($"can top up a maximum of AED {limitedbe} per calendar month per beneficiary", Statuses.Failed);



            //inint transaction in db as pending
            var datanow = DateTimeOffset.Now;
            var setTrans = new Transaction()
            {
                BeneficiaryId = dto.BeneficiaryId,
                UserId = user.Id,

                Date = DateTimeOffset.Now,
                TotalAmount = dto.Amount + appRoles.FeePerTransaction,
                Amount = dto.Amount,
                Fee = appRoles.FeePerTransaction,

                TransactionReference = Generator.RandomString(10),
                Status = TransactionStatus.Pending,
                TransactionStatusHistories = [   //add track status
                    new()
                    {
                        Status = TransactionStatus.Pending,
                        Date = datanow
                    }
                ]
            };

            await Context.AddAsync(setTrans);

            await Context.SaveChangesAsync();

            var result = TransactionDto.Selector.Compile()(setTrans);

            return _Operation.SetSuccess(result);
        }


        public async Task<OperationResult<TransactionDto>> UpdateTransStatus(UpdateTransStatusDto dto)
        {

            var one = await TrackingQuery.Include(x => x.TransactionStatusHistories)
                .FirstOrDefaultAsync(x => x.TransactionReference == dto.TransactionReference);

            if (one is null)
                return ("Transaction not exist", Statuses.Failed);

            one.SourceReference = dto.SourceReference;

            if (one.Status != dto.Status)
            {
                one.Status = dto.Status;
                one.TransactionStatusHistories.Add(
             new TransactionStatusHistory()
             {
                 Date = DateTimeOffset.Now,
                 Status = dto.Status,
             }
             );
            }

            await Context.SaveChangesAsync();


            var result = TransactionDto.Selector.Compile()(one);

            return _Operation.SetSuccess(result);
        }

        //search source and updated
        public async Task<OperationResult<TransactionDto>> UpdateFromSource(UpdateTransStatusDto dto)
        {

            var one = await TrackingQuery.Include(x => x.TransactionStatusHistories)
                .FirstOrDefaultAsync(x => x.SourceReference == dto.SourceReference);

            if (one is null)
                return ("Transaction not exist", Statuses.Failed);

            if (one.Status != dto.Status)
            {
                one.Status = dto.Status;
                one.TransactionStatusHistories.Add(
             new TransactionStatusHistory()
             {
                 Date = DateTimeOffset.Now,
                 Status = dto.Status,
             }
             );
            }

            await Context.SaveChangesAsync();


            var result = TransactionDto.Selector.Compile()(one);

            return _Operation.SetSuccess(result);
        }


    }
}

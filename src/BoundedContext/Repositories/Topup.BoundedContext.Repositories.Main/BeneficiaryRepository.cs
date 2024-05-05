using Meteors;
using Meteors.AspNetCore.Service.BoundedContext;
using Meteors.AspNetCore.Service.BoundedContext.General;
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

namespace Topup.BoundedContext.Repositories.Main
{
    [AutoService]
    public class BeneficiaryRepository : MrRepositoryGeneral<TopupDbContext, Guid, Beneficiary, BeneficiaryDto>, IBeneficiaryRepository
    {
        private readonly AppRoleOptions appRoles;

        public BeneficiaryRepository(TopupDbContext context, IOptionsMonitor<AppRoleOptions> appRoles) : base(context)
        {
            this.appRoles = appRoles.CurrentValue;
        }

        /*
          Note: to check Concurrency  limit row lock by db column update to release or by queue,
        for quick i will solve by adding ratelimit on api call per user
         */
        public override async Task<OperationResult<BeneficiaryDto>> AddAsync(BeneficiaryDto dto)
        {
            Guid? userId = Context.CurrentUserId;

            if (!userId.HasValue || userId != dto.UserId)
                return ("", Statuses.Forbidden);


            using var transaction = await Context.Database.BeginTransactionAsync();
            try
            {
                /*
                 same tranaction block read / get count of Beneficiaries then check role
                commit 
                 */
                var user = await _query<Account>().FirstOrDefaultAsync(x => x.Id == userId);
                if (user is null)
                    return ("An account has been deleted", Statuses.Forbidden);

                var beneficiaries = await Query.Where(x => x.UserId == userId).ToListAsync();

                if (beneficiaries.Count >= appRoles.MaximumBeneficiaries)
                    return ("Cannot add more than 5 Beneficiaries", Statuses.Forbidden);

                //already constraint 
                if (beneficiaries.Any(x => string.Equals(x.Name, dto.Name,
                    StringComparison.OrdinalIgnoreCase)))
                    return ($"{nameof(BeneficiaryInfoDto.Nickname)} already exist", Statuses.Forbidden);


                var operation = await base.AddAsync(dto);

                await transaction.CommitAsync();

                return operation;//can be throw Cannot insert duplicate key row in object 'dbo.Beneficiaries' with unique index 'IX_Beneficiaries_UserId_Name'

            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return (e.Message, Statuses.Failed); //not user friendly
            }

        }

        public async Task<OperationResult<IEnumerable<BeneficiaryInfoDto>>> GetUserBeneficiaries()
        {
            Guid? userId = Context.CurrentUserId;
            if (userId is null)
                return ("userId missing", Statuses.Forbidden);

            var res = await Query.
                Include(x => x.Transactions).Where(x => x.UserId == userId).Select(x => new BeneficiaryInfoDto()
                {
                    Id = x.Id,
                    Nickname = x.Name,
                    PhoneNumber = x.PhoneNumber,
                    TotalTransfer = x.Transactions.
                Where(x => x.Status != TransactionStatus.Failed).Sum(x => x.Amount) //if not comlplated mean can later refound

                }).ToListAsync();

            return _Operation.SetSuccess<IEnumerable<BeneficiaryInfoDto>>(res);
        }
    }
}

using Integration.Balance.HttpService;
using Meteors;
using Meteors.AspNetCore.MVC.Resolver;
using Meteors.AspNetCore.Service.BoundedContext;
using Meteors.OperationContext;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Topup.BoundedContext.DataTransferObjects;
using Topup.Core;
using Topup.Domain.Implementation;
using Topup.Infrastructure.Models;

namespace Topup.BoundedContext.Services
{
    [AutoService]
    public class UserService : IUserService
    {
        private readonly IAccountRepository accountRepository;
        private readonly IOptionRepository optionRepository;
        private readonly IBeneficiaryRepository beneficiaryRepository;
        private readonly ITopupRepository topupRepository;
        private readonly AppRoleOptions appRoles;
        private readonly IHttpResolverService httpResolver;
        private readonly BalanceHttpService balanceHttpService;

        public UserService(IAccountRepository accountRepository,
            IOptionRepository optionRepository, IBeneficiaryRepository beneficiaryRepository,
            ITopupRepository topupRepository, IOptionsMonitor<AppRoleOptions> appRoles,
            IHttpResolverService httpResolver, BalanceHttpService balanceHttpService)
        {
            this.accountRepository = accountRepository;
            this.optionRepository = optionRepository;
            this.beneficiaryRepository = beneficiaryRepository;
            this.topupRepository = topupRepository;
            this.appRoles = appRoles.CurrentValue;
            this.httpResolver = httpResolver;
            this.balanceHttpService = balanceHttpService;
        }


        public async Task<OperationResult<AccessTokenDto>> Login(LoginDto dto)
        {
            return await accountRepository.Login(dto);
        }

        /// <summary>
        /// <para>Auth</para>  simple one method
        /// Oprions, Balance, Beneficiaries
        /// </summary>
        /// <returns></returns>
        public async Task<OperationResult<UserDetailsDto>> Details()
        {
            var userNo = httpResolver.GetCurrentUserContext()?.FindFirst(ClaimTypes.SerialNumber)?.Value;
            if (userNo is null)
                return ("userNo not matched", Statuses.Forbidden);

            //for api can When All task
            return (await optionRepository.GetUserOptions()).Collect(
                await beneficiaryRepository.GetUserBeneficiaries(),
                await balanceHttpService.CurrentBalance(userNo)).Into((op, be, balance) => new UserDetailsDto
                {
                    Balance = balance?.Data?.CurrentBalance ?? 0,
                    Beneficiaries = be?.Data ?? [],
                    Options = op?.Data ?? []
                });
        }

        public async Task<OperationResult<BeneficiaryDto>> AddBeneficiary(BeneficiaryDto dto)
        {
            dto.UserId = httpResolver.GetCurrentUserId<Guid>() ?? default;
            return await beneficiaryRepository.AddAsync(dto);
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
        public async Task<OperationResult<TransactionDto>> Topup(TopupDto dto)
        {
            var userNo = httpResolver.GetCurrentUserContext()?.FindFirst(ClaimTypes.SerialNumber)?.Value;
            if (userNo is null)
                return ("userNo not matched", Statuses.Forbidden);

            //extra check 
            var balanceres = await balanceHttpService.CurrentBalance(userNo);

            if (!balanceres.IsSuccess)
                return (balanceres.Message, Statuses.Failed);

            if (balanceres.Data!.CurrentBalance < dto.Amount + appRoles.FeePerTransaction)
                return ("not enough Balance", Statuses.Failed);


            //The user's balance should be debited first before the top-up transaction is attempted.

            //set as pending  for tracking tranaction even if debit will failed later 
            var res = await topupRepository.Topup(dto);

            if (!res.IsSuccess)
                return res;

            //call debit
            ///TODO message error response and more effecsint response message with details information

            var response = await balanceHttpService.Debit(new()
            {
                Amount = res.Data!.TotalAmount,
                UserNO = userNo
            });

            if (!response.IsSuccess)
            {
                //set failed topup transaction ,, trn should recive ..  for now this enogh for example
                await topupRepository.UpdateTransStatus(new UpdateTransStatusDto()
                {
                    TransactionReference = res.Data!.TransactionReference,
                    Status = TransactionStatus.Failed
                });

                return (response.Message, Statuses.Failed);
            }

            //update transaction to proccing and source 

            var updateres = await topupRepository.UpdateTransStatus(new UpdateTransStatusDto()
            {
                TransactionReference = res.Data!.TransactionReference,
                Status = TransactionStatus.Processing,  //wait to complated event
                SourceReference = response.Data!.TransactionReference
            });

            return updateres;
        }

    }
}

using Meteors;
using Meteors.AspNetCore.Service.BoundedContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topup.BoundedContext.DataTransferObjects.Balance;

namespace Balance.BoundedContext.Repositories
{
    public interface IBalanceRepository : IMrRepository
    {
        Task<OperationResult<TransactionBalanceDto>> ConfirmDebitTransaction(ConfirmDebitTransactionDto dto);
        Task<OperationResult<TransactionBalanceDto>> Credit(CreditDto dto);
        Task<OperationResult<CurrentBalanceDto>> CurrentBalance(string userNO);
        Task<OperationResult<TransactionBalanceDto>> Debit(DebitDto dto);
    }
}

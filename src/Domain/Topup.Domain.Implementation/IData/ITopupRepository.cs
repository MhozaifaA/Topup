using Meteors;
using Meteors.AspNetCore.Service.BoundedContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topup.BoundedContext.DataTransferObjects;

namespace Topup.Domain.Implementation
{
    public interface ITopupRepository : IMrRepository
    {
        Task<OperationResult<List<TransactionDto>>> GetByUserMonth();
        Task<OperationResult<TransactionDto>> Topup(TopupDto dto);
        Task<OperationResult<TransactionDto>> UpdateFromSource(UpdateTransStatusDto dto);
        Task<OperationResult<TransactionDto>> UpdateTransStatus(UpdateTransStatusDto dto);
    }
}

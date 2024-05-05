using Meteors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topup.BoundedContext.DataTransferObjects;

namespace Topup.Domain.Implementation
{
    public interface IUserService
    {
        Task<OperationResult<BeneficiaryDto>> AddBeneficiary(BeneficiaryDto dto);
        Task<OperationResult<UserDetailsDto>> Details();
        Task<OperationResult<AccessTokenDto>> Login(LoginDto dto);
        Task<OperationResult<TransactionDto>> Topup(TopupDto dto);
    }
}

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
    public interface IAccountRepository : IMrRepository
    {
        Task<OperationResult<AccessTokenDto>> Login(LoginDto dto);
    }
}

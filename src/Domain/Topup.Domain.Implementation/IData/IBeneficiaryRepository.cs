using Meteors;
using Meteors.AspNetCore.Service.BoundedContext;
using Meteors.AspNetCore.Service.BoundedContext.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topup.BoundedContext.DataTransferObjects;

namespace Topup.Domain.Implementation
{
    public interface IBeneficiaryRepository : IMrRepositoryGeneral<Guid, BeneficiaryDto>
    {
     
        Task<OperationResult<IEnumerable<BeneficiaryInfoDto>>> GetUserBeneficiaries();
    }
}

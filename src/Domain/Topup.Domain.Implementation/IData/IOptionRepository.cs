using Meteors;
using Meteors.AspNetCore.Service.BoundedContext.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topup.BoundedContext.DataTransferObjects;
using Topup.Infrastructure.Models;

namespace Topup.Domain.Implementation
{
    public interface IOptionRepository : IMrRepositoryGeneral<Guid, Option, OptionDto>
    {
        Task<OperationResult<IEnumerable<OptionDto>>> GetUserOptions();
    }
}

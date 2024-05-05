using MediatR;
using Meteors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topup.BoundedContext.DataTransferObjects;
using Topup.BoundedContext.Transfer.Actions.Queries;
using Topup.Domain.Implementation;

namespace Topup.Core.EventHandlers
{
    public class UserDetailsQueryHandler(IUserService service) : IRequestHandler<UserDetailsQuery, OperationResult<UserDetailsDto>>
    {
        public async Task<OperationResult<UserDetailsDto>> Handle(UserDetailsQuery request, CancellationToken cancellationToken)
        {
            return await service.Details();
        }
    }
}

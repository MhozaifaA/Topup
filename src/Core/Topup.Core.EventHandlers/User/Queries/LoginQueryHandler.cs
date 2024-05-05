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
    public class LoginQueryHandler(IUserService service) : IRequestHandler<LoginQuery, OperationResult<AccessTokenDto>>
    {
        public async Task<OperationResult<AccessTokenDto>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            return await service.Login(request);
        }
    }
}

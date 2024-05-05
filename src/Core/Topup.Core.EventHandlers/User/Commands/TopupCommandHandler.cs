using MediatR;
using Meteors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topup.BoundedContext.DataTransferObjects;
using Topup.BoundedContext.Transfer.Actions.Commands;
using Topup.Domain.Implementation;

namespace Topup.Core.EventHandlers.User.Commands
{
    internal class TopupCommandHandler(IUserService service) : IRequestHandler<TopupCommand, OperationResult<TransactionDto>>
    {
        public async Task<OperationResult<TransactionDto>> Handle(TopupCommand request, CancellationToken cancellationToken)
        {
            return await service.Topup(request);
        }
    }
}

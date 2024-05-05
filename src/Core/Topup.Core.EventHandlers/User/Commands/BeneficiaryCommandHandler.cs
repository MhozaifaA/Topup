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

namespace Topup.Core.EventHandlers
{
    public class BeneficiaryCommandHandler(IUserService service) : IRequestHandler<BeneficiaryCommand, OperationResult<BeneficiaryDto>>
    {
        public async Task<OperationResult<BeneficiaryDto>> Handle(BeneficiaryCommand request, CancellationToken cancellationToken)
        {
            return await service.AddBeneficiary(request);
        }
    }
}

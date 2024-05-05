using MediatR;
using Meteors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topup.BoundedContext.DataTransferObjects;

namespace Topup.BoundedContext.Transfer.Actions.Commands
{
    public class TopupCommand : TopupDto, IRequest<OperationResult<TransactionDto>> { }
    
}

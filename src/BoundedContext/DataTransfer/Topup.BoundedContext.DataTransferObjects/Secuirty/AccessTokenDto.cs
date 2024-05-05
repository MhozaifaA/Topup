using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topup.BoundedContext.DataTransferObjects
{
    public record AccessTokenDto(string Name, string UserNO, string? AccessToken);

}

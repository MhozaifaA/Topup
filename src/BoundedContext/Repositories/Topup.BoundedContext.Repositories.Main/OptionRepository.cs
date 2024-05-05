using Meteors;
using Meteors.AspNetCore.Service.BoundedContext.General;
using Meteors.OperationContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topup.BoundedContext.DataTransferObjects;
using Topup.Domain.Implementation;
using Topup.Infrastructure.Databases.SqlServer;
using Topup.Infrastructure.Models;

namespace Topup.BoundedContext.Repositories.Main
{
    [AutoService]
    public class OptionRepository : MrRepositoryGeneral<TopupDbContext, Guid, Option, OptionDto>, IOptionRepository
    {
        public OptionRepository(TopupDbContext context) : base(context) { }

        public async Task<OperationResult<IEnumerable<OptionDto>>> GetUserOptions()
        {
            Guid? userId = Context.CurrentUserId;
            if (userId is null)
                return ("userId missing",Statuses.Forbidden);

            var res = await _query<UserOption>().//include
                Where(x => x.UserId == userId).Select(x => x.Option).
                Select(OptionDto.Selector).ToListAsync();

            if (res.Count == 0)
                return await FetchAsync();

            return _Operation.SetSuccess<IEnumerable<OptionDto>>(res);

        }
    }
}

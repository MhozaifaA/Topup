using Meteors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Meteors.AspNetCore.MVC;
using Meteors.AspNetCore.MVC.Route;
using Topup.BoundedContext.DataTransferObjects;
using MediatR;
using Topup.BoundedContext.Transfer.Actions.Queries;

namespace Topup.Controllers.Security
{
    [ApiController]
    [Route($"api/security/{MrRoute.ControllerActionRoute}")]
    [Authorize]
    public class AccountController(IMediator mediator) : MrControllerBase
    {

        /// <summary>
        /// { username:huzaifa, password:huzaifa}
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<AccessTokenDto>> Login([FromBody] LoginDto dto)
        {
            return await mediator.Send(new LoginQuery()
            {
                UserName = dto.UserName,
                Password = dto.Password,
            }).ToJsonResultAsync();
        }

    }
}

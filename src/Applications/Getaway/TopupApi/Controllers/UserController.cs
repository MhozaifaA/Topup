using Meteors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Meteors.AspNetCore.MVC;
using Meteors.AspNetCore.MVC.Route;
using Topup.BoundedContext.DataTransferObjects;
using MediatR;
using Topup.BoundedContext.Transfer.Actions.Queries;
using Meteors.AspNetCore.MVC.Attributes;
using Topup.BoundedContext.Transfer.Actions.Commands;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.RateLimiting;

namespace Topup.Controllers.Security
{
    [ApiController]
    [DefaultRoute]
    [Authorize]
    public class UserController(IMediator mediator) : MrControllerBase
    {
        /// <summary>
        /// one api to featch all details...  in general shoudl be split (Balance, Options,Beneficiaries)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Details()
        {
            return await mediator.Send(new UserDetailsQuery()).ToJsonResultAsync();
        }

        [HttpPost]
        [EnableRateLimiting("ConcurrencyLimiter")]
        public async Task<IActionResult> AddBeneficiary([Required][FromBody] BeneficiaryDto dto)
        {
            return await mediator.Send(new BeneficiaryCommand()
            {
                Name = dto.Name,
                PhoneNumber = dto.PhoneNumber,
            }).ToJsonResultAsync();
        }

        [HttpPost]
        [EnableRateLimiting("ConcurrencyLimiter")]
        public async Task<IActionResult> Topup([Required][FromBody] TopupDto dto)
        {
            return await mediator.Send(new TopupCommand()
            {
                Amount = dto.Amount,
                BeneficiaryId = dto.BeneficiaryId
            }).ToJsonResultAsync();
        }
    }
}

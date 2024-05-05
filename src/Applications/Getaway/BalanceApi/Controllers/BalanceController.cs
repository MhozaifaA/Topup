using Balance.BoundedContext.Repositories;
using MassTransit;
using Meteors;
using Meteors.AspNetCore.MVC;
using Meteors.AspNetCore.MVC.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Topup.BoundedContext.DataTransferObjects.Balance;

namespace BalanceApi.Controllers
{
    [ApiController]
    [DefaultRoute]
    public class BalanceController : MrControllerBase<Guid, IBalanceRepository>
    {
        private readonly IPublishEndpoint publish;

        public BalanceController(IBalanceRepository repository, IPublishEndpoint publish) : base(repository)
        {
            this.publish = publish;
        }


        [HttpGet("{userNO}")]   //not good as Get shoud post to pass sensitive user information, simple for now
        public async Task<IActionResult> CurrentBalance([FromRoute] string userNO)
        {
            return await repository.CurrentBalance(userNO).ToJsonResultAsync();
        }


        [HttpPost]  
        public async Task<IActionResult> Credit([FromBody] CreditDto dto)
        {
            return await repository.Credit(dto).ToJsonResultAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Debit([FromBody] DebitDto dto)
        {
            /*for quick example noo need for DDD /cqrs saga and clean to publish event to topup */
            var operation = await repository.Debit(dto);
            

            //pass trn with status
            if(operation.IsSuccess)
            {
                //not correct should be as command
               await publish.Publish<Topup.DebitMessage>(new
                {
                    Trn = operation.Data!.TransactionReference,
                    Status =  (int)operation.Data!.Status,
                    UserNO = dto.UserNO,
                    Amount = dto.Amount,
               }); //need delay to show debit proccing by bank/prvider....
            }

            return operation.ToJsonResult();
        }


        [HttpPost]
        public async Task<IActionResult> ConfirmDebitTransaction([FromBody] ConfirmDebitTransactionDto dto)
        {
            return await repository.ConfirmDebitTransaction(dto).ToJsonResultAsync();
        }
    }
}

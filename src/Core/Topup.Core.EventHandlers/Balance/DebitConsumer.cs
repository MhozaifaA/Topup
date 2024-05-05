using Integration.Balance.HttpService;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topup.BoundedContext.DataTransferObjects;
using Topup.Domain.Implementation;
using Topup.Infrastructure.Models;

namespace Topup;
// simple in that layer .. should be event driven 
public class DebitConsumer : IConsumer<DebitMessage>
{
    private readonly ITopupRepository topupRepository;
    private readonly BalanceHttpService balanceHttpService;

    public DebitConsumer(ITopupRepository topupRepository, BalanceHttpService balanceHttpService)
    {
        this.topupRepository = topupRepository;
        this.balanceHttpService = balanceHttpService;
    }
    public async Task Consume(ConsumeContext<DebitMessage> context)
    {
        await Task.Delay(5000);//delay until trans proccing for real world

        //call confirm

        var res = await balanceHttpService.ConfirmDebitTransaction(new ConfirmDebitRequest()
        {
            TransactionReference = context.Message.Trn,
            Status = context.Message.Status != (int)TransactionStatus.Failed ?
          (int)TransactionStatus.Completed : (int)TransactionStatus.Failed,
            Amount = Math.Abs(context.Message.Amount),
            UserNO = context.Message.UserNO
        });


        if (res.IsSuccess)
        {
            //update local that transaction complated and balance 100% debited 
            await topupRepository.UpdateFromSource(new UpdateTransStatusDto()
            {

                SourceReference = context.Message.Trn,
                TransactionReference = context.Message.Trn,
                Status = context.Message.Status != (int)TransactionStatus.Failed ?
              TransactionStatus.Completed : TransactionStatus.Failed//set complated to show updated tranction if failed update to failed
            });
        }

        //Update Status
    }
}


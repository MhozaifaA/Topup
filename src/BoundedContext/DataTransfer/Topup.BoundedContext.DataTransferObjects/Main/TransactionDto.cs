using Meteors.AspNetCore.Common.AuxiliaryImplemental.Interfaces;
using Meteors.AspNetCore.Helper.Validations.Attribute.DataBaseAnnotations;
using Meteors.AspNetCore.Helper.Validations.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Topup.Infrastructure.Models;

namespace Topup.BoundedContext.DataTransferObjects
{
    public class TransactionDto : ISelector<Transaction, TransactionDto>
    {

        public string TransactionReference { get; set; }

        public string? SourceReference { get; set; }


        public DateTimeOffset Date { get; set; }


        public TransactionStatus Status { get; set; }


        public decimal TotalAmount { get; set; }

        public decimal Amount { get; set; }


        public decimal Fee { get; set; }

        public Guid UserId { get; set; }

        public Guid BeneficiaryId { get; set; }



        public static Expression<Func<Transaction, TransactionDto>> Selector { get; set; } = o => new TransactionDto()
        {
            
            Fee = o.Fee,
            Amount = o.Amount,
            Date = o.Date,
            SourceReference = o.SourceReference,
            Status = o.Status,
            TotalAmount = o.TotalAmount,
            TransactionReference = o.TransactionReference,
            UserId = o.UserId,
            BeneficiaryId = o.BeneficiaryId,
        };
    }
}

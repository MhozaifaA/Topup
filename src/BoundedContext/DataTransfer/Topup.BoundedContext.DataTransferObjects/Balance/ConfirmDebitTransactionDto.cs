using Meteors.AspNetCore.Helper.Validations.Attribute.DataBaseAnnotations;
using Meteors.AspNetCore.Helper.Validations.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topup.Infrastructure.Models;

namespace Topup.BoundedContext.DataTransferObjects.Balance
{
    public class ConfirmDebitTransactionDto
    {

        [Required]
        public string UserNO { get; set; }


        [Required]
        [StringLength(10, MinimumLength = 10)]
        public string TransactionReference { get; set; }



        [Required]
        public TransactionStatus Status { get; set; }


        [Required]
        // [Range(-1e20 + 1, 1e20 - 1)]
        //[Range(-1e20 + 1, -0.001)]
        [Range(0.0001, 1e20 - 1)]
        //debit
        public decimal Amount { get; set; }
    }
}

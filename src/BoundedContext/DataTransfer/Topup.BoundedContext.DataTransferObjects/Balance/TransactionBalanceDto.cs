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
    public class TransactionBalanceDto
    {
      
        public string TransactionReference { get; set; }

      
        public DateTimeOffset Date { get; set; }

        
        public TransactionStatus Status { get; set; }

        public decimal Amount { get; set; }

        public decimal CurrentBalance { get; set; }

        //quick show 
        public string Message => Status.ToString();
    }
}

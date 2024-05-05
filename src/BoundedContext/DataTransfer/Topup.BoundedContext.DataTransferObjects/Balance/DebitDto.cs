using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topup.BoundedContext.DataTransferObjects.Balance
{
    public class DebitDto
    {
        //[Range(-1e20+1, -0.001)]
        [Range(0.0001, 1e20 - 1)]
        public decimal Amount { get; set; } // in general positive should be sent/  but for will merge debit and credit 

        [Required]
        public string UserNO { get; set; }
    }
}

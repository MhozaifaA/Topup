using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Balance.HttpService
{
    public class TransferRequest
    {
        [Range(0.0001, 1e20 - 1)]
        public decimal Amount { get; set; } 

        [Required]
        public string UserNO { get; set; }

    }
}

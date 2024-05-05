using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topup.BoundedContext.DataTransferObjects.Balance
{
    public class CurrentBalanceDto
    {
        public decimal CurrentBalance { get; set; }

        //to check
        public decimal AvailableBalance { get; set; }
    }
}

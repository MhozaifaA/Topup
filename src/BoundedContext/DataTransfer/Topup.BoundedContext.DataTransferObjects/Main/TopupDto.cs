using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topup.BoundedContext.DataTransferObjects
{
    public class TopupDto
    {
        //check zero
        [Required]
        [DeniedValues("{00000000-0000-0000-0000-000000000000}")] ///TODO create verification .for 16 byte
        public Guid BeneficiaryId { get; set; }

        [Range(0.0001, 1e20 - 1)]  //no need to check if pass amount of the user options 5 , 10,  ..
       // [AllowedValues(5,10,20,30,50,75,100)]
        public decimal Amount { get; set; }
    }
}

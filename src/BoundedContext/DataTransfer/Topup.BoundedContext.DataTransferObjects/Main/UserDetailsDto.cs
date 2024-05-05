using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topup.BoundedContext.DataTransferObjects
{
    public class UserDetailsDto
    {
        public decimal Balance { get; set; }
        public IEnumerable<OptionDto> Options { get; set; }
        public IEnumerable<BeneficiaryInfoDto> Beneficiaries { get; set; }
    }
}

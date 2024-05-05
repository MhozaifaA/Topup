using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topup.BoundedContext.DataTransferObjects
{
    public class LoginDto
    {
     
        [Required]
        public string UserName { get; set; }

    
        [Required]
        [MinLength(4)]
        public string Password { get; set; }
    }
}

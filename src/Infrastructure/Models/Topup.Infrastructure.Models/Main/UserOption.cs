using Meteors.AspNetCore.Infrastructure.ModelEntity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topup.Infrastructure.Models
{
    /// <summary>
    /// join table, no join mean all options by default
    /// </summary>
    public class UserOption: BaseEntity<Guid>
    {
        [Required]
        public Guid UserId { get; set; }
        public Account User { get; set; }

        [Required]
        public Guid OptionId { get; set; }
        public Option Option { get; set; }
    }
}

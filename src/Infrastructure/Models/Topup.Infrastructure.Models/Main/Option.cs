using Meteors.AspNetCore.Core;
using Meteors.AspNetCore.Helper.Validations.Attribute.DataBaseAnnotations;
using Meteors.AspNetCore.Helper.Validations.Constants;
using Meteors.AspNetCore.Helper.Validations.Enum;
using Meteors.AspNetCore.Infrastructure.ModelEntity.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topup.Infrastructure.Models
{

    /// <summary>
    /// dynamic top-up options in database
    /// </summary>
    
    [Index(nameof(Name), IsUnique = true)]
    public class Option : BaseEntity<Guid>, INominal
    {
        public Option()
        {
            UserOptions = new HashSet<UserOption>();
        }

        [Required]
        [ColumnDataType(DataBaseTypes.VARCHAR, TypeConstants.NumberString)]
        public required string Name { get; set; }  //will not work with currency
        // seed (AED 5, AED 10, AED 20, AED 30, AED 50, AED 75, AED 100).



        public ICollection<UserOption> UserOptions { get; set; }

    }
}

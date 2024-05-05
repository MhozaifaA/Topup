using Meteors.AspNetCore.Core;
using Meteors.AspNetCore.Helper.Validations.Attribute.DataBaseAnnotations;
using Meteors.AspNetCore.Helper.Validations.Constants;
using Meteors.AspNetCore.Helper.Validations.Enum;
using Meteors.AspNetCore.Infrastructure.ModelEntity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topup.Infrastructure.Models
{
    public class Beneficiary : BaseEntity<Guid>, INominal
    {
        public Beneficiary()
        {
            Transactions = new HashSet<Transaction>();
        }
        /// <summary>
        /// nickname - using name in db for auto order by name by my help lib
        /// </summary>
        [Required]
        [ColumnDataType(DataBaseTypes.NVARCHAR, 20)]
        public string Name { get; set; }

        [Required] //  without phone -country code
        [ColumnDataType(DataBaseTypes.VARCHAR, TypeConstants.NumberString)]
        public string PhoneNumber { get; set; }


        [Required]
        public Guid UserId { get; set; }
        public Account User { get; set; }


        public ICollection<Transaction> Transactions { get; set; }

    }
}

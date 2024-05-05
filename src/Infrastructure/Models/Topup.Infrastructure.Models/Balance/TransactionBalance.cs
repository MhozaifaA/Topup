using Meteors.AspNetCore.Helper.Validations.Attribute.DataBaseAnnotations;
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
    // no need to build history for this example
    public class TransactionBalance : BaseEntity<Guid>
    {
        [Required]
        [ColumnDataType(DataBaseTypes.VARCHAR, 10)]
        public string TransactionReference { get; set; }


        [Required]
        [ColumnDataType(DataBaseTypes.DATETIMEOFFSET)]
        public DateTimeOffset Date { get; set; }


        [Required]
        [ColumnDataType(DataBaseTypes.INT)]
        public TransactionStatus Status { get; set; }


        [Required]
        [ColumnDataType(DataBaseTypes.DECIMAL, 19, 4)]
        public decimal Amount { get; set; }


        [Required]
        public Guid UserId { get; set; }
        public UserBalance User { get; set; }
    }
}

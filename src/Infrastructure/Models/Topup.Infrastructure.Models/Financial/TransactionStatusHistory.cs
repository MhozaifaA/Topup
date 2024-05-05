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
    //track only status
    public class TransactionStatusHistory : BaseEntity<Guid>
    {
        [Required]
        [ColumnDataType(DataBaseTypes.DATETIMEOFFSET)]
        public DateTimeOffset Date { get; set; }


        /// <summary>
        /// Last Status
        /// </summary>
        //TINYINT
        [Required]
        [ColumnDataType(DataBaseTypes.INT)]
        public TransactionStatus Status { get; set; }

        //TRN




        [Required]
        public Guid TransactionId { get; set; }
        public Transaction Transaction { get; set; }
    }
}

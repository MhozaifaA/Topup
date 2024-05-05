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
    //is not complecated strcuture  (Fees, Taxes......)
    //in this project simple one table for trans money details
    public class Transaction : BaseEntity<Guid>
    {

        public Transaction()
        {
            TransactionStatusHistories = new HashSet<TransactionStatusHistory>();
        }

        /// <summary>
        /// Transaction Reference Number -unqiue
        /// </summary>
        [Required]
        [ColumnDataType(DataBaseTypes.VARCHAR, 10)]
        public string TransactionReference { get; set; }


        ///// <summary>
        ///// simple urn
        ///// </summary>
        //[Required]
        //[ColumnDataType(DataBaseTypes.VARCHAR, 10)]
        //public string UniqueReferenceNumber { get; set; }



        /// <summary>
        /// simple 
        /// </summary>
        [ColumnDataType(DataBaseTypes.VARCHAR, 10)]
        public string? SourceReference { get; set; }


        [Required]
        [ColumnDataType(DataBaseTypes.DATETIMEOFFSET)]
        public DateTimeOffset Date { get; set; }


        /// <summary>
        /// Last Status
        /// </summary>
        //TINYINT
        [Required]
        [ColumnDataType(DataBaseTypes.INT)]
        public TransactionStatus Status  { get; set; }



        [Required]
        [ColumnDataType(DataBaseTypes.DECIMAL,19,4)]
        public decimal TotalAmount { get; set; }

        [Required]
        [ColumnDataType(DataBaseTypes.DECIMAL, 19, 4)]
        public decimal Amount { get; set; }
        
        [Required]
        [ColumnDataType(DataBaseTypes.DECIMAL, 19, 4)]
        public decimal Fee { get; set; }

        public ICollection<TransactionStatusHistory> TransactionStatusHistories { get; set; }




        //from Beneficiary we can get the user.. this shourt cut cause cycle relation

        [Required]
        public Guid UserId { get; set; }
        public Account User { get; set; }


        [Required]
        public Guid BeneficiaryId { get; set; }
        public Beneficiary Beneficiary { get; set; }
    }
}

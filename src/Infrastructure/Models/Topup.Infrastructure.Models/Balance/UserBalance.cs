using Meteors.AspNetCore.Helper.Validations.Attribute.DataBaseAnnotations;
using Meteors.AspNetCore.Helper.Validations.Constants;
using Meteors.AspNetCore.Helper.Validations.Enum;
using Meteors.AspNetCore.Infrastructure.ModelEntity.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topup.Infrastructure.Models
{
    public class UserBalance : BaseEntity<Guid>
    {
        public UserBalance()
        {
            TransactionBalances = new HashSet<TransactionBalance>();
        }

        //same in other database Top-up
        [Required]
        [ColumnDataType(DataBaseTypes.VARCHAR, TypeConstants.NumberString)]
        public string UserNO { get; set; }

        /// <summary>
        /// last calculated balance
        /// </summary>
        [Required]
        [ColumnDataType(DataBaseTypes.DECIMAL, 19, 4)]
        public decimal CurrentBalance { get; set; }


        [Required]
        [ColumnDataType(DataBaseTypes.DECIMAL, 19, 4)]
        public decimal AvailableBalance { get; set; }


        [Timestamp]
        public byte[] RowVersion { get; set; }


        public ICollection<TransactionBalance> TransactionBalances { get; set; }
    }
}

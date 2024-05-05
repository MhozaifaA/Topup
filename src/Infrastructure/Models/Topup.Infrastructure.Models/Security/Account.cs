using Meteors.AspNetCore.Core;
using Meteors.AspNetCore.Helper.Validations.Attribute.DataBaseAnnotations;
using Meteors.AspNetCore.Helper.Validations.Constants;
using Meteors.AspNetCore.Helper.Validations.Enum;
using Meteors.AspNetCore.Infrastructure.ModelEntity.Securty;
using Meteors.AspNetCore.Security.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topup.Infrastructure.Models
{
    public class Account : MrIdentityUser<Guid>, INominal, IIdentityGenerationStamp
    {
        public Account()
        {
            Beneficiaries = new HashSet<Beneficiary>();
            UserOptions = new HashSet<UserOption>();
            Transactions = new HashSet<Transaction>();
        }


        //same in other database Top-up index
        [Required]
        [ColumnDataType(DataBaseTypes.VARCHAR, 6)]
        public string UserNO { get; set; }

        [Required]
        [ColumnDataType(DataBaseTypes.NVARCHAR, TypeConstants.MediumString)]
        public string Name { get; set; }


        [ColumnDataType(DataBaseTypes.BIT)]
        [DefaultValue(false)]
        public bool HasVerified { get; set; }


        [ColumnDataType(DataBaseTypes.DATETIMEOFFSET)]
        public DateTimeOffset LastLogin { get; set; }

        /// <summary>
        /// ignor user to login from multi device
        /// <para>send by token as claim, then block all other token which not have same last stamp</para>
        /// <para>Note:We will assume that implement in this application</para>
        /// <para>null to accept inactive</para>
        /// </summary>

        [ColumnDataType(DataBaseTypes.VARCHAR, 32)]
        public string? GenerationStamp { get; set; }


        public ICollection<Beneficiary> Beneficiaries { get; set; }
        public ICollection<UserOption> UserOptions { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}

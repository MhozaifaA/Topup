using Meteors.AspNetCore.Common.AuxiliaryImplemental.Interfaces;
using Meteors.AspNetCore.Core;
using Meteors.AspNetCore.Helper.Validations.Attribute.DataBaseAnnotations;
using Meteors.AspNetCore.Helper.Validations.Constants;
using Meteors.AspNetCore.Helper.Validations.Enum;
using Meteors.AspNetCore.Infrastructure.ModelEntity.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Topup.Infrastructure.Models;

namespace Topup.BoundedContext.DataTransferObjects
{
    public class BeneficiaryDto : IIndex<Guid>, INominal, ISelector<Beneficiary, BeneficiaryDto>
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        [Required] //  without phone -country code
        [StringLength(TypeConstants.NumberString, MinimumLength = 8)]
        public string PhoneNumber { get; set; }


        public Guid UserId { get; set; }


        public static Expression<Func<Beneficiary, BeneficiaryDto>> Selector { get; set; } = o => new BeneficiaryDto()
        {
            Id = o.Id,
            Name = o.Name,
            PhoneNumber = o.PhoneNumber,
            UserId = o.UserId,
        };

        public static Expression<Func<BeneficiaryDto, Beneficiary>> InverseSelector { get; set; } = o => new Beneficiary()
        {
            Id = o.Id,
            Name = o.Name,
            PhoneNumber = o.PhoneNumber,
            UserId = o.UserId,
        };

        public static Action<BeneficiaryDto, Beneficiary> AssignSelector { get; set; } = (o, entity) =>
        {
            entity.Name = o.Name;
            entity.PhoneNumber = o.PhoneNumber;
            entity.UserId = o.UserId;
        };

    }
}

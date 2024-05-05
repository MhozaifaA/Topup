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
    public class OptionDto : IIndex<Guid>, INominal, ISelector<Option, OptionDto>
    {
        public Guid Id { get; set; }


        [Required]
        [StringLength(TypeConstants.NumberString)]
        public string Name { get; set; }

        public static Expression<Func<Option, OptionDto>> Selector { get; set; } = o => new OptionDto()
        {
            Id = o.Id,
            Name = o.Name,
        };

        public static Expression<Func<OptionDto, Option>> InverseSelector { get; set; } = o => new Option()
        {
            Id = o.Id,
            Name = o.Name
        };

        public static Action<OptionDto, Option> AssignSelector { get; set; } = (o, entity) =>
        {
            entity.Name = o.Name;
        };


    }
}

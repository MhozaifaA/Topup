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
    public class BeneficiaryInfoDto
    {
        public Guid Id { get; set; }

        public string Nickname { get; set; }

        public string PhoneNumber { get; set; }

        public decimal TotalTransfer { get; set; }
    }
}

using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topup.Core;

namespace Topup.Core
{
    public class AppRoleOptions
    {
        internal const string _section = "AppRoles";
        public int MaximumBeneficiaries { get; set; }
        public decimal TotalUserPerMonth { get; set; }
        public decimal NotVerifiedPerMonthBeneficiary { get; set; }
        public decimal VerifiedPerMonthBeneficiary { get; set; }
        public decimal FeePerTransaction { get; set; }
    }
}

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AppRoleOptionsExtension
    {
        public static IServiceCollection AddAppRoles(this WebApplicationBuilder builder)
        {
           return builder.Services.Configure<AppRoleOptions>(
                    builder.Configuration.GetSection(AppRoleOptions._section));
        }
    }
}

using Meteors;
using Meteors.AspNetCore.Service.BoundedContext;
using Meteors.OperationContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Topup.BoundedContext.DataTransferObjects;
using Topup.Domain.Implementation;
using Topup.Infrastructure.Databases.SqlServer;
using Topup.Infrastructure.Models;

namespace Topup.BoundedContext.Repositories.Security
{
    [AutoService]
    public class AccountRepository : MrRepository<TopupDbContext, Guid, Account>, IAccountRepository
    {
        private readonly UserManager<Account> UserManager;
        private readonly SignInManager<Account> SignInManager;
        private readonly IConfiguration Configuration;

        public AccountRepository(TopupDbContext context, UserManager<Account> userManager,
            SignInManager<Account> signInManager, IConfiguration configuration) : base(context)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            Configuration = configuration;
        }

        public async Task<OperationResult<AccessTokenDto>> Login(LoginDto dto)
        {
            var one = await UserManager.FindByNameAsync(dto.UserName);
            if (one is null)
                return ("UserName or Password incorrect", Statuses.Forbidden);

            var iscurrectpass = await UserManager.CheckPasswordAsync(one, dto.Password);
            if (!iscurrectpass)
                return ("UserName or Password incorrect", Statuses.Forbidden);


            await SignInManager.SignInAsync(one, false);
            //update lastlogin
            var roles = await UserManager.GetRolesAsync(one);

            return (new AccessTokenDto(one.Name, one.UserNO, GenerateJwtToken(one, roles))
            , $"Successfully");
        }

        private string GenerateJwtToken(Account account, IList<string>? roles = null, params string[] speRoles)
        {

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.UserName),//ratelimit on it
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.SerialNumber, account.UserNO!),
            };

            if (roles != null)
                foreach (var role in roles)
                {
                    if (speRoles.Length != 0)
                        if (!speRoles.Contains(role))
                            continue;
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(Configuration["Jwt:IssuerAudience"],
                  Configuration["Jwt:IssuerAudience"],
                  claims,
                  expires: DateTime.Now.AddMinutes(Configuration.GetSection("Jwt").GetValue<int>("Expires")),
                  signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

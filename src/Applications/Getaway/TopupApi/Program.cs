using MassTransit;
using Meteors.AspNetCore.Core;
using Meteors.AspNetCore.Domain.ConfigureServices;
using Meteors.AspNetCore.Infrastructure.ModelEntity.Interface;
using Meteors.AspNetCore.Service.DependencyInjection;
using Meteors.AspNetCore.Service.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using System.Threading.RateLimiting;
using Topup;
using Topup.BoundedContext.Repositories.Main;
using Topup.BoundedContext.Repositories.Security;
using Topup.BoundedContext.Services;
using Topup.Core.EventHandlers;
using Topup.Infrastructure.Databases.SqlServer;
using Topup.Infrastructure.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddMrDbContext<TopupDbContext>(
(options) =>
{
    //usesqlserver
    options.UseSqlServer(builder.Configuration.GetConnectionString("localhost"));
    //options.UseInMemoryDatabase("TopupDB");
}).AddIdentity<Account, IdentityRole<Guid>>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 4;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
}).AddEntityFrameworkStores<TopupDbContext>().AddDefaultTokenProviders();


builder.AddAppRoles();


// for more: https://github.com/MhozaifaA/Meteors.DependencyInjection.AutoService
builder.Services.AddAutoService(BoundedContextRepositoriesMainAssembly.Assembly,
    BoundedContextRepositoriesSecurityAssembly.Assembly,
    BoundedContextServicesAssembly.Assembly);

builder.Services.AddMrRepository((e) =>
{
    // e.OrderBy(nameof(IBaseEntity.DateCreated)).Then;
    e.OrderBy(nameof(INominal.Name)); //global indexing...this notwork with inmemory
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(
    EventHandlersAssembly.Assembly));

//register http service
builder.Services.AddBalanceHttpService();


builder.Services.AddMassTransit(m =>
{
    m.AddConsumers(Assembly.GetAssembly(typeof(DebitConsumer)));
    m.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("localhost", "/", c =>
        {
            c.Username("guest");
            c.Password("guest");
        });
        cfg.ConfigureEndpoints(ctx);
    });
});

#region Simple Authentication
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    //options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:IssuerAudience"],
        ValidAudience = builder.Configuration["Jwt:IssuerAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
    };
});

#endregion

builder.Services.AddRateLimiter(_ =>
{
    _.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    _.AddPolicy(policyName: "ConcurrencyLimiter", partitioner: httpContext =>
   {
       string userName = httpContext.User.Identity?.Name ?? string.Empty;

       if (!StringValues.IsNullOrEmpty(userName))
       {
           return RateLimitPartition.GetConcurrencyLimiter(userName, _ =>
               new ConcurrencyLimiterOptions
               {
                   PermitLimit = 1,
                   QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                   QueueLimit = 3,
               });
       }
       //TODO no ratelimit should authorize
       return default;
   });

});


builder.Services.AddHealthChecks();

builder.Services.AddMrSwagger();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMrSwagger();
}


app.UseHttpsRedirection();

app.UseStaticFiles();//c swagger


app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter();


app.MapControllers();


app.UseDbContextSeed<TopupDbContext>(async (context, provider) =>
{
    // await context.Database.MigrateAsync();

    // await context.Database.EnsureCreatedAsync();

    await context.AccountsSeedAsync(provider);

    app.DisposeDbContextSeed();
});



app.MapHealthChecks("/health");


app.Run();

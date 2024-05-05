using Balance.BoundedContext.Repositories;
using MassTransit;
using Meteors.AspNetCore.Domain.ConfigureServices;
using Meteors.AspNetCore.Infrastructure.ModelEntity.Interface;
using Meteors.AspNetCore.Service.DependencyInjection;
using Meteors.AspNetCore.Service.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Topup.BoundedContext.Services;
using Topup.Infrastructure.Databases.SqlServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddMrDbContext<BalanceDbContext>(
(options) =>
{
    //usesqlserver
    options.UseSqlServer(builder.Configuration.GetConnectionString("localhost"));
});

builder.Services.AddAutoService(BoundedContextRepositoriesMainAssembly.Assembly);

builder.Services.AddMrRepository((e) =>
{
    e.OrderBy(nameof(IBaseEntity.DateCreated));
});

//simple , 
builder.Services.AddMassTransit(x =>
{
    
    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("localhost", "/", c =>
        {
            c.Username("guest");
            c.Password("guest");
        });
        cfg.ConfigureEndpoints(ctx);
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

app.UseAuthorization();

app.MapControllers();



app.UseDbContextSeed<BalanceDbContext>(async (context, provider) =>
{
    // await context.Database.MigrateAsync();

    // await context.Database.EnsureCreatedAsync();

    await context.UsersSeedAsync(provider);

    app.DisposeDbContextSeed();
});


app.MapHealthChecks("/health");


app.Run();

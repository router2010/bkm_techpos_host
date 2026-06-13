using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TechPosHost.Data;
using TechPosHost.Network;
using TechPosHost.Repository;
using TechPosHost.Routing;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<TerminalRepository>();
builder.Services.AddScoped<TransactionRepository>();
builder.Services.AddScoped<MessageRouter>();

var host = builder.Build();

using var scope = host.Services.CreateScope();

var router =
    scope.ServiceProvider.GetRequiredService<MessageRouter>();

var server =
    new TcpServer(23232, router);

Console.WriteLine("TechPosHost Started");

await server.StartAsync();
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TechPosHost.Data;
using TechPosHost.Network;
using TechPosHost.Repository;
using TechPosHost.Routing;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Warning);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<TerminalRepository>();
builder.Services.AddScoped<TransactionRepository>();
builder.Services.AddScoped<MessageRouter>();
builder.Services.AddScoped<IsoLogRepository>();
builder.Services.AddScoped<CardRepository>();
builder.Services.AddScoped<MerchantRepository>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

using var scope = app.Services.CreateScope();

var router = scope.ServiceProvider.GetRequiredService<MessageRouter>();

var isoLogRepository = scope.ServiceProvider.GetRequiredService<IsoLogRepository>();

var server = new TcpServer(23232, router, isoLogRepository);

_ = Task.Run(async () =>
{
    Console.WriteLine("TechPosHost Started");
    await server.StartAsync();
});

app.Run();
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TechPosHost.Data;
using TechPosHost.Iso8583;
using TechPosHost.Network;
using TechPosHost.Repository;
using TechPosHost.Routing;
using TechPosHost.Security;

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
builder.Services.AddSingleton<MacService>();
builder.Services.AddScoped<TerminalKeyRepository>();
builder.Services.AddSingleton<KeyService>();
builder.Services.AddSingleton<PinBlockService>();
builder.Services.AddSingleton<IsoBuilder>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var isoBuilder = new IsoBuilder();
var msg = new IsoMessage();
var macService = new MacService();

msg.MTI = "0200";

msg.SetField(2, "4506340000000001");
msg.SetField(3, "000000");
msg.SetField(4, "000000001000");
msg.SetField(11, "777999");
msg.SetField(41, "TERM001");

var text =
    isoBuilder.BuildBitmapMessage(
        msg,
        macService);

Console.WriteLine(text);

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
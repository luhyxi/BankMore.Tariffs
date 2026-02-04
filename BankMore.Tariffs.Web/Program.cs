using BankMore.Tariffs.Application.Tariff.Create;
using BankMore.Tariffs.Web.Configs;

using KafkaFlow;

var builder = WebApplication.CreateBuilder(args);

using var loggerFactory = LoggerFactory.Create(config => config.AddConsole());
var startupLogger = loggerFactory.CreateLogger<Program>();

builder.Services.AddKafkaConfig(builder.Configuration, startupLogger);
builder.Services.AddServiceConfigs(startupLogger, builder);

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();
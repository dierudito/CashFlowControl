using DMoreno.CashFlowControl.bff.Endpoints;
using DMoreno.CashFlowControl.bff.Extensions;
using DMoreno.CashFlowControl.Infra.CrossCutting.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfiguration();
builder.Services.RegisterServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.ConfigureDevEnvironment();

app.UseCors(ApiConfigurations.CorsPolicyName);
app.MapEndpoints();

app.Run();
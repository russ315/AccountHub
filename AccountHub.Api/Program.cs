using System.Text.Json.Serialization;
using AccountHub.Api.Extensions;
using AccountHub.Domain.Entities;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddData()
    .AddSwagger()
    .AddAuthorization()
    .AddOptions()
    .AddApplicationServices()
    .AddIntegrationServices()
    .AddExceptionHandler();

builder.Services.AddControllers();

var app = builder.Build();

app.UseExceptionHandler();


app.UseSwagger();
app.UseSwaggerUI();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseHttpsRedirection();


app.Run();

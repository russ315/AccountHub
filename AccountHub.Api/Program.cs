using System.Text.Json.Serialization;
using AccountHub.Api.Extensions;
using AccountHub.Domain.Entities;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder
    .AddData()
    .AddSwagger()
    .AddAuthorization()
    .AddOptions()
    .AddApplicationServices()
    .AddIntegrationServices();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseHttpsRedirection();


app.Run();

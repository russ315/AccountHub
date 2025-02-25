using System.Text.Json.Serialization;
using AccountHub.Api.Extensions;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddJsonOptions(options =>
{
    
});

builder
    .AddData()
    .AddAuthorization()
    .AddOptions()
    .AddSwagger()
    .AddApplicationServices()
    .AddIntegrationServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();


app.Run();

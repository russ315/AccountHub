using AccountHub.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

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

app.UseHttpsRedirection();


app.Run();

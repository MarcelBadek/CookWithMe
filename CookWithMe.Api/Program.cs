using CookWithMe.Data.Data;
using CookWithMe.Data.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var services = builder.Services;

services.ConfigureOptions<DatabaseOptionSetup>();

services.AddDbContext<AppDbContext>((serviceProvider, options)=>
{
    var databaseOptions = serviceProvider.GetService<IOptions<DatabaseOptions>>()!.Value;
        
    options.UseSqlServer(databaseOptions.ConnectionString, sqlServerAction =>
    {
        sqlServerAction.EnableRetryOnFailure(databaseOptions.MaxRetryCount);
        sqlServerAction.CommandTimeout(databaseOptions.CommandTimeout);
    });
    options.EnableDetailedErrors(databaseOptions.EnableDetailedErrors);
    options.EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
using CookWithMe.Data.Data;
using CookWithMe.Data.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CookWithMe.Data;

public static class ConfigureServices
{
    public static void SetupDb(IServiceCollection services)
    {
        services.ConfigureOptions<DatabaseOptionSetup>();

        services.AddDbContext<AppDbContext>((serviceProvider, options)=>
        {
            var databaseOptions = serviceProvider.GetService<IOptions<DatabaseOptions>>()!.Value;
        
            options.UseSqlServer(databaseOptions.ConnectionString, sqlServerAction =>
            {
                sqlServerAction.EnableRetryOnFailure(databaseOptions.MaxRetryCount);
                sqlServerAction.CommandTimeout(databaseOptions.CommandTimeout);
                sqlServerAction.MigrationsAssembly("CookWithMe.Api");
            });
            options.EnableDetailedErrors(databaseOptions.EnableDetailedErrors);
            options.EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging);
        });
    }
}
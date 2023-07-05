using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CookWithMe.Data.Options;

public class DatabaseOptionSetup : IConfigureOptions<DatabaseOptions>
{
    private const string ConfigurationSectionName = "DatabaseOptions";
    private readonly IConfiguration _configuration;

    public DatabaseOptionSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(DatabaseOptions options)
    {
        options.ConnectionString = _configuration.GetConnectionString("dev")!;
        _configuration.GetSection(ConfigurationSectionName).Bind(options);
    }
}
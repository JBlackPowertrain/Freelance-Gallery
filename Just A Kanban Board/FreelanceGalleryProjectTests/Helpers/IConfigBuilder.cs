using Microsoft.Extensions.Configuration;

namespace FreelanceGalleryProjectTests.Helpers;

public static class IConfigBuilder
{
    public static Microsoft.Extensions.Configuration.IConfiguration InitConfig()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Development.json")
            .AddEnvironmentVariables()
            .Build();
        return config; 
    }
}

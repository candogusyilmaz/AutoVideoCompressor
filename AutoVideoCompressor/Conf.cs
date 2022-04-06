using Microsoft.Extensions.Configuration;

namespace AutoVideoCompressor;
internal static class Conf
{
    public static IConfigurationRoot GetAppSettings()
    {
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory());
        builder.AddJsonFile("appsettings.json");

        var config = builder.Build();
        return config;
    }
}

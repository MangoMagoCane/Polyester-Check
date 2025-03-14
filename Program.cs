using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using NetCord.Gateway;
using NetCord.Hosting.Gateway;
using NetCord.Hosting.Services;
using NetCord.Hosting.Services.ApplicationCommands;

using Npgsql;

namespace Polyester;

class Program
{
    public static readonly NpgsqlDataSource npgDataSource;

    static Program()
    {
        IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        DatabaseAppSettings? dbConfig = config.GetRequiredSection("Database").Get<DatabaseAppSettings>();
        String connectionString = dbConfig?.ConnectionString ?? "";

        NpgsqlDataSourceBuilder npgDataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        npgDataSourceBuilder.MapEnum<ClothingItems>();
        npgDataSourceBuilder.MapEnum<ClothingTypes>();

        npgDataSource = npgDataSourceBuilder.Build();
    }

    public static async Task Main(string[] args)
    {
        await using NpgsqlCommand npgCommand = npgDataSource.CreateCommand("SELECT foo FROM test;");
        await using NpgsqlDataReader npgReader = await npgCommand.ExecuteReaderAsync();

        while (await npgReader.ReadAsync())
        {
            Console.WriteLine(npgReader.GetInt32(0));
        }

        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
        builder.Services
            .AddDiscordGateway(options =>
            {
                options.Intents = GatewayIntents.Guilds | GatewayIntents.GuildMessages;
            })
            .AddApplicationCommands();
        IHost host = builder.Build();

        host.AddModules(typeof(Program).Assembly);
        host.UseGatewayEventHandlers();

        await host.RunAsync();
    }
}


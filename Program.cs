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
    public static readonly Dictionary<ClothingItem, int> ClothingItemWeightMap;
    public static readonly Dictionary<ClothingItem, int> ClothingItemDefaultPercentageMap;

    static Program()
    {
        IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        DatabaseAppSettings? dbConfig = config.GetRequiredSection("Database").Get<DatabaseAppSettings>();
        String connectionString = dbConfig?.ConnectionString ?? "";

        NpgsqlDataSourceBuilder npgDataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        npgDataSourceBuilder.MapEnum<ClothingItem>();
        npgDataSourceBuilder.MapEnum<ClothingType>();
        npgDataSource = npgDataSourceBuilder.Build();

        // may want to put into sql table and read into dict statically making the coupling looser or something idek
        ClothingItemWeightMap = new Dictionary<ClothingItem, int>
        {
            {ClothingItem.TShirt, 40},
            {ClothingItem.LongSleeve, 45},
            {ClothingItem.Pants, 45},
            {ClothingItem.Socks, 5},
            {ClothingItem.Hat, 5},
            {ClothingItem.Underwear, 5},
            {ClothingItem.Watch, 2},
            {ClothingItem.Gloves, 5},
            {ClothingItem.Sweater, 45},
            {ClothingItem.Jacket, 45},
            {ClothingItem.Shorts, 25},
        };

        ClothingItemDefaultPercentageMap = new Dictionary<ClothingItem, int>
        {
            {ClothingItem.TShirt, 20},
            {ClothingItem.LongSleeve, 20},
            {ClothingItem.Pants, 20},
            {ClothingItem.Socks, 20},
            {ClothingItem.Hat, 20},
            {ClothingItem.Underwear, 20},
            {ClothingItem.Watch, 20},
            {ClothingItem.Gloves, 20},
            {ClothingItem.Sweater, 20},
            {ClothingItem.Jacket, 20},
            {ClothingItem.Shorts, 20},
        };
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


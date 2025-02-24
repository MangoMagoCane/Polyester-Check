using System.Reflection;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

// using NetCord;
using NetCord.Gateway;
using NetCord.Hosting.Gateway;
using NetCord.Hosting.Services;
using NetCord.Hosting.Services.ApplicationCommands;
// using NetCord.Hosting.Services.Commands;
// using NetCord.Services;
using NetCord.Services.ApplicationCommands;
using Npgsql;
// using NetCord.Services.Commands;

namespace PolyesterCheck;

public enum ClothingItems
{
    shirt, pants, socks, hat, underwear, watch, gloves, sweater, jacket, shorts,
}

public enum ClothingTypes
{
    cotton, wool, leather, denim, silk, bamboo, polyester, nylon, spandex, rayon, acrylic,
}

class Program
{
    public static async Task Main(string[] args)
    {
        IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        DatabaseAppSettings? db_config = config.GetRequiredSection("Database").Get<DatabaseAppSettings>();
        String connectionString = db_config?.ConnectionString ?? "";

        await using NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);
        await using NpgsqlCommand command = dataSource.CreateCommand("SELECT foo FROM test;");
        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            Console.WriteLine(reader.GetInt32(0));
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

public class PolyesterModule : ApplicationCommandModule<ApplicationCommandContext>
{
    [SlashCommand("register", "register!")]
    // public string RegisterItem()
    public string RegisterItem(
            [SlashCommandParameter()] ClothingItems item,
            [SlashCommandParameter()] ClothingTypes type,
            [SlashCommandParameter(MinValue = 0, MaxValue = 100)] float? percentage = -1)
    {
        return $"{item}: {type} {percentage}";
    }

    [SlashCommand("context", "foo")]
    public string PrintContext()
    {
        if (Context.Guild == null)
        {
            return "it is invalid to use this command outside a guild!";
        }

        StringBuilder sb = new StringBuilder();
        PropertyInfo[] properties = Context.GetType().GetProperties();
        foreach (PropertyInfo pi in properties)
        {
            sb.Append(
                string.Format("Name: {0} | Value: {1}\n",
                    pi.Name,
                    pi.GetValue(Context, null)
                )
            );
        }
        sb.Append(Context.GetType().ToString());

        return sb.ToString();
    }
}

public sealed class DatabaseAppSettings
{
    public required string ConnectionString { get; set; } = "";
}

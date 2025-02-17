using Microsoft.Extensions.Hosting;
using NetCord;
using NetCord.Gateway;
using NetCord.Hosting.Gateway;
using NetCord.Hosting.Services;
using NetCord.Hosting.Services.ApplicationCommands;
using NetCord.Hosting.Services.Commands;
using NetCord.Services;
using NetCord.Services.ApplicationCommands;
using NetCord.Services.Commands;

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
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
        builder.Services
            .AddDiscordGateway()
            .AddCommands()
            .AddApplicationCommands();
        IHost host = builder.Build();

        host.AddModules(typeof(Program).Assembly);
        host.UseGatewayEventHandlers();

        await host.RunAsync();
    }
}

public class CustomCommandContext(Message message, GatewayClient client) : CommandContext(message, client)
{
    public GuildUser BotGuildUser => Guild!.Users[Client.Id];
}

public class PolyesterModule : ApplicationCommandModule<ApplicationCommandContext>
{
    [SlashCommand("register", "register!")]
    public string RegisterItem(ClothingItems item, ClothingTypes type)
    {
        Console.WriteLine((int)item);
        return $"{item}: {type}";
    }

}

public class TestModule : CommandModule<CustomCommandContext>
{
    [RequireContext<CustomCommandContext>(RequiredContext.Guild)]
    [Command("test", "foo")]
    public String Test()
    {
        var user = Context.BotGuildUser;
        return user.Nickname ?? user.Username;
    }

}

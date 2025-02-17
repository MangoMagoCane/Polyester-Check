using Microsoft.Extensions.Hosting;
using NetCord;
using NetCord.Hosting.Gateway;
using NetCord.Services.ApplicationCommands;

namespace PolyesterCheck;

class Program
{
    public static void Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddDiscordGateway();
    }
}

public class OhioModule : ApplicationCommandModule<ApplicationCommandContext>
{
    [SlashCommand("ohio", "slava ohio!")]
    public string Ohio(User user)
    {
        return $"{Context.User} {user}";
    }
}

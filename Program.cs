// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;

public class Program
{
    // private static DiscordSocketClient _client;

    public static void Main()
    {
        // Console.WriteLine("foo!");
        // _client = new DiscordSocketClient();
        // _client.Log += Log;

        IConfiguration config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

        string? token = config["BOT_TOKEN"];
        if (token == null) {
            Console.WriteLine("token wrong?");
        } else {
            Console.WriteLine(token);
        }

    }

    // private static Task Log(LogMessage msg)
    // {
    //     Console.writeLine(msg.ToString());
    //     return Task.completedTask;
    // }
}

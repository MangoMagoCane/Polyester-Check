// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;

public class Program
{
    // private static DiscordSocketClient _client;

    public static void Main()
    {
        // Console.WriteLine("foo!");
        // _client = new DiscoordSocketClient();
        // _client.Log += Log;

        var builder = new ConfigurationBuilder()
            .UseStartup<Startup>();
        var configuration = builder.Build();
        Console.WriteLine(builder.GetType());

        string? token = configuration["BOT_TOKEN"];
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

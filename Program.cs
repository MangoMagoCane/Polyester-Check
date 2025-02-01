using System;
using System.ComponentModel;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.TextCommands;
using DSharpPlus.Commands.Processors.TextCommands.Parsing;

namespace PolyesterCheck
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            IConfiguration env_vars = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            string? d_token = env_vars["BOT_TOKEN"];
            if (d_token == null)
            {
                Console.WriteLine("error: null bot token");
                return;
            }
            DiscordClientBuilder d_builder = DiscordClientBuilder.CreateDefault(d_token, DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents);

            // there may be a newer more idiomatic way to config event handlers with Commands
            d_builder.ConfigureEventHandlers
            (
                b => b.HandleMessageCreated(async (s, e) =>
                {
                    if (e.Message.Content.ToLower().Contains("kitty"))
                    {
                        await e.Message.RespondAsync("woof woof!");
                    }
                })
            );

            d_builder.UseCommands((IServiceProvider service_provider, CommandsExtension extension) =>
            {
                TextCommandProcessor text_command_processor = new(new()
                {
                    // second param 'params string[] prefix' puts remaining args in arr, eg. f(1, 2, 3), f(1, [2, 3])
                    PrefixResolver = new DefaultPrefixResolver(true, "%", "slava").ResolvePrefixAsync
                });

                extension.AddCommands([typeof(DoggyCommand)]);
                extension.AddProcessor(text_command_processor);
            }, new CommandsConfiguration()
            {
                RegisterDefaultCommandProcessors = true,
                DebugGuildId = (ulong)Convert.ToInt32(env_vars["DEBUG_GUILD_ID"]),
            });

            DiscordClient d_client = d_builder.Build();
            DiscordActivity d_activity = new("with soft fabrics", DiscordActivityType.Playing);

            await d_client.ConnectAsync(d_activity, DiscordUserStatus.Online);
            await Task.Delay(-1);
        }
    }

    [Command("doggy")]
    public class DoggyCommand
    {
        [Command("puppy"), Description("Makes the sound puppies make...")]
        public static async ValueTask DoggyAsync(CommandContext context)
        {
            await context.RespondAsync("meow meow.");
        }

        [Command("ohio"), Description("OHIO! SLAVA!")]
        public static async ValueTask OhioAsync(CommandContext context)
        {
            await context.RespondAsync("https://media1.tenor.com/m/Ii6FkhiwAK4AAAAd/ohio-astolfo.gif");
        }
    }
}

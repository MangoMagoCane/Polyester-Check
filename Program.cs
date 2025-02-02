using System;
using System.ComponentModel;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Trees;
using DSharpPlus.Commands.Processors.TextCommands;
using DSharpPlus.Commands.Processors.TextCommands.Parsing;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;

namespace PolyesterCheck
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            // environment variables
            IConfiguration env_vars = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            string? d_token = env_vars["BOT_TOKEN"];
            if (d_token == null)
            {
                Console.WriteLine("error: null bot token");
                return;
            }

            string npg_connection_string = ""

            // DSharpPlus
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
                    PrefixResolver = new DefaultPrefixResolver(true, "%", "slava").ResolvePrefixAsync
                });

                extension.AddCommands([typeof(PolyesterCommands), typeof(MemeCommands)]);
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

    public class PolyesterCommands
    {
        [Command("register-item"), Description("Register an item for the Polyester Check")]
        public static async ValueTask DoggyAsync(CommandContext context, [SlashChoiceProvider<ClothingItemsProvider>] int item, [SlashChoiceProvider<FabricTypesProvider>] int fabric, int percentage)
        {
            // await context.RespondAsync($"{item}: {ClothingItemsProvider.clothing_items.ElementAt(item).Name} {fabric} {percentage}");
            await context.RespondAsync($"{item}, {fabric}, {percentage}");
        }

    }

    public class MemeCommands
    {
        [Command("ohio"), Description("OHIO! SLAVA!")]
        public static async ValueTask OhioAsync(CommandContext context)
        {
            // await context.DeferResponseAsync("https://media1.tenor.com/m/Ii6FkhiwAK4AAAAd/ohio-astolfo.gif");
            await context.DeferResponseAsync();
            await context.EditResponseAsync("https://media1.tenor.com/m/Ii6FkhiwAK4AAAAd/ohio-astolfo.gif");
            await context.FollowupAsync("foo");
        }
    }

    public class ClothingItemsProvider : IChoiceProvider
    {
        public static readonly IEnumerable<DiscordApplicationCommandOptionChoice> clothing_items =
        [
            new DiscordApplicationCommandOptionChoice("shirt", 0),
            new DiscordApplicationCommandOptionChoice("pants", 1),
            new DiscordApplicationCommandOptionChoice("socks", 2),
            new DiscordApplicationCommandOptionChoice("hat", 3),
            new DiscordApplicationCommandOptionChoice("underwear", 4),
            new DiscordApplicationCommandOptionChoice("watch", 5),
            new DiscordApplicationCommandOptionChoice("gloves", 6),
            new DiscordApplicationCommandOptionChoice("sweater", 7),
            new DiscordApplicationCommandOptionChoice("jacket", 8),
            new DiscordApplicationCommandOptionChoice("shorts", 9),
            // new DiscordApplicationCommandOptionChoice("", ),
        ];

        public ValueTask<IEnumerable<DiscordApplicationCommandOptionChoice>> ProvideAsync(CommandParameter parameter) =>
            ValueTask.FromResult(clothing_items);
    }

    public class FabricTypesProvider : IChoiceProvider
    {
        public static readonly IEnumerable<DiscordApplicationCommandOptionChoice> fabric_types =
        [
            new DiscordApplicationCommandOptionChoice("cotton", 0),
            new DiscordApplicationCommandOptionChoice("wool", 1),
            new DiscordApplicationCommandOptionChoice("leather", 2),
            new DiscordApplicationCommandOptionChoice("denim", 3),
            new DiscordApplicationCommandOptionChoice("silk", 4),
            new DiscordApplicationCommandOptionChoice("bamboo", 5),
            new DiscordApplicationCommandOptionChoice("polyester", 6),
            new DiscordApplicationCommandOptionChoice("nylon", 7),
            new DiscordApplicationCommandOptionChoice("spandex", 8),
            new DiscordApplicationCommandOptionChoice("rayon", 9),
            new DiscordApplicationCommandOptionChoice("acrylic", 10),
        ];

        public ValueTask<IEnumerable<DiscordApplicationCommandOptionChoice>> ProvideAsync(CommandParameter parameter) =>
            ValueTask.FromResult(fabric_types);
    }
}

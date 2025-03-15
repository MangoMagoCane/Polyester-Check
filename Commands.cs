using System.Reflection;
using System.Text;

using NetCord.Services.ApplicationCommands;

using Npgsql;

namespace Polyester;

public class PolyesterModule : ApplicationCommandModule<ApplicationCommandContext>
{
    [SlashCommand("register", "registers a new item")]
    // public string RegisterItem()
    public string RegisterItem(
            [SlashCommandParameter()] ClothingItem item,
            [SlashCommandParameter()] ClothingType type,
            [SlashCommandParameter(MinValue = 0, MaxValue = 100)] int percentage = -1)
    {
        if (Context.Guild == null)
        {
            return "it is invalid to use this command outside a guild!";
        }

        if (percentage == -1)
        {
            percentage = Program.ClothingItemDefaultPercentageMap[item];
        }

        return $"{item} ({Program.ClothingItemWeightMap[item]}): {type} {percentage}";
    }

    [SlashCommand("context", "foo")]
    public async Task<string> PrintContext()
    {
        if (Context.Guild == null)
        {
            return "it is invalid to use this command outside a guild!";
        }

        await using NpgsqlCommand npgCommand = Program.npgDataSource.CreateCommand("SELECT foo FROM test;");
        await using NpgsqlDataReader npgReader = await npgCommand.ExecuteReaderAsync();

        while (await npgReader.ReadAsync())
        {
            Console.WriteLine(npgReader.GetInt32(0));
        }

        // using NpgsqlCommand npgCommand = npgDataSource.CreateCommand("SELECT foo FROM test;");
        StringBuilder sb = new StringBuilder();
        PropertyInfo[] properties = Context.GetType().GetProperties();
        sb.Append(Context.GetType().ToString());
        foreach (PropertyInfo pi in properties)
        {
            Object? piValue = pi.GetValue(Context, null);
            sb.Append(
                string.Format("Name: \"{0}\" | Value: \"{1}\" | Type: \"{2}\"\n",
                    pi.Name,
                    piValue?.ToString() ?? "<null>",
                    piValue?.GetType()
                )
            );
        }
        sb.Append("\n @Polyester Check");
        sb.Append($"\n {Context.Guild.ToString()}");
        sb.Append($"\n {Context.User.Id}");
        String sbString = sb.ToString();
        Console.WriteLine(sbString);
        return sbString;
    }
}


using System.Reflection;
using System.Text;
using NetCord.Services.ApplicationCommands;
using NetCord;

public class PolyesterModule : ApplicationCommandModule<ApplicationCommandContext>
{
    [SlashCommand("register", "registers a new item")]
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
            Object? piValue = pi.GetValue(Context, null);
            sb.Append(
                string.Format("Name: \"{0}\" | Value: \"{1}\" | Type: \"{2}\"\n",
                    pi.Name,
                    piValue?.ToString() ?? "<null>",
                    piValue?.GetType()
                )
            );
        }
        sb.Append(Context.GetType().ToString());
        sb.Append("\n @Polyester Check");
        sb.Append($"\n {Context.Guild.ToString()}");
        sb.Append($"\n {Context.User.guildId}");
        String sbString = sb.ToString();
        Console.WriteLine(sbString);
        return sbString;
    }
}


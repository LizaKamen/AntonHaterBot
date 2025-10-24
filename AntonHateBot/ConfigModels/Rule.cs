namespace AntonHateBot.ConfigModels;

public class Rule
{
    public string Emoji { get; set; } = "";
    public string Keyword { get; set; } = "";
    public bool IsRegex { get; set; }
}
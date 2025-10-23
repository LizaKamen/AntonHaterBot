namespace AntonHateBot.ConfigModels;

public class UserRules
{
    public string Username { get; set; } = "";
    public Rule[] Rules { get; set; } = [];
}
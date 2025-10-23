namespace AntonHateBot.ConfigModels;

public class AntonHateConfig
{
    public string Token { get; set; } = "";

    public UserRules[] Rulesets { get; set; } = [];
}
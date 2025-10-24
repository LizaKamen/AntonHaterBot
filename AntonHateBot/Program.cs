using AntonHateBot.ConfigModels;
using AntonHateBot.Services;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;

namespace AntonHateBot;

class Program
{
    static void Main(string[] args)
    {
        var configuration = LoadConfiguration();
        var antonHateConfig = configuration.GetSection("AntonHate").Get<AntonHateConfig>() ?? new AntonHateConfig();

        using var cts = new CancellationTokenSource();
        var botClient = new TelegramBotClient(antonHateConfig.Token, cancellationToken: cts.Token);

        var userRuleProvider = new ConfigurationUserRuleProvider(antonHateConfig.Rulesets);
        var ruleMatcher = new RuleMatcher();
        var messageReactionHandler = new MessageReactionHandler(botClient, userRuleProvider, ruleMatcher);

        botClient.OnMessage += messageReactionHandler.HandleAsync;

        Console.ReadLine();
    }

    private static IConfigurationRoot LoadConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();
    }
}

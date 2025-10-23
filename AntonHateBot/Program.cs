using AntonHateBot.ConfigModels;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Configuration;

namespace AntonHateBot;

class Program
{
    static void Main(string[] args)
    {
        var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);
        
        var configuration = builder.Build();
        
        var antonHateConfig = configuration.GetSection("AntonHate").Get<AntonHateConfig>();

        using var cts = new CancellationTokenSource();
        var token = antonHateConfig.Token;
        var rulesets = antonHateConfig.Rulesets;
        var botClient = new TelegramBotClient(token, cancellationToken: cts.Token);

        botClient.OnMessage += React;

        Console.ReadLine();

        async Task React(Message message, UpdateType update)
        {
            var rule = rulesets.FirstOrDefault(r => r.Username.ToUpper().Equals(message.From.FirstName.ToUpper()));
            if (rule != null)
            {
                var text = message.Text != null ? message.Text : message.Caption;
                var emoji = rule.Rules.FirstOrDefault(r => text.ToUpper().Contains(r.Keyword.ToUpper())).Emoji;
                await botClient.SetMessageReaction(message.Chat.Id, message.Id,
                    [new ReactionTypeEmoji() { Emoji = emoji }]);
            }
        }
    }
}
using AntonHateBot.ConfigModels;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System.Text.RegularExpressions;
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
            if (message.From?.FirstName is null)
            {
                return;
            }

            var rule = rulesets.FirstOrDefault(r =>
                string.Equals(r.Username, message.From.FirstName, StringComparison.OrdinalIgnoreCase));

            if (rule == null)
            {
                return;
            }

            var text = message.Text ?? message.Caption;
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            var matchingRule = rule.Rules.FirstOrDefault(r =>
            {
                if (string.IsNullOrEmpty(r.Keyword))
                {
                    return false;
                }

                if (r.IsRegex)
                {
                    try
                    {
                        return Regex.IsMatch(text, r.Keyword, RegexOptions.IgnoreCase);
                    }
                    catch (ArgumentException)
                    {
                        return false;
                    }
                }

                return text.Contains(r.Keyword, StringComparison.OrdinalIgnoreCase);
            });

            if (matchingRule == null)
            {
                return;
            }

            await botClient.SetMessageReaction(message.Chat.Id, message.Id,
                [new ReactionTypeEmoji() { Emoji = matchingRule.Emoji }]);
        }
    }
}
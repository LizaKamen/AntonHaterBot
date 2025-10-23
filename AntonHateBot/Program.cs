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
        var hatredName = antonHateConfig.Username;
        var emoji = antonHateConfig.Emoji;
        var botClient = new TelegramBotClient(token, cancellationToken: cts.Token);

        botClient.OnMessage += React;

        Console.ReadLine();

        async Task React(Message message, UpdateType update)
        {
            if (message.From.Username.ToUpper() == hatredName.ToUpper())
            {
                await botClient.SetMessageReaction(message.Chat.Id, message.Id,
                    new[] { new ReactionTypeEmoji() { Emoji = emoji } });
            }
        }
    }
}
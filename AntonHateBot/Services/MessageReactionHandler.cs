using AntonHateBot.ConfigModels;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.Reactions;

namespace AntonHateBot.Services;

public class MessageReactionHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly IUserRuleProvider _userRuleProvider;
    private readonly IRuleMatcher _ruleMatcher;

    public MessageReactionHandler(
        ITelegramBotClient botClient,
        IUserRuleProvider userRuleProvider,
        IRuleMatcher ruleMatcher)
    {
        _botClient = botClient ?? throw new ArgumentNullException(nameof(botClient));
        _userRuleProvider = userRuleProvider ?? throw new ArgumentNullException(nameof(userRuleProvider));
        _ruleMatcher = ruleMatcher ?? throw new ArgumentNullException(nameof(ruleMatcher));
    }

    public async Task HandleAsync(Message message, UpdateType updateType)
    {
        _ = updateType;

        if (message.From?.FirstName is null)
        {
            return;
        }

        var userRules = _userRuleProvider.FindRulesFor(message.From.FirstName);
        if (userRules is null)
        {
            return;
        }

        var text = message.Text ?? message.Caption;
        if (string.IsNullOrWhiteSpace(text))
        {
            return;
        }

        var matchingRule = _ruleMatcher.Match(userRules, text);
        if (matchingRule is null || string.IsNullOrWhiteSpace(matchingRule.Emoji))
        {
            return;
        }

        await ReactAsync(message, matchingRule);
    }

    private async Task ReactAsync(Message message, Rule matchingRule)
    {
        await _botClient.SetMessageReaction(
            message.Chat.Id,
            message.Id,
            new[] { new ReactionTypeEmoji { Emoji = matchingRule.Emoji } });
    }
}

using AntonHateBot.ConfigModels;

namespace AntonHateBot.Services;

public class ConfigurationUserRuleProvider : IUserRuleProvider
{
    private readonly IReadOnlyDictionary<string, UserRules> _rulesByUsername;

    public ConfigurationUserRuleProvider(IEnumerable<UserRules> userRules)
    {
        if (userRules is null)
        {
            throw new ArgumentNullException(nameof(userRules));
        }

        _rulesByUsername = userRules
            .Where(r => !string.IsNullOrWhiteSpace(r.Username))
            .ToDictionary(r => r.Username, StringComparer.OrdinalIgnoreCase);
    }

    public UserRules? FindRulesFor(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return null;
        }

        return _rulesByUsername.TryGetValue(username, out var rules) ? rules : null;
    }
}

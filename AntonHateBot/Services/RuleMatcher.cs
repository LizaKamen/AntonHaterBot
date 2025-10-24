using System.Text.RegularExpressions;
using AntonHateBot.ConfigModels;

namespace AntonHateBot.Services;

public class RuleMatcher : IRuleMatcher
{
    public Rule? Match(UserRules userRules, string text)
    {
        if (userRules?.Rules is null || string.IsNullOrWhiteSpace(text))
        {
            return null;
        }

        foreach (var rule in userRules.Rules)
        {
            if (string.IsNullOrWhiteSpace(rule.Keyword))
            {
                continue;
            }

            if (rule.IsRegex)
            {
                if (IsRegexMatch(rule.Keyword, text))
                {
                    return rule;
                }

                continue;
            }

            if (text.Contains(rule.Keyword, StringComparison.OrdinalIgnoreCase))
            {
                return rule;
            }
        }

        return null;
    }

    private static bool IsRegexMatch(string pattern, string text)
    {
        try
        {
            return Regex.IsMatch(text, pattern, RegexOptions.IgnoreCase);
        }
        catch (ArgumentException)
        {
            return false;
        }
    }
}

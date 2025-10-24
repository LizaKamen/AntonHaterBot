using AntonHateBot.ConfigModels;

namespace AntonHateBot.Services;

public interface IRuleMatcher
{
    Rule? Match(UserRules userRules, string text);
}

using AntonHateBot.ConfigModels;

namespace AntonHateBot.Services;

public interface IUserRuleProvider
{
    UserRules? FindRulesFor(string username);
}

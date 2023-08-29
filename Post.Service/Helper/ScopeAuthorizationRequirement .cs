using Microsoft.AspNetCore.Authorization;

public class ScopeAuthorizationRequirement : IAuthorizationRequirement
{
    public string RequiredScope { get; }

    public ScopeAuthorizationRequirement(string requiredScope)
    {
        RequiredScope = requiredScope;
    }
}

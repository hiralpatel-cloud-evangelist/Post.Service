using Microsoft.AspNetCore.Authorization;

public class ScopeAuthorizationHandler : AuthorizationHandler<ScopeAuthorizationRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ScopeAuthorizationRequirement requirement)
    {
        if (context.User != null && context.User.Identity.IsAuthenticated)
        {
            if (context.User.HasClaim(c => c.Type == "http://schemas.microsoft.com/identity/claims/scope" && c.Value.Contains(requirement.RequiredScope)))
            {
                context.Succeed(requirement);
            }
        }

        return Task.CompletedTask;
    }
}

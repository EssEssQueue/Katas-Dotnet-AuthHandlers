using AuthenticationAuthorization.WebApi.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace AuthenticationAuthorization.WebApi.Authorization;

public class MagicWordAuthorizationHandler : AuthorizationHandler<MagicWordRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MagicWordRequirement requirement)
    {
        var magicWord = context.User.Claims.FirstOrDefault(x => x.Type == "MagicWord")?.Value;
        if (
            !string.IsNullOrEmpty(magicWord)
            &&
            requirement.MagicWord!.Contains(
                magicWord,
                StringComparison.CurrentCultureIgnoreCase
            )
        )
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
        context.Fail(new AuthorizationFailureReason(this, "Magic word not found"));
        return Task.CompletedTask;
    }
}
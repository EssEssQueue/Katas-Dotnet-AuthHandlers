using Microsoft.AspNetCore.Authorization;

namespace AuthenticationAuthorization.WebApi.Authorization.Requirements;

public class MagicWordRequirement : IAuthorizationRequirement
{
    public MagicWordRequirement(string magicWord) => MagicWord = magicWord;
    public string MagicWord { get; set; }
}
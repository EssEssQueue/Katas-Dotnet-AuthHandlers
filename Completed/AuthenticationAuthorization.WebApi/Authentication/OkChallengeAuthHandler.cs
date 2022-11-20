using System.Text.Encodings.Web;
using AuthenticationAuthorization.WebApi.Authentication.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace AuthenticationAuthorization.WebApi.Authentication;

public class OkChallengeAuthHandler : AuthenticationHandler<OkChallengeAuthConfiguration>
{
    public OkChallengeAuthHandler(IOptionsMonitor<OkChallengeAuthConfiguration> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        return Task.FromResult(AuthenticateResult.Fail("Not suitable for auth"));
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Context.Response.StatusCode = StatusCodes.Status200OK;
        return Task.CompletedTask;
    }
}
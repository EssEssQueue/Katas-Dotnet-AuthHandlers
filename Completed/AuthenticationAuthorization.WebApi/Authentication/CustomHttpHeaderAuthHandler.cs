using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using AuthenticationAuthorization.WebApi.Authentication.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AuthenticationAuthorization.WebApi.Authentication;

public class CustomHttpHeaderAuthHandler
    : AuthenticationHandler<CustomHttpHeaderAuthConfiguration>
{
    public CustomHttpHeaderAuthHandler(
        IOptionsMonitor<CustomHttpHeaderAuthConfiguration> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        await base.HandleChallengeAsync(properties);
        Response.StatusCode = StatusCodes.Status200OK;
        Response.Body = new MemoryStream(Encoding.UTF8.GetBytes("Hello Anonymous People"));
    }

    protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
    {
        await base.HandleForbiddenAsync(properties);
        Response.StatusCode = StatusCodes.Status403Forbidden;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey(Options.HeaderName))
        {
            return AuthenticateResult.NoResult();
        }

        if (!Request.Headers[Options.HeaderName].ToString()
                .Contains(Options.MustContain, StringComparison.CurrentCultureIgnoreCase))
            return AuthenticateResult.Fail("No Magic Word");

        var claims =
            new Claim[]
            {
                new Claim(
                    "MagicWord",
                    Request.Headers[Options.HeaderName].ToString()
                )
            };

        return AuthenticateResult.Success(
            new AuthenticationTicket(
                new ClaimsPrincipal(
                    new ClaimsIdentity(claims, this.Scheme.Name)
                ),
                Scheme.Name
            )
        );
    }
}
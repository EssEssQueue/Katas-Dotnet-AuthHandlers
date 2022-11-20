using Microsoft.AspNetCore.Authentication;

namespace AuthenticationAuthorization.WebApi.Authentication.Options;

public class CustomHttpHeaderAuthConfiguration : AuthenticationSchemeOptions
{
    public string HeaderName { get; set; } = string.Empty;
    public string? MustContain { get; set; }
}
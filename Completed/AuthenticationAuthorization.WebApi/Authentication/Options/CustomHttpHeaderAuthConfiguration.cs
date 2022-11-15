using Microsoft.AspNetCore.Authentication;

namespace AuthenticationAuthorization.WebApi.Authentication.Options;

public class CustomHttpHeaderAuthConfiguration : AuthenticationSchemeOptions
{
    public string HeaderName { get; set; }
    public string? MustBeginWith { get; set; }
    public string? MustContain { get; set; }
}
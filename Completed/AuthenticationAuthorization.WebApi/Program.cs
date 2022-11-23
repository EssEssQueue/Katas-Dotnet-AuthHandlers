using System.Collections.Immutable;
using AuthenticationAuthorization.WebApi.Authentication;
using AuthenticationAuthorization.WebApi.Authentication.Options;
using AuthenticationAuthorization.WebApi.Authorization;
using AuthenticationAuthorization.WebApi.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder);
ConfigureAuthentication(builder);
ConfigureAuthorization(builder);

var app = builder.Build();
ConfigurePipeline(app);
app.Run();

static void ConfigureServices(WebApplicationBuilder builder)
{
    //add services to container
    builder.Services.AddControllers();//And Add Authorization Middlewares
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

static void ConfigureAuthentication(WebApplicationBuilder builder)
{
    builder
        .Services
        .AddAuthentication(options =>
        {
            options.DefaultScheme = "CustomHttpHeaderAuthScheme";
        })
        .AddScheme<CustomHttpHeaderAuthConfiguration, CustomHttpHeaderAuthHandler>
        (
            "CustomHttpHeaderAuthScheme",
            options =>
            {
                builder.Configuration.Bind("CustomHttpHeaderAuth",options);
                options.ForwardChallenge = "OkChallengeAuthScheme";
            }
        )
        .AddScheme<OkChallengeAuthConfiguration, OkChallengeAuthHandler>("OkChallengeAuthScheme", _ => { });
}

static void ConfigureAuthorization(WebApplicationBuilder builder)
{
    builder.Services.AddScoped<IAuthorizationHandler, MagicWordAuthorizationHandler>();
    builder
        .Services
        .AddAuthorization(
            options =>
            {
                options.AddPolicy("VerifyMagicWord", policy =>
                {
                    policy.Requirements.Add(new MagicWordRequirement(builder.Configuration["MagicKeyword"]?.ToString() ?? "DefaultMagicWord"));
                });
                options.AddPolicy("VerifyAnotherMagicWord", policy =>
                {
                    policy.Requirements.Add(new MagicWordRequirement("HardCodedMagicWord"));
                });
            }
        );
}

static void ConfigurePipeline(WebApplication app)
{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthentication();
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers().RequireAuthorization("VerifyMagicWord");
}
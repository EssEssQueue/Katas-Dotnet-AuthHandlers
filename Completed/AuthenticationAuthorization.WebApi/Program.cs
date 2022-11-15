using System.Collections.Immutable;
using AuthenticationAuthorization.WebApi.Authentication;
using AuthenticationAuthorization.WebApi.Authentication.Options;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder);
ConfigureAuthentication(builder);

var app = builder.Build();
ConfigurePipeline(app);
app.Run();

static void ConfigureServices(WebApplicationBuilder builder)
{
    //add services to container
    builder.Services.AddControllers();
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
    app.MapControllers();
}
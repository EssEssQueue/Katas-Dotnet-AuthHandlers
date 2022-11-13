var builder = WebApplication.CreateBuilder(args);
ConfigureContainer(builder);

var app = builder.Build();
ConfigurePipeline(app);
app.Run();

static void ConfigureContainer(WebApplicationBuilder builder)
{
    //add services to container
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

static void ConfigurePipeline(WebApplication app)
{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
}
using FanficsWorld.WebAPI.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var loggerConfiguration = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(builder.Configuration);
Log.Logger = loggerConfiguration.CreateLogger();
Log.Information("Starting up");

try
{
    builder.Host.UseSerilog(Log.Logger);

    // Add services to the container.
    builder.Services.ConfigureServices(builder.Configuration);
    builder.Services.AddConfigurations(builder.Configuration);

    builder.Services.AddControllers();
    builder.Services.AddCors();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddAuthentication();
    builder.Services.ConfigureIdentity();
    builder.Services.ConfigureJwt(builder.Configuration);
    builder.Services.ConfigureSwagger();

    var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseFsExceptionHandler(app.Logger);
    }

    app.UseHttpsRedirection();

    app.UseCors(corsPolicyBuilder => corsPolicyBuilder
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin());
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    await app.SeedDatabaseAsync(app.Configuration, app.Logger);

    await app.RunAsync();
}
catch (Exception e)
{
    Log.Fatal(e, "An exception occured while starting up");
}
finally
{
    Log.Information("Closing app");
    Log.CloseAndFlush();
}
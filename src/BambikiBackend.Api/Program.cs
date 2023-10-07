using System.Reflection;
using BambikiBackend.AI;
using BambikiBackend.Api.Database;
using BambikiBackend.Api.Options;
using BambikiBackend.Api.Services;
using BambikiBackend.Api.Utils;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using Throw;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting...");
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Configuration.AddJsonFile("appsettings.local.json", true, true);

    builder.Host.UseSerilog((context, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration.GetSection("Logging").Throw().IfFalse(e => e.AsEnumerable().Any()).Value)
        , writeToProviders: false);

    // Options
    builder.Services.AddOptions<PostgresOptions>()
        .Bind(builder.Configuration.GetSection("Services:Postgres"))
        .ValidateOnStart();
    builder.Services.AddSingleton<IValidateOptions<PostgresOptions>, PostgresOptionsValidations>();

    builder.Services.AddLogging(loggingBuilder =>
    {
        loggingBuilder.ClearProviders();
        loggingBuilder.AddSerilog(dispose: true);
    });

    // Add services to the container.
    builder.Services.AddFluentValidationAutoValidation(opt =>
    {
        opt.EnableFormBindingSourceAutomaticValidation = true;
    });
    builder.Services.AddSingleton<FireRecognition>();
    builder.Services.AddScoped<ReportsService>();
    builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));

    builder.Services.AddDbContext<BambikiDatabaseContext>(opt =>
    {
        var config = builder.Configuration
            .GetSection("Services:Postgres")
            .Get<PostgresOptions>()
            .ThrowIfNull()
            .Value;
        new PostgresOptionsValidations().ValidateAndThrow(config);


        opt.UseNpgsql(config.ConnectionString);
        opt.EnableDetailedErrors();
        if (builder.Environment.IsDevelopment())
            opt.EnableSensitiveDataLogging();
    });

    builder.Services.AddControllers(opt =>
    {
        opt.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
    });
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();
    app.UseSerilogRequestLogging();

    app.UseCors(b =>
    {
        b.AllowAnyOrigin();
        b.AllowAnyHeader();
    });

    // Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    await using var scope = app.Services.CreateAsyncScope();
    await scope.ServiceProvider.GetRequiredService<BambikiDatabaseContext>().Database.MigrateAsync();

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
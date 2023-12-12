using BoardUserInterface.API.Logging;
using BoardUserInterface.API.SwaggerOptions;
using BoardUserInterface.FileService.Helpers.ExcelMetadata;
using BoardUserInterface.FileService.Helpers.VersionComparer;
using BoardUserInterface.FileService.Helpers.VersionComparer.VersionComparer;
using BoardUserInterface.FileService.Helpers.VersionValidator;
using BoardUserInterface.FileService.Service;
using BoardUserInterface.Repository;
using BoardUserInterface.Service.Template;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using Swashbuckle.AspNetCore.SwaggerGen;

// Configure Serilog
// Program.cs
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    // Use JsonFormatter with indentation (no need for additional parameters)
    .WriteTo.Sink(new CustomRollingFileSink(
        new JsonFormatter(renderMessage: true, formatProvider: null), // Corrected parameters
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs")
    ))
    .CreateLogger();

// Continue with the rest of the Program.cs setup...

var builder = WebApplication.CreateBuilder(args);

// Use Serilog for logging
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();

// Add API versioning configuration
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0); // Default version is set to 1.0
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;

});

// Existing code for Swagger setup
builder.Services.AddEndpointsApiExplorer();


// Register the API version description provider
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddSwaggerGen();

// Add health check services to the container.
builder.Services.AddHealthChecks();

// Inside Program.cs or Startup.cs in ConfigureServices method
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddTransient<IFileService, FileService >();
builder.Services.AddTransient<IExcelMetadataHelper, ExcelMetadataHelper>();
builder.Services.AddTransient<IVersionValidatorHelper, VersionValidatorHelper > ();

builder.Services.AddTransient<ITemplateService, TemplateService>();

builder.Services.AddSingleton<IRepositoryStorage>(provider => new RepositoryStorage("versions.json"));
builder.Services.AddSingleton<IVersionComparerHelper, VersionComparerHelper >();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


var app = builder.Build();

app.UseCors("AllowAll");

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
}


//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Map health check endpoint with custom response
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        var result = System.Text.Json.JsonSerializer.Serialize(
        new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description,
                exception = e.Value.Exception != null ? e.Value.Exception.Message : "none",
                duration = e.Value.Duration.ToString()
            })
        });
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(result);
    }
});


app.Run();

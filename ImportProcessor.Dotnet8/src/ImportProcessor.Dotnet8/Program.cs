using WC.Services.ImportProcessor.Dotnet8.Adapters.v1;
using WC.Services.ImportProcessor.Dotnet8.Adapters.v1.Interfaces;
using WC.Services.ImportProcessor.Dotnet8.Managers.v1;
using WC.Services.ImportProcessor.Dotnet8.Managers.v1.Interfaces;
using WC.Services.ImportProcessor.Dotnet8.Models.v1;
using log4net.Repository;
using System.Reflection;
using Hyland.Unity;
using WC.Services.ImportProcessor.Dotnet8.Connection.Interfaces;
using WC.Services.ImportProcessor.Dotnet8.Connection;


var builder = WebApplication.CreateBuilder(args);

// Set the content root to the current directory
builder.Host.UseContentRoot(Directory.GetCurrentDirectory());

// Set up the configuration
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// Add the configuration to the services
builder.Services.AddSingleton<IConfiguration>(config);

// Add services to the container.
builder.Services.AddControllers();

// Configure the HTTP request pipeline.
// Configure log4net
ILoggerRepository repository = log4net.LogManager.GetRepository(Assembly.GetCallingAssembly());
var fileInfo = new FileInfo(@"log4net.config");
log4net.Config.XmlConfigurator.Configure(repository, fileInfo);
builder.Services.AddSingleton<log4net.ILog>(log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType));
builder.Services.AddScoped<IApplicationConnectionAdapter<Application>, OnBaseConnectionAdapter>();
builder.Services.AddScoped<IWorkViewAdapter, WorkviewAdapter>();
builder.Services.AddScoped<IDocumentAdapter, DocumentAdapter>();
builder.Services.AddScoped<IFileAdapter, WindowsFileAdapter>();
builder.Services.AddScoped<IKeywordAdapter<Hyland.Unity.Keyword>, KeywordAdapter>();
builder.Services.AddScoped<IImportProcessorManager<ImportProcessorResponse>, ImportProcessorManager>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("../swagger/v1/swagger.json", "Customers API V1");
    });
}
app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();

using log4net;
using log4net.Config;
using log4net.Repository;
using System.Reflection;
using WC.Services.CreateGrievanceAppeals.Dotnet8.Manager;

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


// Configure log4net
ILoggerRepository repository = log4net.LogManager.GetRepository(Assembly.GetCallingAssembly());
var fileInfo = new FileInfo(@"log4net.config");
log4net.Config.XmlConfigurator.Configure(repository, fileInfo);


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<log4net.ILog>(log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType));
builder.Services.AddScoped<GrievanceAppealManager>();

var app = builder.Build();
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("../swagger/v1/swagger.json", "Customers API V1");
                });
            }
// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.Run();

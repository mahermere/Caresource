using WC.Services.OnBase.Dotnet8.Managers.v1;
using log4net.Repository;
using System.Reflection;
using WC.Services.OnBase.Dotnet8.Adapters.Interfaces.v1;
using WC.Services.OnBase.Dotnet8.Managers.Interfaces.v1;
using WC.Services.OnBase.Dotnet8.Adapters.v1;
using Microsoft.OpenApi.Models;
using WC.Services.OnBase.Dotnet8.Authorization;
using WC.Services.OnBase.Dotnet8.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using WC.Services.OnBase.Dotnet8.Middlewares;

var builder = WebApplication.CreateBuilder(args);
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
builder.Services.AddSingleton<log4net.ILog>(log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType));

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{

    options.AddSecurityDefinition("basicAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "basic"
    });
    options.AddSecurityDefinition("apiAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Name = "secret-key",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "basicAuth" }
                    },
                    new string[]{}
                }

            });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "apiAuth" }
                    },
                    new string[]{}
                }
            });
});

builder.Services.AddSingleton<log4net.ILog>(log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType));
builder.Services.AddScoped<OnBaseApplicationAbstractFactory, OnBaseUserApplicationFactory>();
builder.Services.AddScoped<IRepository, OnBaseRepository>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IDocumentAdapter, DocumentAdapter>();
builder.Services.AddScoped<IDocumentManager, DocumentManager>();
builder.Services.AddScoped<IWorkViewAdapter, WorkViewAdapter>();
builder.Services.AddScoped<IWorkViewManager, WorkViewManager>();

//Add Authentication
builder.Services.AddAuthentication("BasicAuthentication")
        .AddScheme<AuthenticationSchemeOptions, OnBaseAuthenticationHandler>
        ("BasicAuthentication", null);


// Add Authorization policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OnBaseAuthorization", policy =>
    {
        policy.Requirements.Add(new OnBaseAuthorizationRequirement(config));
    });

});

builder.Services.AddSingleton<IAuthorizationHandler, OnBaseAuthorizationHandler>();

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<OnbaseSetup>();

app.Run();
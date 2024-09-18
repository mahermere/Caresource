using log4net.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using System.Reflection;
using WC.Services.HplcManagement.Dotnet8.Authorization;
using WC.Services.HplcManagement.Dotnet8.Middlewares;
using WC.Services.HplcManagement.Dotnet8.Repository;
using WC.Services.HplcManagement.Dotnet8.Adapters.WorkView;
using WC.Services.HplcManagement.Dotnet8.Managers;
using WC.Services.HplcManagement.Dotnet8.Managers.Interfaces;
using WC.Services.HplcManagement.Dotnet8.Models;
using WC.Services.HplcManagement.Dotnet8.Mappers.Interfaces;
using WC.Services.HplcManagement.Dotnet8.Mappers;
using Object = Hyland.Unity.WorkView.Object;
using static WC.Services.HplcManagement.Dotnet8.Models.Constants;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Set up the configuration
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// Add the configuration to the services
builder.Services.AddSingleton<IConfiguration>(config);

// Configure the HTTP request pipeline.
// Configure log4net
ILoggerRepository repository = log4net.LogManager.GetRepository(Assembly.GetCallingAssembly());
var fileInfo = new FileInfo(@"log4net.config");
log4net.Config.XmlConfigurator.Configure(repository, fileInfo);
builder.Services.AddSingleton<log4net.ILog>(log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType));
builder.Services.AddMemoryCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

builder.Services.AddScoped<OnBaseApplicationAbstractFactory, OnBaseUserApplicationFactory>();
builder.Services.AddScoped<IRepository, OnBaseRepository>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IHttpRequestResolver, HttpRequestResolver>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IModelMapper<Data, Object>, WorkViewLocationToData>();
builder.Services.AddScoped<IModelMapper<Data, Object>, WorkViewProductToData>();
builder.Services.AddScoped<IModelMapper<Data, Object>, WorkViewPhoneToData>();
builder.Services.AddScoped<IModelMapper<Data, Object>, WorkViewProviderToData>();
builder.Services.AddScoped<IModelMapper<Data, Object>, WorkViewRequestToData>();
builder.Services.AddScoped<IModelMapper<Data, Object>, WorkViewStateToData>();
builder.Services.AddScoped<IModelMapper<Data, Object>, WorkViewTinToData>();
builder.Services.AddScoped<IAdapter, Adapter>();
builder.Services.AddScoped<IRequestManager, RequestManager>();
builder.Services.AddScoped<IDataSetManager, DataSetManager>();


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

Constants.SetServiceProvider(app.Services);

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<OnbaseSetup>();

app.Run();

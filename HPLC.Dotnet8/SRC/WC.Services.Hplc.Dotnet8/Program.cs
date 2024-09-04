using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using System.Reflection;
using WC.Services.Hplc.Dotnet8.Adapters.WorkView;
using WC.Services.Hplc.Dotnet8.Adapters.WorkView.Interfaces;
using WC.Services.Hplc.Dotnet8.Authorization;
using WC.Services.Hplc.Dotnet8.Managers;
using WC.Services.Hplc.Dotnet8.Managers.Interfaces;
using WC.Services.Hplc.Dotnet8.Mappers;
using WC.Services.Hplc.Dotnet8.Mappers.Interfaces;
using WC.Services.Hplc.Dotnet8.Models;
using WC.Services.Hplc.Dotnet8.Repository;
using WC.Services.Hplc.Dotnet8.Middlewares;

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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

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
builder.Services.AddScoped<IRequestManager, RequestManager>();
builder.Services.AddScoped<IDataSetManager, DataSetManager>();
builder.Services.AddScoped<IAdapter, Adapter>();
builder.Services.AddScoped<IModelMapper<WorkViewObject, Request>, WorkViewRequestClassRequest>();
builder.Services.AddScoped<IModelMapper<WorkViewObject, Provider>, WorkViewProviderClassProvider>();
builder.Services.AddScoped<IHieModelMapper<WorkViewObject, Request>, WorkViewRequestHieClassRequest>();
builder.Services.AddScoped<IHieModelMapper<WorkViewObject, Provider>, WorkViewProviderHieClassProvider>();
builder.Services.AddScoped<IModelMapper<WorkViewObject, Product>, WorkViewProductClassProduct>();
builder.Services.AddScoped<IModelMapper<WorkViewObject, State>, WorkViewStateClassState>();
builder.Services.AddScoped<IModelMapper<WorkViewObject, Tin>, WorkViewTinClassTin>();
builder.Services.AddScoped<IModelMapper<WorkViewObject, Location>, WorkViewLocationClassLocation>();
builder.Services.AddScoped<IModelMapper<WorkViewObject, Phone>, WorkViewPhoneClassPhone>();
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

//
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

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<OnbaseSetup>();

app.Run();

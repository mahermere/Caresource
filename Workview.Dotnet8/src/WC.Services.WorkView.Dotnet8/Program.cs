using log4net.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using System.Reflection;
using WC.Services.WorkView.Dotnet8.Adapters.v5;
using WC.Services.WorkView.Dotnet8.Authorization;
using WC.Services.WorkView.Dotnet8.Managers.v5;
using WC.Services.WorkView.Dotnet8.Mappers;
using WC.Services.WorkView.Dotnet8.Mappers.v5;
using WC.Services.WorkView.Dotnet8.Mappers.v5.Interfaces;
using WC.Services.WorkView.Dotnet8.Middlewares;
using WC.Services.WorkView.Dotnet8.Models.v5;
using WC.Services.WorkView.Dotnet8.Repository;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Set up the configuration
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// Configure log4net
ILoggerRepository repository = log4net.LogManager.GetRepository(Assembly.GetCallingAssembly());
var fileInfo = new FileInfo(@"log4net.config");
log4net.Config.XmlConfigurator.Configure(repository, fileInfo);

// Add the configuration to the services
builder.Services.AddSingleton<IConfiguration>(config);
builder.Services.AddSingleton<log4net.ILog>(log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType));

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
builder.Services.AddScoped<IModelMapper<WorkViewObject, WorkViewBaseObject>,WorkViewObjectModelMapper<WorkViewBaseObject>>();
builder.Services.AddScoped<IRetrieveAdapter, RetrieveAdapter>();
builder.Services.AddScoped<IRetrieveManager, RetrieveManager>();
builder.Services.AddScoped<ICreateAdapter, CreateAdapter>();
builder.Services.AddScoped<ICreateManager, CreateManager>();
builder.Services.AddScoped<ISearchAdapter, SearchAdapter>();
builder.Services.AddScoped<ISearchManager, SearchManager>();
builder.Services.AddScoped<IDataSetAdapter, DataSetAdapter>();
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
    app.UseSwaggerUI();
    //app.UseSwaggerUI(c =>
    //{
    //    c.SwaggerEndpoint("../swagger/v1/swagger.json", "Customers API V1");
    //});
}
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<OnbaseSetup>();

app.Run();
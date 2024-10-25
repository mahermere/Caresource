using log4net.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Reflection;
using WC.Services.Document.Dotnet8.Adapters.v6.Interfaces;
using WC.Services.Document.Dotnet8.Adapters.v6;
using WC.Services.Document.Dotnet8.Authorization;
using WC.Services.Document.Dotnet8.Middlewares;
using Microsoft.OpenApi.Models;
using WC.Services.Document.Dotnet8.Repository;
using WC.Services.Document.Dotnet8.Models.v6;
using WC.Services.Document.Dotnet8.Managers.v6;
using WC.Services.Document.Dotnet8.Managers.v6.Interfaces;
using Hyland.Unity;
using Document = WC.Services.Document.Dotnet8.Models.v6.Document;

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

ILoggerRepository repository = log4net.LogManager.GetRepository(Assembly.GetCallingAssembly());
var fileInfo = new FileInfo(@"log4net.config");
log4net.Config.XmlConfigurator.Configure(repository, fileInfo);

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

builder.Services.AddSingleton<log4net.ILog>(log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType));
builder.Services.AddScoped<OnBaseApplicationAbstractFactory, OnBaseUserApplicationFactory>();
builder.Services.AddScoped<IRepository, OnBaseRepository>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IGetDocumentAdapter<OnBaseDocument>, OnBaseGetDocumentAdapter>();
builder.Services.AddScoped<IOnBaseAdapter, OnBaseAdapter>();
builder.Services.AddScoped<IOnBaseSqlAdapter<DocumentHeader>, OnBaseSqlAdapter>();
builder.Services.AddScoped<IDocumentManager, DocumentManager>();
builder.Services.AddScoped<ICreateDocumentManager<OnBaseDocument>, CreateDocumentManager>();
builder.Services.AddScoped<ISqlAdapter, SqlAdapter>();
builder.Services.AddScoped<IExportDocumentManager, ExportDocumentsManager>();
builder.Services.AddScoped<IGetDocumentManager<Document>, GetDocumentManager>();
builder.Services.AddScoped<IKeywordAdapter, KeywordAdapter>();
builder.Services.AddScoped<IKeywordManager, KeywordManager>();
builder.Services.AddScoped<IMemberManager, MemberManager>();
builder.Services.AddScoped<IProviderManager, ProviderManager>();
builder.Services.AddScoped<ICreateKeywordAdapter<Keyword>, OnBaseCreateKeywordAdapter>();
builder.Services.AddScoped<IFileAdapter, WindowsFileAdapter>();
builder.Services.AddScoped<ICreateDocumentAdapter, OnBaseCreateDocumentAdapter>();

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
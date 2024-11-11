// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    Program.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

using FXIAuthentication.Core.Http;
using Microsoft.OpenApi.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
	options.DescribeAllParametersInCamelCase();
	options.UseInlineDefinitionsForEnums();
	options.SwaggerDoc("v1",
		new OpenApiInfo { Version = "v1", Title = "Internal authentication api Service API" });
});

builder.Services.AddCareSourceDefaultWebApiConfiguration(builder.Configuration);
builder.Services.AddApiVersioning();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.AddCareSourceDefaultWebApiConfiguration(app.Environment);

app.UseSwaggerUI(options =>
{
	options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
	options.RoutePrefix = string.Empty;
});

app.Run();
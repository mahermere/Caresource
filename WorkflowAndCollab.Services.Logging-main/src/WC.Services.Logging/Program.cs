//  ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    WC.Services.Logging
//    Program.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace WC.Services.Logging
{
	using System.Text.Json.Serialization;
	using CareSource.WC.Core.Http;
	using Microsoft.AspNetCore.Mvc;
	using NLog.Extensions.Logging;
	using MvcJsonOptions = Microsoft.AspNetCore.Mvc.JsonOptions;

	public class Program
	{
		public static void Main(string[] args)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

			builder.Services.AddApiVersioning(setup =>
			{
				setup.DefaultApiVersion = new ApiVersion(1, 0);
				setup.AssumeDefaultVersionWhenUnspecified = true;
				setup.ReportApiVersions = true;
			});

			builder.Services.AddCareSourceDefaultWebApiConfiguration(builder.Configuration);
			// Add services to the container.

			builder.Services.Configure<JsonOptions>(o
				=> o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
			builder.Services.Configure<MvcJsonOptions>(o
				=> o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			WebApplication app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();

			app.MapControllers();

			NLog.LogManager.Configuration =
				new NLogLoggingConfiguration(app.Configuration.GetSection("NLog"));
			
			app.Run();
		}
	}
}
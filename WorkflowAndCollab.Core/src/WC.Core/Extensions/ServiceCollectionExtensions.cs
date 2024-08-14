// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   ServiceCollectionExtensions.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Extensions
{
	using System;
	using System.IO;
	using System.Linq;
	using CareSource.WC.Core.DependencyInjection;
	using CareSource.WC.Core.DependencyInjection.Interfaces;
	using CareSource.WC.Core.Diagnostics;
	using CareSource.WC.Core.Diagnostics.Models;
	using CareSource.WC.Core.Helpers;
	using CareSource.WC.Core.Helpers.Interfaces;
	using CareSource.WC.Core.Http.Swagger.Interfaces;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;
	using Swashbuckle.AspNetCore.SwaggerGen;

	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddCareSourceServices(
			this IServiceCollection services,
			IConfiguration configuration)
		{
			EnvironmentVariableHelper environmentVariableHelper = new EnvironmentVariableHelper();
			CareSourceEnvironmentHelper careSourceEnvironmentHelper =
				new CareSourceEnvironmentHelper(environmentVariableHelper);

			//Add all dependencies
			if (!services.Any(sc => sc.ServiceType == typeof(IAssemblyHelper)))
			{
				IReflectionHelper reflectionHelper =
					new ReflectionHelper(new AssemblyHelper(new FileHelper()));

				IEnvironmentContextFactory environmentContextFactory =
					new EnvironmentContextFactory(
						careSourceEnvironmentHelper,
						environmentVariableHelper);

				DependencyModuleLoader dependencyModuleLoader = new DependencyModuleLoader(
					reflectionHelper,
					environmentContextFactory);

				dependencyModuleLoader.LoadAllDependenciesFromModules(services);
			}

			return services;
		}

		public static IServiceCollection AddCareSourceLogging(
			this IServiceCollection services,
			IConfiguration configuration)
		{
			services.AddLogging(
				loggingBuilder =>
				{
					loggingBuilder.ClearProviders();

					IConfigurationSection loggingConfig = configuration.GetSection("Logging");

					if (loggingConfig.Exists())
					{
						loggingBuilder.AddConfiguration(loggingConfig);

						IConfigurationSection fileSection = loggingConfig.GetSection("File");
						if (fileSection.Exists())
						{
							var fileConfig = fileSection.Get<FileLoggerConfiguration>();

							if (!Directory.Exists(fileConfig.LoggingDirectory))
							{
								Directory.CreateDirectory(fileConfig.LoggingDirectory);
							}

							services.AddSingleton<ILoggerProvider>(
								p =>
									new FileLoggerProvider(
										p,
										fileConfig));
						}

						IConfigurationSection interactiveSection =
							loggingConfig.GetSection("Interactive");
						if (interactiveSection.Exists())
						{
							var interactiveConfig =
								interactiveSection.Get<InteractiveLoggerConfiguration>();

							services.AddSingleton<ILoggerProvider>(
								p =>
									new InteractiveLoggerProvider(
										interactiveConfig
									));
						}

						IConfigurationSection consoleSection = loggingConfig.GetSection("Console");
						if (consoleSection.Exists())
						{
							loggingBuilder.AddConsole();
						}

						IConfigurationSection debugSection = loggingConfig.GetSection("Debug");
						if (debugSection.Exists())
						{
							loggingBuilder.AddDebug();
						}
					}
				});

			return services;
		}

		public static IServiceCollection AddSwaggerConfigurations(
			this IServiceCollection services,
			IConfiguration configuration,
			Action<SwaggerGenOptions> setupAction = null)
		{
			ISwaggerConfigurationManager swaggerConfigManager =
				(ISwaggerConfigurationManager)services.BuildServiceProvider().GetService(
					typeof(ISwaggerConfigurationManager));

			if (swaggerConfigManager == null)
			{
				throw new Exception(
					$"Both types '{typeof(ISwaggerConfigurationManager).Name}' " +
					$"and '{typeof(ICareSourceEnvironmentHelper).Name}' must be correctly" +
					" dependency injected to add swagger configurations.");
			}

			swaggerConfigManager.Configure(services, configuration, setupAction);

			return services;
		}
	}
}
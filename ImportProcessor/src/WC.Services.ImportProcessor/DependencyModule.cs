namespace ImportProcessor
{

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;
	using CareSource.WC.OnBase.Core.DependencyInjection;
	using CareSource.WC.OnBase.Core.Diagnostics;
	using CareSource.WC.OnBase.Core.Http;
	using CareSource.WC.OnBase.Core.Http.Interfaces;
	using ImportProcessor.Adapters.v1;
	using ImportProcessor.Adapters.v1.Interfaces;
	using ImportProcessor.Managers.v1;
	using ImportProcessor.Managers.v1.Interfaces;
	using ImportProcessor.Models;
	using ImportProcessor.Models.v1;
	using Unity;
	using Unity.Lifetime;
	using ImportProcessorManager = ImportProcessor.Managers.v1.ImportProcessorManager;
	using DocumentAdapter = ImportProcessor.Adapters.v1.DocumentAdapter;
	using WorkviewAdapter = ImportProcessor.Adapters.v1.WorkviewAdapter;
	using CareSource.WC.OnBase.Core.Connection.Interfaces;
	using Hyland.Unity;


	/// <summary>
	/// Represents the data used to define a the dependency module
	/// </summary>
	/// <seealso cref="IDependencyModule" />
	public class DependencyModule : IDependencyModule
	{
		/// <summary>
		/// Loads the specified container.
		/// </summary>
		/// <param name="container">The container.</param>
		public void Load(IUnityContainer container)
		{
			log4net.Config.XmlConfigurator.Configure();

			container.RegisterType<
				IRequestAuthorizer,
				OnBaseRequestAuthorizer>();

			container.RegisterType<
				IImportProcessorManager<ImportProcessorResponse>,
				ImportProcessorManager>();

			container.RegisterType<
				IWorkViewAdapter,
				WorkviewAdapter>();

			container.RegisterType<
				IDocumentAdapter,
				DocumentAdapter>();

			container.RegisterType<
				IFileAdapter,
				WindowsFileAdapter>();

			container.RegisterType<
				IKeywordAdapter<Hyland.Unity.Keyword>,
				KeywordAdapter>();

			container.RegisterSingleton<ServiceLoggerProvider>();
			container.RegisterType<
					Microsoft.Extensions.Logging.ILogger,
					ServiceLogger>
				(new PerResolveLifetimeManager());
		}

		/// <summary>
		/// Gets the load order of the dependency module.
		/// </summary>
		public ushort LoadOrder => 500;
	}
}
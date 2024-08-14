//  ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    OnBase.Core
//    CoreDependencyModule.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace CareSource.WC.OnBase.Core.DependencyInjection
{
	using System;
	using CareSource.WC.OnBase.Core.Configuration;
	using CareSource.WC.OnBase.Core.Configuration.Interfaces;
	using CareSource.WC.OnBase.Core.Connection;
	using CareSource.WC.OnBase.Core.Connection.Interfaces;
	using CareSource.WC.OnBase.Core.Diagnostics;
	using CareSource.WC.OnBase.Core.Diagnostics.Interfaces;
	using CareSource.WC.OnBase.Core.Helpers;
	using CareSource.WC.OnBase.Core.Helpers.Interfaces;
	using CareSource.WC.OnBase.Core.Http;
	using CareSource.WC.OnBase.Core.Http.Interfaces;
	using CareSource.WC.OnBase.Core.Services;
	using CareSource.WC.OnBase.Core.Services.Interfaces;
	using CareSource.WC.OnBase.Core.Transaction;
	using CareSource.WC.OnBase.Core.Transaction.Interfaces;
	using Hyland.Unity;
	using Unity;
	using Unity.Lifetime;

	public class CoreDependencyModule : IDependencyModule
	{
		public ushort LoadOrder => 50;

		public void Load(IUnityContainer container)
		{
			container.RegisterType<ITransactionContextManager, TransactionContextManager>(
				new HierarchicalLifetimeManager());

			container.RegisterType<IEventContextManager, HttpEventContextManager>();
			container.RegisterType<IHttpTransactionContextParser, HttpTransactionContextParser>();

			container.RegisterSingleton<IAssemblyHelper, AssemblyHelper>();
			container.RegisterSingleton<IJsonSerializerHelper, JsonSerializerHelper>();
			container.RegisterSingleton<IXmlSerializerHelper, XmlSerializerHelper>();

			container.RegisterType<ISettingsAdapter, AppSettingsAdapter>();

			container.RegisterType<ILogger, Log4NetLogger>();
			container.RegisterType<Microsoft.Extensions.Logging.ILogger, ServiceLogger>();

			container.RegisterType<IApplicationConnectionAdapter<Application>,
				OnBaseConnectionAdapter>(new HierarchicalLifetimeManager());

			container.RegisterType<IRequestAuthorizer, OnBaseRequestAuthorizer>();
			container.RegisterSingleton<IHttpStatusExceptionParser, HttpStatusExceptionParser>();

			container.RegisterType<IRestClient, RestClient>();
		}
	}
}
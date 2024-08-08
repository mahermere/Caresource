// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Messaging
//   DependencyModule.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

using CareSource.WC.OnBase.Core.DependencyInjection;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(UnityWebApiActivator)
		, nameof(UnityWebApiActivator.Start))]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(UnityWebApiActivator)
		, nameof(UnityWebApiActivator.Shutdown))]

namespace HplcManagement
{
	using System.Runtime.Caching;
	using CareSource.WC.OnBase.Core.Connection.Interfaces;
	using CareSource.WC.OnBase.Core.DependencyInjection;
	using CareSource.WC.OnBase.Core.Http;
	using CareSource.WC.OnBase.Core.Http.Interfaces;
	using HplcManagement.Mappers.v1;
	using HplcManagement.Mappers.v1.Interfaces;
	using HplcManagement.Models;
	using HplcManagement.Models.v1;
	using Hyland.Unity.WorkView;
	using Microsoft.Extensions.Logging;
	using Unity;
	using Unity.Lifetime;

	/// <summary>
	/// Functions describing a CareSource.WC.Services.Hplc.DependencyModule object.
	/// </summary>
	/// <seealso cref="IDependencyModule" />
	public class DependencyModule : IDependencyModule
	{
		/// <summary>
		/// Loads the specified dependency collection.
		/// </summary>
		/// <param name="container"></param>
		public void Load(IUnityContainer container)
		{
			container.RegisterInstance(typeof(MemoryCache), MemoryCache.Default);

			container.RegisterSingleton<IHttpStatusExceptionParser, HttpStatusExceptionParser>();
			container.RegisterType<IHttpRequestResolver, HttpRequestResolver>();
			container.RegisterType<ILogger, ServiceLogger>();
			container.RegisterType<IApplicationConnectionAdapter<Hyland.Unity.Application>,
				OnBaseConnectionAdapter>(new HierarchicalLifetimeManager());
			LoadV1(container);
		}

		/// <summary>
		/// Loads the v1 dependency objects into the container.
		/// </summary>
		/// <param name="container">The container.</param>

		/// <summary>
		/// Loads the v1 dependency objects into the container.
		/// </summary>
		/// <param name="container">The container.</param>
		private void LoadV1(IUnityContainer container)
		{
			container.RegisterType<Managers.v1.IRequestManager, Managers.v1.RequestManager>();
			container.RegisterType<Adapters.v1.WorkView.IAdapter, Adapters.v1.WorkView.Adapter>();
			container.RegisterType<Managers.v1.IDataSetManager, Managers.v1.DataSetManager>();

			container.RegisterType<ILogger, ServiceLogger>();

			container.RegisterType<
				IModelMapper<Data, Object>,
				WorkViewRequestToData>();


		}

		/// <summary>
		/// Gets the load order.
		/// </summary>
		public ushort LoadOrder => 500;
	}
}
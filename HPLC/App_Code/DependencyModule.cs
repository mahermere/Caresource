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

namespace WC.Services.Hplc
{
	using System.Runtime.Caching;
	using CareSource.WC.OnBase.Core.Http;
	using CareSource.WC.OnBase.Core.Http.Interfaces;
	using Unity;
	using WC.Services.Hplc.Mappers.v2;
	using WC.Services.Hplc.Models;
	using WC.Services.Hplc.Models.v2;

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
			LoadV1(container);
			LoadV2(container);
		}

		/// <summary>
		/// Loads the v1 dependency objects into the container.
		/// </summary>
		/// <param name="container">The container.</param>
		private void LoadV1(IUnityContainer container)
		{
			container.RegisterType<Managers.v1.IRequestManager, Managers.v1.RequestManager>();
			container.RegisterType<Adapters.v1.WorkView.IAdapter, Adapters.v1.WorkView.Adapter>();
			container.RegisterType<Managers.v1.IDataSetManager, Managers.v1.DataSetManager>();
			container.RegisterType<Adapters.v1.WorkView.IRestClient, Adapters.v1.WorkView.RestClient>();
		}

		/// <summary>
		/// Loads the v2 dependency objects into the container.
		/// </summary>
		/// <param name="container">The container.</param>
		private void LoadV2(IUnityContainer container)
		{
			container.RegisterType<Managers.v2.IRequestManager, Managers.v2.RequestManager>();
			container.RegisterType<Adapters.v2.WorkView.IAdapter, Adapters.v2.WorkView.Adapter>();
			container.RegisterType<Managers.v2.IDataSetManager, Managers.v2.DataSetManager>();

			container.RegisterType<
				IModelMapper<WorkViewObject, Request>,
				WorkViewRequestClassRequest>();

			container.RegisterType<
				IModelMapper<WorkViewObject, Provider>,
				WorkViewProviderClassProvider>();

			container.RegisterType<
				IHieModelMapper<WorkViewObject, Request>,
				WorkViewRequestHieClassRequest>();

			container.RegisterType<
				IHieModelMapper<WorkViewObject, Provider>,
				WorkViewProviderHieClassProvider>();

			container.RegisterType<
				IModelMapper<WorkViewObject, Product>,
				WorkViewProductClassProduct>();

			container.RegisterType<
				IModelMapper<WorkViewObject, State>,
				WorkViewStateClassState>();

			container.RegisterType<
				IModelMapper<WorkViewObject, Tin>,
				WorkViewTinClassTin>();

			container.RegisterType<
				IModelMapper<WorkViewObject, Location>,
				WorkViewLocationClassLocation>();

			container.RegisterType<
				IModelMapper<WorkViewObject, Phone>,
				WorkViewPhoneClassPhone>();
		}

		/// <summary>
		/// Gets the load order.
		/// </summary>
		public ushort LoadOrder => 500;
	}
}
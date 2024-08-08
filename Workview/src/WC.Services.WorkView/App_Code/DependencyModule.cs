// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   Workview
//   DependencyModule.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView
{
	using System.Collections.Generic;
	using CareSource.WC.Entities.WorkView;
	using CareSource.WC.Entities.Workview.v2;
	using CareSource.WC.OnBase.Core.DependencyInjection;
	using CareSource.WC.OnBase.Core.Diagnostics;
	using CareSource.WC.Services.WorkView.Adapters;
	using CareSource.WC.Services.WorkView.Adapters.v1;
	using CareSource.WC.Services.WorkView.Managers;
	using CareSource.WC.Services.WorkView.Managers.v1;
	using CareSource.WC.Services.WorkView.Mappers.v2;
	using CareSource.WC.Services.WorkView.Models.v5;
	using Hyland.Unity.WorkView;
	using Unity;
	using Unity.Lifetime;
	using WorkviewObject = CareSource.WC.Entities.WorkView.WorkviewObject;
	using WorkviewObjectHylandMapper = Mappers.WorkviewObjectHylandMapper;
	using WorkviewObjectRequestMapper = Mappers.WorkviewObjectRequestMapper;

	public class DependencyModule : IDependencyModule
	{
		public ushort LoadOrder => 500;

		public void Load(IUnityContainer container)
		{
			container.RegisterSingleton<ServiceLoggerProvider>();
			container.RegisterType<
					Microsoft.Extensions.Logging.ILogger,
					ServiceLogger>
				(new PerResolveLifetimeManager());

			ConfigureVersion1(ref container);

			ConfigureVersion2(ref container);

			ConfigureVersion4(ref container);
			ConfigureVersion5(ref container);
		}

		private void ConfigureVersion1(ref IUnityContainer container)
		{
			container
				.RegisterType<Mappers.IModelMapper<
						WorkviewObject, System.Tuple<Application, Object>>,
					WorkviewObjectHylandMapper>();

			container
				.RegisterType<Mappers.IModelMapper<
						WorkviewObject,
						WorkviewObjectRequest>,
					WorkviewObjectRequestMapper>();

			container
				.RegisterType<
					IWorkviewObjectAdapter<WorkviewObject>,
					WorkviewObjectAdapter>();

			container
				.RegisterType<
					IWorkViewApplicationManager,
					WorkViewApplicationManager>();
		}

		private void ConfigureVersion2(ref IUnityContainer container)
		{
			container
				.RegisterType<Mappers.v2.IModelMapper<
						Entities.Workview.v2.WorkviewObject, System.Tuple<Application, Object>>,
					Mappers.v2.WorkviewObjectHylandMapper>();

			container
				.RegisterType<Mappers.v2.IModelMapper<
						Entities.Workview.v2.WorkviewObject,
						Entities.Workview.v2.WorkviewObjectGetRequest>,
					Mappers.v2.WorkviewObjectRequestMapper>();

			container
				.RegisterType<Mappers.v2.IModelMapper<
						IEnumerable<Entities.Workview.v2.WorkviewObject>,
						WorkviewObjectBatchRequest>,
					WorkViewPostObjectRequestMapper>();

			container
				.RegisterType<
					Managers.v2.IWorkViewApplicationManager,
					Managers.v2.WorkViewApplicationManager>();

			container
				.RegisterType<Adapters.v2.IWorkviewObjectAdapter<Entities.Workview.v2.WorkviewObject>,
					Adapters.v2.WorkviewObjectAdapter>();

			container
				.RegisterType<
					Managers.v2.IWorkViewApplicationManager,
					Managers.v2.WorkViewApplicationManager>();
		}

		private void ConfigureVersion4(ref IUnityContainer container)
		{
			container
				.RegisterType<
					IModelMapper<
						Entities.Workview.v2.WorkviewObject, System.Tuple<Application, Object>>,
					Mappers.v2.WorkviewObjectHylandMapper>();

			container
				.RegisterType<
					Mappers.v4.IModelMapper<
						Entities.Workview.v2.WorkviewObject,
						Entities.Workview.v2.WorkviewObjectGetRequest>,
					Mappers.v4.WorkviewObjectRequestMapper>();

			container
				.RegisterType<
					Mappers.v4.IModelMapper<
						IEnumerable<Entities.Workview.v2.WorkviewObject>,
						WorkviewObjectBatchRequest>,
					Mappers.v4.WorkViewPostObjectRequestMapper>();

			container
				.RegisterType<
					Managers.v4.IWorkViewApplicationManager,
					Managers.v4.WorkViewApplicationManager>();

			container
				.RegisterType<Adapters.v4.IWorkViewObjectAdapter<Entities.Workview.v2.WorkviewObject>,
					Adapters.v4.WorkViewObjectAdapter>();

			container
				.RegisterType<
					Managers.v4.IWorkViewApplicationManager,
					Managers.v4.WorkViewApplicationManager>();

			container.RegisterType<
					Managers.v4.IDataSetManager,
					Managers.v4.DataSetManager>();

			container
				.RegisterType<Mappers.v4.IModelMapper<
						Entities.Workview.v2.WorkviewObject, System.Tuple<Application, Object>>,
					Mappers.v4.WorkviewObjectHylandMapper>();
		}

		private void ConfigureVersion5(ref IUnityContainer container)
		{
			container.RegisterType<
				Mappers.v5.IModelMapper<WorkViewObject, WorkViewBaseObject>,
				Mappers.v5.WorkViewObjectModelMapper<WorkViewBaseObject>>();

			container.RegisterType<
				Managers.v5.IRetrieveManager,
				Managers.v5.RetrieveManager>();

			container.RegisterType<
				Adapters.v5.IRetrieveAdapter,
				Adapters.v5.RetrieveAdapter>();

			container.RegisterType<
				Adapters.v5.ICreateAdapter,
				Adapters.v5.CreateAdapter>();

			container.RegisterType<
				Managers.v5.ICreateManager,
				Managers.v5.CreateManager>();

			container.RegisterType<
				Adapters.v5.ISearchAdapter,
				Adapters.v5.SearchAdapter>();

			container.RegisterType<
				Managers.v5.ISearchManager,
				Managers.v5.SearchManager>();

			container.RegisterType<
				Managers.v5.IDataSetManager,
				Managers.v5.DataSetManager>();


			container.RegisterType<
				Adapters.v5.IDataSetAdapter,
				Adapters.v5.DataSetAdapter>();
		}
	}
}
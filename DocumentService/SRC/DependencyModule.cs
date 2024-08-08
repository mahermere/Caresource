// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Caresource.WC.Services.Document.WC.Services.Document
//   DependencyModule.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document
{
	using CareSource.WC.Services.Document;
	using System.Runtime.Caching;
	using CareSource.WC.Entities.Documents;
	using CareSource.WC.Entities.WorkView.v2;
	using CareSource.WC.OnBase.Core.DependencyInjection;
	using CareSource.WC.OnBase.Core.Http;
	using CareSource.WC.OnBase.Core.Http.Interfaces;
	using CareSource.WC.Services.Document.Adapters;
	using CareSource.WC.Services.Document.Managers;
	using CareSource.WC.Services.Document.Models;
	using Hyland.Unity;
	using Unity;
	using Unity.Lifetime;
	using Document = CareSource.WC.Entities.Documents.Document;
	using DocumentManager = CareSource.WC.Services.Document.Managers.DocumentManager;
	using OnBaseDocumentAdapter = CareSource.WC.Services.Document.Adapters.OnBaseDocumentAdapter;
	using OnBaseGetDocumentAdapter = Adapters.OnBaseGetDocumentAdapter;

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
			container.RegisterType<
				IRequestAuthorizer,
				OnBaseRequestAuthorizer>();

			 container.RegisterType<
				IMemberDocumentManager<DocumentHeader>,
				MemberDocumentManager>();

			container.RegisterType<
				IProviderDocumentManager<DocumentHeader>,
				ProviderDocumentManager>();

			container.RegisterType<Managers.IDocumentManager<DocumentHeader>,
				DocumentManager>();

			container.RegisterType<Adapters.ISearchDocumentAdapter<DocumentHeader>,
				OnBaseDocumentAdapter>();

			container.RegisterType<
				Adapters.IGetDocumentAdapter<OnBaseDocument>,
				OnBaseGetDocumentAdapter>();

			container.RegisterType<
				Adapters.IGetDocumentAdapter<OnBaseDocument>,
				Adapters.OnBaseGetDocumentAdapter>();

			container.RegisterType<
				Managers.IGetDocumentManager<Document>,
				Managers.GetDocumentManager>();

			container.RegisterType<
				IKeywordAdapter,
				KeywordAdapter>();

			container.RegisterType<
				IKeywordManager,
				KeywordManager>();

			container.RegisterType<
				ICreateDocumentManager<OnBaseDocument>,
				CreateDocumentManager>();

			container.RegisterType<
				ICreateDocumentAdapter,
				OnBaseCreateDocumentAdapter>();

			container.RegisterType<
				ICreateKeywordAdapter<Keyword>,
				OnBaseCreateKeywordAdapter>();

			container.RegisterType<
				IFileAdapter,
				WindowsFileAdapter>();

			container.RegisterSingleton<CareSource.WC.Services.Document.ServiceLoggerProvider>();
			container.RegisterType<
				Microsoft.Extensions.Logging.ILogger,
				CareSource.WC.Services.Document.ServiceLogger>
				(new PerResolveLifetimeManager());

			LoadV3(container);
			LoadV4(container);
			LoadV5(container);
			LoadV6(container);

		}

		private static void LoadV3(IUnityContainer container)
		{
			container.RegisterType<
				Managers.v3.IKeywordManager,
				Managers.v3.KeywordManager>();

			container.RegisterType<
				Adapters.v3.IKeywordAdapter,
				Adapters.v3.KeywordAdapter>();

			container.RegisterType<
				Adapters.v3.ICreateDocumentAdapter,
				Adapters.v3.OnBaseCreateDocumentAdapter>();

			container.RegisterType<
				Adapters.v3.IGetDocumentAdapter<Models.v3.OnBaseDocument>,
				Adapters.v3.OnBaseGetDocumentAdapter>();

			container.RegisterType<
				Adapters.v3.ISearchDocumentAdapter<DocumentHeader>,
				Adapters.v3.OnBaseDocumentAdapter>();

			container.RegisterType<
				Adapters.v3.IFileAdapter,
				Adapters.v3.WindowsFileAdapter>();

			container.RegisterType<
				Managers.v3.IDocumentManager<DocumentHeader>,
				Managers.v3.DocumentManager>();

			container.RegisterType<
				Managers.v3.IGetDocumentManager<Document>,
				Managers.v3.GetDocumentManager>();

			container.RegisterType<
				Managers.v3.ICreateDocumentManager<Models.v3.OnBaseDocument>,
				Managers.v3.CreateDocumentManager>();
		}

		/// <summary>
		/// Load Version 4 dependencies into the provided container.
		/// </summary>
		/// <param name="container">The container.</param>
		private static void LoadV4(IUnityContainer container)
		{
			container.RegisterType<
				Managers.v4.IClaimsManager<Models.v4.Document>,
				Managers.v4.ClaimsManager>();

			container.RegisterType<
				Adapters.v4.ISearchDocumentAdapter<DocumentHeader>,
				Adapters.v4.OnBaseDocumentAdapter>();

			container.RegisterType<
				Managers.v4.IMemberManager,
				Managers.v4.MemberManager>();

			container.RegisterType<
				Managers.v4.IProviderManager,
				Managers.v4.ProviderManager>();

			container.RegisterType<
				Adapters.v4.IGetDocumentAdapter<Models.v4.OnBaseDocument>,
				Adapters.v4.OnBaseGetDocumentAdapter>();

			container.RegisterType<
				Managers.v4.IGetDocumentManager<Models.v4.Document>,
				Managers.v4.GetDocumentManager>();

			container.RegisterType<
				Managers.v4.IDocumentManager<DocumentHeader>,
				Managers.v4.DocumentManager>();

			container.RegisterType<
				Adapters.v4.ISearchWorkViewAdapter<WorkViewObject>,
				Adapters.v4.WorkView.SearchWorkViewAdapter>();

		}

		/// <summary>
		/// Load Version 5 dependencies into the provided container.
		/// </summary>
		/// <param name="container">The container.</param>
		private static void LoadV5(IUnityContainer container)
		{
			container.RegisterInstance(
				typeof(MemoryCache),
				MemoryCache.Default);

			container.RegisterType<
				Managers.v5.IDocumentManager,
				Managers.v5.DocumentManager>();

			container.RegisterType<
				Managers.v5.ICreateDocumentManager<Models.v5.OnBaseDocument>,
				Managers.v5.CreateDocumentManager>();

			container.RegisterType<
				Adapters.v5.IOnBaseSqlAdapter<DocumentHeader>,
				Adapters.v5.OnBaseSqlAdapter>();

			container.RegisterType<
				Adapters.v5.IOnBaseAdapter,
				Adapters.v5.OnBaseAdapter>();

			container.RegisterType<
				Managers.v5.IMemberManager,
				Managers.v5.MemberManager>();

			container.RegisterType<
				Managers.v5.IProviderManager,
				Managers.v5.ProviderManager>();

			container.RegisterType<
				Adapters.v5.IGetDocumentAdapter<Models.v5.OnBaseDocument>,
				Adapters.v5.OnBaseGetDocumentAdapter>();

			container.RegisterType<
				Adapters.v5.IFileAdapter,
				Adapters.v5.WindowsFileAdapter>();

			container.RegisterType<
				Adapters.v5.ICreateDocumentAdapter,
				Adapters.v5.OnBaseCreateDocumentAdapter>();

			container.RegisterType<
				Adapters.v5.ICreateKeywordAdapter<Keyword>,
				Adapters.v5.OnBaseCreateKeywordAdapter>();

			container.RegisterType<
				Managers.v5.IKeywordManager,
				Managers.v5.KeywordManager>();

			container.RegisterType<
				Adapters.v5.IKeywordAdapter,
				Adapters.v5.KeywordAdapter>();

			container.RegisterType<
				Managers.v5.IGetDocumentManager<Models.v5.Document>,
				Managers.v5.GetDocumentManager>();
		}

		/// <summary>
		/// Load Version 6 dependencies into the provided container.
		/// </summary>
		/// <param name="container">The container.</param>
		private static void LoadV6(IUnityContainer container)
		{
			container.RegisterInstance(
				typeof(MemoryCache),
				MemoryCache.Default);

			container.RegisterType<
				Managers.v6.IDocumentManager,
				Managers.v6.DocumentManager>();

			container.RegisterType<
				Managers.v6.ICreateDocumentManager<Models.v6.OnBaseDocument>,
				Managers.v6.CreateDocumentManager>();

			container.RegisterType<
				Adapters.v6.IOnBaseSqlAdapter<Models.v6.DocumentHeader>,
				Adapters.v6.OnBaseSqlAdapter>();

			container.RegisterType<
				Adapters.v6.IOnBaseAdapter,
				Adapters.v6.OnBaseAdapter>();

			container.RegisterType<
				Managers.v6.IMemberManager,
				Managers.v6.MemberManager>();

			container.RegisterType<
				Managers.v6.IProviderManager,
				Managers.v6.ProviderManager>();

			container.RegisterType<
				Adapters.v6.IGetDocumentAdapter<Models.v6.OnBaseDocument>,
				Adapters.v6.OnBaseGetDocumentAdapter>();

			container.RegisterType<
				Adapters.v6.IFileAdapter,
				Adapters.v6.WindowsFileAdapter>();

			container.RegisterType<
				Adapters.v6.ICreateDocumentAdapter,
				Adapters.v6.OnBaseCreateDocumentAdapter>();

			container.RegisterType<
				Adapters.v6.ICreateKeywordAdapter<Keyword>,
				Adapters.v6.OnBaseCreateKeywordAdapter>();

			container.RegisterType<
				Managers.v6.IKeywordManager,
				Managers.v6.KeywordManager>();

			container.RegisterType<
				Adapters.v6.IKeywordAdapter,
				Adapters.v6.KeywordAdapter>();

			container.RegisterType<
				Managers.v6.IGetDocumentManager<Models.v6.Document>,
				Managers.v6.GetDocumentManager>();

			container.RegisterType<
				Adapters.v6.ISqlAdapter,
				Adapters.v6.SqlAdapter>();

			container.RegisterType<
				Managers.v6.IExportDocumentManager,
				Managers.v6.ExportDocumentsManager>();

		}
		/// <summary>
		/// Gets the load order of the dependency module.
		/// </summary>
		public ushort LoadOrder => 500;
	}
}
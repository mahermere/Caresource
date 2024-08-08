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
	using CareSource.WC.Entities.Documents;
	using CareSource.WC.Entities.WorkView.v2;
	using CareSource.WC.OnBase.Core.DependencyInjection;
	using CareSource.WC.OnBase.Core.Http;
	using CareSource.WC.OnBase.Core.Http.Interfaces;
	using CareSource.WC.Services.Document.Adapters.v4;
	using CareSource.WC.Services.Document.Adapters.v4.WorkView;
	using CareSource.WC.Services.Document.Adapters.v6;
	using CareSource.WC.Services.Document.Managers;
	using CareSource.WC.Services.Document.Managers.v4;
	using CareSource.WC.Services.Document.Models;
	using Hyland.Unity;
	using Unity;
	using Document = CareSource.WC.Entities.Documents.Document;
	using DocumentManager = CareSource.WC.Services.Document.Managers.DocumentManager;
	using ICreateDocumentAdapter = CareSource.WC.Services.Document.Adapters.ICreateDocumentAdapter;
	using IFileAdapter = CareSource.WC.Services.Document.Adapters.IFileAdapter;
	using IKeywordAdapter = CareSource.WC.Services.Document.Adapters.IKeywordAdapter;
	using KeywordAdapter = CareSource.WC.Services.Document.Adapters.KeywordAdapter;
	using OnBaseCreateDocumentAdapter = CareSource.WC.Services.Document.Adapters.OnBaseCreateDocumentAdapter;
	using OnBaseCreateKeywordAdapter = CareSource.WC.Services.Document.Adapters.OnBaseCreateKeywordAdapter;
	using OnBaseDocumentAdapter = CareSource.WC.Services.Document.Adapters.OnBaseDocumentAdapter;
	using OnBaseGetDocumentAdapter = Adapters.OnBaseGetDocumentAdapter;
	using WindowsFileAdapter = CareSource.WC.Services.Document.Adapters.WindowsFileAdapter;

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
				Adapters.v3.IKeywordAdapter,
				Adapters.v3.KeywordAdapter>();

			container.RegisterType<
				IKeywordManager,
				KeywordManager>();

			container.RegisterType<
				Managers.v3.IKeywordManager,
				Managers.v3.KeywordManager>();

			container.RegisterType<
				ICreateDocumentManager<OnBaseDocument>,
				CreateDocumentManager>();

			container.RegisterType<
				ICreateDocumentAdapter,
				OnBaseCreateDocumentAdapter>();

			container.RegisterType<Adapters.ICreateKeywordAdapter<Keyword>,
				OnBaseCreateKeywordAdapter>();

			container.RegisterType<
				IFileAdapter,
				WindowsFileAdapter>();

			container.RegisterType<
				ISearchWorkViewAdapter<WorkViewObject>,
				SearchWorkViewAdapter>();


			LoadV4(container);
			LoadV5(container);
		}



		/// <summary>
		/// Load Version 4 dependencies into the provided container.
		/// </summary>
		/// <param name="container">The container.</param>
		private static void LoadV4(IUnityContainer container)
		{
			container.RegisterType<
				IClaimsManager<Models.v4.Document>,
				ClaimsManager>();

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
		}


		/// <summary>
		/// Load Version 5 dependencies into the provided container.
		/// </summary>
		/// <param name="container">The container.</param>
		private static void LoadV5(IUnityContainer container)
		{
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
				Managers.v5.IClaimsManager<Models.v5.Document>,
				Managers.v5.ClaimsManager>();

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
		/// Gets the load order of the dependency module.
		/// </summary>
		public ushort LoadOrder => 500;
	}
}
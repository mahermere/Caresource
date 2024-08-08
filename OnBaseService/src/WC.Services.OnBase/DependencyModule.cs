// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   DependencyModule.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.OnBase
{
	using CareSource.WC.OnBase.Core.DependencyInjection;
	using CareSource.WC.OnBase.Core.Http.Interfaces;
	using CareSource.WC.Services.OnBase.Adapters.Interfaces.v1;
	using CareSource.WC.Services.OnBase.Adapters.v1;
	using CareSource.WC.Services.OnBase.Managers.Interfaces.v1;
	using CareSource.WC.Services.OnBase.Managers.v1;
	using log4net.Config;
	using Unity;

	/// <summary>
	///    Represents the data used to define a the dependency module
	/// </summary>
	/// <seealso cref="IDependencyModule" />
	public class DependencyModule : IDependencyModule
	{
		/// <summary>
		///    Loads the specified container.
		/// </summary>
		/// <param name="container">The container.</param>
		public void Load(IUnityContainer container)
		{
			XmlConfigurator.Configure();

			//container.RegisterType<SwaggerMiddelware>();
			container.RegisterType<IRequestAuthorizer, OnBaseRequestAuthorizer>();
			container.RegisterType<
				IRequestAuthorizer,
				OnBaseRequestAuthorizer>();

			container.RegisterType<
				IDocumentManager,
				DocumentManager>();

			container.RegisterType<
				IDocumentAdapter,
				DocumentAdapter>();

			container.RegisterType<
				IWorkViewManager,
				WorkViewManager>();

			container.RegisterType<
				IWorkViewAdapter,
				WorkViewAdapter>();
		}

		/// <summary>
		///    Gets the load order of the dependency module.
		/// </summary>
		public ushort LoadOrder => 500;
	}
}
using System;
using Unity;

namespace WorkFlowAndCollab.API.Appeal
{
	using CareSource.WC.Entities.Appeals;
	using CareSource.WC.Entities.WorkView;
	using System.Web.Http;
	using Unity.AspNet.WebApi;
	using CareSource.WC.OnBase.Core.Configuration;
	using WorkFlowAndCollab.API.OBAppeals.Managers;
	using WorkFlowAndCollab.API.OBAppeals.Managers.Interfaces;
	using WorkFlowAndCollab.Integrations.WorkViewObjects.Interfaces;
	using WorkFlowAndCollab.Integrations.WorkViewObjects;
	using System.Configuration;

	/// <summary>
	/// Specifies the Unity configuration for the main container.
	/// </summary>
	public static class UnityConfig
	{
		#region Unity Container
		private static Lazy<IUnityContainer> container =
		  new Lazy<IUnityContainer>(() =>
		  {
			  var container = new UnityContainer();
			  RegisterTypes(container);
			  return container;
		  });

		/// <summary>
		/// Configured Unity Container.
		/// </summary>
		public static IUnityContainer Container => container.Value;
		#endregion

		/// <summary>
		/// Registers the type mappings with the Unity container.
		/// </summary>
		/// <param name="container">The unity container to configure.</param>
		/// <remarks>
		/// There is no need to register concrete types such as controllers or
		/// API controllers (unless you want to change the defaults), as Unity
		/// allows resolving a concrete type even if it was not previously
		/// registered.
		/// </remarks>
		public static void RegisterTypes(IUnityContainer container)
		{
			// NOTE: To load from web.config uncomment the line below.
			// Make sure to add a Unity.Configuration to the using statements.
			// container.LoadConfiguration();

			// TODO: Register your type's mappings here.
			// container.RegisterType<IProductRepository, ProductRepository>();

			var onBaseConfiguration = new OnBaseConfiguration();
			onBaseConfiguration.DataSource = ConfigurationManager.AppSettings["OnBase.DataSource"];
			onBaseConfiguration.Domain = ConfigurationManager.AppSettings["OnBase.Domain"];
			onBaseConfiguration.Password = ConfigurationManager.AppSettings["OnBase.Password"];
			onBaseConfiguration.Url = ConfigurationManager.AppSettings["OnBase.Url"];
			onBaseConfiguration.UserName = ConfigurationManager.AppSettings["OnBase.UserName"];
			
			container.RegisterInstance(typeof(LoggingConfiguration), new LoggingConfiguration());
			container.RegisterInstance(typeof(OnBaseConfiguration), onBaseConfiguration);
			container.RegisterSingleton<IAppealsManager<Appeal>, AppealsManager>();
			container.RegisterSingleton<IProviderAppealsManager<Appeal>, ProviderAppealsManager>();
			container.RegisterSingleton<IMemberAppealsManager<Appeal>, MemberAppealsManager>();
			container.RegisterSingleton<IClaimAppealsManager<Appeal>, ClaimAppealsManager>();
			container.RegisterSingleton<IGrievancesManager<Appeal>, GrievancesManager>();
			container.RegisterSingleton<IMemberGrievancesManager<Appeal>, MemberGrievancesManager>();
			container.RegisterSingleton<IProviderGrievancesManager<Appeal>, ProviderGrievancesManager>();
			//container.RegisterSingleton<IWorkViewObjectsManager<WorkViewObjectsHeader>, WorkViewObjectsManager>();
			container.RegisterSingleton<IWorkViewObjectsBroker<WorkViewObjectsHeader>, WorkViewObjectsBroker>();

			GlobalConfiguration.Configuration.DependencyResolver =
					new UnityDependencyResolver(container);

		}
	}
}
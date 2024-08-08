using System.Web.Http;
using CareSource.WC.OnBase.Core.DependencyInjection;
using Unity.AspNet.WebApi;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(WorkFlowAndCollab.API.OBAppeals.UnityWebApiActivator), nameof(WorkFlowAndCollab.API.OBAppeals.UnityWebApiActivator.Start))]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(WorkFlowAndCollab.API.OBAppeals.UnityWebApiActivator), nameof(WorkFlowAndCollab.API.OBAppeals.UnityWebApiActivator.Shutdown))]

namespace WorkFlowAndCollab.API.OBAppeals
{
    /// <summary>
    /// Provides the bootstrapping for integrating Unity with WebApi when it is hosted in ASP.NET.
    /// </summary>
    public static class UnityWebApiActivator
    {
        /// <summary>
        /// Integrates Unity when the application starts.
        /// </summary>
        public static void Start() 
        {
            // Use UnityHierarchicalDependencyResolver if you want to use
            // a new child container for each IHttpController resolution.
            // var resolver = new UnityHierarchicalDependencyResolver(UnityConfig.Container);
            var resolver = new UnityDependencyResolver(UnityConfig.Container);

            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }

        /// <summary>
        /// Disposes the Unity container when the application is shut down.
        /// </summary>
        public static void Shutdown()
        {
            UnityConfig.Container.Dispose();
        }
    }
}
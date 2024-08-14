using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using Unity;

namespace CareSource.WC.OnBase.Core.DependencyInjection
{
    public static class DependencyModuleLoader
    {
        public static void LoadAllDependenciesFromModules(IUnityContainer container)
        {
            IList<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

            IEnumerable<Type> dependencyModuleTypes = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => t != typeof(IDependencyModule)
						&& typeof(IDependencyModule).IsAssignableFrom(t));

            IEnumerable<IDependencyModule> dependencyModules =
                dependencyModuleTypes.Select(t => Activator.CreateInstance(t))
                .Cast<IDependencyModule>()
                .OrderBy(dm => dm.LoadOrder);
            
            foreach (IDependencyModule dependencyModule in dependencyModules)
            {
                dependencyModule.Load(container);
            }

            container.AddExtension(new Diagnostic());
        }

        public static TDependencyType GetDependency<TDependencyType>()
        {
            var registration = UnityConfig.Container.Registrations
                .FirstOrDefault(r => r.RegisteredType == typeof(TDependencyType));

            if (registration == null || registration.MappedToType == null)
                throw new Exception($"Could not find type '{typeof(TDependencyType).Name}' as a registered dependency.");

            return UnityConfig.Container.Resolve<TDependencyType>();
        }
    }
}

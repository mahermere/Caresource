using CareSource.WC.OnBase.Core.Helpers.Interfaces;
using System.Reflection;
using System.Web;

namespace CareSource.WC.OnBase.Core.Helpers
{
    public class AssemblyHelper : IAssemblyHelper
    {
        public Assembly GetWebEntryAssembly()
        {
            var entryAssembly = Assembly.GetEntryAssembly();

            if (entryAssembly != null)
                return entryAssembly;

            if (HttpContext.Current?.ApplicationInstance == null)
                return null;

            var type = HttpContext.Current.ApplicationInstance.GetType();
            while (type != null && type.Namespace == "ASP")
            {
                type = type.BaseType;
            }

            return type == null ? null : type.Assembly;
        }
    }
}

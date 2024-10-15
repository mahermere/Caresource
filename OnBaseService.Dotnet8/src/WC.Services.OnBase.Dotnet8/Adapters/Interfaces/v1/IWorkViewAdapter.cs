using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WC.Services.OnBase.Dotnet8.Adapters.Interfaces.v1
{
	using System.Collections.Generic;
    using WC.Services.OnBase.Dotnet8.Models.v1;

    public interface IWorkViewAdapter
	{
		/// <summary>
		///    Returns all applications.
		/// </summary>
		/// <returns></returns>
		IEnumerable<WVApplication> GetApplications();

		WVApplication GetApplicationById(long applicationId);

		WVApplication GetApplicationByName(string applicationName);

    }
}

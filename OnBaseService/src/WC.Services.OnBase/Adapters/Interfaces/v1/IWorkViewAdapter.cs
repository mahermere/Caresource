using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareSource.WC.Services.OnBase.Adapters.Interfaces.v1
{
	using System.Collections.Generic;
	using CareSource.WC.Services.OnBase.Models.v1;

	public interface IWorkViewAdapter
	{
		/// <summary>
		///    Returns all applications.
		/// </summary>
		/// <returns></returns>
		IEnumerable<WVApplication> GetApplications();
	}
}

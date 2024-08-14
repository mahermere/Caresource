// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   IAssemblyHelper.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Helpers.Interfaces
{
	using System.Collections.Generic;
	using System.Reflection;

	public interface IAssemblyHelper
	{
		IList<Assembly> GetAssemblies();

		void LoadAllAssembliesInApplicationDirectory();

		void LoadAssemblyFromAssemblyPath(
			string assemblyPath);

		string GetEntryAssemblyName();
	}
}
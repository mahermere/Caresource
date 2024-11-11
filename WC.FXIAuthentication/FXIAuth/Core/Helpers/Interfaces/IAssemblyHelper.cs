// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    IAssemblyHelper.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Helpers;

using System.Reflection;

public interface IAssemblyHelper
{
	IList<Assembly> GetAssemblies();

	void LoadAllAssembliesInApplicationDirectory();

	void LoadAssemblyFromAssemblyPath(
		string assemblyPath);

	string GetEntryAssemblyName();
}
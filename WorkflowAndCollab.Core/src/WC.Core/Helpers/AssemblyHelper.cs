// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   AssemblyHelper.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Helpers
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using CareSource.WC.Core.DependencyInjection;
	using CareSource.WC.Core.Helpers.Interfaces;
	using Microsoft.Extensions.DependencyModel;

	[DependencyInjectionDependency]
	public class AssemblyHelper : IAssemblyHelper
	{
		private readonly IFileHelper _fileHelper;

		protected IList<Assembly> Assemblies;

		protected IList<string> FailedToLoadAssemblies;

		public AssemblyHelper(
			IFileHelper fileHelper)
		{
			_fileHelper = fileHelper;

			FailedToLoadAssemblies = new List<string>();

			if (null != DependencyContext.Default)
			{
				Assemblies = DependencyContext.Default?.RuntimeLibraries
					?.Select(a => GetAssemblyFromAssemblyName(a.Name))
					.Where(a => null != a)
					.ToList();
			}
			else
			{
				Assemblies = AppDomain.CurrentDomain.GetAssemblies()
					.ToList();
			}
		}

		public IList<Assembly> GetAssemblies() => Assemblies;

		public void LoadAllAssembliesInApplicationDirectory()
		{
			string applicationDirectory = _fileHelper.GetApplicationDirectory();

			List<string> files = Directory.GetFiles(
					applicationDirectory,
					"*.dll",
					SearchOption.AllDirectories)
				.ToList();

			files.AddRange(
				Directory.GetFiles(
					applicationDirectory,
					"*.exe",
					SearchOption.AllDirectories));

			foreach (string assemblyPath in files)
			{
				LoadAssemblyFromAssemblyPath(assemblyPath);
			}
		}

		public void LoadAssemblyFromAssemblyPath(
			string assemblyPath)
		{
			IEnumerable<string> assemblies = Assemblies.Select(
				a => a.GetName()
					.Name);

			if (!assemblies.Contains(Path.GetFileNameWithoutExtension(assemblyPath)))
			{
				Assembly assembly = Assembly.LoadFile(assemblyPath);
				Assemblies.Add(assembly);
			}
		}

		public string GetEntryAssemblyName() => Assembly.GetEntryAssembly()
			.FullName;

		protected virtual Assembly GetAssemblyFromAssemblyName(
			string assemblyName)
		{
			Assembly assembly = null;

			try
			{
				assembly = Assembly.Load(new AssemblyName(assemblyName));
			}
			catch (Exception)
			{
				FailedToLoadAssemblies.Add(assemblyName);
			}

			return assembly;
		}
	}
}
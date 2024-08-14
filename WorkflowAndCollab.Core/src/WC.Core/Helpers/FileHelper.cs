// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   FileHelper.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Helpers
{
	using System.IO;
	using CareSource.WC.Core.DependencyInjection;
	using CareSource.WC.Core.Helpers.Interfaces;
	using Microsoft.Extensions.PlatformAbstractions;

	[DependencyInjectionDependency]
	public class FileHelper : IFileHelper
	{
		public string GetApplicationDirectory()
			=> PlatformServices.Default.Application.ApplicationBasePath;

		public bool DirectoryExists(
			string path) => Directory.Exists(path);
	}
}
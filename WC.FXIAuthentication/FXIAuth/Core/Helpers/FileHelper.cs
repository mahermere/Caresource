// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    FileHelper.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Helpers;

using FXIAuthentication.Core.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;

[DependencyInjectionDependency]
public class FileHelper : IFileHelper
{
	public string GetApplicationDirectory()
		=> PlatformServices.Default.Application.ApplicationBasePath;

	public bool DirectoryExists(
		string path) => Directory.Exists(path);
}
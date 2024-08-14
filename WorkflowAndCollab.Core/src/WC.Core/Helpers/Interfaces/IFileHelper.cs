// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   IFileHelper.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Helpers.Interfaces
{
	public interface IFileHelper
	{
		string GetApplicationDirectory();

		bool DirectoryExists(
			string path);
	}
}
// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   IFileAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Adapters.v6
{
	using System.IO;

	public interface IFileAdapter
	{
		void DeleteFile(string fullPath);

		bool FileExists(string fullFilePath);

		FileInfo GetFileInfo(string path);

		string GetFileNameWithoutExtension(string fullFilePath);

		DirectoryInfo PathCreate(string path);
		bool PathExists(string path);

		Stream ReadFileContents(string path);
	}
}
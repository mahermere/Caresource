﻿namespace CareSource.WC.Services.Document.Adapters
{
	using System.IO;

	public interface IFileAdapter
	{
		bool PathExists(string path);

		DirectoryInfo PathCreate(string path);

		bool FileExists(string fullFilePath);

		FileInfo GetFileInfo(string path);

		string GetFileNameWithoutExtension(string fullFilePath);

		Stream ReadFileContents(string path);

		void DeleteFile(string fullPath);
	}
}
// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   WindowsFileAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.ImportProcessor.Dotnet8.Adapters.v1
{
	using System;
	using System.IO;
    using WC.Services.ImportProcessor.Dotnet8.Adapters.v1.Interfaces;
    using Microsoft.Extensions.Logging;

	public class WindowsFileAdapter : IFileAdapter
	{
		private readonly log4net.ILog _logger;

		public WindowsFileAdapter(log4net.ILog logger) => _logger = logger;

		public bool PathExists(string path)
			=> Directory.Exists(path);

		public DirectoryInfo PathCreate(string path)
		{
			if (PathExists(path))
			{
				_logger.Debug($"No action needed, path '{path}' already exists.");
				return new DirectoryInfo(path);
			}

			DirectoryInfo directoryInfo = Directory.CreateDirectory(path);

			_logger.Debug($"Successfully found path for '{path}'.");

			return directoryInfo;
		}

		public bool FileExists(string fullFilePath)
			=> File.Exists(fullFilePath);

		public FileInfo GetFileInfo(string path)
		{
			if (!File.Exists(path))
			{
				throw new ArgumentException($"Could not find file at path '{path}'.");
			}

			_logger.Debug($"Successfully found file info for '{path}'.");

			return new FileInfo(path);
		}

		public string GetFileNameWithoutExtension(string fullFilePath)
		{
			if (!File.Exists(fullFilePath))
			{
				throw new ArgumentException($"Could not find file at path '{fullFilePath}'.");
			}

			_logger.Debug($"Successfully found file info for '{fullFilePath}'.");

			return Path.GetFileNameWithoutExtension(fullFilePath);
		}

		public Stream ReadFileContents(string fullPath)
		{
			if (!File.Exists(fullPath))
			{
				throw new ArgumentException(
					$"Could not find file at path '{fullPath}' to read contents.");
			}

			FileStream fileStream = new FileStream(
				fullPath,
				FileMode.Open);

			_logger.Debug($"Successfully pulled file content from file '{fullPath}'.");

			return fileStream;
		}

		public void DeleteFile(string fullPath)
		{
			if (!File.Exists(fullPath))
			{
				throw new ArgumentException($"Could not find file at path '{fullPath}' to delete.");
			}

			File.Delete(fullPath);

			_logger.Debug($"Successfully deleted file '{fullPath}'.");
		}
	}
}
// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   WindowsFileAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Adapters.v6
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Threading.Tasks;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using Hyland.Types;
	using Microsoft.Extensions.Logging;

	public class WindowsFileAdapter : IFileAdapter
	{
		private readonly ILogger _logger;

		public WindowsFileAdapter(ILogger logger) => _logger = logger;

		public bool PathExists(string path)
			=> Directory.Exists(path);

		public DirectoryInfo PathCreate(string path)
		{
			if (PathExists(path))
			{
				_logger.LogDebug($"No action needed, path '{path}' already exists.");
				return new DirectoryInfo(path);
			}

			DirectoryInfo directoryInfo = Directory.CreateDirectory(path);

			_logger.LogDebug($"Successfully found path for '{path}'.");

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

			_logger.LogDebug($"Successfully found file info for '{path}'.");

			return new FileInfo(path);
		}

		public string GetFileNameWithoutExtension(string fullFilePath)
		{
			if (!File.Exists(fullFilePath))
			{
				throw new ArgumentException($"Could not find file at path '{fullFilePath}'.");
			}

			_logger.LogDebug($"Successfully found file info for '{fullFilePath}'.");

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

			_logger.LogDebug($"Successfully pulled file content from file '{fullPath}'.");

			return fileStream;
		}

		public void DeleteFile(string fullPath)
		{
			if (!File.Exists(fullPath))
			{
				throw new ArgumentException($"Could not find file at path '{fullPath}' to delete.");
			}

			File.Delete(fullPath);

			_logger.LogDebug($"Successfully deleted file '{fullPath}'.");
		}

		public Task SaveFile(
			string path,
			byte[] fileData)
		{
			FileStream stream = File.Create(path);

			Task saveTask = stream.WriteAsync(fileData,0, fileData.Length);

			return saveTask;
		}


	}
}
namespace CareSource.WC.Services.Document.Adapters
{
	using System;
	using System.IO;
	using Microsoft.Extensions.Logging;
	using CareSource.WC.OnBase.Core.ExtensionMethods;

	public class WindowsFileAdapter : IFileAdapter
	{
		private readonly ILogger _logger;

		public WindowsFileAdapter(ILogger logger) => _logger = logger;

		public bool PathExists(string path)
		{
			return Directory.Exists(path);
		}

		public DirectoryInfo PathCreate(string path)
		{
			if (PathExists(path))
			{
				_logger.LogDebug($"No action needed, path '{path}' already exists.");
				return new DirectoryInfo(path);
			}

			var directoryInfo = Directory.CreateDirectory(path);

			_logger.LogDebug($"Successfully found path for '{path}'.");

			return directoryInfo;
		}

		public bool FileExists(string fullFilePath)
		{
			return File.Exists(fullFilePath);
		}

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

			var fileStream = new FileStream(fullPath, FileMode.Open);

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
	}
}
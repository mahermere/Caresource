using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CareSource.WC.Core.Diagnostics.Models;
using CareSource.WC.Core.Helpers.Interfaces;
using CareSource.WC.Core.Transaction.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CareSource.WC.Core.Diagnostics
{
	using System.Collections.Concurrent;

	[ProviderAlias("File")]
	public class FileLoggerProvider : BatchingLoggerProvider
	{
        private readonly IServiceProvider _serviceProvider;

        private FileLoggerConfiguration _config;
        private readonly ConcurrentDictionary<string, FileLogger> _loggers =
	        new ConcurrentDictionary<string, FileLogger>();

        public FileLoggerProvider(IServiceProvider serviceProvider
			, FileLoggerConfiguration config)
				: base(config)
        {
            _serviceProvider = serviceProvider;
            _config = config;
        }

        protected override async Task WriteMessagesAsync(IEnumerable<Tuple<DateTimeOffset, string>> messages, CancellationToken cancellationToken)
        {
	        Directory.CreateDirectory(_config.LoggingDirectory);

	        foreach (var group in messages.GroupBy(GetGrouping))
	        {
		        var fullName = GetFullName(group.Key);
		        var fileInfo = new FileInfo(fullName);

		        string ext = fullName.Substring(fullName.LastIndexOf("."));
		        string filename = fullName.Substring(0, fullName.LastIndexOf("."));

				if (_config.FileSizeLimit > 0 && fileInfo.Exists && fileInfo.Length > _config.FileSizeLimit)
				{
					string newFullName;

					  int i = 1;
			        do
			        {
				        newFullName = $"{filename}_{i++}{ext}";
				        fileInfo = new FileInfo(newFullName);
			        } while (_config.FileSizeLimit > 0 &&
			                 fileInfo.Exists &&
			                 fileInfo.Length > _config.FileSizeLimit);

			        fullName = newFullName;
				}

		        using (var streamWriter = File.AppendText(fullName))
		        {
			        foreach (var message in group)
			        {
				        await streamWriter.WriteAsync(message.Item2);
			        }
		        }
	        }

	        RollFiles();
        }

		private string GetFullName((int Year, int Month, int Day, int Hour, int Minute) group)
		{
			if (!_config?.LoggingFileName?.Contains(".") ?? true)
			{
				throw new ArgumentException("Logging File Name requires an extension.");
			}

			var loggingDate =
				new DateTime(group.Year, group.Month, group.Day, group.Hour, group.Minute, 0)
					.ToString(_config.DatePattern);

			return $"{_config.LoggingDirectory}\\{loggingDate}_{_config.LoggingFileName}";
		}

		private (int Year, int Month, int Day, int Hour, int Minute) GetGrouping(Tuple<DateTimeOffset, string> message)
		{
			return (message.Item1.Year, message.Item1.Month, message.Item1.Day, message.Item1.Hour, message.Item1.Minute);
		}

		protected void RollFiles()
		{
			if (_config.RetainedFileCountLimit > 0)
			{
				var files = new DirectoryInfo(_config.LoggingDirectory)
					 .GetFiles("*" + _config.LoggingFileName)
					 .OrderByDescending(f => f.Name)
					 .Skip(_config.RetainedFileCountLimit.Value);

				foreach (var item in files)
				{
					item.Delete();
				}
			}
		}

        public override ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name =>
                new FileLogger(
                    _serviceProvider.GetService<ITransactionContextManager>()
                    , _serviceProvider.GetService<IJsonSerializerHelper>()
                    , this
                    , name));
        }
    }
}

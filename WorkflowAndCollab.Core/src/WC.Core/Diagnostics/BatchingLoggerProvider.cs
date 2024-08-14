using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CareSource.WC.Core.Diagnostics.Models;
using Microsoft.Extensions.Logging;

namespace CareSource.WC.Core.Diagnostics
{
	using Microsoft.AspNetCore.Mvc.Rendering;

	public abstract class BatchingLoggerProvider : ILoggerProvider
	{
      public BatchLoggerConfiguration Configuration { get; private set; }

		private readonly List<Tuple<DateTimeOffset, string>> _currentBatch = new List<Tuple<DateTimeOffset, string>>();

		private BlockingCollection<Tuple<DateTimeOffset, string>> _messageQueue;
		private Task _outputTask;
		private CancellationTokenSource _cancellationTokenSource;

		protected BatchingLoggerProvider(BatchLoggerConfiguration config)
		{
			Configuration = config;

			if (Configuration.BatchSize <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(Configuration.BatchSize), $"{nameof(Configuration.BatchSize)} must be a positive number.");
			}
			if (Configuration.FlushPeriod <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(Configuration.FlushPeriod), $"{nameof(Configuration.FlushPeriod)} must be longer than zero.");
			}

			if (Configuration.IsEnabled)
			{
				Start();
			}
		}

		protected abstract Task WriteMessagesAsync(IEnumerable<Tuple<DateTimeOffset, string>> messages, CancellationToken token);

		private async Task ProcessLogQueue()
		{
			while (!_cancellationTokenSource.IsCancellationRequested)
			{
				var limit = Configuration.BatchSize ?? int.MaxValue;

				while (limit > 0 && _messageQueue.TryTake(out var message))
				{
					_currentBatch.Add(message);
					limit--;
				}

				if (_currentBatch.Count > 0)
				{
					try
					{
						await WriteMessagesAsync(_currentBatch, _cancellationTokenSource.Token);
					}
					catch
					{
						// ignored
					}

					_currentBatch.Clear();
				}

				await IntervalAsync(TimeSpan.FromMinutes(Configuration.FlushPeriod.Value), _cancellationTokenSource.Token);
			}
		}

		protected virtual Task IntervalAsync(TimeSpan interval, CancellationToken cancellationToken)
		{
			return Task.Delay(interval, cancellationToken);
		}

		internal void AddMessage(DateTimeOffset timestamp, string message)
		{
			if (!_messageQueue.IsAddingCompleted)
			{
				try
				{
					_messageQueue.Add(new Tuple<DateTimeOffset, string>(timestamp, message), _cancellationTokenSource.Token);
				}
				catch
				{
					//cancellation token canceled or CompleteAdding called
				}
			}
		}

		private void Start()
		{
			_messageQueue = Configuration.BackgroundQueueSize == null ?
				 new BlockingCollection<Tuple<DateTimeOffset, string>>(
					 new ConcurrentQueue<Tuple<DateTimeOffset, string>>()) :
				 new BlockingCollection<Tuple<DateTimeOffset, string>>(
					 new ConcurrentQueue<Tuple<DateTimeOffset, string>>(), Configuration.BackgroundQueueSize.Value);

			_cancellationTokenSource = new CancellationTokenSource();
			_outputTask = Task.Run(ProcessLogQueue);
		}

		private void Stop()
		{
			_cancellationTokenSource.Cancel();
			_messageQueue.CompleteAdding();

			try
			{
				_outputTask.Wait(TimeSpan.FromMinutes(Configuration.FlushPeriod.Value));
			}
			catch (TaskCanceledException)
			{
			}
			catch (AggregateException ex) when (ex.InnerExceptions.Count == 1 && ex.InnerExceptions[0] is TaskCanceledException)
			{
			}
		}

		public void Dispose()
		{
			if (Configuration.IsEnabled)
			{
				Stop();
			}
		}

		public abstract ILogger CreateLogger(string categoryName);
	}
}

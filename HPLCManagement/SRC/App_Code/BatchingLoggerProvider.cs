// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   Core
//   BatchingLoggerProvider.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace HplcManagement
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using CareSource.WC.OnBase.Core.Configuration.Interfaces;
	using CareSource.WC.OnBase.Core.Diagnostics.Models;
	using Microsoft.Extensions.Logging;

	public abstract class BatchingLoggerProvider : ILoggerProvider
	{
		private readonly List<Tuple<DateTimeOffset, string>> _currentBatch =
			new List<Tuple<DateTimeOffset, string>>();
		private CancellationTokenSource _cancellationTokenSource;
		private BlockingCollection<Tuple<DateTimeOffset, string>> _messageQueue;
		private Task _outputTask;

		protected BatchingLoggerProvider(ISettingsAdapter config)
		{
			Configuration = new ServiceLoggerConfiguration();

			Configuration.BackgroundQueueSize = config.GetSetting(
				"Logging:Batch:BackGroundQueueSize",
				Configuration.BackgroundQueueSize);

			Configuration.BatchSize = config.GetSetting(
				"Logging:Batch:BatchSize",
				Configuration.BatchSize);

			Configuration.FlushPeriod = config.GetSetting(
				"Logging:Batch:BackGroundQueueSize",
				Configuration.BackgroundQueueSize);

			Configuration.EventId = config.GetSetting(
				"Logging:Batch:EventId",
				Configuration.EventId);

			Configuration.LogLevel = new LogLevelConfiguration();

			Configuration.LogLevel.Default =
				(LogLevel)Enum.Parse(
					typeof(LogLevel),
					config.GetSetting(
						"Logging:Batch:LogLevel:Default",
						Configuration.LogLevel.Default.ToString()));

			if (Configuration.BatchSize <= 0)
			{
				throw new ArgumentOutOfRangeException(
					nameof(Configuration.BatchSize),
					$"{nameof(Configuration.BatchSize)} must be a positive number.");
			}

			if (Configuration.FlushPeriod <= 0)
			{
				throw new ArgumentOutOfRangeException(
					nameof(Configuration.FlushPeriod),
					$"{nameof(Configuration.FlushPeriod)} must be longer than zero.");
			}

			if (Configuration.IsEnabled)
			{
				Start();
			}
		}

		public BatchLoggerConfiguration Configuration { get; }

		public void Dispose()
		{
			if (Configuration.IsEnabled)
			{
				Stop();
			}
		}

		public abstract ILogger CreateLogger(string categoryName);

		protected virtual Task IntervalAsync(
			TimeSpan interval,
			CancellationToken cancellationToken)
			=> Task.Delay(
				interval,
				cancellationToken);

		protected abstract Task WriteMessagesAsync(
			IEnumerable<Tuple<DateTimeOffset, string>> messages,
			CancellationToken token);

		internal void AddMessage(
			DateTimeOffset timestamp,
			string message)
		{
			if (!_messageQueue.IsAddingCompleted)
			{
				try
				{
					_messageQueue.Add(
						new Tuple<DateTimeOffset, string>(
							timestamp,
							message),
						_cancellationTokenSource.Token);
				}
				catch
				{
					//cancellation token canceled or CompleteAdding called
				}
			}
		}

		private async Task ProcessLogQueue()
		{
			while (!_cancellationTokenSource.IsCancellationRequested)
			{
				int limit = Configuration.BatchSize ?? int.MaxValue;

				while (limit > 0 &&
						_messageQueue.TryTake(out Tuple<DateTimeOffset, string> message))
				{
					_currentBatch.Add(message);
					limit--;
				}

				if (_currentBatch.Count > 0)
				{
					try
					{
						await WriteMessagesAsync(
							_currentBatch,
							_cancellationTokenSource.Token);
					}
					catch
					{
						// ignored
					}

					_currentBatch.Clear();
				}

				await IntervalAsync(
					TimeSpan.FromMilliseconds(Configuration.FlushPeriod.Value),
					_cancellationTokenSource.Token);
			}
		}

		private void Start()
		{
			CreateLogger("Service");
			_messageQueue = Configuration.BackgroundQueueSize == null
				? new BlockingCollection<Tuple<DateTimeOffset, string>>(
					new ConcurrentQueue<Tuple<DateTimeOffset, string>>())
				: new BlockingCollection<Tuple<DateTimeOffset, string>>(
					new ConcurrentQueue<Tuple<DateTimeOffset, string>>(),
					Configuration.BackgroundQueueSize.Value);

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
			catch (TaskCanceledException) { }
			catch (AggregateException ex) when (ex.InnerExceptions.Count == 1 &&
															ex.InnerExceptions[0] is TaskCanceledException)
			{ }
		}
	}
}
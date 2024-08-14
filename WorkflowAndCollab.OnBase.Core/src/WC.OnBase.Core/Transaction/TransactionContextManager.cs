//  ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    OnBase.Core
//    TransactionContextManager.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace CareSource.WC.OnBase.Core.Transaction
{
	using System;
	using System.Diagnostics;
	using System.Threading;
	using CareSource.WC.Entities.Transactions;
	using CareSource.WC.OnBase.Core.Configuration.Interfaces;
	using CareSource.WC.OnBase.Core.Helpers.Interfaces;
	using CareSource.WC.OnBase.Core.Transaction.Interfaces;
	using ActivityContext = CareSource.WC.Entities.Transactions.ActivityContext;
	using ExecutionContext = CareSource.WC.Entities.Transactions.ExecutionContext;

	public class TransactionContextManager : ITransactionContextManager
	{
		private readonly IAssemblyHelper _assemblyHelper;
		private readonly IJsonSerializerHelper _jsonSerializerHelper;
		private readonly ISettingsAdapter _settingsAdapter;

		private readonly ThreadLocal<ProcessContext> _tlProcessContext =
			new ThreadLocal<ProcessContext>();

		private TransactionContext _transactionContext;

		public TransactionContextManager(IAssemblyHelper assemblyHelper,
			IJsonSerializerHelper jsonSerializerHelper,
			ISettingsAdapter settingsAdapter)
		{
			_assemblyHelper = assemblyHelper;
			_jsonSerializerHelper = jsonSerializerHelper;
			_settingsAdapter = settingsAdapter;
		}

		public TransactionContext CurrentContext
		{
			get
			{
				_transactionContext = _transactionContext ?? InitializeContext(null);

				if (!_tlProcessContext.IsValueCreated)
				{
					_tlProcessContext.Value = InitializeThreadContext(new StackFrame(1));
				}

				_transactionContext.ProcessContext = _tlProcessContext.Value;

				return _transactionContext;
			}

			set => _transactionContext = value;
		}

		public TransactionContext CopyCurrentContext()
		{
			string jsonTC = _jsonSerializerHelper.ToJson(_transactionContext);
			return _jsonSerializerHelper.FromJson<TransactionContext>(jsonTC);
		}

		public TransactionContext InitializeContext(
			TransactionContext context)
		{
			if (context == null)
			{
				context = new TransactionContext();
			}

			return new TransactionContext
			{
				BusinessContext = context.BusinessContext,
				ExecutionContext = new ExecutionContext
				{
					StartedOn = DateTime.Now,
					Server = Environment.MachineName,
					Environment =
						_settingsAdapter.GetSetting("OnBase.DataSource")?.Replace("OBServer_", ""),
					Process = _assemblyHelper.GetWebEntryAssembly()?.FullName?
						.Split(new[] { "," }, StringSplitOptions.None)?[0]
				},
				EventContext = new EventContext
				{
					CorrelationGuid = context.EventContext?.CorrelationGuid ??
						Guid.NewGuid().ToString(),
					ParentGuid = context.EventContext?.CurrentGuid,
					CurrentGuid = Guid.NewGuid().ToString(),
					Destination = context.EventContext?.Destination,
					Source = context.EventContext?.Source ?? "None Given",
					ApplicationName = _assemblyHelper.GetWebEntryAssembly()?.FullName?
						.Split(new[] { "," }, StringSplitOptions.None)?[0]
				},
				Status = Status.Success,
				EventData = context.EventData,
				ProcessContext = InitializeThreadContext(new StackFrame(1))
			};
		}

		public void FinalizeContext(TransactionContext context)
		{
			if (context?.ExecutionContext != null)
			{
				context.ExecutionContext.EndedOn = DateTime.Now;
			}

			if (context?.ProcessContext?.ActivityContext != null)
			{
				context.ProcessContext.ActivityContext.EndedOn = DateTime.Now;
			}

			context.SecurityContext = null;
		}

		public void PopulateTransactionException(
			Exception exception)
		{
			if (CurrentContext != null && exception != null)
			{
				CurrentContext.Status = Status.Error;

				if (CurrentContext.ProcessContext?.ActivityContext != null)
				{
					CurrentContext.ProcessContext.Exception = new ExceptionContext
					{
						Status = Status.Error,
						StartedOn = CurrentContext.ExecutionContext?.StartedOn ?? DateTime.Now,
						EndedOn = CurrentContext.ExecutionContext?.EndedOn ?? DateTime.Now,
						MachineName = Environment.MachineName,
						Message = exception.Message,
						ActivityGuid = CurrentContext.ProcessContext.ActivityContext.ActivityGuid,
						Category = exception.GetType().BaseType?.Name,
						StackTrace = exception.StackTrace
					};

					CurrentContext.ProcessContext.ActivityContext.Status = Status.Error;
				}
			}
		}

		private ProcessContext InitializeThreadContext(StackFrame callingFrame)
		{
			Thread thread = Thread.CurrentThread;

			return new ProcessContext
			{
				ActivityName = callingFrame?.GetMethod()?.DeclaringType?.FullName,
				ActivityContext = new ActivityContext
				{
					ActivityGuid = Guid.NewGuid().ToString(),
					ActivityType = thread.GetType().Name,
					ProcessId = thread.ManagedThreadId.ToString(),
					Process = callingFrame?.GetMethod()?.Name,
					Status = Status.Success,
					StartedOn = DateTime.Now
				}
			};
		}
	}
}
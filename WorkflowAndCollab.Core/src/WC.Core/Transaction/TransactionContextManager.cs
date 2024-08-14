namespace CareSource.WC.Core.Transaction
{
	using System;
	using System.Threading;
	using CareSource.WC.Core.Helpers.Interfaces;
	using CareSource.WC.Core.Transaction.Interfaces;
	using CareSource.WC.Entities.Transactions;
	using ExecutionContext = Entities.Transactions.ExecutionContext;

	public class TransactionContextManager : ITransactionContextManager
	{
		private readonly ICareSourceEnvironmentHelper _careSourceEnvironmentHelper;
		private readonly IAssemblyHelper _assemblyHelper;
		private readonly IJsonSerializerHelper _jsonSerializerHelper;

		private readonly AsyncLocal<TransactionContext> _tlTransactionContext =
			new AsyncLocal<TransactionContext>();

		private readonly ThreadLocal<ProcessContext> _tlProcessContext =
			new ThreadLocal<ProcessContext>();

		public TransactionContextManager(
			ICareSourceEnvironmentHelper careSourceEnvironmentHelper,
			IAssemblyHelper assemblyHelper,
			IJsonSerializerHelper jsonSerializerHelper)
		{
			_careSourceEnvironmentHelper = careSourceEnvironmentHelper;
			_assemblyHelper = assemblyHelper;
			_jsonSerializerHelper = jsonSerializerHelper;
		}

		public TransactionContext CurrentContext
		{
			get
			{
				if (_tlTransactionContext.Value == null)
				{
					_tlTransactionContext.Value = InitializeContext(null);
				}

				if (!_tlProcessContext.IsValueCreated)
				{
					_tlProcessContext.Value = InitializeProcessContext();
				}

				_tlTransactionContext.Value.ProcessContext = _tlProcessContext.Value;

				return _tlTransactionContext.Value;
			}

			set => _tlTransactionContext.Value = value;
		}

		public TransactionContext CopyCurrentContext()
		{
			var jsonTC = _jsonSerializerHelper.ToJson(CurrentContext);
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
					Environment = _careSourceEnvironmentHelper.GetCareSourceEnvironment(),
					Process = _assemblyHelper.GetEntryAssemblyName()
						?
						.Split(
							",",
							StringSplitOptions.None)?[0]
				},
				EventContext = new EventContext
				{
					CorrelationGuid = context.EventContext?.CorrelationGuid ??
					                  Guid.NewGuid()
						                  .ToString(),
					ParentGuid = context.EventContext?.CurrentGuid,
					CurrentGuid = Guid.NewGuid()
						.ToString(),
					Destination = context.EventContext?.Destination,
					Source = context.EventContext?.Source,
					Event = context.EventContext?.Event,
					ApplicationName = _assemblyHelper.GetEntryAssemblyName()
				},
				Status = Status.Success,
				ProcessContext = InitializeProcessContext(),
				EventData = context.EventData
			};
		}

		public void FinalizeContext(
			TransactionContext context)
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
			if (CurrentContext != null)
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
						Category = exception.GetType()
							.BaseType.Name,
						StackTrace = exception.StackTrace
					};

					CurrentContext.ProcessContext.ActivityContext.Status = Status.Error;
				}
			}
		}

		private ProcessContext InitializeProcessContext()
		{
			return new ProcessContext
			{
				ActivityName = Thread.GetDomain()?.FriendlyName,
				ActivityContext = new ActivityContext
				{
					ActivityGuid = Guid.NewGuid()
						.ToString(),
					ActivityType = Thread.CurrentThread.GetType()
						.Name,
					ProcessId = Thread.CurrentThread.ManagedThreadId.ToString(),
					Process = Thread.CurrentThread.Name,
					Status = Status.Success,
					StartedOn = DateTime.Now
				}
			};
		}
	}
}
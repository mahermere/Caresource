// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   ITransactionContextManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Transaction.Interfaces
{
	using System;
	using CareSource.WC.Entities.Transactions;

	public interface ITransactionContextManager
	{
		TransactionContext CurrentContext { get; set; }

		TransactionContext CopyCurrentContext();

		TransactionContext InitializeContext(
			TransactionContext context);

		void FinalizeContext(
			TransactionContext context);

		void PopulateTransactionException(
			Exception exception);
	}
}
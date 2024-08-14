// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   ITransactionContextManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.OnBase.Core.Transaction.Interfaces
{
    using CareSource.WC.Entities.Transactions;
    using System;

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
// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    ITransactionContextManager.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Transaction;

using FXIAuthentication.Core.Transaction.Models;

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
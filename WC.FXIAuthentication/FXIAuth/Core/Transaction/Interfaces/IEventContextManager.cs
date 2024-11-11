// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    IEventContextManager.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Transaction;

using FXIAuthentication.Core.Transaction.Models;

public interface IEventContextManager
{
	void SetEventContext(
		TransactionContext context,
		params object[] eventArgs);
}
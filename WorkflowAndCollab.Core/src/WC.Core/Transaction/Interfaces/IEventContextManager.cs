// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   IEventContextManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

using CareSource.WC.Entities.Transactions;

namespace CareSource.WC.Core.Transaction.Interfaces
{
	public interface IEventContextManager
	{
		void SetEventContext(
			TransactionContext context,
			params object[] eventArgs);
	}
}
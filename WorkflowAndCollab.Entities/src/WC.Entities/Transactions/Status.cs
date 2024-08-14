// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Entities
//   Status.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Transactions
{
	public enum Status
	{
		New = 1,
		Inprogress = 2,
		Success = 3,
		Pending = 4,
		Suspended = 5,
		Success_With_Exception = 6,
		Error = 7,
		Ready_To_Purge = 8
	}
}
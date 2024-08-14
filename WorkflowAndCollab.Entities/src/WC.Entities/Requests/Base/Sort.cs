// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Entities
//   Sorting.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Requests.Base
{
	public class Sort
	{
		public string ColumnName { get; set; }

		public bool SortAscending { get; set; } = true;
	}
}
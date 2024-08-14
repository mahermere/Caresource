// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CareSource.WC.Entities.WC.Entities
//   Paging.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Requests.Base
{
	public class Paging
	{
		public int PageNumber { get; set; } = 1;

		public int PageSize { get; set; } = 100;
	}
}
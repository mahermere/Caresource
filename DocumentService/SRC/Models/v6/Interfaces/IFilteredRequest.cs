﻿// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   ICountRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Models.v6
{
	using System;
	using System.Collections.Generic;
	using CareSource.WC.Entities.Common;
	using CareSource.WC.OnBase.Core.Diagnostics;

	/// <summary>
	///    Basic Information needed to search for documents
	/// </summary>
	public interface IFilteredRequest : IRequest
	{
		IEnumerable<string> DocumentTypes { get; set; }

		DateTime? EndDate { get; set; }

		IEnumerable<Filter> Filters { get; set; }

		DateTime? StartDate { get; set; }
	}
}
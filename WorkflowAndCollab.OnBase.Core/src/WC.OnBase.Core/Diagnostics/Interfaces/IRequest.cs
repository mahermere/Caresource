// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   IRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.OnBase.Core.Diagnostics
{
	using System;

	public interface IRequest
	{
		Guid CorrelationGuid { get; set; }

		DateTime RequestDateTime { get; set; }

		string SourceApplication { get; set; }

		string UserId { get; set; }
	}
}
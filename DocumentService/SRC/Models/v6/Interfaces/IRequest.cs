// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2023. All rights reserved.
// 
//   WC.Services.Document
//   IRequest.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Models.v6
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
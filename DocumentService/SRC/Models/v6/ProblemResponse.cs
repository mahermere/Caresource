// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   ValidationProblemResponse.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Models.v6
{
	using System;
	using System.Net;

	public class ProblemResponse : BaseResponse<string>
	{

		public ProblemResponse(
			Guid correlationGuid
			)
			: base(
				$"An unexpected error has occurred, please see the  Document service Log, id: {correlationGuid}",
				(int)HttpStatusCode.InternalServerError,
				correlationGuid)
		{
			
		}
	}
}
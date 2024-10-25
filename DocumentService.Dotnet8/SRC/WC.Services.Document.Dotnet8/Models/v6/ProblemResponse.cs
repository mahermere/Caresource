// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   ValidationProblemResponse.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Models.v6
{
	using System;
	using System.Net;
    using WC.Services.Document.Dotnet8.Models.v6.Responses;

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
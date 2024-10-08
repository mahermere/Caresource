// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   ImportProcessor
//   KeywordUpdateResponse.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.ImportProcessor.Dotnet8.Models.v1
{
    using CareSource.WC.Entities.Exceptions;
    using CareSource.WC.Entities.Responses;
    using System;
    using WC.Services.ImportProcessor.Dotnet8.Models.v1.OnBase;
	using CareSource.WC.Entities.Responses.Base;

    public class KeywordUpdateResponse : CareSource.WC.Entities.Responses.Base.BaseResponse<ResponseData>
	{
		/// <summary>
		///    Initializes a new instance of the <see cref="KeywordUpdateResponse" /> class.
		/// </summary>
		/// <param name="status">The status.</param>
		/// <param name="message">The message.</param>
		/// <param name="errorCode">The error code.</param>
		/// <param name="correlationGuid">The correlation unique identifier.</param>
		/// <param name="responseData">The response data.</param>
		public KeywordUpdateResponse(
			ResponseStatus status,
			string message,
			ErrorCode errorCode,
			Guid correlationGuid,
			ResponseData responseData)
			: base(
				status,
				message,
				errorCode,
				correlationGuid,
				responseData)
		{ }
	}

	//public class KeywordUpdateResult
	//{
	//	public string CorrelationGuid { get; set; }
	//	public ResponseData ResponseData { get; set; }
	//	public int status { get; set; }
	//	public string title { get; set; }
	//}

	public class ResponseData
	{
		public int[] SuccessfulIds { get; set; }
		public Failure[] Failures { get; set; }
	}

	public class Failure
	{
		public string Id { get; set; }
		public string[] Messages { get; set; }
	}
}
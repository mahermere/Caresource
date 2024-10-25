﻿// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   BaseResponse.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Models.v6.Responses
{
	using System;
    using System.Text.Json.Serialization;
    
	public class BaseResponse<TResult>
	{
		public BaseResponse(
			string title,
			int status,
			Guid correlationGuid,
			TResult result)
		{
			Title = title;
			Status = status;
			CorrelationGuid = correlationGuid;
			ResponseData = result;
		}


		public BaseResponse(
			string title,
			int status,
			Guid correlationGuid)
			: this(
				title,
				status,
				correlationGuid,
				default)
		{ }

		/// <summary>
		///    Gets or sets the Base Response{ T Model} Trace Identifier
		/// </summary>
		public Guid CorrelationGuid { get; set; }

		/// <summary>
		///    Gets or sets the Base Response{ T Model} Response Data
		/// </summary>
		[JsonPropertyName("responseData")]
		[JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]
		public virtual TResult ResponseData { get; set; }

		/// <summary>
		///    The HTTP status code([RFC7231], Section 6) generated by the origin server for this occurrence of
		///    the problem.
		/// </summary>
		[JsonPropertyName("status")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public int? Status { get; set; }

		/// <summary>
		///    A short, human-readable summary of the problem type.It SHOULD NOT change from occurrence to
		///    occurrence
		///    of the problem, except for purposes of localization(e.g., using proactive content negotiation;
		///    see[RFC7231], Section 3.4).
		/// </summary>
		[JsonPropertyName("title")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Title { get; set; }
	}
}
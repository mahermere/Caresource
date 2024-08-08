// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Messaging
//   BaseResponse.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace HplcManagement.Models.Core
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Web.Http.ModelBinding;
	using Newtonsoft.Json;

	/// <summary>
	///    Data and functions describing a CareSource.WC.Messaging.Models.BaseResponse object.
	/// </summary>
	/// <typeparam name="TResult">
	/// The type of the model returned if the method returns data.
	/// </typeparam>
	public class BaseResponse<TResult>: ValidationProblemDetails
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BaseResponse{TModel}"/> class.
		/// </summary>
		public BaseResponse()
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseResponse{TModel}"/> class.
		/// </summary>
		/// <param name="title">The title.</param>
		/// <param name="correlationGuid">The correlation unique identifier.</param>
		/// <param name="status">The status.</param>
		/// <param name="result">The result.</param>
		public BaseResponse(
			string title,
			Guid correlationGuid,
			HttpStatusCode status,
			TResult result)
		{
			Status = (int)status;
			CorrelationGuid = correlationGuid;
			Title = title;
			Result = result;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseResponse{TModel}" /> class.
		/// </summary>
		/// <param name="modelState"><see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary" />
		/// containing the validation errors.</param>
		/// <param name="correlationGuid">The correlation unique identifier.</param>
		public BaseResponse(
			ModelStateDictionary modelState,
			Guid correlationGuid)
			: base(modelState)
				=> CorrelationGuid = correlationGuid;

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseResponse{TModel}" /> class.
		/// </summary>
		/// <param name="errors">The validation errors.</param>
		/// <param name="correlationGuid">The correlation unique identifier.</param>
		public BaseResponse(
			IDictionary<string, string[]> errors,
			Guid correlationGuid)
			: base(errors)
				=> CorrelationGuid = correlationGuid;

		/// <summary>
		///    Gets or sets the Base Response{ T Model} Errors
		/// </summary>
		/// <remarks>
		///	If there are any errors then they will be here.  Overriding the base class errors so
		///	the property can be ignored if null
		/// </remarks>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public new IDictionary<string, string[]> Errors
			=> base.Errors.Any() ? base.Errors : null ;

		/// <summary>
		///    Gets or sets the Base Response{ T Model} Response Data
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public TResult Result { get; set; }

		/// <summary>
		///    Gets or sets the Base Response{ T Model} Trace Identifier
		/// </summary>
		public Guid CorrelationGuid { get; set; }
	}
}
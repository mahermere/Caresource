// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   ValidationProblemResponse.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.HplcManagement.Dotnet8.Models.Core
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System;
	using System.Net;
	

	/// <summary>
	///    Data and functions describing a CareSource.WC.Services.Hplc.Models.ValidationProblemResponse
	///    object.
	/// </summary>
	/// <seealso cref="BaseResponse{ModelStateDictionary}" />
	public sealed class ValidationProblemResponse : BaseResponse<ModelStateDictionary>
	{
		/// <summary>
		///    Initializes a new instance of the <see cref="ValidationProblemResponse" /> class.
		/// </summary>
		/// <param name="modelState">
		///    <see cref="T:Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary" />
		///    containing the validation errors.
		/// </param>
		/// <param name="correlationGuid">The correlation unique identifier.</param>
		public ValidationProblemResponse(
			ModelStateDictionary modelState,
			Guid correlationGuid)
			: base(
				modelState,
				correlationGuid)
		{
			Status = (int)HttpStatusCode.BadRequest;
			Title = "One or more validation errors occurred.";
			
		}
	}
}
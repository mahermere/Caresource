// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   ProviderController.Claims.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Controllers.v4
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Web.Http;
	using CareSource.WC.Entities.Common;
	using CareSource.WC.Entities.Exceptions;
	using CareSource.WC.Entities.Responses;
	using CareSource.WC.Services.Document.Models.v4;
	using Swashbuckle.Swagger.Annotations;
	using CareSource.WC.OnBase.Core.ExtensionMethods;

	/// <summary>
	/// Data and functions describing a
	/// CareSource.WC.Services.Document.Controllers.v4.ProviderController object.
	/// </summary>
	/// <remarks>
	///	This file specifically deals with provider claims and will call the WorkView service to
	/// load a claim
	/// </remarks>
	/// <seealso cref="System.Web.Http.ApiController" />
	public partial class ProviderController 
	{
		/// <summary>
		/// Searches for provider documents by the claim id/number.
		/// </summary>
		/// <param name="id">The Claim Identifier.</param>
		/// <param name="request">The request.</param>
		/// <returns>
		///	A list of claims matching the Claim Id and with member Id and Provider id
		/// </returns>
		/// <exception cref="NotImplementedException"></exception>
		/// <remarks>
		///	Searches OnBase WorkView and Documents for the documents associated wth the Id
		/// </remarks>
		[Route("Claims/byClaimId/{id}")]
		[HttpGet]
		[Obsolete("Please use the V5 routes.")]
		//[SwaggerResponse(HttpStatusCode.OK, Type = typeof(DocumentKeywordResponse))]
		[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(ValidationProblemResponse))]
		[SwaggerResponse(HttpStatusCode.Unauthorized)]
		[SwaggerResponse(HttpStatusCode.InternalServerError)]
		public IHttpActionResult SearchByClaim(
			string id,
			[FromUri]ClaimsRequest request)
		{
			try
			{
				IEnumerable<Document> docs = _claimsManager.ClaimSearch(
					id,
					request);

				return Ok(
					new SearchResults<Document>()
					{
						Results = docs,
						TotalRecordCount = docs.Count()
					});
			}
			catch (OnBaseExceptionBase onBaseException)
			{
				_logger.LogError(
					onBaseException,
					onBaseException.Message);
				return Content(
					HttpStatusCode.OK,
					new DocumentTypesCountResponse(
						onBaseException.Message,
						(int)ResponseStatus.Error,
						request.CorrelationGuid,
						new Dictionary<string, int>()));
			}
			catch (Exception exception)
			{
				_logger.LogError(
					exception,
					exception.Message);

				return Content(
					HttpStatusCode.BadRequest,
					new TotalDocumentCountResponse(
						"An error occurred, please see logging and reference the Correlation Guid",
						(int)ResponseStatus.Error,
						request.CorrelationGuid,
						0));
			}
		}

	}
}
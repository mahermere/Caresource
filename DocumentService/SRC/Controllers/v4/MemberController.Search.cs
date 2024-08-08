// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   MemberController.Search.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Controllers.v4
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Web.Http;
	using CareSource.WC.Entities.Exceptions;
	using CareSource.WC.Entities.Responses;
	using CareSource.WC.Services.Document.Models.v4;
	using CareSource.WC.OnBase.Core.ExtensionMethods;

	/// <summary>
	///    Version 4 of the Document Controller to retrieve Member document search's
	/// </summary>
	/// <seealso cref="System.Web.Http.ApiController" />
	public partial class MemberController
	{
		/// <summary>
		///    Get the total records that match the parameters
		/// </summary>
		/// <param name="memberId"></param>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		[HttpGet]
		[Route("{memberId}/DocumentTypesCount")]
		[Obsolete("Please use the V5 routes.")]
		public IHttpActionResult DocumentTypesCount(
			string memberId,
			[FromUri] DocumentTypesCountRequest request)
		{
			try
			{
				if (!VerifyRequest(request))
				{
					throw new Exception(
						ModelState.Values.First(v => v.Errors.Any())
							.Errors.First()
							.ErrorMessage);
				}

				IDictionary<string, int> docTypes = _memberManager.GetDocumentTypesCount(
					memberId,
					request);

				_logger.LogInformation($"{docTypes.Count} document types match this query.");

				return Content(
					HttpStatusCode.OK,
					new DocumentTypesCountResponse(
						SuccessMessage,
						(int)HttpStatusCode.OK,
						request.CorrelationGuid,
						docTypes));
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
// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WorkFlowAndCollab.API.OBAppeals
//   AppealController.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.OBAppeals.Controllers.v1
{
	using System;
	using System.Linq;
	using System.Net;
	using System.Web.Http;
	using CareSource.WC.Entities.Appeals;
	using CareSource.WC.Entities.Common.Interfaces;
	using CareSource.WC.Entities.Exceptions;
	using CareSource.WC.Entities.Responses;
	using CareSource.WC.OnBase.Core.Diagnostics.Interfaces;
	using CareSource.WC.OnBase.Core.Http.Filters;
	using CareSource.WC.Services.OBAppeals.Managers.Interfaces;

	[RoutePrefix("api/v1")]
	[OnBaseAuthorizeFilter]
	public class AppealController : ApiController
	{
		/// <summary>
		///    Success Message Document Controller option
		/// </summary>
		private const string SuccessMessage = "Success";

		public AppealController(
			IAppealsManager<Appeal> appealsManager,
			IProviderAppealsManager<Appeal> providerAppealsManager,
			IMemberAppealsManager<Appeal> memberAppealsManager,
			IClaimAppealsManager<Appeal> claimAppealsManager,
			ILogger logger)
		{
			_appealsManager = appealsManager;
			_providerAppealsManager = providerAppealsManager;
			_memberAppealsManager = memberAppealsManager;
			_claimAppealsManager = claimAppealsManager;
			_logger = logger;
		}

		private readonly IAppealsManager<Appeal> _appealsManager;
		private readonly IProviderAppealsManager<Appeal> _providerAppealsManager;
		private readonly IMemberAppealsManager<Appeal> _memberAppealsManager;
		private readonly IClaimAppealsManager<Appeal> _claimAppealsManager;
		private readonly ILogger _logger;

		[HttpGet]
		[Route("search/appeal/{appealId}")]
		public IHttpActionResult SearchAppeals(
			string appealId,
			[FromUri] ListAppealsRequest request)
		{
			if (request.CorrelationGuid.Equals(Guid.Empty))
			{
				request.CorrelationGuid = Guid.NewGuid();
			}

			try
			{
				ValidateModelState();

				ISearchResults<Appeal> appeals = _appealsManager.Search(appealId, request);

				return Content(
					HttpStatusCode.OK,
					new AppealResponse(
						ResponseStatus.Success,
						SuccessMessage,
						ErrorCode.Success,
						request.CorrelationGuid,
						appeals.TotalRecordCount,
						appeals.Results.FirstOrDefault()));
			}
			catch (OnBaseExceptionBase onBaseException)
			{
				_logger.LogError(onBaseException.Message, onBaseException);
				return Content(
					HttpStatusCode.OK,
					new AppealResponse(
						ResponseStatus.Error,
						onBaseException.Message,
						onBaseException.ErrorCode,
						request.CorrelationGuid,
						0,
						null));
			}
			catch (Exception exception)
			{
				_logger.LogError(exception.Message, exception);
				return Content(
					HttpStatusCode.BadRequest,
					new AppealResponse(
						ResponseStatus.Error,
						exception.Message,
						ErrorCode.UnknownError,
						request.CorrelationGuid,
						0,
						null));
			}
			finally
			{
				_logger.LogInfo("End SearchAppeals");
			}
		}


		[HttpGet]
		[Route("search/appeal/provider/{providerId}")]
		public IHttpActionResult SearchProviderAppeals(
			string providerId,
			[FromUri] ListAppealsRequest request)
		{
			if (request.CorrelationGuid.Equals(Guid.Empty))
			{
				request.CorrelationGuid = Guid.NewGuid();
			}

			try
			{
				ValidateModelState();

				ISearchResults<Appeal> appeals = _providerAppealsManager.Search(providerId, request);

				return Content(
					HttpStatusCode.OK,
					new ListAppealsResponse(
						ResponseStatus.Success,
						SuccessMessage,
						ErrorCode.Success,
						request.CorrelationGuid,
						appeals.TotalRecordCount,
						appeals.Results));
			}
			catch (OnBaseExceptionBase onBaseException)
			{
				_logger.LogError(onBaseException.Message, onBaseException);
				return Content(
					HttpStatusCode.OK,
					new ListWorkViewObjectsResponse(
						ResponseStatus.Error,
						onBaseException.Message,
						onBaseException.ErrorCode,
						request.CorrelationGuid,
						0,
						null));
			}
			catch (Exception exception)
			{
				_logger.LogError(exception.Message, exception);
				return Content(
					HttpStatusCode.BadRequest,
					new ListWorkViewObjectsResponse(
						ResponseStatus.Error,
						exception.Message,
						ErrorCode.UnknownError,
						request.CorrelationGuid,
						0,
						null));
			}
			finally
			{
				_logger.LogInfo("End SearchProviderAppeals");
			}
		}

		[HttpGet]
		[Route("search/appeal/member/{memberId}")]
		public IHttpActionResult SearchMemberAppeals(
			string memberId,
			[FromUri] ListAppealsRequest request)
		{
			if (request.CorrelationGuid.Equals(Guid.Empty))
			{
				request.CorrelationGuid = Guid.NewGuid();
			}

			try
			{
				ValidateModelState();

				ISearchResults<Appeal> appeals = _memberAppealsManager.Search(memberId, request);

				return Content(
					HttpStatusCode.OK,
					new ListAppealsResponse(
						ResponseStatus.Success,
						SuccessMessage,
						ErrorCode.Success,
						request.CorrelationGuid,
						appeals.TotalRecordCount,
						appeals.Results));
			}
			catch (OnBaseExceptionBase onBaseException)
			{
				_logger.LogError(onBaseException.Message, onBaseException);
				return Content(
					HttpStatusCode.OK,
					new ListWorkViewObjectsResponse(
						ResponseStatus.Error,
						onBaseException.Message,
						onBaseException.ErrorCode,
						request.CorrelationGuid,
						0,
						null));
			}
			catch (Exception exception)
			{
				_logger.LogError(exception.Message, exception);
				return Content(
					HttpStatusCode.BadRequest,
					new ListWorkViewObjectsResponse(
						ResponseStatus.Error,
						exception.Message,
						ErrorCode.UnknownError,
						request.CorrelationGuid,
						0,
						null));
			}
			finally
			{
				_logger.LogInfo("End SearchMemberAppeals");
			}
		}

		[HttpGet]
		[Route("search/appeal/claim/{claimId}")]
		public IHttpActionResult SearchClaimAppeals(
			string claimId,
			[FromUri] ListAppealsRequest request)
		{
			if (request.CorrelationGuid.Equals(Guid.Empty))
			{
				request.CorrelationGuid = Guid.NewGuid();
			}

			try
			{
				ValidateModelState();

				ISearchResults<Appeal> appeals = _claimAppealsManager.Search(claimId, request);

				return Content(
					HttpStatusCode.OK,
					new ListAppealsResponse(
						ResponseStatus.Success,
						SuccessMessage,
						ErrorCode.Success,
						request.CorrelationGuid,
						appeals.TotalRecordCount,
						appeals.Results));
			}
			catch (OnBaseExceptionBase onBaseException)
			{
				_logger.LogError(onBaseException.Message, onBaseException);
				return Content(
					HttpStatusCode.OK,
					new ListWorkViewObjectsResponse(
						ResponseStatus.Error,
						onBaseException.Message,
						onBaseException.ErrorCode,
						request.CorrelationGuid,
						0,
						null));
			}
			catch (Exception exception)
			{
				_logger.LogError(exception.Message, exception);
				return Content(
					HttpStatusCode.BadRequest,
					new ListWorkViewObjectsResponse(
						ResponseStatus.Error,
						exception.Message,
						ErrorCode.UnknownError,
						request.CorrelationGuid,
						0,
						null));
			}
			finally
			{
				_logger.LogInfo("End SearchClaimAppeals");
			}
		}

		[HttpGet]
		[Route("search/appeal/object/{objectId}")]
		public IHttpActionResult GetAppeal(
			long objectId,
			[FromUri] WorkviewObjectItemRequest request)
		{
			if (request.CorrelationGuid.Equals(Guid.Empty))
			{
				request.CorrelationGuid = Guid.NewGuid();
			}

			try
			{
				ValidateModelState();

				Appeal appeal = _appealsManager.GetAppeal(objectId, request);

				return Content(
					HttpStatusCode.OK,
					new AppealResponse(
						ResponseStatus.Success,
						SuccessMessage,
						ErrorCode.Success,
						request.CorrelationGuid,
						1,
						appeal));
			}
			catch (OnBaseExceptionBase onBaseException)
			{
				_logger.LogError(onBaseException.Message, onBaseException);
				return Content(
					HttpStatusCode.OK,
					new AppealResponse(
						ResponseStatus.Error,
						onBaseException.Message,
						onBaseException.ErrorCode,
						request.CorrelationGuid,
						0,
						null));
			}
			catch (Exception exception)
			{
				_logger.LogError(exception.Message, exception);
				return Content(
					HttpStatusCode.BadRequest,
					new AppealResponse(
						ResponseStatus.Error,
						exception.Message,
						ErrorCode.UnknownError,
						request.CorrelationGuid,
						0,
						null));
			}
			finally
			{
				_logger.LogInfo("End GetAppeal");
			}
		}

		private void ValidateModelState()
		{
			if (!ModelState.IsValid)
			{
				throw new Exception(
					ModelState.Values.First(v => v.Errors.Any())
						.Errors.First()
						.ErrorMessage);
			}
		}
	}
}
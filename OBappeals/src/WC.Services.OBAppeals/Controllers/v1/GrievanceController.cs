// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WorkFlowAndCollab.API.OBAppeals
//   GrievanceController.cs
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

	[Route("api/v1/")]
	[OnBaseAuthorizeFilter]
	public class GrievanceController : ApiController
	{
		/// <summary>
		///    Success Message Document Controller option
		/// </summary>
		private const string SuccessMessage = "Success";

		public GrievanceController(
			IGrievancesManager<Appeal> grievancesManager,
			IProviderGrievancesManager<Appeal> providerGrievancesManager,
			IMemberGrievancesManager<Appeal> memberGrievancesManager,
			ILogger logger)
		{
			_grievancesManager = grievancesManager;
			_providerGrievancesManager = providerGrievancesManager;
			_memberGrievancesManager = memberGrievancesManager;
			_logger = logger;

		}

		private readonly IGrievancesManager<Appeal> _grievancesManager;
		private readonly IProviderGrievancesManager<Appeal> _providerGrievancesManager;
		private readonly IMemberGrievancesManager<Appeal> _memberGrievancesManager;
		private readonly ILogger _logger;

		[HttpGet]
		[Route("api/v1/search/grievance/{grievanceId}")]
		public IHttpActionResult SearchGrievances(
			string grievanceId,
			[FromUri] ListAppealsRequest request)
		{
			if (request.CorrelationGuid.Equals(Guid.Empty))
			{
				request.CorrelationGuid = Guid.NewGuid();
			}

				try
				{
					ValidateModelState();

					ISearchResults<Appeal> grievances = _grievancesManager.Search(grievanceId, request);

					return Content(
						HttpStatusCode.OK,
						new AppealResponse(
							ResponseStatus.Success,
							SuccessMessage,
							ErrorCode.Success,
							request.CorrelationGuid,
							grievances.TotalRecordCount,
							grievances.Results.FirstOrDefault()));
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
					_logger.LogInfo("End SearchGrievances");
				}
		}

		[HttpGet]
		[Route("api/v1/search/grievance/provider/{providerId}")]
		public IHttpActionResult SearchProviderGrievances(
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

					ISearchResults<Appeal> grievances =
						_providerGrievancesManager.Search(providerId, request);

					return Content(
						HttpStatusCode.OK,
						new ListAppealsResponse(
							ResponseStatus.Success,
							SuccessMessage,
							ErrorCode.Success,
							request.CorrelationGuid,
							grievances.TotalRecordCount,
							grievances.Results));
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
					_logger.LogInfo("End SearchProviderGrievances");
				}
		}

		[HttpGet]
		[Route("api/v1/search/grievance/member/{memberId}")]
		public IHttpActionResult SearchMemberGrievances(
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

					ISearchResults<Appeal>
						grievances = _memberGrievancesManager.Search(memberId, request);

					return Content(
						HttpStatusCode.OK,
						new ListAppealsResponse(
							ResponseStatus.Success,
							SuccessMessage,
							ErrorCode.Success,
							request.CorrelationGuid,
							grievances.TotalRecordCount,
							grievances.Results));
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
					_logger.LogInfo("End SearchMemberGrievances");
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

		[HttpGet]
		[Route("api/v1/search/grievance/object/{objectId}")]
		public IHttpActionResult GetGrievance(
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

					Appeal grievance = _grievancesManager.GetAppeal(objectId, request);

					return Content(
						HttpStatusCode.OK,
						new AppealResponse(
							ResponseStatus.Success,
							SuccessMessage,
							ErrorCode.Success,
							request.CorrelationGuid,
							1,
							grievance));
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
					_logger.LogInfo("End GetGrievance");
				}
			}
		}
}
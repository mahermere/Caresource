// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Hplc
//   RequestManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Managers.v2
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Caching;
	using System.Web.Http.ModelBinding;
	using CareSource.WC.OnBase.Core.Diagnostics.Interfaces;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using Newtonsoft.Json;
	using WC.Services.Hplc.Adapters.v2.WorkView;
	using WC.Services.Hplc.Mappers.v2;
	using WC.Services.Hplc.Models.v2;
	using Filter = WC.Services.Hplc.Models.v2.Filter;
	using Request = WC.Services.Hplc.Models.v2.Request;

	/// <summary>
	///    Data and functions describing a CareSource.WC.Services.Hplc.Managers.v1.RequestManager
	/// object.
	/// </summary>
	/// <seealso cref="IRequestManager" />
	public class RequestManager : IRequestManager
	{
		private readonly IDataSetManager _dataSetManager;
		private readonly ILogger _logger;
		private readonly MemoryCache _memoryCache;
		private readonly IAdapter _workViewAdapter;
		private readonly IModelMapper<WorkViewObject, Request> _requestMapper;
		private readonly IHieModelMapper<WorkViewObject, Request> _hieRequestMapper;

		/// <summary>
		///    Initializes a new instance of the <see cref="RequestManager" /> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="workViewAdapter">The work view adapter.</param>
		/// <param name="dataSetManager">The data set manager.</param>
		/// <param name="memoryCache"></param>
		/// <param name="requestMapper"></param>
		/// <param name="hieRequestMapper"></param>
		public RequestManager(
			ILogger logger,
			IAdapter workViewAdapter,
			IDataSetManager dataSetManager,
			MemoryCache memoryCache,
			IModelMapper<WorkViewObject,Request> requestMapper,
			IHieModelMapper<WorkViewObject, Request> hieRequestMapper)
		{
			_workViewAdapter = workViewAdapter;
			_logger = logger;
			_dataSetManager = dataSetManager;
			_memoryCache = memoryCache;
			_requestMapper = requestMapper;
			_hieRequestMapper = hieRequestMapper;
		}

		/// <summary>
		///    Updates the request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public long UpdateRequest(Request request)
			=> throw new NotImplementedException();

		/// <summary>
		///    Creates the request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		public long CreateRequest(HplcServiceRequest request)
		{
			_logger.LogDebug(
				$"Starting: {nameof(CreateRequest)}",
				new Dictionary<string, object> { { "request", request } });

			request.RequestData.JsonData = JsonConvert.SerializeObject(request.RequestData);

			WorkViewObject workViewRequest = _requestMapper.GetMappedModel(request.RequestData);
			long result = _workViewAdapter.CreateRequest(workViewRequest);

			return result;
		}

		/// <summary>
		/// Searches the by npi.
		/// </summary>
		/// <param name="applicationNumber">The object identifier.</param>
		/// <param name="npi">The provider npi.</param>
		/// <returns></returns>
		public StatusResponse SearchByNpi(
			string applicationNumber,
			string npi)
		{
			SearchRequest sr = new SearchRequest()
			{
				Filters = new[]
				{
					new Filter()
					{
						Name = Constants.Request.ApplicationNumber,
						Value = applicationNumber
					}
				},
				SourceApplication = "OnBase HPLC Service",
				UserId = "OnBase Service"
			};

			Request request = HieSearch(sr).FirstOrDefault();

			if (request == null)
			{
				return null;
			}

			IEnumerable<Provider> providers = request.Providers.Where(
				p =>
					!p.Npi.IsNullOrWhiteSpace()
					&& p.Npi.Equals(
						npi,
						StringComparison.InvariantCultureIgnoreCase)
					|| !p.GroupNpi.IsNullOrWhiteSpace()
					&& p.GroupNpi.Equals(
						npi,
						StringComparison.InvariantCultureIgnoreCase));

			return new StatusResponse
			{
				RequestId = request.Id,
				RequestType = request.Type,
				ApplicationId = request.ApplicationId,
				RequestDate = request.Date,
				Providers = providers
			};
		}

		/// <summary>
		/// Searches the specified request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		private IEnumerable<Request> HieSearch(SearchRequest request)
		{
			IEnumerable<WorkViewObject> wvo = _workViewAdapter.HieSearch(
				Constants.Request.ClassName,
				Constants.WorkViewObjects.Filters.RequestDetails,
				request.Attributes.Select(a => a.Name).ToArray(),
				request.Filters
					.ToDictionary(
						f => f.Name,
						f => f.Value));

			return wvo.Select(o => _hieRequestMapper.GetMappedModel(o));
		}

		/// <summary>
		/// Searches the specified request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		public IEnumerable<Request> Search(SearchRequest request)
		{
			IEnumerable<WorkViewObject> wvo = _workViewAdapter.Search(
				Constants.Request.ClassName,
				Constants.WorkViewObjects.Filters.RequestDetails,
				request.Attributes.Select(a => a.Name).ToArray(),
				request.Filters
					.ToDictionary(
						f => f.Name,
						f => f.Value));

			return wvo.Select(o => _requestMapper.GetMappedModel(o));
		}

		/// <summary>
		///    Gets the workview object.
		/// </summary>
		/// <param name="objectId">The object identifier.</param>
		/// <returns></returns>
		public Request GetRequest(long objectId)
		{
			Request request = _requestMapper.GetMappedModel(_workViewAdapter.GetObject(objectId));

			return request;
		}

		/// <summary>
		///    Validates the request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="modelState">State of the model.</param>
		/// <returns></returns>
		public bool ValidateRequest(
			HplcServiceRequest request,
			ModelStateDictionary modelState)
		{
			if (request == null)
			{
				modelState.AddModelError(
					Constants.Request.ClassName,
					"The request could not be parsed, please check the JSON syntax.");
				return false;
			}

			List<string> hpatList = _dataSetManager
				.GetDataSet(Constants.DataSetNames.HealthPartnerAgreementType)
				.ToList();

			request.RequestData.TrimAllStrings();
			ValidateProducts(
				request,
				modelState);

			if (!request.RequestData.HealthPartnerAgreementType.IsNullOrWhiteSpace())
			{
				if (!hpatList.Contains(request.RequestData.HealthPartnerAgreementType))
				{
					modelState.AddModelError(
						$"RequestData.HealthPartnerAgreementType",
						$"[{request.RequestData.HealthPartnerAgreementType}] is not a valid"
						+ $" {Constants.DataSetNames.HealthPartnerAgreementType} Type.");
				}
			}

			ValidateProviders(
				request.RequestData.Providers,
				modelState);

			if (!request.RequestData.Type.IsNullOrWhiteSpace())
			{
				List<string> requestTypes = _dataSetManager
					.GetDataSet("Provider Maintenance Request Types")
					.ToList();
				if (!requestTypes.Contains(request.RequestData.Type))
				{
					modelState.AddModelError(
						"RequestData.Type",
						$"[{request.RequestData.Type}] is not a valid request type");
				}
			}

			return modelState.IsValid;
		}

		private void ValidateProviders(
			IEnumerable<Provider> requestDataProviders,
			ModelStateDictionary modelState)
		{
			if (!requestDataProviders.SafeAny())
			{
				return;
			}

			List<string> actions = _dataSetManager.GetDataSet(Constants.DataSetNames.ActionType)
				.ToList();

			List<string> providerTypes = _dataSetManager.GetDataSet(Constants.DataSetNames.ProviderType)
				.ToList();

			List<string> languages = _dataSetManager.GetDataSet(Constants.DataSetNames.HplcProviderLanguage)
				.ToList();

			int i = 0;
			foreach (Provider provider in requestDataProviders)
			{
				provider.TrimAllStrings();

				if (!actions.Contains(provider.ActionType))
				{
					modelState.AddModelError(
						$"RequestData.Providers[{i}].ActionType",
						$"[{provider.ActionType}] is not a valid Action Type.");
				}

				if (!provider.Language.IsNullOrWhiteSpace())
				{
					if (!languages.Contains(provider.Language, StringComparer.InvariantCultureIgnoreCase))
					{
						modelState.AddModelError(
							$"RequestData.Providers[{i}].Language",
							$"[{provider.Language}] is not a valid Language, use Language Other.");
					}
				}

				if (!providerTypes.Contains(provider.Type))
				{
					modelState.AddModelError(
						$"RequestData.Providers[{i}].ProviderType",
						$"[{provider.Type}] is not a valid Provider Type.");
				}

				ValidateLocations(provider.Locations, modelState, i);
				i++;
			}
		}

		private void ValidateProducts(
			HplcServiceRequest request,
			ModelStateDictionary modelState)
		{
			if (!request.RequestData.Products.SafeAny())
			{
				return;
			}

			List<string> products = _dataSetManager.GetDataSet(Constants.DataSetNames.ApiProducts)
				.ToList();

			List<string> productErrors =
			(
				from product in request.RequestData.Products
				where !products.Contains(product.Name.SafeTrim())
				select $"[{product.Name.SafeTrim()}] is not a valid Product.").ToList();

			if (productErrors.SafeAny())
			{
				modelState.AddModelError(
					"RequestData.Products",
					string.Join(
						$",{Environment.NewLine}",
						productErrors));
			}
		}

		private void ValidateLocations(
			IEnumerable<Location> providerLocations,
			ModelStateDictionary modelState,
			int iProvider)
		{
			List<string> actions = _dataSetManager
				.GetDataSet(Constants.DataSetNames.ActionTypeProviderLocations).ToList();

			List<string> addressTypes = _dataSetManager
				.GetDataSet(Constants.DataSetNames.AddressType).ToList();

			List<string> phoneTypes = _dataSetManager
				.GetDataSet(Constants.DataSetNames.PhoneTypeProvider).ToList();

			int iLocation = 0;
			
			foreach (Location location in providerLocations)
			{
				location.TrimAllStrings();

				if (!actions.Contains(location.ActionType))
				{
					modelState.AddModelError(
						$"RequestData.Providers[{iProvider}].Location[{iLocation}].ActionType",
						$"[{location.ActionType}] is not a valid Action Type.");
				}

				foreach (string type in location.Types)
				{
					if (!addressTypes.Contains(type))
					{
						modelState.AddModelError(
							$"RequestData.Providers[{iProvider}].Location[{iLocation}].Type",
							$"[{type}] is not a valid Location Type.");
					}
				}

				int iPhone = 0;
				foreach (Phone phone in location.Phones)
				{
					if (!phoneTypes.Contains(phone.Type))
					{
						modelState.AddModelError(
							$"RequestData.Providers[{iProvider}].Location[{iLocation}].Phone[{iPhone++}",
							$"[{phone.Type}] is not a valid Phone Type.");
					}
				}
				
				iLocation++;
			}
		}
	}
}
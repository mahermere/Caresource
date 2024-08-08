// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2023. All rights reserved.
// 
//   WC.Services.HplcManagement
//   RequestManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace HplcManagement.Managers.v1
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Caching;
	using System.Security.Permissions;
	using System.Web.Http.ModelBinding;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using HplcManagement.Adapters.v1.WorkView;
	using HplcManagement.Mappers.v1.Interfaces;
	using HplcManagement.Models.v1;
	using Hyland.Unity.WorkView;
	using Microsoft.Extensions.Logging;
	using Newtonsoft.Json;
	using Attribute = Hyland.Unity.WorkView.Attribute;
	using Filter = HplcManagement.Models.v1.Filter;
	using Object = Hyland.Unity.WorkView.Object;

	/// <summary>
	///    Data and functions describing a CareSource.WC.Services.Hplc.Managers.v1.RequestManager
	///    object.
	/// </summary>
	/// <seealso cref="IRequestManager" />
	public class RequestManager : IRequestManager
	{
		/// <summary>
		///    Initializes a new instance of the <see cref="RequestManager" /> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="workViewAdapter">The work view adapter.</param>
		/// <param name="dataSetManager">The data set manager.</param>
		/// <param name="memoryCache"></param>
		/// <param name="requestMapper"></param>
		public RequestManager(
			ILogger logger,
			IAdapter workViewAdapter,
			IDataSetManager dataSetManager,
			MemoryCache memoryCache,
			IModelMapper<Data, Object> requestMapper)
		{
			_workViewAdapter = workViewAdapter;
			_logger = logger;
			_dataSetManager = dataSetManager;
			_memoryCache = memoryCache;
			_requestMapper = requestMapper;
		}

		private readonly IDataSetManager _dataSetManager;
		private readonly ILogger _logger;
		private readonly MemoryCache _memoryCache;
		private readonly IModelMapper<Data, Object> _requestMapper;
		private readonly IAdapter _workViewAdapter;

		/// <summary>
		///    Updates the request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public long UpdateRequest(HplcServiceRequest request)
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

			request.RequestData.Properties.Add(
				"JsonData",
				JsonConvert.SerializeObject(request.RequestData));
			//request.JsonData = JsonConvert.SerializeObject(request.RequestData);

			long result = _workViewAdapter.CreateRequest(request.RequestData);

			return result;
		}

		/// <summary>
		///    Searches the by npi.
		/// </summary>
		/// <param name="applicationNumber">The object identifier.</param>
		/// <param name="npi">The provider npi.</param>
		/// <returns></returns>
		public StatusResponse SearchByNpi(
			string applicationNumber,
			string npi)
		{
			SearchRequest sr = new SearchRequest
			{
				Filters = new[]
				{
					new Filter
					{
						Name = Constants.Request.ApplicationNumber,
						Value = applicationNumber
					}
				},
				SourceApplication = "OnBase HPLC Service",
				UserId = "OnBase Service"
			};

			Data request = HieSearch(sr)
				.FirstOrDefault();

			if (request == null)
			{
				return null;
			}

			IEnumerable<Data> providers = request.Related
				.Where(r => r.ClassName == Constants.Provider.ClassName)
				.Where(
					p => p.Properties[Constants.Provider.Npi]
							.Equals(
								npi,
								StringComparison.InvariantCultureIgnoreCase)
						|| p.Properties[Constants.Provider.GroupNpi]
							.Equals(
								npi,
								StringComparison.InvariantCultureIgnoreCase));

			return new StatusResponse
			{
				RequestId = request.Id,
				RequestType = request.Properties[Constants.Request.Type],
				RequestDate = Convert.ToDateTime(request.Properties[Constants.Request.Date]),
				Providers = providers.GroupBy(d => d.Id)
					.Select(d => d.First())
			};
		}

		/// <summary>
		///    Searches the specified request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		public IEnumerable<Data> Search(SearchRequest request)
			=> _workViewAdapter.Search(
				Constants.Request.ClassName,
				Constants.WorkViewObjects.Filters.RequestDetails,
				request.Attributes.Select(a => a.Name)
					.ToArray(),
				request.Filters
					.ToDictionary(
						f => f.Name,
						f => f.Value));

		/// <summary>
		///    Gets the workview object.
		/// </summary>
		/// <param name="objectId">The object identifier.</param>
		/// <returns></returns>
		public Data GetRequest(long objectId)
			=> _workViewAdapter.GetObject(objectId);

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

			string requestDataClassName = request.RequestData.ClassName;
			Class wvRequest = _workViewAdapter.GetClass(requestDataClassName);
			if (wvRequest == null)
			{
				modelState.AddModelError(
					$"Invalid Class Name '{requestDataClassName}'",
					$"Class Name '{requestDataClassName}' is not a valid class");
			}

			
			ValidateProperties(
				request.RequestData.Properties,
				wvRequest,
				modelState,
				string.Empty,
				null);

			IEnumerable<string> hpatList = _dataSetManager
				.GetDataSet(Constants.DataSetNames.HealthPartnerAgreementType);

			request.RequestData.TrimAllStrings();

			KeyValuePair<string, string> hpat =
				request.RequestData.Properties.FirstOrDefault(
					k => k.Key == Constants.Request.HealthPartnerAgreementType);

			if (hpat.Value != null)
			{
				if (!hpatList.Contains(hpat.Value))
				{
					modelState.AddModelError(
						"Request.Properties.HealthPartnerAgreementType",
						$"[{hpat.Value}] is not a valid"
						+ $" {Constants.DataSetNames.HealthPartnerAgreementType}.");
				}
			}

			if (request.RequestData.Properties.ContainsKey(Constants.Request.Type))
			{
				IEnumerable<string> requestTypes = _dataSetManager
					.GetDataSet("Provider Maintenance Request Types");

				string requestType = request.RequestData.Properties[Constants.Request.Type];
				if (requestType.IsNullOrWhiteSpace())
				{
					modelState.AddModelError(
						"Request",
						$"[{Constants.Request.Type}] is required");
				}

				if (!requestTypes.Contains(requestType))
				{
					modelState.AddModelError(
						"Request.RequestType",
						$"[{requestType}] is not a valid Request Type.");
				}
			}
			else
			{
				modelState.AddModelError(
					"Request.RequestType",
					$"[{Constants.Request.Type}] is required");
			}

			ValidateProducts(
				request.RequestData.Related.Where(
					r => r.ClassName.Equals(
						Constants.Products.Product,
						StringComparison.InvariantCultureIgnoreCase)),
				modelState);

			ValidateProviders(
				request.RequestData.Related.Where(
					r => r.ClassName.Equals(
						Constants.Provider.ClassName,
						StringComparison.InvariantCultureIgnoreCase)),
				modelState);

			ValidateRelated(
				request.RequestData.Related.Where(
					r => r.ClassName.Equals(
						Constants.Tin.ClassName,
						StringComparison.InvariantCultureIgnoreCase)),
				modelState,
				"",
				null);

			return modelState.IsValid;
		}

		/// <summary>
		///    Searches the specified request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		private IEnumerable<Data> HieSearch(SearchRequest request)
			=> _workViewAdapter.HieSearch(
				Constants.Request.ClassName,
				Constants.WorkViewObjects.Filters.RequestDetails,
				request.Attributes.Select(a => a.Name)
					.ToArray(),
				request.Filters
					.ToDictionary(
						f => f.Name,
						f => f.Value));

		private void ValidateRelated(
			IEnumerable<Data> data,
			ModelStateDictionary modelState,
			string parentName,
			int? parentCounter)
		{
			if (data.SafeAny())
			{
				Class wvTin = _workViewAdapter.GetClass(data.First().ClassName);

				string parentKey = parentCounter.HasValue
					? $".{parentName}[{parentCounter}]"
					: string.Empty;
				int counter = 0;
				foreach (Data datum in data)
				{
					ValidateProperties(
						datum.Properties,
						wvTin,
						modelState,
						$"Request{parentKey}",
						counter++);
				}
			}
		}
		
		private void ValidateLocations(
			IEnumerable<Data> providerLocations,
			ModelStateDictionary modelState,
			int iProvider)
		{
			IEnumerable<string> actions = _dataSetManager
				.GetDataSet(Constants.DataSetNames.ActionTypeProviderLocations);


			IEnumerable<string> addressTypes = _dataSetManager
				.GetDataSet(Constants.DataSetNames.AddressType);

			int iLocation = 0;
			Class wvLocation = _workViewAdapter.GetClass(Constants.Location.ClassName);

			foreach (Data location in providerLocations)
			{
				location.TrimAllStrings();

				ValidateProperties(
					location.Properties,
					wvLocation,
					modelState,
					$"Request.Provider[{iProvider}]",
					iLocation);

				if (location.Properties.ContainsKey(Constants.Location.ActionType))
				{
					string actionType = location.Properties[Constants.Location.ActionType];
					if (actionType.IsNullOrWhiteSpace())
					{
						modelState.AddModelError(
							$"Request.Providers[{iProvider}].Location[{iLocation}]",
							"[ActionType] is required");
					}
					else if (!actions.Contains(actionType))
					{
						modelState.AddModelError(
							$"Request.Providers[{iProvider}].Location[{iLocation}].ActionType",
							$"[{actionType}] is not a valid Action Type.");
					}
				}

				if (location.Properties.ContainsKey(Constants.Location.AddressType))
				{
					string addressType = location.Properties[Constants.Location.AddressType];
					if (addressType.IsNullOrWhiteSpace())
					{
						modelState.AddModelError(
							$"Request.Providers[{iProvider}].Location[{iLocation}]",
							$"[{Constants.Location.AddressType}] is required");
					}
					else if (!addressTypes.Contains(addressType))
					{
						modelState.AddModelError(
							$"Request.Providers[{iProvider}].Location[{iLocation}]"
							+ $".{Constants.Location.AddressType}",
							$"[{addressType}] is not a valid Action Type.");
					}
				}

				ValidatePhones(
					modelState,
					location.Related.Where(r => r.ClassName.Equals(Constants.Phone.ClassName)),
					iProvider,
					iLocation);

				iLocation++;
			}
		}

		private void ValidatePhones(
			ModelStateDictionary modelState,
			IEnumerable<Data> phones,
			int iProvider,
			int iLocation)
		{
			int iPhone = 0;

			Class wvPhone = _workViewAdapter.GetClass(Constants.Phone.ClassName);
			string parent = $"Request.Provider[{iProvider}].Location[{iLocation}]";

			List<string> phoneTypes = _dataSetManager
					.GetDataSet(Constants.DataSetNames.PhoneTypeProvider)
					.ToList();

			foreach (Data phone in phones)
			{
				phone.TrimAllStrings();

				ValidateProperties(
					phone.Properties,
					wvPhone,
					modelState,
					parent,
					iPhone);

				string phoneType = phone.Properties[Constants.Phone.Type];

				if (!phoneTypes.Contains(phoneType))
				{
					modelState.AddModelError(
						$"{parent}.Phone[{iPhone++}]",
						$"[{phoneType}] is not a valid Phone Type.");
				}
			}
		}

		private void ValidateProducts(
			IEnumerable<Data> requestProducts,
			ModelStateDictionary modelState)
		{
			if (!requestProducts.SafeAny())
			{
				return;
			}

			List<string> products = _dataSetManager.GetDataSet(Constants.DataSetNames.ApiProducts)
				.ToList();

			List<string> productErrors =
				(
					from product in requestProducts
					where !products.Contains(product.Properties[Constants.Products.Name])
					select $"[{product.Properties[Constants.Products.Name]}] is not a valid Product.")
				.ToList();

			if (productErrors.SafeAny())
			{
				modelState.AddModelError(
					"RequestData.Products",
					string.Join(
						$",{Environment.NewLine}",
						productErrors));
			}
		}

		private void ValidateProperties(
			Dictionary<string, string> properties,
			Class wvClass,
			ModelStateDictionary modelState,
			string parent,
			int? count)
		{
			string countString = count.HasValue
				? $"[{count}]"
				: string.Empty;

			parent = parent.IsNullOrWhiteSpace()
				? string.Empty
				: $"{parent}.";

			string modelKeyParent = $"{parent}{wvClass.Name}{countString}";
			IDictionary<string, string> updates = new Dictionary<string, string>();
			foreach (KeyValuePair<string, string> property in properties)
			{
				if (property.Value.IsNullOrWhiteSpace())
				{
					continue;
				}

				
				Attribute attribute = null;
				string modelKey = $"{modelKeyParent}.{property.Key}";
				try
				{
					attribute = wvClass.Attributes.Find(property.Key);
				}
				catch
				{
					modelState.AddModelError(
						$"{modelKeyParent}",
						"Is not a valid property name");
				}

				if (attribute != null)
				{
					KeyValuePair<string, string> kvp = property;

					switch (attribute.AttributeType)
					{
						case AttributeType.Relation:
						case AttributeType.Integer:
							if (!long.TryParse(
									property.Value,
									out _))
							{
								modelState.AddModelError(
									modelKey,
									$" [{property.Value}] cannot be converted to an INT64");
							}

							break;
						case AttributeType.Decimal:
						case AttributeType.Currency:
							if (!decimal.TryParse(
									property.Value,
									out _))
							{
								modelState.AddModelError(
									modelKey,
									$"[{property.Value}] cannot be converted to a Decimal");
							}

							break;
						case AttributeType.Float:
							if (!float.TryParse(
									property.Value,
									out _))
							{
								modelState.AddModelError(
									modelKey,
									$"[{property.Value}] cannot be converted to a Float");
							}

							break;
						case AttributeType.Date:
							if (!DateTime.TryParse(
									property.Value,
									out _))
							{
								modelState.AddModelError(
									modelKey,
									$"[{property.Value}] cannot be converted to a Date");
							}

							break;
						case AttributeType.DateTime:
							if (!DateTime.TryParse(
									property.Value,
									out _))
							{
								modelState.AddModelError(
									modelKey,
									$"[{property.Value}] cannot be converted to a DateTime");
							}

							break;
						case AttributeType.Boolean:
							if (!bool.TryParse(
									property.Value,
									out _))
							{
								modelState.AddModelError(
									modelKey,
									$"[{property.Value}] cannot be converted to a Boolean");
							}

							break;
						case AttributeType.Document:
							break;
						case AttributeType.EncryptedAlphanumeric:
						case AttributeType.Alphanumeric:
						case AttributeType.FormattedText:
							if (property.Value.SafeTrim()
									.Length
								> attribute.DataLength)
							{
								modelState.AddModelError(
									$"{modelKey}",
									$"The value '{property.Value.SafeTrim()}' with length "
									+ $"[{property.Value.Length}], exceeds the maximum length of "
									+ $"[{attribute.DataLength}] characters.");
							}

							break;
						case AttributeType.Text:
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
				}
				else
				{
					modelState.AddModelError(
						$"{modelKey}",
						"Is not a valid property name");
				}
			}
		}

		private void ValidateProviders(
			IEnumerable<Data> providers,
			ModelStateDictionary modelState)
		{
			if (!providers.SafeAny())
			{
				return;
			}

			IEnumerable<string> actions = _dataSetManager.GetDataSet(Constants.DataSetNames.ActionType);

			IEnumerable<string> providerTypes = _dataSetManager
				.GetDataSet(Constants.DataSetNames.ProviderType);

			IEnumerable<string> languages = _dataSetManager
				.GetDataSet(Constants.DataSetNames.HplcProviderLanguage);

			int count = 0;
			Class wvProvider = _workViewAdapter.GetClass(Constants.Provider.ClassName);

			string providerMessage = $"Request.Providers[{count}]";
			foreach (Data provider in providers)
			{
				provider.TrimAllStrings();

				ValidateProperties(
					provider.Properties,
					wvProvider,
					modelState,
					"Request",
					count);

				string providerAction = provider.Properties[Constants.Provider.Action];

				if (!actions.Contains(providerAction))
				{
					modelState.AddModelError(
						$"{providerMessage}.ActionType",
						$"[{providerAction}] is not a valid Action Type.");
				}

				string providerLanguage = provider.Properties[Constants.Provider.Language];

				if (!providerLanguage.IsNullOrWhiteSpace())
				{
					if (!languages.Contains(
							providerLanguage,
							StringComparer.InvariantCultureIgnoreCase))
					{
						modelState.AddModelError(
							$"{providerMessage}.Language",
							$"[{providerLanguage}] is not a valid Language, use Language Other.");
					}
				}

				string providerType = provider.Properties
					.First(p => p.Key == Constants.Provider.Type)
					.Value;

				if (!providerTypes.Contains(providerType))
				{
					modelState.AddModelError(
						$"{providerMessage}.ProviderType",
						$"[{providerType}] is not a valid Provider Type.");
				}

				ValidateLocations(
					provider.Related
						.Where(
							r => r.ClassName.Equals(
								Constants.Location.ClassName,
								StringComparison.CurrentCultureIgnoreCase)),
					modelState,
					count);

				ValidateRelated(
					provider.Related
						.Where(
							r => r.ClassName.Equals(
								Constants.Boards.ClassName,
								StringComparison.CurrentCultureIgnoreCase)),
					modelState,
					provider.ClassName,
					count);

				ValidateLanguages(
					provider.Related
						.Where(
							r => r.ClassName.Equals(
								Constants.Language.ClassName,
								StringComparison.CurrentCultureIgnoreCase)),
					modelState,
					count);

				count++;
			}
		}

		private void ValidateLanguages(
			IEnumerable<Data> languages,
			ModelStateDictionary modelState,
			int providerCount)
		{
			ValidateRelated(
				languages,
				modelState,
				Constants.Provider.ClassName,
				providerCount);

			IEnumerable<string> languageValues =
				_dataSetManager.GetDataSet(Constants.DataSetNames.HplcProviderLanguage);

			foreach (Data language in languages)
			{
				int count = 0;
				if (!languageValues.Contains(language.Properties[Constants.Language.LanguageProperty]))
				{
					modelState.AddModelError(
						$"Request.Providers[{providerCount}].Languages[{count}]",
						$"{language.Properties[Constants.Language.LanguageProperty]} is not a valid Language");
				}

				count++;
			}

		}
	}
}
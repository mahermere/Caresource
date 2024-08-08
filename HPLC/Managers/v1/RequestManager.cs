// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Hplc
//   RequestManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Managers.v1
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Caching;
	using System.Web.Http.ModelBinding;
	using CareSource.WC.Entities.WorkView;
	using CareSource.WC.OnBase.Core.Diagnostics.Interfaces;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using WC.Services.Hplc.Adapters.v1.WorkView;
	using WC.Services.Hplc.Models.v1;
	using Filter = WC.Services.Hplc.Models.v1.Filter;
	using Request = WC.Services.Hplc.Models.v1.Request;

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

		/// <summary>
		///    Initializes a new instance of the <see cref="RequestManager" /> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="workViewAdapter">The work view adapter.</param>
		/// <param name="dataSetManager">The data set manager.</param>
		/// <param name="memoryCache"></param>
		public RequestManager(
			ILogger logger,
			IAdapter workViewAdapter,
			IDataSetManager dataSetManager,
			MemoryCache memoryCache)
		{
			_workViewAdapter = workViewAdapter;
			_logger = logger;
			_dataSetManager = dataSetManager;
			_memoryCache = memoryCache;
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

			long id = _workViewAdapter.CreateRequest(request);

			IDictionary<long, string> products =
				(IDictionary<long, string>) _memoryCache.Get("API - Products ObjectIds");

			List<ObjectPostRequest> postRequests = _workViewAdapter.CreateProducts(
				id,
				request.RequestData.Products.Select(
					requestDataProduct => products
						.FirstOrDefault(p => p.Value.Equals(requestDataProduct.SafeTrim()))
						.Key)).ToList();

			postRequests.Add(
			_workViewAdapter.CreateEntityTin(
				id,
				request.RequestData.EntityTin));

			_workViewAdapter.SaveXRefObjects(postRequests);

			CreateProviders(
				id,
				request.RequestData.Providers);

			_logger.LogDebug($"Finished: {nameof(CreateRequest)}");

			return id;
		}
		
		private void CreateProviders(
			in long requestId,
			IEnumerable<Provider> providers)
		{
			foreach (Provider p in providers)
			{
				ObjectPostRequest provider = _workViewAdapter.CreateProvider(
					requestId,
					p);

				long providerId = _workViewAdapter.SaveXRefObject(provider);

				if (p.Locations == null
					|| !p.Locations.Any())
				{
					continue;
				}

				foreach (Location location in p.Locations)
				{
					List<ObjectPostRequest> locations = _workViewAdapter.CreateProviderLocations(
						providerId,
						new List<Location> { location });

					IEnumerable<WorkviewObject> locationIds = _workViewAdapter.SaveXRefObjects(locations);

					if (location.Phones == null
						|| !location.Phones.Any())
					{
						continue;
					}

					List<ObjectPostRequest> phones = new List<ObjectPostRequest>();

					foreach (WorkviewObject locationId in locationIds
						.Where(l => l.Attributes
							.Any(a => a.Name == "AddressType" && a.Value =="Practice")))
					{
						 phones.AddRange(_workViewAdapter.CreateProviderLocationPhones(
							locationId.Id.GetValueOrDefault(0),
							location.Phones.ToList()));
						
					}
					_workViewAdapter.SaveXRefObjects(phones);
				}
			}
		}

		/// <summary>
		/// Searches the by npi.
		/// </summary>
		/// <param name="applicationNumber">The object identifier.</param>
		/// <param name="providerNpi">The provider npi.</param>
		/// <returns></returns>
		public Request SearchByNpi(
			string applicationNumber,
			string providerNpi)
		{
			SearchRequest sr = new SearchRequest()
			{
				Filters = new[]
				{
					new Filter()
					{
						Name = "ApplicationNumber",
						Value = applicationNumber
					}
				},
				SourceApplication = "OnBase HPLC Service",
				UserId = "OnBase Service"
			};

			Request request = Search(sr).FirstOrDefault();


			return request != null && request.Providers.Any(
				p => p.Npi.Equals(
					providerNpi,
					StringComparison.InvariantCultureIgnoreCase))
				? request
				: null;
			
		}

		/// <summary>
		/// Searches the specified request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		public IEnumerable<Request> Search(SearchRequest request)
		{
			IEnumerable<WorkviewObject> wvo = _workViewAdapter.Search(
				Constants.WorkViewObjects.Classes.Request,
				Constants.WorkViewObjects.Filters.RequestDetails,
				request.Attributes.Select(a => a.Name).ToArray(),
				request.Filters
					.ToDictionary(
						f => f.Name,
						f => f.Value));

			return wvo.Select(CreateAndPopulateRequest);

		}

		/// <summary>
		///    Gets the workview object.
		/// </summary>
		/// <param name="objectId">The object identifier.</param>
		/// <returns></returns>
		public Request GetRequest(long objectId)
		{
			WorkviewObject obj = _workViewAdapter.GetObject(
				objectId,
				"Request",
				"API - Request Details");

			Request request = CreateAndPopulateRequest(obj);

			return request;
		}

		private Request CreateAndPopulateRequest(WorkviewObject obj)
		{
			long objectId;
			Request request = CreateRequest(obj);

			IEnumerable<WorkviewObject> products = _workViewAdapter.Search(
				"RequestXProduct",
				"API - Request Products",
				new string[] { "Product.ProductName" },
				new Dictionary<string, string> { { "request", obj.Id.ToString() } });

			request.Products = products.Select(
				p => p.Attributes.First()
					.Value);

			IEnumerable<WorkviewObject> tin = _workViewAdapter.Search(
				"TIN",
				"All Entity TIN's",
				new string[] { "EntityTIN" },
				new Dictionary<string, string> { { "linktoRequest", obj.Id.ToString() } });

			request.EntityTin = tin.FirstOrDefault()
				?.Attributes?.FirstOrDefault()
				?.Value;

			PopulateProviders(
				obj.Id.Value,
				request);
			return request;
		}

		private void PopulateProviders(
			long requestId,
			Request request)
		{
			IEnumerable<WorkviewObject> providers = _workViewAdapter.Search(
				"Provider",
				"API - Provider Details",
				new string[]
				{
					Constants.ActionType,
					Constants.CaqhNumber,
					Constants.Degree,
					Constants.DateOfBirth,
					Constants.GroupNpi,
					Constants.MedicaidId,
					Constants.MedicareId,
					Constants.Name,
					Constants.Npi,
					Constants.ProviderRaceEthnicity,
					Constants.SecondarySpecialty,
					Constants.Specialty,
					Constants.Ssn,
					Constants.Tin,
					Constants.ProviderType,
					$"{Constants.LinkToRequest}.RequestID",
					Constants.Comments,
					Constants.LicenseNumber,
					Constants.DeaNumber,
					Constants.BoardCertified,
					Constants.HospitalAffiliations,
					Constants.ProviderGroupWebsite,
					Constants.TeleMedicineServicesProvided,
					Constants.GroupId,
					Constants.ProviderStatus
				},
				new Dictionary<string, string>
					{ { $"{Constants.LinkToRequest}.RequestID", requestId.ToString() } });

			request.Providers = providers.Select(
				p => new Provider
				{
					ActionType = p.Attributes
						.FirstOrDefault(a => a.Name == Constants.ActionType)
						?.Value,
					BoardCertified = p.Attributes
						.FirstOrDefault(a => a.Name == Constants.BoardCertified)
						?.Value,
					CaqhNumber = p.Attributes
						.FirstOrDefault(a => a.Name == Constants.CaqhNumber)
						?.Value,
					Comments = p.Attributes
						.FirstOrDefault(a => a.Name == Constants.Comments)
						?.Value,
					DeaNumber = p.Attributes
						.FirstOrDefault(a => a.Name == Constants.DeaNumber)
						?.Value,
					Degree = p.Attributes
						.FirstOrDefault(a => a.Name == Constants.Degree)
						?.Value,
					Dob = p.Attributes
						.FirstOrDefault(a => a.Name == Constants.DateOfBirth)
						?.Value.ToSafeDateTime(),
					GroupNpi = p.Attributes
						.FirstOrDefault(a => a.Name == Constants.GroupNpi)
						?.Value,
					HospitalAffiliations = p.Attributes
						.FirstOrDefault(a => a.Name == Constants.HospitalAffiliations)
						?.Value,
					Id = p.Id.GetValueOrDefault(0),
					LicenseNumber = p.Attributes
						.FirstOrDefault(a => a.Name == Constants.LicenseNumber)
						?.Value,
					MedicaidId = p.Attributes
						.FirstOrDefault(a => a.Name == Constants.MedicaidId)
						?.Value,
					MedicareId = p.Attributes
						.FirstOrDefault(a => a.Name == Constants.MedicareId)
						?.Value,
					Name = p.Attributes
						.FirstOrDefault(a => a.Name == Constants.Name)
						?.Value,
					Npi = p.Attributes
						.FirstOrDefault(a => a.Name == Constants.Npi)
						?.Value,
					ProviderGroupWebsite = p.Attributes
						.FirstOrDefault(a => a.Name == Constants.ProviderGroupWebsite)
						?.Value,
					RaceEthnicity = p.Attributes
						.FirstOrDefault(a => a.Name == Constants.ProviderRaceEthnicity)
						?.Value,
					SecondarySpecialty = p.Attributes
						.FirstOrDefault(a => a.Name == Constants.SecondarySpecialty)
						?.Value,
					Specialty = p.Attributes
						.FirstOrDefault(a => a.Name == Constants.Specialty)
						?.Value,
					Ssn = p.Attributes
						.FirstOrDefault(a => a.Name == Constants.Ssn)
						?.Value,
					Tin = p.Attributes
						.FirstOrDefault(a => a.Name == Constants.Tin)
						?.Value,
					Type = p.Attributes
						.FirstOrDefault(a => a.Name == Constants.ProviderType)
						?.Value,
					TelemedicineServicesProvided = (p.Attributes
						.FirstOrDefault(a => a.Name == Constants.TeleMedicineServicesProvided)
						?.Value).IsNullOrWhiteSpace()
						? false
						: Convert.ToBoolean(
							p.Attributes
								.FirstOrDefault(a => a.Name == Constants.TeleMedicineServicesProvided)
								?.Value),
					GroupId = p.Attributes
						.FirstOrDefault(a => a.Name == Constants.GroupId)
						?.Value,
					Status = p.Attributes
						.FirstOrDefault(a => a.Name == Constants.ProviderStatus)
						?.Value,
					Locations = PopulateLocations(p.Id.GetValueOrDefault(0))
				});
		}

		private IEnumerable<Location> PopulateLocations(long providerId)
		{
			IEnumerable<WorkviewObject> locations = _workViewAdapter.Search(
				"Location",
				"API - Location",
				new string[]
				{
					Constants.Location.ActionType,
					Constants.Location.AddressType,
					Constants.Location.Capacity,
					Constants.Location.City,
					Constants.Location.County,
					Constants.Location.GenderRestrictions,
					Constants.Location.MaxAge,
					Constants.Location.MinAge,
					Constants.Location.Notes,
					Constants.Location.PostalCode,
					Constants.Location.PrimaryLocation,
					Constants.Location.State,
					Constants.Location.Street1, 
					Constants.Location.Street2,
					$"{Constants.Location.LinkToProvider}.objectid"
				},
				new Dictionary<string, string>
					{ { $"{Constants.Location.LinkToProvider}.objectid", providerId.ToString() } });

			return locations.Select(
				l =>
					new Location
					{
						Phones = PopulatePhones(l.Id.GetValueOrDefault(0)),
						ActionType = l.Attributes
							.FirstOrDefault(a => a.Name == Constants.Location.ActionType)
							?.Value,
						Capacity =
							(l.Attributes
								.FirstOrDefault(a => a.Name == Constants.Location.Capacity)
								?.Value).IsNullOrWhiteSpace()
								? 0
								: Convert.ToInt32(
									l.Attributes
										.FirstOrDefault(a => a.Name == Constants.Location.Capacity)
										?.Value),
						City = l.Attributes
							.FirstOrDefault(a => a.Name == Constants.Location.City)
							?.Value,
						County = l.Attributes
							.FirstOrDefault(a => a.Name == Constants.Location.County)
							?.Value,
						GenderRestrictions = l.Attributes
							.FirstOrDefault(a => a.Name == Constants.Location.GenderRestrictions)
							?.Value,
						MaxAge = (l.Attributes
							.FirstOrDefault(a => a.Name == Constants.Location.MaxAge)
							?.Value).IsNullOrWhiteSpace()
							? 0
							: Convert.ToInt32(
								l.Attributes
									.FirstOrDefault(a => a.Name == Constants.Location.MaxAge)
									?.Value),
						MinAge = (l.Attributes
							.FirstOrDefault(a => a.Name == Constants.Location.MinAge)
							?.Value).IsNullOrWhiteSpace()
							? 0
							: Convert.ToInt32(
								l.Attributes
									.FirstOrDefault(a => a.Name == Constants.Location.MinAge)
									?.Value),
						Notes = l.Attributes
							.FirstOrDefault(a => a.Name == Constants.Location.Notes)
							?.Value,
						PostalCode = l.Attributes
							.FirstOrDefault(a => a.Name == Constants.Location.PostalCode)
							?.Value,
						IsPrimary = !(l.Attributes
							.FirstOrDefault(a => a.Name == Constants.Location.PrimaryLocation)
							?.Value).IsNullOrWhiteSpace() && Convert.ToBoolean(
							l.Attributes
								.FirstOrDefault(a => a.Name == Constants.Location.PrimaryLocation)
								?.Value),
						State = l.Attributes
							.FirstOrDefault(a => a.Name == Constants.Location.State)
							?.Value,
						Street1 = l.Attributes
							.FirstOrDefault(a => a.Name == Constants.Location.Street1)
							?.Value,
						Street2 = l.Attributes
							.FirstOrDefault(a => a.Name == Constants.Location.Street2)
							?.Value,
						Type = new[]
						{
							l.Attributes
								.FirstOrDefault(a => a.Name == Constants.Location.AddressType)
								?.Value
						}
					});
		}

		private IEnumerable<Phone> PopulatePhones(long locationId)
		{
			IEnumerable<WorkviewObject> phones = _workViewAdapter.Search(
				"Phone",
				"API - Phones",
				new string[]
				{
					Constants.Location.Phone.Number,
					Constants.Location.Phone.Type,

					$"{Constants.Location.Phone.LinkToLocation}.objectid",

				},
				new Dictionary<string, string>
					{ { $"{Constants.Location.Phone.LinkToLocation}.objectid", locationId.ToString() } });

			return phones.Select(p =>
				new Phone()
				{
					Type = p.Attributes
						.FirstOrDefault(a => a.Name == Constants.Location.Phone.Type)
						?.Value,
					Number = p.Attributes
						.FirstOrDefault(a => a.Name == Constants.Location.Phone.Number)
						?.Value
				});
		}

		private static Request CreateRequest(WorkviewObject workViewObject)
		{
			Request request = new Request();

			string attributeDateValue = workViewObject.Attributes
				.FirstOrDefault(a => a.Name == Constants.ChangeEffectiveDate)
				?.Value;

			if (!attributeDateValue.IsNullOrWhiteSpace())
			{
				request.ChangeEffectiveDate = DateTime.Parse(attributeDateValue);
			}

			request.ContactName = workViewObject.Attributes
				.FirstOrDefault(a => a.Name == Constants.ContactName)?.Value;

			request.ContactPhone = workViewObject.Attributes
				.FirstOrDefault(a => a.Name == Constants.ContactPhone)?.Value;

			attributeDateValue = workViewObject.Attributes
				.FirstOrDefault(a => a.Name == Constants.RequestDate)
				?.Value;

			if (!attributeDateValue.IsNullOrWhiteSpace())
			{
				request.Date = DateTime.Parse(attributeDateValue);
			}

			request.ContactEmail = workViewObject.Attributes
				.FirstOrDefault(a => a.Name == Constants.EntityContactEmail)
				?.Value;
			request.EntityName = workViewObject.Attributes
				.FirstOrDefault(a => a.Name == Constants.EntityName)?.Value;

			request.EntityTin = workViewObject.Attributes
				.FirstOrDefault(a => a.Name == Constants.EntityTin)?.Value;

			request.Id = workViewObject.Id.GetValueOrDefault(0);

			request.Notes = workViewObject.Attributes
				.FirstOrDefault(a => a.Name == Constants.RequestNotes)?.Value;

			request.Status = workViewObject.Attributes
				.FirstOrDefault(a => a.Name == Constants.RequestStatus)?.Value;

			request.Type = workViewObject.Attributes
				.FirstOrDefault(a => a.Name == Constants.RequestType)?.Value;

			attributeDateValue = workViewObject.Attributes
				.FirstOrDefault(a => a.Name == Constants.CareSourceReceivedDate)?.Value;

			if (!attributeDateValue.IsNullOrWhiteSpace())
			{
				request.CareSourceReceivedDate = DateTime.Parse(attributeDateValue);
			}
			
			request.PrimaryState = workViewObject.Attributes
				.FirstOrDefault(a => a.Name == Constants.PrimaryState)?.Value;

			request.SignatoryEmail = workViewObject.Attributes
				.FirstOrDefault(a => a.Name == Constants.SignatoryEmail)?.Value;

			request.SignatoryName = workViewObject.Attributes
				.FirstOrDefault(a => a.Name == Constants.SignatoryName)?.Value;

			request.SignatoryTitle = workViewObject.Attributes
				.FirstOrDefault(a => a.Name == Constants.SignatoryTitle)?.Value;

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
					Constants.WorkViewObjects.Classes.Request,
					"The request could not be parsed, please check the JSON syntax.");
				return false;
			}

			request.RequestData.TrimAllStrings();
			ValidateProducts(
				request,
				modelState);

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
			if (!requestDataProviders.Any())
			{
				return;
			}

			List<string> actions = _dataSetManager.GetDataSet(Constants.ActionType)
				.ToList();

			List<string> providerTypes = _dataSetManager.GetDataSet(Constants.ProviderType)
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
			if (!request.RequestData.Products.Any())
			{
				return;
			}

			List<string> products = _dataSetManager.GetDataSet("API - Products")
				.ToList();


			List<string> productErrors =
			(
				from product in request.RequestData.Products
				where !products.Contains(product.SafeTrim())
				select $"[{product.SafeTrim()}] is not a valid Product.").ToList();

			if (productErrors.Any())
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
			List<string> actions = _dataSetManager.GetDataSet("Action Type - Provider Locations")
				.ToList();

			List<string> addressTypes = _dataSetManager.GetDataSet("Address Type")
				.ToList();

			List<string> phoneTypes = _dataSetManager.GetDataSet("Phone Type - Provider")
				.ToList();

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

				foreach (string type in location.Type)
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
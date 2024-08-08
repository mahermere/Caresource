// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Hplc
//   Adapter.Requests.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Adapters.v1.WorkView
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using CareSource.WC.Entities.WorkView;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using WC.Services.Hplc.Managers.v1;
	using WC.Services.Hplc.Models.v1;

	public partial class Adapter
	{
		/// <summary>
		///    Creates a Request in the WorkView HPLC Management Application.
		/// </summary>
		/// <remarks>
		///    Method [post]s data to the WorkView application and returns the object id of the created
		///    object
		/// </remarks>
		/// <param name="request">The request.</param>
		/// <returns>
		///    The object Id of the created Request in WorkView
		/// </returns>
		public long CreateRequest(HplcServiceRequest request)
		{
			ObjectPostRequest wvRequest = new ObjectPostRequest
			{
				ApplicationName = _applicationName,
				ClassName = Constants.WorkViewObjects.Classes.Request,
			};

			List<WorkviewAttributeRequest> attributes = new List<WorkviewAttributeRequest>();

			attributes.Add(
				new WorkviewAttributeRequest
				{
					Name = Constants.CareSourceReceivedDate,
					Value = request.RequestData.CareSourceReceivedDate.GetValueOrDefault(DateTime.Now)
						.ToString("s")
				});

			if (request.RequestData.ChangeEffectiveDate.HasValue)
			{
				attributes.Add(
					new WorkviewAttributeRequest
					{
						Name = Constants.ChangeEffectiveDate,
						Value = request.RequestData.ChangeEffectiveDate.GetValueOrDefault()
							.ToString("s")
					});
			}

			attributes.Add(
				new WorkviewAttributeRequest
				{
					Name = Constants.ContactName,
					Value = request.RequestData.ContactName
				});
			
			attributes.Add(
				new WorkviewAttributeRequest
				{
					Name = Constants.ContactPhone,
					Value = request.RequestData.ContactPhone
				});

			if (!request.RequestData.ContactEmail.IsNullOrWhiteSpace())
			{
				attributes.Add(
					new WorkviewAttributeRequest
					{
						Name = Constants.EntityContactEmail,
						Value = request.RequestData.ContactEmail
					});
			}

			if (!request.RequestData.EntityName.IsNullOrWhiteSpace())
			{
				attributes.Add(
					new WorkviewAttributeRequest
					{
						Name = Constants.EntityName,
						Value = request.RequestData.EntityName
					});
			}

			attributes.Add(
				new WorkviewAttributeRequest
				{
					Name = Constants.RequestDate,
					Value = request.RequestData.Date.ToString("s")
				});

			if (!request.RequestData.Notes.IsNullOrWhiteSpace())
			{
				attributes.Add(
					new WorkviewAttributeRequest
					{
						Name = Constants.RequestNotes,
						Value = request.RequestData.Notes
					});
			}

			attributes.Add(
				new WorkviewAttributeRequest
				{
					Name = Constants.RequestSource,
					Value = request.RequestData.Source
				});

			attributes.Add(
				new WorkviewAttributeRequest
				{
					Name = Constants.RequestType,
					Value = request.RequestData.Type
				});

			attributes.Add(
				new WorkviewAttributeRequest
				{
					Name = Constants.SignatoryEmail,
					Value = request.RequestData.SignatoryEmail
				});

			attributes.Add(
				new WorkviewAttributeRequest
				{
					Name = Constants.SignatoryName,
					Value = request.RequestData.SignatoryName
				});

			attributes.Add(
				new WorkviewAttributeRequest
				{
					Name = Constants.SignatoryTitle,
					Value = request.RequestData.SignatoryTitle
				});

			wvRequest.Attributes = attributes;

			return Post<ObjectPostRequest, long>(wvRequest);
		}

		/// <summary>
		///    Creates a provider and associates it with the provided Request Id.
		/// </summary>
		/// <param name="requestId">The request identifier.</param>
		/// <param name="providers">List of providers</param>
		/// <returns></returns>
		public List<ObjectPostRequest> CreateProviders(
			long requestId,
			IEnumerable<Provider> providers)
			=> providers.Select(
					provider => CreateProvider(
						requestId,
						provider))
				.ToList();

		/// <summary>
		///    Creates the provider.
		/// </summary>
		/// <param name="requestId">The request identifier.</param>
		/// <param name="provider">The provider.</param>
		/// <returns></returns>
		public ObjectPostRequest CreateProvider(
			long requestId,
			Provider provider)
		{
			ObjectPostRequest p = new ObjectPostRequest
			{
				ApplicationName = _applicationName,
				ClassName = Constants.WorkViewObjects.Classes.Provider,
				Attributes = null
			};

			List<WorkviewAttributeRequest> attributes = new List<WorkviewAttributeRequest>
			{
				new WorkviewAttributeRequest
				{
					Name = Constants.LinkToRequest,
					Value = requestId.ToString()
				}
			};

			if (!provider.ActionType.IsNullOrWhiteSpace())
			{
				attributes.Add(
					new WorkviewAttributeRequest
					{
						Name = Constants.ActionType,
						Value = provider.ActionType
					});
			}

			if (!provider.CaqhNumber.IsNullOrWhiteSpace())
			{
				attributes.Add(
					new WorkviewAttributeRequest
					{
						Name = Constants.CaqhNumber,
						Value = provider.CaqhNumber
					});
			}

			if (!provider.Degree.IsNullOrWhiteSpace())
			{
				attributes.Add(
					new WorkviewAttributeRequest
					{
						Name = Constants.Degree,
						Value = provider.Degree
					});
			}

			if (provider.Dob.HasValue)
			{
				attributes.Add(
					new WorkviewAttributeRequest
					{
						Name = Constants.DateOfBirth,
						Value = provider.Dob.Value.ToShortDateString()
					});
			}

			if (!provider.GroupNpi.IsNullOrWhiteSpace())
			{
				attributes.Add(
					new WorkviewAttributeRequest
					{
						Name = Constants.GroupNpi,
						Value = provider.GroupNpi
					});
			}

			if (!provider.MedicaidId.IsNullOrWhiteSpace())
			{
				attributes.Add(
					new WorkviewAttributeRequest
					{
						Name = Constants.MedicaidId,
						Value = provider.MedicaidId
					});
			}

			if (!provider.MedicareId.IsNullOrWhiteSpace())
			{
				attributes.Add(
					new WorkviewAttributeRequest
					{
						Name = Constants.MedicareId,
						Value = provider.MedicareId
					});
			}

			if (!provider.Name.IsNullOrWhiteSpace())
			{
				attributes.Add(
					new WorkviewAttributeRequest
					{
						Name = Constants.Name,
						Value = provider.Name
					});
			}

			if (!provider.Npi.IsNullOrWhiteSpace())
			{
				attributes.Add(
					new WorkviewAttributeRequest
					{
						Name = Constants.Npi,
						Value = provider.Npi
					});
			}

			if (!provider.RaceEthnicity.IsNullOrWhiteSpace())
			{
				attributes.Add(
					new WorkviewAttributeRequest
					{
						Name = Constants.ProviderRaceEthnicity,
						Value = provider.RaceEthnicity
					});
			}

			if (!provider.Tin.IsNullOrWhiteSpace())
			{
				attributes.Add(
					new WorkviewAttributeRequest
					{
						Name = Constants.Tin,
						Value = provider.Tin
					});
			}

			if (!provider.Type.IsNullOrWhiteSpace())
			{
				attributes.Add(
					new WorkviewAttributeRequest
					{
						Name = Constants.ProviderType,
						Value = provider.Type
					});
			}

			if (!provider.SecondarySpecialty.IsNullOrWhiteSpace())
			{
				attributes.Add(
					new WorkviewAttributeRequest
					{
						Name = Constants.SecondarySpecialty,
						Value = provider.SecondarySpecialty
					});
			}

			if (!provider.Specialty.IsNullOrWhiteSpace())
			{
				attributes.Add(
					new WorkviewAttributeRequest
					{
						Name = Constants.Specialty,
						Value = provider.Specialty
					});
			}

			if (!provider.Ssn.IsNullOrWhiteSpace())
			{
				attributes.Add(
					new WorkviewAttributeRequest
					{
						Name = Constants.Ssn,
						Value = provider.Ssn
					});
			}

			if (!provider.LicenseNumber.IsNullOrWhiteSpace())
			{
				attributes.Add(
					new WorkviewAttributeRequest
					{
						Name = Constants.LicenseNumber,
						Value = provider.LicenseNumber
					});
			}

			if (!provider.DeaNumber.IsNullOrWhiteSpace())
			{
				attributes.Add(
					new WorkviewAttributeRequest
					{
						Name = Constants.DeaNumber,
						Value = provider.DeaNumber
					});
			}

			if (!provider.BoardCertified.IsNullOrWhiteSpace())
			{
				attributes.Add(
					new WorkviewAttributeRequest
					{
						Name = Constants.BoardCertified,
						Value = provider.BoardCertified
					});
			}

			if (!provider.HospitalAffiliations.IsNullOrWhiteSpace())
			{
				attributes.Add(
					new WorkviewAttributeRequest
					{
						Name = Constants.HospitalAffiliations,
						Value = provider.HospitalAffiliations
					});
			}

			if (!provider.ProviderGroupWebsite.IsNullOrWhiteSpace())
			{
				attributes.Add(
					new WorkviewAttributeRequest
					{
						Name = Constants.ProviderGroupWebsite,
						Value = provider.ProviderGroupWebsite
					});
			}

			if (!provider.Comments.IsNullOrWhiteSpace())
			{
				attributes.Add(
					new WorkviewAttributeRequest
					{
						Name = Constants.Comments,
						Value = provider.Comments
					});
			}
			
			attributes.Add(
				new WorkviewAttributeRequest
				{
					Name = Constants.TeleMedicineServicesProvided,
					Value = Convert.ToString(provider.TelemedicineServicesProvided)
				});


			if (!provider.GroupId.IsNullOrWhiteSpace())
			{
				attributes.Add(
					new WorkviewAttributeRequest
					{
						Name = Constants.GroupId,
						Value = provider.GroupId
					});
			}

			attributes.Add(
				new WorkviewAttributeRequest
				{
					Name = Constants.PrimaryCareProvider,
					Value = Convert.ToString(provider.IsPrimaryCareProvider)
				});


			p.Attributes = attributes;

			return p;
		}

		/// <summary>
		///    Gets or sets the Adapter Create Provider Locations
		/// </summary>
		public List<ObjectPostRequest> CreateProviderLocations(
			long providerObjectId,
			List<Location> locations)
		{
			List<ObjectPostRequest> objects = new List<ObjectPostRequest>();

			foreach (Location location in locations)
			{
				foreach (string locationType in location.Type)
				{
					List<WorkviewAttributeRequest> attributes = new List<WorkviewAttributeRequest>
					{
						new WorkviewAttributeRequest
						{
							Name = Constants.Location.LinkToProvider,
							Value = providerObjectId.ToString()
						}
					};

					if (!location.ActionType.IsNullOrWhiteSpace())
					{
						attributes.Add(
							new WorkviewAttributeRequest
							{
								Name = Constants.Location.ActionType,
								Value = location.ActionType
							});
					}

					attributes.Add(
						new WorkviewAttributeRequest
						{
							Name = Constants.Location.AddressType,
							Value = locationType
						});

					if (locationType == "Practice")
					{
						attributes.Add(
							new WorkviewAttributeRequest
							{
								Name = Constants.Location.PrimaryLocation,
								Value = Convert.ToString(location.IsPrimary)
							});

						attributes.Add(
							new WorkviewAttributeRequest
							{
								Name = Constants.Location.Capacity,
								Value = Convert.ToString(location.Capacity)
							});

						attributes.Add(
							new WorkviewAttributeRequest
							{
								Name = Constants.Location.MinAge,
								Value = Convert.ToString(location.MinAge)
							});

						attributes.Add(
							new WorkviewAttributeRequest
							{
								Name = Constants.Location.MaxAge,
								Value = Convert.ToString(location.MaxAge)
							});

						if (!location.GenderRestrictions.IsNullOrWhiteSpace())
						{
							attributes.Add(
								new WorkviewAttributeRequest
								{
									Name = Constants.Location.GenderRestrictions,
									Value = location.GenderRestrictions
								});
						}
					}

					if (!location.Notes.IsNullOrWhiteSpace())
					{
						attributes.Add(
							new WorkviewAttributeRequest
							{
								Name = Constants.Location.Notes,
								Value = location.Notes
							});
					}

					if (!location.City.IsNullOrWhiteSpace())
					{
						attributes.Add(
							new WorkviewAttributeRequest
							{
								Name = Constants.Location.City,
								Value = location.City
							});
					}

					if (!location.County.IsNullOrWhiteSpace())
					{
						attributes.Add(
							new WorkviewAttributeRequest
							{
								Name = Constants.Location.County,
								Value = location.County
							});
					}

					attributes.Add(
						new WorkviewAttributeRequest
						{
							Name = Constants.Location.LinkToProvider,
							Value = Convert.ToString(providerObjectId)
						});

					if (!location.PostalCode.IsNullOrWhiteSpace())
					{
						attributes.Add(
							new WorkviewAttributeRequest
							{
								Name = Constants.Location.PostalCode,
								Value = location.PostalCode
							});
					}

					if (!location.State.IsNullOrWhiteSpace())
					{
						attributes.Add(
							new WorkviewAttributeRequest
							{
								Name = Constants.Location.State,
								Value = location.State
							});
					}

					if (!location.Street1.IsNullOrWhiteSpace())
					{
						attributes.Add(
							new WorkviewAttributeRequest
							{
								Name = Constants.Location.Street1,
								Value = location.Street1
							});
					}

					if (!location.Street2.IsNullOrWhiteSpace())
					{
						attributes.Add(
							new WorkviewAttributeRequest
							{
								Name = Constants.Location.Street2,
								Value = location.Street2
							});
					}

					ObjectPostRequest p = new ObjectPostRequest
					{
						ApplicationName = _applicationName,
						ClassName = Constants.Location.ClassName,
						Attributes = attributes,
					};

					objects.Add(p);
				}
			}

			return objects;
		}

		/// <summary>
		///    Creates the provider location phones.
		/// </summary>
		/// <param name="locationObjectId">The location object identifier.</param>
		/// <param name="phones">The phones.</param>
		/// <returns></returns>
		public List<ObjectPostRequest> CreateProviderLocationPhones(
			long locationObjectId,
			List<Phone> phones)
			=> phones.Select(
					phone => new List<WorkviewAttributeRequest>
					{
						CreateWorkViewRequest(
							Constants.Location.Phone.LinkToLocation,
							Convert.ToString(locationObjectId)),
						CreateWorkViewRequest(
							Constants.Location.Phone.Type,
							phone.Type),
						CreateWorkViewRequest(
							Constants.Location.Phone.Number,
							phone.Number)
					})
				.Select(
					attributes => new ObjectPostRequest
					{
						ApplicationName = _applicationName,
						ClassName = Constants.Location.Phone.ClassName,
						Attributes = attributes
					})
				.ToList();

		/// <summary>
		///    Creates a product request association with the provided Request Id
		/// </summary>
		/// <param name="requestId">The request identifier.</param>
		/// <param name="productIds">The product identifier.</param>
		/// <returns></returns>
		public IEnumerable<ObjectPostRequest> CreateProducts(
			long requestId,
			IEnumerable<long> productIds)
		{
			List<ObjectPostRequest> products = new List<ObjectPostRequest>();
			foreach (long productId in productIds)
			{
				products.Add(
					CreateXRefObject(
						"RequestXProduct",
						new KeyValuePair<string, long>(
							Constants.WorkViewObjects.Classes.Request,
							requestId),
						new KeyValuePair<string, long>(
							"Product",
							productId)));
			}

			return products;
		}

		/// <summary>
		///    Gets the object.
		/// </summary>
		/// <param name="objectId">The object identifier.</param>
		/// <param name="className">The class name.</param>
		/// <param name="filterName">The filter name.</param>
		/// <returns></returns>
		public WorkviewObject GetObject(
			in long objectId,
			string className,
			string filterName)
		{
			ObjectGetRequest request = new ObjectGetRequest
			{
				ClassName = className,
				FilterName = filterName
			};

			string route = $"/{objectId}";

			WorkviewObject result = Get<ObjectGetRequest, WorkviewObject>(
				request,
				route);

			return result;
		}

		/// <summary>
		///    Searches the specified class name.
		/// </summary>
		/// <param name="className">The class name.</param>
		/// <param name="filterName">The filter name.</param>
		/// <param name="attributes"></param>
		/// <param name="filters">The filters.</param>
		/// <returns></returns>
		public IEnumerable<WorkviewObject> Search(
			string className,
			string filterName,
			string[] attributes,
			IDictionary<string, string> filters)
		{
			ObjectGetRequest request = new ObjectGetRequest
			{
				ClassName = className,
				FilterName = filterName,
				Attributes = attributes,
				Filters = filters.Select(
					f => new FilterRequest
					{
						Name = f.Key,
						Value = f.Value
					})
			};

			return Get<ObjectGetRequest, IEnumerable<WorkviewObject>>(
				request,
				"/search");
		}

		/// <summary>
		///    Creates the tin.
		/// </summary>
		/// <param name="requestId">The request identifier.</param>
		/// <param name="entityTin">The entity tin.</param>
		public ObjectPostRequest CreateEntityTin(
			in long requestId,
			string entityTin)
		{
			ObjectPostRequest wvRequest =
				new ObjectPostRequest
				{
					ApplicationName = _applicationName,
					ClassName = Constants.Tin,
					Attributes = new List<WorkviewAttributeRequest>
					{
						new WorkviewAttributeRequest
						{
							Name = Constants.LinkToRequest,
							Value = requestId.ToString()
						},
						new WorkviewAttributeRequest
						{
							Name = Constants.EntityTin,
							Value = entityTin
						}
					}
				};

			return wvRequest;
		}

		/// <summary>
		///    Creates the xref records.
		/// </summary>
		/// <param name="records">The records.</param>
		/// <returns></returns>
		public IEnumerable<WorkviewObject> SaveXRefObjects(IEnumerable<ObjectPostRequest> records)
		{
			ObjectBatchPostRequest wvRequest =
				new ObjectBatchPostRequest { WorkViewObjects = records };

			IEnumerable<WorkviewObject> response =
				Post<ObjectBatchPostRequest, IEnumerable<WorkviewObject>>(
					wvRequest,
					"/batch");

			return response;
		}

		/// <summary>
		///    Creates the xref records.
		/// </summary>
		/// <param name="record">The record.</param>
		/// <returns></returns>
		public long SaveXRefObject(ObjectPostRequest record)
			=> Post<ObjectPostRequest, long>(record);

		/// <summary>
		///    Creates the xref entry.
		/// </summary>
		/// <param name="className">The class name.</param>
		/// <param name="parent">The parent.</param>
		/// <param name="child">The child.</param>
		/// <returns></returns>
		public ObjectPostRequest CreateXRefObject(
			string className,
			KeyValuePair<string, long> parent,
			KeyValuePair<string, long> child)
		{
			ObjectPostRequest wvRequest = new ObjectPostRequest
			{
				ApplicationName = _applicationName,
				ClassName = className,
				Attributes = null
			};

			List<WorkviewAttributeRequest> attributes = new List<WorkviewAttributeRequest>
			{
				new WorkviewAttributeRequest
				{
					Name = parent.Key,
					Value = parent.Value.ToString()
				},
				new WorkviewAttributeRequest
				{
					Name = child.Key,
					Value = child.Value.ToString()
				}
			};

			wvRequest.Attributes = attributes;

			return wvRequest;
		}

		private WorkviewAttributeRequest CreateWorkViewRequest(
			string name,
			string value)
		{
			if (!value.IsNullOrWhiteSpace())
			{
				return new WorkviewAttributeRequest
				{
					Name = name,
					Value = value
				};
			}

			return null;
		}

		/// <summary>
		///    Updates a Request in the WorkView HPLC Management Application.
		/// </summary>
		/// <remarks>
		///    Method [put]s data to the WorkView application and returns the object id of the created
		///    object
		/// </remarks>
		/// <param name="request">The request.</param>
		/// <returns>
		///    The object Id of the updated Request in WorkView
		/// </returns>
		public long UpdateRequest(HplcServiceRequest request)
			=> 0;
	}
}
// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   ImportProcessor
//   WorkviewAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.ImportProcessor.Dotnet8.Adapters.v1
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Globalization;
	using System.Linq;
	using System.Threading.Tasks;
	using CareSource.WC.Entities.Workview.v2;
	//using CareSource.WC.OnBase.Core.Configuration.Interfaces;
	//using CareSource.WC.OnBase.Core.Connection.Interfaces;
	using Microsoft.Extensions.Logging;
	using Hyland.Unity.WorkView;
	using Application = Hyland.Unity.Application;
	using Attribute = Hyland.Unity.WorkView.Attribute;
	using Object = Hyland.Unity.WorkView.Object;
    using WC.Services.ImportProcessor.Dotnet8.Adapters.v1.Interfaces;
    using WC.Services.ImportProcessor.Dotnet8.Models.v1;
    using WC.Services.ImportProcessor.Dotnet8.Connection.Interfaces;

    public class WorkviewAdapter : IWorkViewAdapter
	{
		private readonly string _application;
		private readonly string _classClaims;
		private readonly string _classDocuments;

		private readonly IApplicationConnectionAdapter<Application> _hylandApplication;

        //private readonly Applications _applications;
        private readonly log4net.ILog _logger;
        private bool _disposed;
        private readonly IConfiguration _configuration;
        public WorkviewAdapter(
			IApplicationConnectionAdapter<Application> hylandApplication,
            IConfiguration _configuration,
            log4net.ILog logger)
		{
			_hylandApplication = hylandApplication;
			_logger = logger;
			_application = _configuration["WorkViewSettings:WorkView.Application"];
			_classDocuments = _configuration["WorkViewSettings:WorkView.Class.Documents"];
			_classClaims = _configuration["WorkViewSettings:WorkView.Class.Claims"];
		}

		public long CreateDocumentObject(
			List<WorkviewAttributeRequest> attributes)
		{
			_logger.Debug("Creating Document Object (WV)");

			WorkviewObjectPostRequest dataRequest = new WorkviewObjectPostRequest
			{
				ApplicationName = _application,
				ClassName = _classDocuments,
				Attributes = attributes
			};

			WorkviewObject wvo = CreateObject(dataRequest);

			long? id = wvo.Id;

			return wvo.Id != null
				? (long)wvo.Id
				: -1;
		}

		public long CreateClaimsObjects(
			long objectId,
			Claim[] claims)
		{
			WorkviewObjectBatchRequest request = new WorkviewObjectBatchRequest();

			_logger.Debug("Creating Claims Objects (WV)");

			try
			{
				int page = 0;
				int amount = 100;

				WorkviewAttributeRequest linkToDocAttr = new WorkviewAttributeRequest
				{
					Name = "LinkToDocumentData",
					Value = objectId.ToString()
				};

				do
				{
					IEnumerable<WorkviewObjectPostRequest> workviewObjects =
						claims.Skip((page++ * amount))
							.Take(amount)
							.Select(
								c =>
								{
									return new WorkviewObjectPostRequest()
									{
										ApplicationName = _application,
										ClassName = _classClaims,
										Attributes =
											new[]
											{
												new WorkviewAttributeRequest
												{
													Name = "ClaimNumber",
													Value = c.attributes.First(a => a.Name == "Claim Number")
														.Value
												},
												new WorkviewAttributeRequest
												{
													Name = "MemberID",
													Value = c.attributes.First(a => a.Name == "Member ID")
														.Value
												},
												linkToDocAttr
											}
									};
								});

					request.WorkviewObjects = workviewObjects;

					IEnumerable<WorkviewObject> mappedModel = GetMappedModel(request);

					Task<IEnumerable<WorkviewObject>> wvo = CreateNewObjects(mappedModel);

				} while (page * amount < claims.Length);

				return claims.Length;
			}
			catch (Exception e)
			{
                _logger.Error("An unexpected error occurred attempting to create the Claims Class Object.", e);
                throw;
			}
		}

		private WorkviewObject CreateObject(WorkviewObjectPostRequest wvo)
		{
			Hyland.Unity.WorkView.Application wvApplication = LoadWvApplication(wvo.ApplicationName);

			Class wvClass = LoadWvClass(
				wvo.ClassName,
				wvApplication);

			_logger.Info(
				$"Loaded WorkView Class: {wvo.ApplicationName}");

			Object createdWvObject = wvClass.CreateObject(true);
			AttributeValueModifier attrModifier =
				createdWvObject.CreateAttributeValueModifier();

			foreach (WorkviewAttributeRequest attr in wvo.Attributes)
			{
				Attribute wvAttr =
					wvClass.Attributes.FirstOrDefault(a => a.Name == attr.Name);

				if (wvAttr == null)
				{
					throw new KeyNotFoundException(
						$"Could not find attribute '{attr.Name}' " +
						$"for class '{wvClass.Name}' in application " +
						$"'{wvApplication.Name}'.");
				}

				SetAttribute(
					attrModifier,
					wvAttr,
					attr.Value);
			}

			attrModifier.ApplyChanges();

			return new WorkviewObject
			{
				Id = createdWvObject.ID,
				ClassName = createdWvObject.Class.Name
			};
		}

		public IEnumerable<WorkviewObject> GetMappedModel(WorkviewObjectBatchRequest original)
		{
			return original?.WorkviewObjects.Select(
				wo => new WorkviewObject
				{
					ApplicationName = wo.ApplicationName,
					ClassName = wo.ClassName,

					Attributes = wo.Attributes.Select(
						a => new WorkviewAttribute
						{
							Name = a.Name,
							Value = a.Value
						})
				});
		}

		public async Task<IEnumerable<WorkviewObject>> CreateNewObjects(
			IEnumerable<WorkviewObject> workviewObjects)
		{
			Hyland.Unity.WorkView.Application wvApplication = null;
			Class wvClass = null;

			IEnumerable<Task<Object>> wvObjects =
				workviewObjects.OrderBy(e => e.ApplicationName)
					.Select(
						async wvo =>
						{
							if (wvApplication == null ||
								 !wvo.ApplicationName.Equals(wvApplication.Name))
							{
								wvApplication = LoadWvApplication(wvo.ApplicationName);
							}

							if (wvClass == null ||
								 !wvo.ClassName.Equals(wvClass.Name))
							{
								wvClass = LoadWvClass(
									wvo.ClassName,
									wvApplication);
							}

							Object createdWvObject = wvClass.CreateObject(true);

							if (createdWvObject == null)
							{
								throw new Exception(
									"Current running service account does not have access to create" +
									$" WorkView Class '{wvClass.Name}' object.");
							}

							AttributeValueModifier attrModifier =
								createdWvObject.CreateAttributeValueModifier();

							foreach (WorkviewAttribute attr in wvo.Attributes)
							{
								Attribute wvAttr =
									wvClass.Attributes.FirstOrDefault(a => a.Name == attr.Name);

								if (wvAttr == null)
								{
									throw new ArgumentException(
										$"Could not find attribute '{attr.Name}' " +
										$"for class '{wvClass.Name}' in application " +
										$"'{wvApplication.Name}'.");
								}

								SetAttribute(
									attrModifier,
									wvAttr,
									attr.Value);
							}

							attrModifier.ApplyChanges();

							return createdWvObject;
						});

			Object[] objects = await Task.WhenAll(wvObjects);


			return objects.Select(o => new WorkviewObject
			{
				ApplicationId = wvApplication.ID,
				ApplicationName = wvApplication.Name,
				ClassId = o.Class.ID,
				ClassName = o.Class.Name,
				Id = o.ID,
				Name = o.Name,
				CreatedBy = o.CreatedBy?.RealName,
				CreatedDate = o.CreatedDate,
				RevisionBy = o.RevisionBy?.RealName,
				RevisionDate = o.RevisionDate,
				Attributes = o.AttributeValues
					.Select(
						a => new WorkviewAttribute
						{
							Name = a.Name,
							Value = a.ToString()
						})
					.ToList()
			});
		}

		private Hyland.Unity.WorkView.Application LoadWvApplication(string applicationName)
		{
			Hyland.Unity.WorkView.Application wvApplication = _hylandApplication.Application
				.WorkView.Applications.Find(applicationName);

			if (wvApplication == null)
			{
				throw new ArgumentException(
					$"Could not find WorkView Application Name '{applicationName}'.");
			}

			return wvApplication;
		}

		private Class LoadWvClass(
			string className,
			Hyland.Unity.WorkView.Application wvApplication)
		{
			Class wvClass = wvApplication
				.Classes.Find(className);

			if (wvClass == null)
			{
				throw new ArgumentException(
					$"Could not find WorkView Class Name '{className}'.");
			}

			return wvClass;
		}

		private void SetAttribute(
			AttributeValueModifier attrModifier,
			Attribute attribute,
			string? value)
		{
			switch (attribute.AttributeType)
			{
				case AttributeType.Alphanumeric:
					if (value.Length > attribute.DataLength)
					{
						throw new ArgumentException(
							$"Given value '{value}' is over the max length of " +
							$"'{attribute.DataLength}' for attribute '{attribute.Name}'.");
					}

					attrModifier.SetAttributeValue(
						attribute,
						value);
					break;
				case AttributeType.Text:
					attrModifier.SetAttributeValue(
						attribute,
						value);
					break;
				case AttributeType.Boolean:
					attrModifier.SetAttributeValue(
						attribute,
						Convert.ToBoolean(value));
					break;
				case AttributeType.Currency:
					attrModifier.SetAttributeValue(
						attribute,
						Convert.ToDouble(value));
					//attrModifier.SetAttributeValue(
					//	attribute,
					//	Convert.ToDouble(value)) ??
					//	throw new ArgumentException(
					//		$"Given value '{value}' could not be " +
					//		$"converted to a double for Attribute '{attribute.Name}'.");
					break;
				case AttributeType.Date:
					DateTime dtValue;
                    DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtValue);
                    attrModifier.SetAttributeValue(
						attribute,
						dtValue);
					break;
				case AttributeType.DateTime:
                    DateTime dtValue1;
                    DateTime.TryParseExact(value, "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtValue1);
                    attrModifier.SetAttributeValue(
						attribute,
						dtValue1);
					break;
				case AttributeType.Decimal:
				case AttributeType.Float:
					attrModifier.SetAttributeValue(
						attribute,
						Convert.ToDecimal(value));
					break;
				case AttributeType.Integer:
					attrModifier.SetAttributeValue(
						attribute,
						Convert.ToInt64(value));
					break;
				case AttributeType.Relation:
					attrModifier.SetAttributeValue(
						attribute,
						Convert.ToInt64(value));
					break;
				default:
					throw new Exception(
						"Could not set attribute value for type " +
						$"'{attribute.AttributeType.ToString()}'. This has not been implemented.");
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		///    Finalizes an instance of the <see cref="WorkviewAdapter" /> class.
		/// </summary>
		~WorkviewAdapter()
			=> Dispose(false);

		/// <summary>
		///    Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing">
		///    <c>true</c> to release both managed and unmanaged resources;
		///    <c>false</c> to release only unmanaged resources.
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
			{
				return;
			}

			_disposed = true;
		}
    }
}
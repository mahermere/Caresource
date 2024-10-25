// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   OnBaseAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Adapters.v6
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using Hyland.Unity;
	using Hyland.Unity.UnityForm;
	using Hyland.Unity.WorkView;
	using Microsoft.Extensions.Logging;
	using Application = Hyland.Unity.Application;
	using Document = Hyland.Unity.Document;
	using InvalidDocumentTypeException = CareSource.WC.Entities.Exceptions.InvalidDocumentTypeException;
	using InvalidKeywordException = CareSource.WC.Entities.Exceptions.InvalidKeywordException;
    using log4net.Repository.Hierarchy;
    using log4net;
    using WC.Services.Document.Dotnet8.Adapters.v6.Interfaces;
    using WC.Services.Document.Dotnet8.Models.v6;
    using WC.Services.Document.Dotnet8.Repository;
    using WC.Services.Document.Dotnet8.Models.v6.Interfaces;
    using WC.Services.Document.Dotnet8.App_code;

    //using WC.Services.Document.MVC.Dotnet8.Adapters.Interfaces;

    public class OnBaseAdapter : IOnBaseAdapter
	{
		private readonly IGetDocumentAdapter<OnBaseDocument> _getDocumentAdapter;
        private readonly IRepository _repo;
        private readonly log4net.ILog _logger;

		private readonly Dictionary<int, string> _columnDataTypeMaps = new Dictionary<int, string>
		{
			{ 1, "KeyValueBig" },   //1 = Numeric 20
			{ 2, "KeyValueChar" },  //2 = Dual Table Alpha
			{ 3, "KeyValueCurr"},   //3 = Currency
			{ 4, "KeyValueDate" },  //4 = Date
			{ 5, "KeyValueFloat" }, //5 = Float
			{ 6, "KeyValueSmall" }, //6 = Numeric 9
			{ 9, "KeyValueTod" },   //9 = DateTime
			{ 10, "KeyValueChar" }, //10 = Single Table Alpha
			{ 11, "KeyValueCurr"},  //11 = Specific Currency
			{ 12, "KeyValueChar" }, //12 = Mixed Case Dual Table Alpha
			{ 13, "KeyValueChar" }  //13 = Mixed Case Single Table Alpha
		};

		public OnBaseAdapter(
            IRepository repo,
			IGetDocumentAdapter<OnBaseDocument> getDocumentAdapter,
            log4net.ILog logger)
		{
            _repo = repo;
            _getDocumentAdapter = getDocumentAdapter;
			_logger = logger;
		}
		
		public OnBaseDocument GetDocument(IDownloadRequest request)
			=> _getDocumentAdapter.GetDocument(
				request.DocumentId,
				request);

		public IEnumerable<DocumentType> GetDocumentTypes(
			IEnumerable<string> requestDocumentTypes)
		{
            _logger.DebugFormat(
			"Starting {0}, Document Types: {1}",
			nameof(GetDocumentTypes),
			requestDocumentTypes);

            if (!requestDocumentTypes.SafeAny())
			{
				return _repo.Application.Core.DocumentTypes;
			}
			
			List<DocumentType> docTypes = new List<DocumentType>();

			docTypes.AddRange(
				requestDocumentTypes.Select(GetDocumentType));

			List<string> errors =
				requestDocumentTypes
					.Where(
						docType => docTypes.All(
							dt => dt is null ||
								!dt.Name.Equals(
									docType,
									StringComparison.CurrentCultureIgnoreCase)))
					.ToList();

			string invalidDocTypes = string.Join( ", ", errors);
			
			if (errors.Any())
			{
				throw new InvalidDocumentTypeException(invalidDocTypes);
			}

			_logger.InfoFormat("Ending {0}", nameof(GetDocumentTypes));

            return docTypes;
		}

		public DocumentType GetDocumentType(string docTypeName)
			=> _repo.Application.Core.DocumentTypes.FirstOrDefault(
				dt => dt.Name.Equals(
					docTypeName,
					StringComparison.CurrentCultureIgnoreCase));

		public IEnumerable<KeywordType> GetKeywordTypes(IEnumerable<string> keywordNames)
		{
   //         _logger.DebugFormat(
   //         StringResouces.Getting_Keyword_Types,
			//new Dictionary<string, object> { { StringResouces.keywords, keywordNames } });

            List<KeywordType> keywords = new List<KeywordType>();
			if (keywordNames != null &&
				keywordNames.Any())
			{
				keywords.AddRange(keywordNames.Select(GetKeywordByName));
			}

			return keywords;
		}

		public KeywordType GetKeywordByName(string keywordName)
		{
			//_logger.DebugFormat(
			//	StringResouces.Getting_Keyword_Type,
			//	new Dictionary<string, object> { { StringResouces.keyword, keywordName } });

			KeywordType keywordType = _repo.Application.Core.KeywordTypes.FirstOrDefault(
				dt => dt.Name.Equals(
					keywordName,
					StringComparison.CurrentCultureIgnoreCase));

			if (keywordType == null)
			{
				throw new InvalidKeywordException(keywordName);
			}

			return keywordType;
		}

		public IEnumerable<Class> GetWorkViewClasses(string applicationName)
		{
			string appName = applicationName;

			Hyland.Unity.WorkView.Application wvApplication = GetWvApplication();

			return wvApplication.Classes.ToList();

			Hyland.Unity.WorkView.Application GetWvApplication()
			{
				Hyland.Unity.WorkView.Application app = _repo.Application.WorkView
					.Applications.FirstOrDefault(a => a.Name == appName);

				if (app == null)
				{
					throw new ArgumentException($"No WorkView Application found with name [{appName}]");
				}

				return app;
			}
		}

		public User CurrentUser => _repo.Application.CurrentUser;

		public (FormTemplate, StoreNewUnityFormProperties) GetUnityFormTemplate(string templateName)
		{
			//FormTemplate template =
			//	_repo.Application.Core.UnityFormTemplates.FirstOrDefault(
			//		t => t.Name == templateName || t.DocumentType.Name == templateName) ??
			//	_repo.Application.Core.UnityFormTemplates.FirstOrDefault(
			//		t => t.Name == templateName || t.Name == templateName);
			FormTemplate template = _repo.Application.Core.UnityFormTemplates.Find(Convert.ToInt32(templateName));

            if (template == null)
			{
				throw new Models.v6.InvalidDocumentTypeException(templateName);
			}

			StoreNewUnityFormProperties storeNewUnityFormProperties =
				_repo.Application.Core.Storage.CreateStoreNewUnityFormProperties(
					template);

			return (template, storeNewUnityFormProperties);
		}

		public long? CreateUnityForm(StoreNewUnityFormProperties props)
		{
			Document newDoc = _repo.Application.Core.Storage.StoreNewUnityForm(props);

			return newDoc.ID;
		}

		public string GetKeywordValueDbColumnName(KeywordType kt)
			=> _columnDataTypeMaps[GetKeyTypeTableDataType(kt)];

		public int GetKeyTypeTableDataType(KeywordType kt)
		{
			int datatype = Convert.ToInt32(
				typeof(KeywordType)
					.GetField(
						"_dataType",
						BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(kt)); ;

			return datatype;
		}
	}
}
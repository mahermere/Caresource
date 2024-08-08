// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   ImportProcessor
//   WorkviewAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace ImportProcessor.Adapters.v1
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using CareSource.WC.OnBase.Core.Connection.Interfaces;
	using Hyland.Unity;
	using ImportProcessor.Adapters.v1.Interfaces;
	using Microsoft.Extensions.Logging;

	public class KeywordAdapter : IKeywordAdapter<Keyword>
	{
		private readonly IApplicationConnectionAdapter<Application> _hylandApplication;
		private readonly ILogger _logger;

		public KeywordAdapter(
			IApplicationConnectionAdapter<Application> applicationConnectionAdapter,
			ILogger logger)
		{
			_hylandApplication = applicationConnectionAdapter;
			_logger = logger;
		}

		public Keyword CreateKeyword(string value, string keywordTypeName, string defaultKeywordDocumentTypeName, bool blankKeyword)
		{
			if (string.IsNullOrWhiteSpace(defaultKeywordDocumentTypeName))
			{
				throw new ArgumentException(
					"Must have Document Type object when adding add a new document to OnBase.");
			}

			DocumentTypeList documentTypeList = _hylandApplication
				.Application
				.Core
				.DocumentTypes;

			Hyland.Unity.DocumentType documentType = documentTypeList.Find(defaultKeywordDocumentTypeName);

			if (documentType == null)
			{
				throw new ArgumentException($"Could not find document type '{defaultKeywordDocumentTypeName}' in OnBase.");
			}

			_logger.LogDebug($"Retrieving keyword type from OnBase API for '{keywordTypeName}' keyword type name.");

			List<KeywordType> keywordTypes = _hylandApplication
				.Application
				.Core
				.KeywordTypes
					.Where(kwt => keywordTypeName == kwt.Name)
				.ToList();

			if (keywordTypes.Count == 0)
			{
				throw new Exception(
					$"Could not find keyword type names '{keywordTypeName}' in OnBase.");
			}

			KeywordType keywordType = keywordTypes.FirstOrDefault();

			return _CreateKeyword(value, keywordType, documentType, blankKeyword);
		}

		private Keyword _CreateKeyword(string value, KeywordType keywordType
				, DocumentType defaultKeywordDocumentType, bool blankKeyword)
		{
			_logger.LogDebug($"Creating keyword from OnBase API for '{keywordType.Name}' keyword type name" +
					$", a value of '{value}'" +
					$", a default document type name '{defaultKeywordDocumentType?.Name ?? "NULL"}'" +
					$", and blank keyword setting '{blankKeyword}'.");

			Keyword keyword = null;

			if (string.IsNullOrWhiteSpace(value) && defaultKeywordDocumentType != null)
			{
				keyword = keywordType.CreateKeywordWithDefaultValue(defaultKeywordDocumentType);
			}
			else if (blankKeyword)
			{
				keyword = keywordType.CreateBlankKeyword();
			}
			else
			{
				try
				{
					//Check the type this keyword needs to be converted to.
					KeywordDataType dataType = keywordType.DataType;
					switch (dataType)
					{
						case KeywordDataType.AlphaNumeric:
							keyword = keywordType.CreateKeyword(value);
							break;
						case KeywordDataType.Numeric20:
							keyword = keywordType.CreateKeyword(Convert.ToDecimal(value));
							break;
						case KeywordDataType.Numeric9:
							keyword = keywordType.CreateKeyword(Convert.ToInt64(value));
							break;
						case KeywordDataType.Currency:
							keyword =
								 keywordType.CreateKeyword(Convert.ToDecimal(Convert.ToDecimal(value).ToString("F")));
							break;
						case KeywordDataType.FloatingPoint:
							keyword = keywordType.CreateKeyword(float.Parse(value));
							break;
						case KeywordDataType.Date:
						case KeywordDataType.DateTime:
							keyword = keywordType.CreateKeyword(Convert.ToDateTime(value));
							break;
						default:
							break;
					}
				}
				catch (Exception e)
				{
					throw new ArgumentException(
						$"Could not create keyword for keyword type [{keywordType.ID}] {keywordType.Name} because of following error ${e.Message}.");
				}
			}

			if (keyword == null)
			{
				throw new Exception(
					$"Could not create keyword for keyword type [{keywordType.ID}] {keywordType.Name}.");
			}

			return keyword;
		}
	}
}
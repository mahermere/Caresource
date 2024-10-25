// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.Document
//   KeywordAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Adapters
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using Microsoft.Extensions.Logging;
	using WC.Services.Document.Dotnet8.Models;
	using Hyland.Unity;
    using WC.Services.Document.Dotnet8.Adapters.Interfaces;
    using Document = Hyland.Unity.Document;

    public class KeywordAdapter : IKeywordAdapter
	{
		/// <summary>
		///    Initializes a new instance of the <see cref="OnBaseGetDocumentAdapter" /> class.
		/// </summary>
		/// <param name="applicationConnectionAdapter">The application connection adapter.</param>
		/// <param name="logger"></param>
		public KeywordAdapter(
			IApplicationConnectionAdapter<Application> applicationConnectionAdapter,
			ILogger logger)
		{
			_hylandApplication = applicationConnectionAdapter;
			_logger = logger;
		}

		private readonly IApplicationConnectionAdapter<Application> _hylandApplication;

		private IEnumerable<Document> Documents { get; set; }

		private readonly ILogger _logger;
		public (bool isValid, IEnumerable<long> badIds) ValidateDocumentIds(IEnumerable<long> documentIds)
		{
			List<long> errors = new List<long>();

			List<Document> documents = new List<Document>();

			foreach (long id in documentIds)
			{
				Document doc = _hylandApplication.Application.Core.GetDocumentByID(id);

				if (doc == null)
				{
					errors.Add(id);
					_logger.LogInformation($"No Document found for Id: [{id}].");
					continue;
				}

				documents.Add(doc);
			}

			Documents = documents.AsEnumerable();
			return (!errors.Any(), errors);
		}

		public (bool isValid, Dictionary<long, IEnumerable<string>> badKeywords)
			ValidateKeywords(IEnumerable<KeywordUpdate> updates)
		{
			Dictionary<long, IEnumerable<string>> keywordErrors =
				new Dictionary<long, IEnumerable<string>>();

			foreach (Document document in Documents)
			{
				List<string> errors = (
					from keyword in updates.FirstOrDefault(item => item.DocumentId.Equals(document.ID))
						.Keywords
					from krt in document.DocumentType.KeywordRecordTypes
					where !krt.KeywordTypes.Any(kt => kt.Name.Equals(
						keyword.Key,
						StringComparison.InvariantCultureIgnoreCase))
					select keyword.Key).ToList();

				if (errors.Any())
				{
					keywordErrors.Add(document.ID, errors);
				}
			}

			return (!keywordErrors.Any(), keywordErrors);
		}

		/// <summary>
		///    Updates the keywords.
		/// </summary>
		/// <param name="updates">The updates.</param>
		/// <returns>
		///    A list of all the document Ids that were updated
		/// </returns>
		public (IEnumerable<long> successfulIds, IEnumerable<string> errors) UpdateKeywords(
			IEnumerable<KeywordUpdate> updates)
		{
			List<long> successfulIds = new List<long>();
			List<string> errors = new List<string>();

			foreach (Document document in Documents)
			{
				try
				{
					KeywordModifier keywordModifier = document.CreateKeywordModifier();

					foreach (KeyValuePair<string, string> keyword in updates
						.FirstOrDefault(d => d.DocumentId == document.ID)
						.Keywords)
					{

						KeywordType keywordType =
							_hylandApplication.Application.Core.KeywordTypes.FirstOrDefault(
								kwt => kwt.Name.Equals(
									keyword.Key,
									StringComparison.InvariantCulture));

						Keyword newKeyword = MapValueToKeyword(
							keywordType,
							keyword.Value);

						_logger.LogDebug(
							$"Created new {newKeyword.KeywordType.Name} value {newKeyword.Value}");

						Keyword currentKeyword = document.KeywordRecords.Find(keywordType)
							?.Keywords.Find(keywordType);

						keywordModifier.UpdateKeyword(
							currentKeyword,
							newKeyword);
					}

					const int maxAttempts = 3;
					int lockAttempt = 1;
					do
					{
						using (DocumentLock documentLock = document.LockDocument())
						{
							if (documentLock.Status != DocumentLockStatus.LockObtained)
							{
								_logger.LogInformation(
									"Unable to obtain Exclusive lock to update document "
									+ $"[{document.ID}]; Attempt #{lockAttempt}");

								Thread.Sleep(
									int.Parse(
										System.Configuration.ConfigurationManager
											.AppSettings["OnBase.DocumentLock.Wait"]));
							}
							else
							{
								keywordModifier.ApplyChanges();
								successfulIds.Add(document.ID);
								_logger.LogInformation(
									$"Updated document [{document.ID}] on attempt #{lockAttempt}");

								documentLock.Release();
								break;
							}
						}
					} while (lockAttempt++ < maxAttempts);

					if (lockAttempt > maxAttempts)
					{
						_logger.LogError(
							$"Unable to obtain Exclusive lock to update document [{document.ID}]");
						errors.Add($"Unable to obtain Exclusive lock to update document [{document.ID}]");
					}
				}
				catch (InvalidCastException invalidCastException)
				{
					_logger.LogError(
						invalidCastException,
						invalidCastException.Message);

					errors.Add($"{document.ID} {invalidCastException.Message}");
					continue;
				}
				catch (Exception e)
				{
					_logger.LogError(
						e,
						e.Message);
					throw;
				}
			}

			return (successfulIds, errors);
		}

		private Keyword MapValueToKeyword(
			KeywordType keywordType,
			string keywordValue)
		{
			if (!string.IsNullOrWhiteSpace(keywordValue))
			{
				switch (keywordType.DataType)
				{
					case KeywordDataType.Numeric20:
					case KeywordDataType.SpecificCurrency:
					case KeywordDataType.Currency:
                        decimal decimalValue;
                        if (decimal.TryParse(
                            keywordValue
                                .Replace(
                                    "$",
                                    string.Empty)
                                .Replace(
                                    "%",
                                    string.Empty),
                            out decimalValue))
                        {
                            return keywordType.CreateKeyword(decimalValue);
                        }

                        break;

					case KeywordDataType.Date:
					case KeywordDataType.DateTime:
                        DateTime? dateValue = DateTime.TryParse(keywordValue, out var parsedDate) ? parsedDate : (DateTime?)null;

                        if (dateValue.HasValue)
						{
							return keywordType.CreateKeyword(dateValue.Value);
						}

						break;
					case KeywordDataType.FloatingPoint:
                        if (double.TryParse(
                        keywordValue
                            .Replace(
                                "$",
                                string.Empty)
                            .Replace(
                                "%",
                                string.Empty),
                        out var doubleValue))
                        {
                            return keywordType.CreateKeyword(doubleValue);
                        }
                        break;
					case KeywordDataType.Numeric9:
                        if (long.TryParse(
                        keywordValue
                            .Replace(
                                "$",
                                string.Empty)
                            .Replace(
                                "%",
                                string.Empty),
                        out var longValue))
						{
							return keywordType.CreateKeyword(longValue);
						}

						break;
					case KeywordDataType.AlphaNumeric:
					case KeywordDataType.Undefined:
					default:
						return keywordType.CreateKeyword(keywordValue);
				}

				throw new InvalidCastException(
					$"Keyword value of {keywordValue} cannot be converted "
					+ $"to {keywordType.DataType.ToString()}");
			}

			return keywordType.CreateKeyword(null);
		}

        
    }
}
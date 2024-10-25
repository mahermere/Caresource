// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   KeywordAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Adapters.v6
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using Hyland.Unity;
	using Microsoft.Extensions.Logging;
    using WC.Services.Document.Dotnet8.Adapters.v6.Interfaces;
    using WC.Services.Document.Dotnet8.Models.v6;
    using WC.Services.Document.Dotnet8.Models.v6.Requests;
    using WC.Services.Document.Dotnet8.Repository;
    using Document = Hyland.Unity.Document;


    /// <summary>
    ///    Data and functions describing a CareSource.WC.Services.Document.Adapters.v3.KeywordAdapter
    ///    object.
    /// </summary>
    /// <seealso cref="IKeywordAdapter" />
    public class KeywordAdapter : IKeywordAdapter
	{
        /// <summary>
        ///    The hyland application
        /// </summary>
        private readonly IRepository _repo;

        /// <summary>
        ///    The logger
        /// </summary>
        private readonly log4net.ILog _logger;
		private readonly IConfiguration _configuration;
		/// <summary>
		/// Initializes a new instance of the <see cref="KeywordAdapter"/> class.
		/// </summary>
		/// <param name="applicationConnectionAdapter">The application connection adapter.</param>
		/// <param name="logger">The logger.</param>
		public KeywordAdapter(
            IRepository repo,
            log4net.ILog logger)
		{
			_repo = repo;
			_logger = logger;
		}

		/// <summary>
		///    Updates the keywords.
		/// </summary>
		/// <param name="updates">The updates.</param>
		/// <returns>
		///    A list of all the document Ids that were updated
		/// </returns>
		public (IEnumerable<long> successfulIds, IDictionary<long, IEnumerable<string>> errors)
			UpdateKeywords(BatchUpdateKeywordsRequest updates)
		{
			List<long> successfulIds = new List<long>();
			IDictionary<long, IEnumerable<string>> updateErrors
				= new Dictionary<long, IEnumerable<string>>();

			foreach (KeywordUpdate doc in updates.RequestData)
			{
				List<string> errors = new List<string>();
				try
				{
					Document document =
						_repo.Application.Core.GetDocumentByID(doc.DocumentId);

					if (document == null)
					{
						errors.Add($"No Document found for Id: [{doc.DocumentId}].");
						_logger.Debug($"No Document found for Id: [{doc.DocumentId}].");
					}
					else
					{
						KeywordModifier keywordModifier = document.CreateKeywordModifier();

						foreach (KeyValuePair<string, string> keyword in doc.Keywords)
						{
							if (!KeywordExistsOnDocument(
								document.DocumentType.KeywordRecordTypes,
								keyword.Key))
							{
								errors.Add($"Keyword [{keyword.Key}] is not valid for document.");

								continue;
							}

							KeywordType keywordType =
								_repo.Application.Core.KeywordTypes.FirstOrDefault(
									kwt => kwt.Name.Equals(
										keyword.Key,
										StringComparison.InvariantCulture));

							if (keywordType == null)
							{
								errors.Add($"Keyword is not valid[{keyword.Key}].");

								continue;
							}

							Keyword newKeyword = MapValueToKeyword(
								keywordType,
								keyword.Value);

							_logger.Debug(
								$"Created new '{newKeyword.KeywordType.Name}' on document " +
								$"[{document.ID}] with a value [{keyword.Value}]");

							Keyword currentKeyword = document.KeywordRecords.Find(keywordType)
								?.Keywords.Find(keywordType);

							keywordModifier.UpdateKeyword(
								currentKeyword,
								newKeyword);
						}

						if (!errors.Any())
						{
							UpdateDocument(
								document,
								keywordModifier,
								successfulIds,
								errors);
						}
					}
				}
				catch (InvalidCastException invalidCastException)
				{
					_logger.Error(
						invalidCastException.Message,
						invalidCastException);

					errors.Add(invalidCastException.Message);
				}
				catch (Exception e)
				{
					_logger.Error(e.Message);
					errors.Add(e.Message);
				}

				if (errors.Any())
				{
					if (updateErrors.Any(ue => ue.Key == doc.DocumentId))
					{
						KeyValuePair<long, IEnumerable<string>> item
							= updateErrors.First(ue => ue.Key == doc.DocumentId);

						List<string> docErrors = item.Value.ToList();

						docErrors.AddRange(errors);

						updateErrors.Remove(item);

						KeyValuePair<long, IEnumerable<string>> newItem
							= new KeyValuePair<long, IEnumerable<string>>(
								doc.DocumentId,
								docErrors);

						updateErrors.Add(
							doc.DocumentId,
							docErrors);
					}
					else
					{
						updateErrors.Add(
							doc.DocumentId,
							errors);
					}
				}
			}

			return (successfulIds, updateErrors);
		}

		private bool KeywordExistsOnDocument(
			KeywordRecordTypeList keywordRecordTypes,
			string keywordName)
			=> keywordRecordTypes.Any(
				krt => krt.KeywordTypes.Any(
					kt => kt.Name.Equals(
						keywordName,
						StringComparison.InvariantCulture)));

		/// <summary>
		///    Maps the value to keyword.
		/// </summary>
		/// <param name="keywordType">Type of the keyword.</param>
		/// <param name="keywordValue">The keyword value.</param>
		/// <returns></returns>
		/// <exception cref="InvalidCastException">
		///    Keyword value of {keywordValue} cannot be converted " +
		///    $"to {keywordType.DataType.ToString()}
		/// </exception>
		private Keyword MapValueToKeyword(
			KeywordType keywordType,
			string keywordValue)
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
				$"Keyword value of {keywordValue} cannot be converted " +
				$"to {keywordType.DataType.ToString()}");
		}

		private void UpdateDocument(
			Document document,
			IUnityModifier keywordModifier,
			List<long> successfulIds,
			List<string> errors)
		{
			const int maxAttempts = 3;
			int lockAttempt = 1;
			do
			{
				using (DocumentLock documentLock = document.LockDocument())
				{
					if (documentLock.Status != DocumentLockStatus.LockObtained)
					{
						_logger.Info(
							"Unable to obtain Exclusive lock to update document " +
							$"[{document.ID}]; Attempt #{lockAttempt}");

						Thread.Sleep(
							int.Parse(
								_configuration["OnBase.DocumentLock.Wait"]));
					}
					else
					{
						keywordModifier.ApplyChanges();
						successfulIds.Add(document.ID);
						_logger.Info(
							$"Updated document [{document.ID}] on attempt #{lockAttempt}");

						documentLock.Release();
						break;
					}
				}
			} while (lockAttempt++ < maxAttempts);

			if (lockAttempt > maxAttempts)
			{
				_logger.Info(
					$"Unable to obtain Exclusive lock to update document [{document.ID}]");
				errors.Add("Unable to obtain Exclusive lock to update document.");
			}
		}
    }
}
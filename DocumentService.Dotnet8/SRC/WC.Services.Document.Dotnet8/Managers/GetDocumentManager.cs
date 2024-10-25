// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.Document
//   GetDocumentManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Managers
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Threading.Tasks;
	using CareSource.WC.Entities.Common.Interfaces;
	using CareSource.WC.Entities.Documents;
    using WC.Services.Document.Dotnet8.Adapters.Interfaces;
    using WC.Services.Document.Dotnet8.Managers.Interfaces;
    using WC.Services.Document.Dotnet8.Models;

	/// <summary>
	/// Represents the data used to define a the get document manager
	/// </summary>
	public class GetDocumentManager : IGetDocumentManager<Document>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GetDocumentManager"/> class.
		/// </summary>
		/// <param name="adapter">The adapter.</param>
		public GetDocumentManager(
			IGetDocumentAdapter<OnBaseDocument> adapter)
			=> _adapter = adapter;

		/// <summary>
		/// Adapter Get Document Manager option
		/// </summary>
		private readonly IGetDocumentAdapter<OnBaseDocument> _adapter;

		/// <summary>
		/// Gets the document.
		/// </summary>
		/// <param name="documentId">The document identifier.</param>
		/// <param name="requestData">The request data.</param>
		/// <returns></returns>
		public Document GetDocument(
			long documentId,
			GetDocumentRequest requestData)
		{
			OnBaseDocument document = _adapter.GetDocument(
				documentId,
				requestData);

			string documentData = null;

			if (document.FileData != null)
			{
				// Return as byte array.
				using (MemoryStream streamReader = new MemoryStream())
				{
					document.FileData.CopyTo(streamReader);

					document.FileData.Close();
					document.FileData.Dispose();

					byte[] result = streamReader.ToArray();
					documentData = Convert.ToBase64String(result);
				}
			}

			return new Document
			{
				FileData = documentData,
				Filename = document.Filename,
				Id = document.Id,
				Name = document.Name,
				Type = document.Type,
				DisplayColumns = (Dictionary<string, string>)document.Keywords
			};
		}

		public async Task<(IEnumerable<Document>, int)> SearchAsync(
			ListDocumentsRequest request)
		{
			ISearchResults<OnBaseDocument> onBaseDocuments = await _adapter
				.SearchDocumentsAsync(request)
				.ConfigureAwait(false);

			IEnumerable<Task<Document>> documentTasks = onBaseDocuments.Results
				.Select(async document =>
				{
					string fileData = null;

					if (document.FileData != null)
					{
						using (MemoryStream ms = new MemoryStream())
						{
							document.FileData.CopyTo(ms);

							//You have to rewind the MemoryStream before copying
							ms.Seek(0, SeekOrigin.Begin);

							fileData = Convert.ToBase64String(ms.ToArray(), Base64FormattingOptions.None);

							document.FileData.Close();
							document.FileData.Dispose();
						}
					}

					return new Document
					{
						FileData = fileData,
						Filename = document.Filename,
						Id = document.Id,
						Name = document.Name,
						Type = document.Type,
						DisplayColumns = (Dictionary<string, string>)document.Keywords
					};
				});

			Document[] documents = await Task.WhenAll(documentTasks).ConfigureAwait(false);

			return (documents, onBaseDocuments.TotalRecordCount);
		}

		public Document GetDocument(
			long documentId,
			IEnumerable<string> requestData)
		{
			OnBaseDocument document = _adapter.GetDocument(
				documentId,
				requestData);

			return new Document
			{
				Id = document.Id,
				Name = document.Name,
				Type = document.Type,
				DisplayColumns = (Dictionary<string, string>)document.Keywords
			};
		}
	}
}
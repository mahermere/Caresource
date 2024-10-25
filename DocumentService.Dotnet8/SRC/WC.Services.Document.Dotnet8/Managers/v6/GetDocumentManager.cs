// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   GetDocumentManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Managers.v6
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Threading.Tasks;
    using WC.Services.Document.Dotnet8.Adapters.v6.Interfaces;
    using WC.Services.Document.Dotnet8.Managers.v6.Interfaces;
    using WC.Services.Document.Dotnet8.Models.v6;
    using WC.Services.Document.Dotnet8.Models.v6.Interfaces;
    using WC.Services.Document.Dotnet8.Models.v6.Requests;

    /// <summary>
    ///    Represents the data used to define a the get document manager
    /// </summary>
    public class GetDocumentManager : IGetDocumentManager<Document>
	{
		/// <summary>
		///    Adapter Get Document Manager option
		/// </summary>
		private readonly IGetDocumentAdapter<OnBaseDocument> _adapter;

		/// <summary>
		///    Initializes a new instance of the <see cref="GetDocumentManager" /> class.
		/// </summary>
		/// <param name="adapter">The adapter.</param>
		public GetDocumentManager(
			IGetDocumentAdapter<OnBaseDocument> adapter)
			=> _adapter = adapter;

		/// <summary>
		///    Gets the document.
		/// </summary>
		/// <param name="documentId">The document identifier.</param>
		/// <param name="requestData">The request data.</param>
		/// <returns></returns>
		public Document GetDocument(
			long documentId,
			DownloadRequest requestData)
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
				DocumentDate = document.DocumentDate,
				Name = document.Name,
				Type = document.Type,
				DisplayColumns = (Dictionary<string, string>)document.Keywords
			};
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
				DocumentDate = document.DocumentDate,
				DisplayColumns = (Dictionary<string, string>)document.Keywords
			};
		}

		public Task<(IEnumerable<Document>, int)> SearchAsync(ISearchRequest request)
			=> throw new NotImplementedException();
	}
}
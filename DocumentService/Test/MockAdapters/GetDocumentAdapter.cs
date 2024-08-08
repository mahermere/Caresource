// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.Document.Tests
//   GetDocumentAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Tests.MockAdapters
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Threading.Tasks;
	using CareSource.WC.Entities.Common.Interfaces;
	using CareSource.WC.Entities.Documents;
	using CareSource.WC.Entities.Documents.Interfaces;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.Services.Document.Adapters;
	using CareSource.WC.Services.Document.Models;

	internal class GetDocumentAdapter : IGetDocumentAdapter<OnBaseDocument>
	{

		private readonly IEnumerable<OnBaseDocument> _documents = new []
		{
			new OnBaseDocument
			{
				Id = 123456,
				Filename = "Testing Document 123456.txt",
				Name = "Testing Document 123456",
				Type = "Mock Document"
			},
			new OnBaseDocument{
				Id = 123457,
				Filename = "JTesting Document 123457.txt",
				Name = "Testing Document 123457",
				Type = "Mock Document"
			}
		};
		/// <summary>
		/// Gets the document.
		/// </summary>
		/// <param name="documentId">The document identifier.</param>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		/// <exception cref="NullReferenceException">
		/// The {request.GetType().Name}
		/// or
		/// No document found for id:{documentId}
		/// </exception>
		public OnBaseDocument GetDocument(
			long documentId,
			GetDocumentRequest request)
		{
			if (request.UserId.IsNullOrWhiteSpace())
			{
				throw new NullReferenceException($"The {request.GetType().Name}.UserId is Required");
			}

			OnBaseDocument document = _documents.FirstOrDefault(d => d.Id == documentId);

			if (document == null)
			{
				throw new NullReferenceException($"No document found for id:{documentId}.");
			}

			document.FileData = GetMemoryStream();

			return document;
		}

		public OnBaseDocument GetDocument(long documentId, IEnumerable<string> keywords)
		{
			throw new NotImplementedException();
		}

		public Task<ISearchResults<OnBaseDocument>> SearchDocumentsAsync(
			IListDocumentsRequest request) => throw new NotImplementedException();

		private Stream GetMemoryStream()
		{
			MemoryStream stream = new MemoryStream(100);
			StreamWriter writer = new StreamWriter(stream);

			string bytes = "This is test stream data";
			writer.Write(bytes);
			writer.Flush();
			stream.Position = 0;

			return stream;
		}
	}
}
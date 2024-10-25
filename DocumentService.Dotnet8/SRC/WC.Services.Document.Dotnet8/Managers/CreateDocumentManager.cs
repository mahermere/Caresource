namespace WC.Services.Document.Dotnet8.Managers
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using CareSource.WC.Entities.Documents;
	using Microsoft.Extensions.Logging;
	using WC.Services.Document.Dotnet8.Adapters;
    using WC.Services.Document.Dotnet8.Adapters.Interfaces;
    using WC.Services.Document.Dotnet8.Managers.Interfaces;
    using WC.Services.Document.Dotnet8.Models;

	public class CreateDocumentManager : ICreateDocumentManager<OnBaseDocument>
	{
		private readonly ICreateDocumentAdapter _createDocumentAdapter;
		private readonly IGetDocumentAdapter<OnBaseDocument> _getDocumentAdapter;
		private readonly IConfiguration _configuration;
		private readonly IFileAdapter _fileAdapter;
		private readonly ILogger _logger;

		public CreateDocumentManager(ICreateDocumentAdapter createDocumentAdapter,
			IGetDocumentAdapter<OnBaseDocument> getDocumentAdapter,
			IConfiguration configuration,
			IFileAdapter fileAdapter,
			ILogger logger)
		{
			_configuration = configuration;
			_createDocumentAdapter = createDocumentAdapter;
			_getDocumentAdapter = getDocumentAdapter;
			_fileAdapter = fileAdapter;
			_logger = logger;
		}

		public OnBaseDocument CreateDocument(CreateDocumentFileLinkRequest request)
		{
            string rootFilePath = _configuration["FileServerSettings:OnBase.FileServer.Root"];
            string importFilePath = _configuration["FileServerSettings:Import.FileDrop"];
            var filePath = $"{rootFilePath}\\{importFilePath}\\{request.RequestData.FileName}";

			if (!_fileAdapter.FileExists(filePath))
			{
				throw new FileNotFoundException($"Unable to find file '{request.RequestData.FileName}'.");
			}

			long? documentId = null;

			using (var fileStream = _fileAdapter
				.ReadFileContents(filePath))
			{
				documentId = _createDocumentAdapter.CreateDocument(
					request.RequestData.DocumentType
					, request.RequestData.Keywords
					, request.RequestData.FileName
					, new List<Stream> { fileStream });
			}

			if (documentId.HasValue)
			{
				var document = _getDocumentAdapter.GetDocument(
					documentId.Value,
					new GetDocumentRequest
					{
						DisplayColumns = request.RequestData.Keywords
							.Select(kvp => kvp.Key)
							.ToList(),
						CorrelationGuid = request.CorrelationGuid,
						SourceApplication = request.SourceApplication,
						UserId = request.UserId
					});

				if (document != null)
				{
					document.FileData = null;
					document.Filename = null;
					_fileAdapter.DeleteFile(filePath);
				}

				return document;
			}

			return null;
		}
	}
}
namespace CareSource.WC.Services.Document.Managers.v3
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using CareSource.WC.Entities.Documents;
	using CareSource.WC.OnBase.Core.Configuration.Interfaces;
	using CareSource.WC.Services.Document.Adapters.v3;
	using CareSource.WC.Services.Document.Models.v3;
	using Microsoft.Extensions.Logging;

	public class CreateDocumentManager : ICreateDocumentManager<OnBaseDocument>
	{
		private readonly ICreateDocumentAdapter _createDocumentAdapter;
		private readonly IGetDocumentAdapter<OnBaseDocument> _getDocumentAdapter;
		private readonly ISettingsAdapter _settingsAdapter;
		private readonly IFileAdapter _fileAdapter;
		private readonly ILogger _logger;

		public CreateDocumentManager(ICreateDocumentAdapter createDocumentAdapter,
			IGetDocumentAdapter<OnBaseDocument> getDocumentAdapter,
			ISettingsAdapter settingsAdapter,
			IFileAdapter fileAdapter,
			ILogger logger)
		{
			_settingsAdapter = settingsAdapter;
			_createDocumentAdapter = createDocumentAdapter;
			_getDocumentAdapter = getDocumentAdapter;
			_fileAdapter = fileAdapter;
			_logger = logger;
		}

		public OnBaseDocument CreateDocument(CreateDocumentFileLinkRequest request)
		{
			var rootFilePath = _settingsAdapter.GetSetting("OnBase.FileServer.Root");
			var importFilePath = _settingsAdapter.GetSetting("Import.FileDrop");
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
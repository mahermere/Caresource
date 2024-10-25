namespace WC.Services.Document.Dotnet8.Adapters.Interfaces
{
	using System.Collections.Generic;
	using System.IO;

	public interface ICreateDocumentAdapter
	{
		long? CreateDocument(string documentType, IDictionary<string, string> keywords, string fileName, List<Stream> documentPages);
	}
}
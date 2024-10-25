namespace WC.Services.Document.Dotnet8.Models.v6
{
	using CareSource.WC.Entities.Exceptions;

	public class DocumentNotFoundException : OnBaseExceptionBase
	{
		public DocumentNotFoundException(long documentId)
		: base(ErrorCode.NoDocuments, $"No document found for id:{documentId}.")
		{ }

	}
}
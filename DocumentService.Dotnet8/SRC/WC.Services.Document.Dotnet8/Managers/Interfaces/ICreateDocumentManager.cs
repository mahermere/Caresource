namespace WC.Services.Document.Dotnet8.Managers.Interfaces
{
	using WC.Services.Document.Dotnet8.Models;

	public interface ICreateDocumentManager<TDocumentDataModel>
	{
		TDocumentDataModel CreateDocument(CreateDocumentFileLinkRequest request);
	}
}

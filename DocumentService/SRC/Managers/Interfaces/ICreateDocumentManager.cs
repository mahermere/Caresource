namespace CareSource.WC.Services.Document.Managers
{
	using CareSource.WC.Services.Document.Models;

	public interface ICreateDocumentManager<TDocumentDataModel>
	{
		TDocumentDataModel CreateDocument(CreateDocumentFileLinkRequest request);
	}
}

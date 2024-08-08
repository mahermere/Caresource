namespace CareSource.WC.Services.Document.Adapters
{
	public interface ICreateKeywordAdapter<TKeywordDataModel>
	{
		TKeywordDataModel CreateKeyword(string value, string keywordTypeName
			, string defaultKeywordDocumentTypeName, bool blankKeyword);
	}
}
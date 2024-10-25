namespace WC.Services.Document.Dotnet8.Adapters.Interfaces
{
	public interface ICreateKeywordAdapter<TKeywordDataModel>
	{
		TKeywordDataModel CreateKeyword(string value, string keywordTypeName
			, string defaultKeywordDocumentTypeName, bool blankKeyword);
	}
}
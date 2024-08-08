namespace ImportProcessor.Adapters.v1.Interfaces
{
	public interface IKeywordAdapter<TKeywordDataModel>
	{
		TKeywordDataModel CreateKeyword(string value, string keywordTypeName
			, string defaultKeywordDocumentTypeName, bool blankKeyword);
	}
}
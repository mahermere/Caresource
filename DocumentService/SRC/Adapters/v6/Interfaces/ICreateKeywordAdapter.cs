// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   ICreateKeywordAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Adapters.v6
{
	using Hyland.Unity;

	public interface ICreateKeywordAdapter<out TKeywordDataModel>
	{
		TKeywordDataModel CreateKeyword(
			string value,
			string keywordTypeName,
			string defaultKeywordDocumentTypeName,
			bool blankKeyword);

		TKeywordDataModel CreateKeyword(
			string value,
			string keywordTypeName,
			DocumentType documentType,
			bool blankKeyword);
	}
}
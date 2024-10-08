﻿// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   WC.Services.Document
//   IOnBaseAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Adapters.v6
{
	using System.Collections.Generic;
	using CareSource.WC.Services.Document.Models.v6;
	using Hyland.Unity;
	using Hyland.Unity.UnityForm;
	using Hyland.Unity.WorkView;

	public interface IOnBaseAdapter
	{
		/// <summary>
		/// Gets the current user.
		/// </summary>
		User CurrentUser { get; }

		/// <summary>
		/// Gets the document.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		OnBaseDocument GetDocument(IDownloadRequest request);

		/// <summary>
		/// Gets the document types.
		/// </summary>
		/// <param name="requestDocumentTypes">The request document types.</param>
		/// <returns></returns>
		IEnumerable<DocumentType> GetDocumentTypes(IEnumerable<string> requestDocumentTypes);

		DocumentType GetDocumentType(string docTypeName);

		KeywordType GetKeywordByName(string keywordName);

		IEnumerable<KeywordType> GetKeywordTypes(IEnumerable<string> keywordTypeNames);
		
		/// <summary>
		/// Gets the unity form template.
		/// </summary>
		/// <param name="templateName">The template name.</param>
		/// <returns></returns>
		(FormTemplate, StoreNewUnityFormProperties) GetUnityFormTemplate(string templateName);

		/// <summary>
		/// Creates a new unity form and populates its values.
		/// </summary>
		/// <param name="props">The props.</param>
		/// <returns></returns>
		long? CreateUnityForm(StoreNewUnityFormProperties props);

		IEnumerable<Class> GetWorkViewClasses(string applicationName);
		string GetKeywordValueDbColumnName(KeywordType kt);

		int GetKeyTypeTableDataType(KeywordType kt);
	}
}
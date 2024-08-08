// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   IDocumentAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.OnBase.Adapters.Interfaces.v1
{
	using System.Collections.Generic;
	using CareSource.WC.Services.OnBase.Models.v1;

	public interface IDocumentAdapter
	{
		/// <summary>
		/// Gets the document type group.
		/// </summary>
		/// <param name="documentTypeGroupId">The document type group identifier.</param>
		/// <returns></returns>
		DocumentTypeGroup GetDocumentTypeGroup(long documentTypeGroupId);

		/// <summary>
		///    Returns all document Types.
		/// </summary>
		/// <returns></returns>
		IEnumerable<DocumentType> DocumentTypes();

		/// <summary>
		/// Searches the by keyword.
		/// </summary>
		/// <param name="keywordName">Name of the keyword.</param>
		/// <returns></returns>
		IEnumerable<DocumentType> SearchByKeyword(string keywordName);

		/// <summary>
		///    Returns all document Types for the specified Group Id.
		/// </summary>
		/// <returns></returns>
		IEnumerable<DocumentType> DocumentTypes(long documentTypeGroupId);

		/// <summary>
		///    Returns all Document Type Groups.
		/// </summary>
		/// <returns></returns>
		IEnumerable<DocumentTypeGroup> DocumentTypeGroups();

		/// <summary>
		///    Returns all the Keywords
		/// </summary>
		/// <returns></returns>
		IEnumerable<Keyword> Keywords();

		/// <summary>
		///    Keywords for the specified document type identifier.
		/// </summary>
		/// <param name="documentTypeId">The document type identifier.</param>
		/// <returns></returns>
		IEnumerable<Keyword> Keywords(long documentTypeId);

		/// <summary>
		///    Keywords the type groups.
		/// </summary>
		/// <returns></returns>
		IEnumerable<KeywordTypeGroup> KeywordTypeGroups();
	}
}
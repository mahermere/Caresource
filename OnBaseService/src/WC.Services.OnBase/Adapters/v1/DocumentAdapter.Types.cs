// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   DocumentAdapter.Types.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.OnBase.Adapters.v1
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Hyland.Unity;
	using DocumentType = CareSource.WC.Services.OnBase.Models.v1.DocumentType;

	/// <summary>
	///    Data and functions describing a
	///    CareSource.WC.Services.OnBase.Adapters.v1.DocumentTypeGroupAdapter object.
	/// </summary>
	public partial class DocumentAdapter
	{
		/// <summary>
		///    Gets the document type list.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<DocumentType> DocumentTypes()
			=> _applicationCore
				.Application
				.Core
				.DocumentTypes
				.Select(
					documentType => new DocumentType
					{
						Id = documentType.ID,
						Name = documentType.Name
					});

		public IEnumerable<DocumentType> DocumentTypes(long documentTypeGroupId)
			=> _applicationCore.Application.Core.DocumentTypeGroups
				.FirstOrDefault(d => d.ID.Equals(documentTypeGroupId))
				?.DocumentTypes
				.Select(
					documentType => new DocumentType
					{
						Id = documentType.ID,
						Name = documentType.Name
					});

		public DocumentType DocumentType(long documentTypeId)
		{
			Hyland.Unity.DocumentType documentType = _applicationCore.Application.Core.DocumentTypes
				.FirstOrDefault(d => d.ID.Equals(documentTypeId));

			if (documentType == null)
			{
				return null;
			}

			return new DocumentType
			{
				Id = documentType.ID,
				Name = documentType.Name,
				Keywords = Keywords(documentTypeId)
			};
		}


		public IEnumerable<DocumentType> SearchByKeyword(string keywordName)
		{
			IEnumerable<Hyland.Unity.DocumentType> documentTypes = _applicationCore.Application.Core.DocumentTypes
				.FindAll(d => d.KeywordRecordTypes
					.Any(k => k.KeywordTypes.Any(kw => kw.Name == keywordName)));

			if (documentTypes == null || !DocumentTypes().Any())
			{
				return null;
			}

			return documentTypes.Select(
				d =>
					new DocumentType
					{
						Id = d.ID,
						Name = d.Name
					});
		}
	}
}
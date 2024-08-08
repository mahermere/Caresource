// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   DocumentAdapter.TypeGroups.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.OnBase.Adapters.v1
{
	using System.Collections.Generic;
	using System.Linq;
	using CareSource.WC.Services.OnBase.Models.v1;

	/// <summary>
	///    Data and functions describing a
	///    CareSource.WC.Services.OnBase.Adapters.v1.DocumentTypeGroupAdapter object.
	/// </summary>
	public partial class DocumentAdapter
	{
		public DocumentTypeGroup GetDocumentTypeGroup(long documentGroupTypeId)
		{
			Hyland.Unity.DocumentTypeGroup documentType =
				_applicationCore.Application.Core.DocumentTypeGroups
					.FirstOrDefault(d => d.ID.Equals(documentGroupTypeId));

			if (documentType == null)
			{
				return null;
			}

			return new DocumentTypeGroup
			{
				Id = documentType.ID,
				Name = documentType.Name,
				DocumentTypes = DocumentTypes(documentGroupTypeId)
			};
		}

		/// <summary>
		///    Gets the document type group list.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<DocumentTypeGroup> DocumentTypeGroups()
			=> _applicationCore.Application.Core.DocumentTypeGroups
				.Select(
					documentType
						=> new DocumentTypeGroup
						{
							Id = documentType.ID,
							Name = documentType.Name
						});
	}
}
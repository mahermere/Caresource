// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.Document
//   GetDocumentManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Managers.v4
{
	using System.Linq;
	using CareSource.WC.Entities.Documents;
	using CareSource.WC.Entities.Exceptions;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.Services.Document.Adapters.v4;
	using CareSource.WC.Services.Document.Models.v4;


	/// <summary>
	/// Represents the data used to define a the document manager
	/// </summary>
	public class DocumentManager : BaseDocumentManager<DocumentHeader>,
											IDocumentManager<DocumentHeader>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DocumentManager"/> class.
		/// </summary>
		/// <param name="adapter">The adapter.</param>
		public DocumentManager(
			ISearchDocumentAdapter<DocumentHeader> adapter)
			: base(adapter) { }

		/// <summary>
		/// Validates the request.
		/// </summary>
		/// <exception cref="InvalidRequestException"></exception>
		protected override void ValidateRequest()
		{
			base.ValidateRequest();

			if (!RequestData.DocumentTypes.Any()
				&& !RequestData.Filters.Any()
				&& RequestData.StartDate.IsNullOrWhiteSpace())
			{
				throw new InvalidRequestException();
			}
		}
		/// <summary>
		/// Sets the display columns.
		/// </summary>
		protected override void SetDisplayColumns()
		{ }

		/// <summary>
		/// Sets the filters.
		/// </summary>
		protected override void SetFilters()
		{ }

		public long GetTotalRecords(TotalDocumentCountRequest request)
			=> ((Adapters.v4.OnBaseDocumentAdapter)DocumentAdapter).GetTotalRecords(request);
	}
}
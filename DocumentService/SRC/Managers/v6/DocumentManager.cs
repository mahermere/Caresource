// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   DocumentManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Managers.v6
{
	using CareSource.WC.Entities.Documents;
	using CareSource.WC.Services.Document.Adapters.v6;
	using Microsoft.Extensions.Logging;
	using DocumentHeader = CareSource.WC.Services.Document.Models.v6.DocumentHeader;

	/// <summary>
	///    Represents the data used to define a the document manager
	/// </summary>
	public class DocumentManager : BaseDocumentManager, IDocumentManager
	{
		public DocumentManager(
			IOnBaseSqlAdapter<DocumentHeader> documentAdapter,
			IOnBaseAdapter onBaseAdapter,
			ILogger logger)
			: base(
				documentAdapter,
				onBaseAdapter,
				logger)
		{ }

	}
}
// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   DocumentManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Managers.v5
{
	using CareSource.WC.Entities.Documents;
	using Microsoft.Extensions.Logging;
	using CareSource.WC.Services.Document.Adapters.v5;

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
// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   DocumentManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Document.Dotnet8.Managers.v6
{
	using CareSource.WC.Entities.Documents;
	using WC.Services.Document.Dotnet8.Adapters.v6;
	using Microsoft.Extensions.Logging;
	using DocumentHeader = WC.Services.Document.Dotnet8.Models.v6.DocumentHeader;
    using WC.Services.Document.Dotnet8.Managers.v6;
    using WC.Services.Document.Dotnet8.Adapters.v6.Interfaces;
    using WC.Services.Document.Dotnet8.Managers.v6.Interfaces;

    /// <summary>
    ///    Represents the data used to define a the document manager
    /// </summary>
    public class DocumentManager : BaseDocumentManager, IDocumentManager
	{
		public DocumentManager(
			IOnBaseSqlAdapter<DocumentHeader> documentAdapter,
			IOnBaseAdapter onBaseAdapter,
			log4net.ILog logger)
			: base(
				documentAdapter,
				onBaseAdapter,
				logger)
		{ }

	}
}
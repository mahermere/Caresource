// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   DocumentAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.OnBase.Dotnet8.Adapters.v1
{
	using Hyland.Unity;
    using System.Collections.Generic;
    using WC.Services.OnBase.Dotnet8.Adapters.Interfaces.v1;
    using WC.Services.OnBase.Dotnet8.Connection.Interfaces;
    using WC.Services.OnBase.Dotnet8.Models.v1;
    using WC.Services.OnBase.Dotnet8.Repository;

    /// <summary>
    ///    Data and functions describing a
    ///    CareSource.WC.Services.OnBase.Adapters.v1.DocumentTypeGroupAdapter object.
    /// </summary>
    public partial class DocumentAdapter : IDocumentAdapter
	{
		private readonly IRepository _repo;

		/// <summary>
		///    Initializes a new instance of the <see cref="DocumentAdapter" /> class.
		/// </summary>
		/// <param name="repository">The application connection adapter.</param>
		public DocumentAdapter(IRepository repo)
			=> _repo = repo;

    }
}
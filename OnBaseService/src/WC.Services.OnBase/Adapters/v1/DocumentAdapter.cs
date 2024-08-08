// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   DocumentAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.OnBase.Adapters.v1
{
	using CareSource.WC.OnBase.Core.Connection.Interfaces;
	using CareSource.WC.Services.OnBase.Adapters.Interfaces.v1;
	using Hyland.Unity;

	/// <summary>
	///    Data and functions describing a
	///    CareSource.WC.Services.OnBase.Adapters.v1.DocumentTypeGroupAdapter object.
	/// </summary>
	public partial class DocumentAdapter : IDocumentAdapter
	{
		private readonly IApplicationConnectionAdapter<Application> _applicationCore;

		/// <summary>
		///    Initializes a new instance of the <see cref="DocumentAdapter" /> class.
		/// </summary>
		/// <param name="applicationConnectionAdapter">The application connection adapter.</param>
		public DocumentAdapter(
			IApplicationConnectionAdapter<Application> applicationConnectionAdapter)
			=> _applicationCore = applicationConnectionAdapter;
	}
}
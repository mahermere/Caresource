// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   DocumentAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------using System;

namespace CareSource.WC.Services.OnBase.Adapters.v1
{
	using CareSource.WC.OnBase.Core.Connection.Interfaces;
	using CareSource.WC.Services.OnBase.Adapters.Interfaces.v1;
	using Hyland.Unity;

	/// <summary>
	///    Data and functions describing a
	///    CareSource.WC.Services.OnBase.Adapters.v1.WorkViewAdapter object.
	/// </summary>
	public partial class WorkViewAdapter : IWorkViewAdapter
	{
		private readonly IApplicationConnectionAdapter<Application> _applicationConnectionAdapter;

		/// <summary>
		///    Initializes a new instance of the <see cref="WorkViewAdapter" /> class.
		/// </summary>
		/// <param name="applicationConnectionAdapter">The application connection adapter.</param>

		public WorkViewAdapter(
			IApplicationConnectionAdapter<Application> applicationConnectionAdapter)
			=> _applicationConnectionAdapter = applicationConnectionAdapter;
		}
}
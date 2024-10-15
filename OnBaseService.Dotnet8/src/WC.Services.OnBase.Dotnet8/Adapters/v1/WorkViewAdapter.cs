// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   DocumentAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------using System;

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
    ///    CareSource.WC.Services.OnBase.Adapters.v1.WorkViewAdapter object.
    /// </summary>
    public partial class WorkViewAdapter : IWorkViewAdapter
	{
		private readonly IRepository _repo;

		/// <summary>
		///    Initializes a new instance of the <see cref="WorkViewAdapter" /> class.
		/// </summary>
		/// <param name="repo">The application connection adapter.</param>

		public WorkViewAdapter(IRepository repo)
			=> _repo = repo;

       
    }
}
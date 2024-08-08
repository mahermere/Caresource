// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Hplc
//   Adapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Adapters.v2.WorkView
{
	using System;
	using System.Configuration;
	using System.Linq;
	using CareSource.WC.OnBase.Core.Diagnostics.Interfaces;
	using Hyland.Unity.WorkView;
	using WC.Services.Hplc.Mappers.v2;
	using WC.Services.Hplc.Models.v2;
	using OnBaseConnectionAdapter = WC.Services.Hplc.OnBaseConnectionAdapter;

	/// <summary>
	///    Functions defining a CareSource.WC.Services.Hplc.Adapters.v1.WorkViewAdapter object.
	/// </summary>
	/// <seealso cref="IAdapter" />
	public partial class Adapter
		: IAdapter
	{
		private readonly int _timeout;
		private readonly ILogger _logger;
		private readonly string _applicationName;
		private readonly Application _workViewApplication;
		private readonly IModelMapper<WorkViewObject, Request> _requestMapper;
		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
		// this needs to be here so we do not randomly GC after the initial get
		private readonly OnBaseConnectionAdapter _application;
		private readonly IHieModelMapper<WorkViewObject, Request> _hieRequestMapper;

		/// <summary>
		/// Initializes a new instance of the <see cref="Adapter" /> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="application">The application.</param>
		/// <param name="requestMapper">The request mapper.</param>
		public Adapter(
			ILogger logger,
			OnBaseConnectionAdapter application,
			IModelMapper<WorkViewObject, Models.v2.Request> requestMapper,
			IHieModelMapper<WorkViewObject, Models.v2.Request> hieRequestMapper)
		{
			_application = application;
			
			_logger = logger;

			_timeout = Convert.ToInt32(
				ConfigurationManager.AppSettings.Get("OnBase.Connection.Timeout"));

			_applicationName = ConfigurationManager
				.AppSettings.Get("Services.WorkView.ApplicationName");

			_workViewApplication = _application.Application.WorkView.Applications.FirstOrDefault(
				a => a.Name.Equals(
					_applicationName,
					StringComparison.CurrentCultureIgnoreCase));

			_requestMapper = requestMapper;
			_hieRequestMapper = hieRequestMapper;
		}
	}
}
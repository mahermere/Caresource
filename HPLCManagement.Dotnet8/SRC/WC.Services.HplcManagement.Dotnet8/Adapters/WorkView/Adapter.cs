// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Hplc
//   Adapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

using System;
using System.Configuration;
using System.Linq;
using Hyland.Unity.WorkView;
using Object = Hyland.Unity.WorkView.Object;
//using OnBaseConnectionAdapter = WC.Services.HplcManagement.Dotnet8.OnBaseConnectionAdapter;
using WC.Services.HplcManagement.Dotnet8.Mappers.Interfaces;
using WC.Services.HplcManagement.Dotnet8.Models;
using WC.Services.HplcManagement.Dotnet8.Authorization;
using WC.Services.HplcManagement.Dotnet8.Repository;

namespace WC.Services.HplcManagement.Dotnet8.Adapters.WorkView
{
    /// <summary>
    ///    Functions defining a CareSource.WC.Services.Hplc.Adapters.v1.WorkViewAdapter object.
    /// </summary>
    /// <seealso cref="IAdapter" />
    public partial class Adapter
		: IAdapter
	{
		private readonly int _timeout;
		private readonly log4net.ILog _logger;
		private readonly string _applicationName;
		private readonly Application _workViewApplication;
		private readonly IModelMapper<Data, Object> _mapper;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        // this needs to be here so we do not randomly GC after the initial get
        private readonly OnBaseApplicationAbstractFactory _applicationFactory;
        private static IConfiguration _configuration;
        private readonly IRepository _repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="Adapter" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="application">The application.</param>
        /// <param name="Mapper">The model mapper.</param>
        public Adapter(
			log4net.ILog logger,
            IRepository repo,
			IModelMapper<Data, Object> mapper,
			IConfiguration configuration)
		{
			_configuration = configuration;
            _repo = repo;
            _logger = logger;
			_timeout = Convert.ToInt32(_configuration["OnBaseSettings:OnBase.Connection.Timeout"]);

			_applicationName = _configuration["WorkViewSettings:ApplicationName"];

            _workViewApplication = _repo.Application.WorkView.Applications.FirstOrDefault(
                a => a.Name.Equals(
                    _applicationName,
                    StringComparison.CurrentCultureIgnoreCase));

            _mapper = mapper;
		}
	}
}
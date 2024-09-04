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
using WC.Services.Hplc.Dotnet8.Adapters.WorkView.Interfaces;
using WC.Services.Hplc.Dotnet8.Authorization;
using WC.Services.Hplc.Dotnet8.Mappers.Interfaces;
using WC.Services.Hplc.Dotnet8.Models;
using WC.Services.Hplc.Dotnet8.Repository;
using OnBaseConnectionAdapter = WC.Services.Hplc.Dotnet8.OnBaseConnectionAdapter;

namespace WC.Services.Hplc.Dotnet8.Adapters.WorkView
{
    /// <summary>
    ///    Functions defining a CareSource.WC.Services.Hplc.Adapters.v1.WorkViewAdapter object.
    /// </summary>
    /// <seealso cref="IAdapter" />
    public partial class Adapter : IAdapter
    {
        private readonly int _timeout;
        private readonly log4net.ILog _logger;
        private readonly string _applicationName;
        private readonly Application _workViewApplication;
        private readonly IConfiguration _configuration;
        private readonly IModelMapper<WorkViewObject, Request> _requestMapper;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        // this needs to be here so we do not randomly GC after the initial get
        private readonly OnBaseApplicationAbstractFactory _applicationFactory;
        private readonly IHieModelMapper<WorkViewObject, Request> _hieRequestMapper;
        private readonly IRepository _repo;
        /// <summary>
        /// Initializes a new instance of the <see cref="Adapter" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="application">The application.</param>
        /// <param name="requestMapper">The request mapper.</param>
        public Adapter(
            log4net.ILog logger,
            IRepository repo,
            IModelMapper<WorkViewObject, Request> requestMapper,
            IHieModelMapper<WorkViewObject, Request> hieRequestMapper,
            IConfiguration configuration)
        {
            _repo = repo;
            _configuration = configuration;
            _logger = logger;
            _timeout = Convert.ToInt32(_configuration["OnBaseSettings:OnBase.Connection.Timeout"]);
            _applicationName = _configuration["WorkViewSettings:ApplicationName"];
            _workViewApplication = _repo.Application.WorkView.Applications.FirstOrDefault(
                a => a.Name.Equals(
                    _applicationName,
                    StringComparison.CurrentCultureIgnoreCase));

            _requestMapper = requestMapper;
            _hieRequestMapper = hieRequestMapper;
        }

        

        WorkViewObject IAdapter.GetObject(long objectId)
        {
            throw new NotImplementedException();
        }

        IEnumerable<WorkViewObject> IAdapter.HieSearch(string className, string filterName, string[] attributes, IDictionary<string, string> filters)
        {
            throw new NotImplementedException();
        }

        IEnumerable<WorkViewObject> IAdapter.Search(string className, string filterName, string[] attributes, IDictionary<string, string> filters)
        {
            throw new NotImplementedException();
        }
    }
}
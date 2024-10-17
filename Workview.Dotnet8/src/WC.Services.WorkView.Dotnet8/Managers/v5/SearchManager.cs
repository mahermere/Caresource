// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   Workview
//   RetrieveManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.WorkView.Dotnet8.Managers.v5
{
	using System.Collections.Generic;
	using WC.Services.WorkView.Dotnet8.Adapters.v5;
	using WC.Services.WorkView.Dotnet8.Mappers.v5;
	using WC.Services.WorkView.Dotnet8.Models.v5;
	using Microsoft.Extensions.Logging;
    using WC.Services.WorkView.Dotnet8.Mappers.v5.Interfaces;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    //using CareSource.WC.OnBase.Core.ExtensionMethods;

    public class SearchManager : ISearchManager
	{
		private readonly IModelMapper<WorkViewObject, WorkViewBaseObject> _modelMapper;
		private readonly ISearchAdapter _searchAdapter;
		private readonly log4net.ILog _logger;

		public SearchManager(
			IModelMapper<WorkViewObject, WorkViewBaseObject> modelMapper,
			ISearchAdapter searchAdapter,
			log4net.ILog logger)
		{
			_modelMapper = modelMapper;
			_searchAdapter = searchAdapter;
			_logger = logger;
		}

		public bool ValidateRequest(
			string workviewApplicationName,
			SearchRequest request,
			ModelStateDictionary modelState)
		{
			(bool isValid, Dictionary<string, string[]> errors) validate =
				_searchAdapter.ValidateRequest(
					workviewApplicationName,
					request);

			foreach (KeyValuePair<string, string[]> error in validate.errors)
			{
				modelState.AddModelError(
					error.Key,
					string.Join(
						", ",
						error.Value));
			}

			return validate.isValid;
		}

		public IEnumerable<WorkViewObject> Search(
			string workViewApplicationName,
			SearchRequest request)
		{
			_logger.Info(
				$"{nameof(SearchManager)}.{nameof(Search)} call, " +
				$"to get{workViewApplicationName}.{request.ClassName}.");

			return _searchAdapter.Search(
				workViewApplicationName,
				request);
		}
	}
}
// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   Workview
//   RetrieveManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Managers.v5
{
	using System.Collections.Generic;
	using System.Web.Http.ModelBinding;
	using CareSource.WC.Services.WorkView.Adapters.v5;
	using CareSource.WC.Services.WorkView.Mappers.v5;
	using CareSource.WC.Services.WorkView.Models.v5;
	using Microsoft.Extensions.Logging;
	using CareSource.WC.OnBase.Core.ExtensionMethods;

	public class SearchManager : ISearchManager
	{
		private readonly IModelMapper<WorkViewObject, WorkViewBaseObject> _modelMapper;
		private readonly ISearchAdapter _searchAdapter;
		private readonly ILogger _logger;

		public SearchManager(
			IModelMapper<WorkViewObject, WorkViewBaseObject> modelMapper,
			ISearchAdapter searchAdapter,
			ILogger logger)
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
			_logger.LogInformation(
				$"{nameof(SearchManager)}.{nameof(Search)} call, " +
				$"to get{workViewApplicationName}.{request.ClassName}.");

			return _searchAdapter.Search(
				workViewApplicationName,
				request);
		}
	}
}
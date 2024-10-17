// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    Workview
//    SearchAdapter.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace WC.Services.WorkView.Dotnet8.Adapters.v5
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	//using CareSource.WC.OnBase.Core.Connection.Interfaces;
	using WC.Services.WorkView.Dotnet8.Mappers.v5;
	using WC.Services.WorkView.Dotnet8.Models.v5;
	using Hyland.Unity.WorkView;
	using Application = Hyland.Unity.Application;
	using Attribute = Hyland.Unity.WorkView.Attribute;
	using Filter = WC.Services.WorkView.Dotnet8.Models.v5.Filter;
	using Microsoft.Extensions.Logging;
    using WC.Services.WorkView.Dotnet8.Mappers.v5.Interfaces;
    using WC.Services.WorkView.Dotnet8.Repository;

    //using CareSource.WC.OnBase.Core.ExtensionMethods;

    public class SearchAdapter : BaseAdapter, ISearchAdapter
	{
		/// <summary>
		///    Initializes a new instance of the <see cref="SearchAdapter" /> class.
		/// </summary>
		/// <param name="modelMapper">
		///    The model mapper.
		/// </param>
		/// <param name="applicationConnectionAdapter">
		///    The application connection adapter.
		/// </param>
		/// <param name="logger"></param>
		public SearchAdapter(
			IModelMapper<WorkViewObject, WorkViewBaseObject> modelMapper,
			IRepository repo,
			log4net.ILog logger)
			:
			base(
				modelMapper,
				repo,
				null,
				logger) { }

		public IEnumerable<WorkViewObject> Search(
			string workViewApplicationName,
			SearchRequest request)
		{
			Logger.Info(
				$"{nameof(SearchAdapter)}.{nameof(Search)} call, " +
				$"to get{workViewApplicationName}.{request.ClassName}.");

			SetWorkViewApplication(workViewApplicationName);

			Class wvClass = GetWvClass(request.ClassName);

			if (wvClass == null)
			{
				throw new ArgumentOutOfRangeException(nameof(request.ClassName));
			}

			DynamicFilterQuery query = wvClass.CreateDynamicFilterQuery();
			foreach (Filter filter in request.Filters)
			{
				query.AddConstraint(
					filter.Name,
					Operator.Equal,
					filter.Value);
			}

			FilterQueryResultItemList results = query.Execute(10000);

			return results.Select(
				o => ModelMapper.GetMappedModel(wvClass.GetObjectByID(o.ObjectID)));
		}

		public (bool, Dictionary<string, string[]>) ValidateRequest(
			string workviewApplicationName,
			SearchRequest request)
		{
			Dictionary<string, string[]> errors = new Dictionary<string, string[]>();

			SetWorkViewApplication(workviewApplicationName);

			Class wvClass = GetWvClass(request.ClassName);

			if (wvClass == null)
			{
				errors.Add(
					$"{request.ClassName}",
					new[] { $"Class not found: {request.ClassName}" });
			}
			else
			{
				foreach (Filter filter in request.Filters)
				{
					ValidateRequestData(
						wvClass,
						filter.Name,
						errors);
				}
			}

			return (!errors.Any(), errors);
		}

		private void ValidateRequestData(
			Class wvClass,
			string attributeName,
			Dictionary<string, string[]> errors)
		{
			Attribute attr = wvClass.Attributes.Find(attributeName);
			if (attr == null)
			{
				errors.Add(
					$"{wvClass.Name}.{attributeName}",
					new[] { $"Property not found: {attributeName}" });
			}
		}
	}
}
// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Hplc
//   Adapter.DataSets.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Adapters.v1.WorkView
{
	using System.Collections.Generic;
	using System.Linq;
	using CareSource.WC.Entities.WorkView;
	using Newtonsoft.Json;

	public partial class Adapter
	{
		/// <summary>
		///    Gets the data set.
		/// </summary>
		/// <param name="dataSetName">The dataset name.</param>
		/// <param name="className">The class name.</param>
		/// <returns></returns>
		public IEnumerable<string> GetDataSet(
			string dataSetName,
			string className)
		{
			_logger.LogDebug($"Application Name {_applicationName}");
			DataSetRequest obj = new DataSetRequest
			{
				UserId = _httpRequestResolver.BasicAuthUserName(),
				ApplicationName = _applicationName,
				ClassName = className
			};
			_logger.LogDebug($"DataSetRequest:{JsonConvert.SerializeObject(obj)}");

			string action = $"/dataset/{dataSetName}";

			IEnumerable<string> values = Get<Request, IEnumerable<string>>(
				obj,
				action);

			return values;
		}

		/// <summary>
		///    Gets the class as data set.
		/// </summary>
		/// <param name="dataSetName">The data set name.</param>
		/// <param name="className">The class name.</param>
		/// <param name="filters">list of filters to search on</param>
		/// <returns></returns>
		public IDictionary<long, string> GetClassAsDataSet(
			string dataSetName,
			string className,
			IDictionary<string, string> filters)
		{
			ObjectGetRequest obj = new ObjectGetRequest
			{
				UserId = _httpRequestResolver.BasicAuthUserName(),
				ApplicationName = _applicationName,
				ClassName = className,
				FilterName = dataSetName,
				Filters = filters.Select(
					f => new FilterRequest
					{
						Name = f.Key,
						Value = f.Value
					})
			};

			string action = "/search";

			IEnumerable<WorkviewObject> result = Get<ObjectGetRequest, IEnumerable<WorkviewObject>>(
				obj,
				action);


			return result.ToDictionary(
				e => e.Id.GetValueOrDefault(0),
				e => e.Attributes.First()
					.Value);
		}
	}
}
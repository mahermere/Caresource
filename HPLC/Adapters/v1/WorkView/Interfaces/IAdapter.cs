// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Hplc
//   IAdapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Adapters.v1.WorkView
{
	using System;
	using System.Collections.Generic;
	using CareSource.WC.Entities.WorkView;
	using WC.Services.Hplc.Models.v1;

	/// <summary>
	/// </summary>
	/// <seealso cref="System.IDisposable" />
	public interface IAdapter : IDisposable
	{
		/// <summary>
		///    Creates the tin.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="requestDataEntityTin">The request data entity tin.</param>
		ObjectPostRequest CreateEntityTin(
			in long id,
			string requestDataEntityTin);

		/// <summary>
		///    Creates the product.
		/// </summary>
		/// <param name="requestId">The request identifier.</param>
		/// <param name="productIds">The product identifier.</param>
		/// <returns></returns>
		IEnumerable<ObjectPostRequest> CreateProducts(
			long requestId,
			IEnumerable<long> productIds);

		/// <summary>
		///    Creates the provider.
		/// </summary>
		/// <param name="requestId">The request identifier.</param>
		/// <param name="providers">The providers.</param>
		/// <returns></returns>
		List<ObjectPostRequest> CreateProviders(
			long requestId,
			IEnumerable<Provider> providers);

		/// <summary>
		/// Creates the provider.
		/// </summary>
		/// <param name="requestId">The request identifier.</param>
		/// <param name="provider">The provider.</param>
		/// <returns></returns>
		ObjectPostRequest CreateProvider(
			long requestId,
			Provider provider);

		/// <summary>
		///    Creates the request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns></returns>
		long CreateRequest(HplcServiceRequest request);

		/// <summary>
		///    Creates the xref entry.
		/// </summary>
		/// <param name="className">The class name.</param>
		/// <param name="parent">The parent.</param>
		/// <param name="child">The child.</param>
		/// <returns></returns>
		ObjectPostRequest CreateXRefObject(
			string className,
			KeyValuePair<string, long> parent,
			KeyValuePair<string, long> child);

		/// <summary>
		///    Creates the xref records.
		/// </summary>
		/// <param name="records">The records.</param>
		/// <returns></returns>
		IEnumerable<WorkviewObject> SaveXRefObjects(IEnumerable<ObjectPostRequest> records);

		/// <summary>
		/// Saves the x reference object.
		/// </summary>
		/// <param name="records">The records.</param>
		/// <returns></returns>
		long SaveXRefObject(ObjectPostRequest records);

		/// <summary>
		///    Gets the class as data set.
		/// </summary>
		/// <param name="dataSetName">The data set name.</param>
		/// <param name="className">The class name.</param>
		/// <param name="filters"></param>
		/// <returns></returns>
		IDictionary<long, string> GetClassAsDataSet(
			string dataSetName,
			string className,
			IDictionary<string, string> filters);

		/// <summary>
		///    Gets the data set.
		/// </summary>
		/// <param name="dataSetName">The data set name.</param>
		/// <param name="className">The class name.</param>
		/// <returns></returns>
		IEnumerable<string> GetDataSet(
			string dataSetName,
			string className);

		/// <summary>
		///    Gets the object.
		/// </summary>
		/// <param name="objectId">The object identifier.</param>
		/// <param name="className">The class name.</param>
		/// <param name="filterName">The filter name.</param>
		/// <returns></returns>
		WorkviewObject GetObject(
			in long objectId,
			string className,
			string filterName);

		/// <summary>
		///    Searches the specified class name.
		/// </summary>
		/// <param name="className">The class name.</param>
		/// <param name="filterName">The filter name.</param>
		/// <param name="attributes">The attributes.</param>
		/// <param name="filters">The filters.</param>
		/// <returns></returns>
		IEnumerable<WorkviewObject> Search(
			string className,
			string filterName,
			string[] attributes,
			IDictionary<string, string> filters);

		/// <summary>
		/// Creates the provider location phones.
		/// </summary>
		/// <param name="locationObjectId">The location object identifier.</param>
		/// <param name="phones">The phones.</param>
		/// <returns></returns>
		List<ObjectPostRequest> CreateProviderLocationPhones(
			long locationObjectId,
			List<Phone> phones);

		/// <summary>
		/// Creates the provider locations.
		/// </summary>
		/// <param name="providerId">The provider identifier.</param>
		/// <param name="locations">The locations.</param>
		/// <returns></returns>
		List<ObjectPostRequest> CreateProviderLocations(long providerId,
			List<Location> locations);
	}
}
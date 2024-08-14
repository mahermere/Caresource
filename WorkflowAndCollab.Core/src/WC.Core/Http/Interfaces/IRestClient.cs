// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   IRestClient.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Http.Interfaces
{
	using System.Net;
	using System.Net.Http;

	public interface IRestClient
	{
		(HttpStatusCode StatusCode, string Response, TResponseModel Model) Get<TRequestModel, TResponseModel>(
			string contentType,
			string endPoint,
			string action,
			string userName,
			string password,
			TRequestModel requestData);

		(HttpStatusCode StatusCode, string Response, TResponseModel Model) Post<TRequestModel, TResponseModel>(
			string contentType,
			string endPoint,
			string action,
			string userName,
			string password,
			TRequestModel requestData);

		(HttpStatusCode StatusCode, string Response, TResponseModel Model) Put<TRequestModel, TResponseModel>(
			string contentType,
			string endPoint,
			string action,
			string userName,
			string password,
			TRequestModel requestData);

		(HttpStatusCode StatusCode, string Response, TResponseModel Model) Patch<TRequestModel, TResponseModel>(
			string contentType,
			string endPoint,
			string action,
			string userName,
			string password,
			TRequestModel requestData);

		(HttpStatusCode StatusCode, string Response, TResponseModel Model) Delete<TRequestModel, TResponseModel>(
			string contentType,
			string endPoint,
			string action,
			string userName,
			string password,
			TRequestModel requestData);
	}
}
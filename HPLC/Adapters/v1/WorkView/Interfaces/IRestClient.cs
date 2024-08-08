// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   Class2.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Adapters.v1.WorkView
{
	using System.Net;

	public interface IRestClient
	{
		(HttpStatusCode StatusCode, string Response, TResponseModel Model) Delete<TRequestModel,
			TResponseModel>(
			string contentType,
			string endPoint,
			string action,
			string userName,
			string password,
			TRequestModel requestData);

		(HttpStatusCode StatusCode, string Response, TResponseModel Model) Get<TRequestModel,
			TResponseModel>(
			string contentType,
			string endPoint,
			string action,
			string userName,
			string password,
			TRequestModel requestData);

		//(HttpStatusCode StatusCode, string Response, TResponseModel Model) Patch<TRequestModel,
		//	TResponseModel>(
		//	string contentType,
		//	string endPoint,
		//	string action,
		//	string userName,
		//	string password,
		//	TRequestModel requestData);

		(HttpStatusCode StatusCode, string Response, TResponseModel Model) Post<TRequestModel,
			TResponseModel>(
			string contentType,
			string endPoint,
			string action,
			string userName,
			string password,
			TRequestModel requestData);

		(HttpStatusCode StatusCode, string Response, TResponseModel Model) Put<TRequestModel,
			TResponseModel>(
			string contentType,
			string endPoint,
			string action,
			string userName,
			string password,
			TRequestModel requestData);
	}
}
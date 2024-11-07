// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    IRestClient.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Http;

using System.Net;

public interface IRestClient
{
	(HttpStatusCode StatusCode, string Response, TResponseModel Model) Get
	<TRequestModel,
		TResponseModel>(
		string contentType,
		string endPoint,
		string action,
		string userName,
		string password,
		TRequestModel requestData);

	(HttpStatusCode StatusCode, string Response, TResponseModel Model) Post
	<TRequestModel,
		TResponseModel>(
		string contentType,
		string endPoint,
		string action,
		string userName,
		string password,
		TRequestModel requestData);

	(HttpStatusCode StatusCode, string Response, TResponseModel Model) Put
	<TRequestModel,
		TResponseModel>(
		string contentType,
		string endPoint,
		string action,
		string userName,
		string password,
		TRequestModel requestData);

	(HttpStatusCode StatusCode, string Response, TResponseModel Model) Patch
	<TRequestModel,
		TResponseModel>(
		string contentType,
		string endPoint,
		string action,
		string userName,
		string password,
		TRequestModel requestData);

	(HttpStatusCode StatusCode, string Response, TResponseModel Model) Delete
	<TRequestModel,
		TResponseModel>(
		string contentType,
		string endPoint,
		string action,
		string userName,
		string password,
		TRequestModel requestData);
}
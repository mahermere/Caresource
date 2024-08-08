// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   Class1.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Adapters.v1.WorkView
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Net;
	using System.Net.Http;
	using System.Net.Http.Headers;
	using System.Text;
	using System.Web;
	using CareSource.WC.Entities.Transactions;
	using CareSource.WC.OnBase.Core.Diagnostics.Interfaces;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.OnBase.Core.Helpers.Interfaces;
	using CareSource.WC.OnBase.Core.Services;
	using CareSource.WC.OnBase.Core.Transaction.Interfaces;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;

	/// <summary>
	/// </summary>
	public class RestClient : IRestClient
	{
		private readonly HttpClient _client;
		private readonly IJsonSerializerHelper _jsonSerializerHelper;
		private readonly ILogger _logger;
		private readonly ITransactionContextManager _transactionContextManager;
		private readonly IXmlSerializerHelper _xmlSerializerHelper;

		public RestClient(
			IJsonSerializerHelper jsonSerializerHelper,
			IXmlSerializerHelper xmlSerializerHelper,
			ITransactionContextManager transactionContextManager,
			ILogger logger)
		{
			_jsonSerializerHelper = jsonSerializerHelper;
			_xmlSerializerHelper = xmlSerializerHelper;
			_transactionContextManager = transactionContextManager;
			_logger = logger;

			_client = new HttpClient();
		}

		private StringContent BuildBody<TRequestModel>(
			TRequestModel request,
			string contentType)
			=> new StringContent(
				_jsonSerializerHelper.ToJson(request),
				Encoding.UTF8,
				contentType);

		private void BuildClient(
			string username,
			string password)
		{
			_client.DefaultRequestHeaders.Clear();
			_client.DefaultRequestHeaders.Authorization = null;

			if (username == null
				&& password == null)
			{
				username = _transactionContextManager.CurrentContext?.SecurityContext
					?.AuthenticatedUserId;
				password = _transactionContextManager.CurrentContext?.SecurityContext?.Password;

				_logger.LogInfo("Pulling username and password from TransactionContext.");
			}

			if (!username.IsNullOrWhiteSpace()
				&& !password.IsNullOrWhiteSpace())
			{
				_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
					"Basic",
					$"{username}:{password}".Base64Encode());

				_logger.LogDebug(
					$"Adding Username '{username}' and Password '{password}' to Authorization header.");
			}
		}

		public (HttpStatusCode StatusCode, string Response, TResponseModel Model) Delete
			<TRequestModel, TResponseModel>(
				string contentType,
				string endPoint,
				string action,
				string userName,
				string password,
				TRequestModel requestData)
		{
			_logger.LogInfo($"Sending DELETE request to endpoint '{endPoint}{action}'.");

			BuildClient(
				userName,
				password);

			SetRequestHeaders(
				_client.DefaultRequestHeaders,
				contentType);

			_logger.LogDebug(
				"Successfully built request.",
				new Dictionary<string, object>
				{
					{
						"Headers", _client.DefaultRequestHeaders
							.Select(
								h => new
								{
									Name = h.Key,
									h.Value
								})
							.ToList()
					},
					{ "Endpoint", Path.Combine(endPoint + action) },
					{ "QueryString", GetQueryString(requestData) },
					{ "Request", requestData }
				});

			(HttpStatusCode StatusCode, string Response, TResponseModel Model) returnValue
				= default((HttpStatusCode, string, TResponseModel));

			try
			{
				HttpResponseMessage response = _client.DeleteAsync(
						$"{Path.Combine(endPoint + action)}" + $"?{GetQueryString(requestData)}")
					.Result;

				returnValue = ProcessResponse<TResponseModel>(
					contentType,
					response);
			}
			catch (Exception ex)
			{
				returnValue.Response = ex.InnerException?.Message ?? ex.Message;
			}

			return returnValue;
		}

		private TResponseModel DeserializeResponse<TResponseModel>(
			string response,
			string serializationType)
		{
			TResponseModel model;

			try
			{
				if (serializationType == MediaTypes.ApplicationJson)
				{
					model = _jsonSerializerHelper.FromJson<TResponseModel>(response);
				}
				else if (serializationType == MediaTypes.ApplicationJson)
				{
					model = _xmlSerializerHelper.FromXml<TResponseModel>(response);
				}
				else
				{
					throw new Exception(
						$"Unable to deserialize response for serialization type '{serializationType}'.");
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex.Message,
					ex);
				model = default(TResponseModel);
			}

			return model;
		}

		public (HttpStatusCode StatusCode, string Response, TResponseModel Model) Get<TRequestModel,
			TResponseModel>(
			string contentType,
			string endPoint,
			string action,
			string userName,
			string password,
			TRequestModel requestData)
		{
			_logger.LogInfo($"Sending GET request to endpoint '{endPoint}{action}'.");

			BuildClient(
				userName,
				password);

			SetRequestHeaders(
				_client.DefaultRequestHeaders,
				contentType);

			_logger.LogDebug(
				"Successfully built request.",
				new Dictionary<string, object>
				{
					{
						"Headers", _client.DefaultRequestHeaders
							.Select(
								h => new
								{
									Name = h.Key,
									h.Value
								})
							.ToList()
					},
					{ "Endpoint", Path.Combine(endPoint + action) },
					{ "QueryString", GetQueryString(requestData) },
					{ "Request", requestData }
				});

			(HttpStatusCode StatusCode, string Response, TResponseModel Model) returnValue =
				default((HttpStatusCode, string, TResponseModel));

			try
			{
				HttpResponseMessage response = _client.GetAsync(
						$"{Path.Combine(endPoint + action)}" + $"?{GetQueryString(requestData)}")
					.Result;

				returnValue = ProcessResponse<TResponseModel>(
					contentType,
					response);
			}
			catch (Exception ex)
			{
				returnValue.Response = ex.InnerException?.Message ?? ex.Message;
			}

			return returnValue;
		}

		private string GetQueryString(
			object obj,
			string prefix = "")
		{
			if (obj == null)
			{
				return string.Empty;
			}

			string vQueryString = JsonConvert.SerializeObject(obj);

			JObject jObj = (JObject)JsonConvert.DeserializeObject(vQueryString);

			return string.Join(
				"&",
				jObj.Children()
					.Cast<JProperty>()
					.Select(
						jp =>
						{
							if (jp.Value.Type == JTokenType.Array)
							{
								int count = 0;
								return string.Join(
									"&",
									jp.Value.ToList()
										.Select(
											p =>
											{
												if (p.HasValues)
												{
													return GetQueryString(
														JsonConvert.DeserializeObject(p.ToString()),
														$"{jp.Name}[{count++}]");
												}

												string value = p.ToString();
												return value.IsNullOrWhiteSpace()
													? string.Empty
													: $"{jp.Name}[{count++}]={HttpUtility.UrlEncode(value)}";
											}));
							}

							if (jp.Value.HasValues)
							{
								return GetQueryString(
									JsonConvert.DeserializeObject(jp.Value.ToString()),
									jp.Name);
							}

							string s = jp.Value.ToString();

							return
								s.IsNullOrWhiteSpace()
									? string.Empty
									: $"{(prefix.Length > 0 ? $"{prefix}.{jp.Name}" : jp.Name)}"
									+ $"={HttpUtility.UrlEncode(s)}";
						}));
		}

		//public (HttpStatusCode StatusCode, string Response, TResponseModel Model) Patch<TRequestModel,
		//	TResponseModel>(
		//	string contentType,
		//	string endPoint,
		//	string action,
		//	string userName,
		//	string password,
		//	TRequestModel requestData)
		//{
		//	_logger.LogInfo($"Sending PATCH request to endpoint '{endPoint}{action}'.");

		//	BuildClient(
		//		userName,
		//		password);

		//	StringContent content = BuildBody(
		//		requestData,
		//		contentType);

		//	SetRequestHeaders(
		//		_client.DefaultRequestHeaders,
		//		contentType);

		//	_logger.LogDebug(
		//		"Successfully built request.",
		//		new Dictionary<string, object>
		//		{
		//			{
		//				"Headers", _client.DefaultRequestHeaders
		//					.Select(
		//						h => new
		//						{
		//							Name = h.Key,
		//							h.Value
		//						})
		//					.ToList()
		//			},
		//			{ "Endpoint", Path.Combine(endPoint + action) },
		//			{ "Request", requestData }
		//		});

		//	(HttpStatusCode StatusCode, string Response, TResponseModel Model) returnValue
		//		= default((HttpStatusCode, string, TResponseModel));

		//	try
		//	{
		//		HttpResponseMessage response = _client
		//			.PatchAsync(
		//				Path.Combine(endPoint + action),
		//				content)
		//			.Result;

		//		returnValue = ProcessResponse<TResponseModel>(
		//			contentType,
		//			response);
		//	}
		//	catch (Exception ex)
		//	{
		//		returnValue.Response = ex.InnerException?.Message ?? ex.Message;
		//	}

		//	return returnValue;
		//}

		public (HttpStatusCode StatusCode, string Response, TResponseModel Model) Post<TRequestModel,
			TResponseModel>(
			string contentType,
			string endPoint,
			string action,
			string userName,
			string password,
			TRequestModel requestData)
		{
			_logger.LogInfo($"Sending POST request to endpoint '{endPoint}{action}'.");

			BuildClient(
				userName,
				password);

			StringContent content = BuildBody(
				requestData,
				contentType);

			SetRequestHeaders(
				_client.DefaultRequestHeaders,
				contentType);

			_logger.LogDebug(
				"Successfully built request.",
				new Dictionary<string, object>
				{
					{
						"Headers", _client.DefaultRequestHeaders
							.Select(
								h => new
								{
									Name = h.Key,
									h.Value
								})
							.ToList()
					},
					{ "Endpoint", Path.Combine(endPoint + action) },
					{ "Request", requestData }
				});
			(HttpStatusCode StatusCode, string Response, TResponseModel Model) returnValue
				= default((HttpStatusCode, string, TResponseModel));
			try
			{
				HttpResponseMessage response = _client
					.PostAsync(
						Path.Combine(endPoint + action),
						content)
					.Result;

				returnValue = ProcessResponse<TResponseModel>(
					contentType,
					response);
			}
			catch (Exception ex)
			{
				returnValue.Response = ex.InnerException?.Message ?? ex.Message;
			}

			return returnValue;
		}

		private (HttpStatusCode StatusCode, string Response, TResponseModel Model)
			ProcessResponse<TResponseModel>(
				string contentType,
				HttpResponseMessage response)
		{
			HttpStatusCode statusCode = response.StatusCode;

			string results = response.Content
				.ReadAsStringAsync()
				.Result;

			TResponseModel model = DeserializeResponse<TResponseModel>(
				results,
				contentType);

			return (statusCode, results, model);
		}

		public (HttpStatusCode StatusCode, string Response, TResponseModel Model) Put<TRequestModel,
			TResponseModel>(
			string contentType,
			string endPoint,
			string action,
			string userName,
			string password,
			TRequestModel requestData)
		{
			_logger.LogInfo($"Sending PUT request to endpoint '{endPoint}{action}'.");

			BuildClient(
				userName,
				password);

			StringContent content = BuildBody(
				requestData,
				contentType);

			SetRequestHeaders(
				_client.DefaultRequestHeaders,
				contentType);

			_logger.LogDebug(
				"Successfully built request.",
				new Dictionary<string, object>
				{
					{
						"Headers", _client.DefaultRequestHeaders
							.Select(
								h => new
								{
									Name = h.Key,
									h.Value
								})
							.ToList()
					},
					{ "Endpoint", Path.Combine(endPoint + action) },
					{ "Request", requestData }
				});

			(HttpStatusCode StatusCode, string Response, TResponseModel Model) returnValue
				= default((HttpStatusCode, string, TResponseModel));

			try
			{
				HttpResponseMessage response = _client
					.PutAsync(
						Path.Combine(endPoint + action),
						content)
					.Result;

				returnValue = ProcessResponse<TResponseModel>(
					contentType,
					response);
			}
			catch (Exception ex)
			{
				returnValue.Response = ex.InnerException?.Message ?? ex.Message;
			}

			return returnValue;
		}

		private void SetRequestHeaders(
			HttpRequestHeaders headers,
			string contentType)
		{
			_logger.LogInfo(
				"Start Set Request Headers",
				new Dictionary<string, object>
				{
					{"ContentType", contentType },
					{"Headers", headers }
				}
			);

			headers.Add(
				"ContentType",
				contentType);

			if (!contentType.IsNullOrWhiteSpace())
			{
				headers.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
			}
			else
			{
				headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypes.ApplicationJson));

				_logger.LogInfo(
					$"No ContentType given. Defaulting to '{MediaTypes.ApplicationJson}'.");
			}

			if (_transactionContextManager.CurrentContext == null)
			{
				return;
			}

			TransactionContext transactionContext = _transactionContextManager.CopyCurrentContext();
			transactionContext.SecurityContext = null;

			string encodedTc = _jsonSerializerHelper.ToJson(transactionContext);

			headers.Add(
				"X-Transaction-Context",
				encodedTc.Base64Encode());
			headers.Add(
				"X-Transaction-Context-Content-Type",
				MediaTypes.ApplicationJson);

			_logger.LogDebug("Adding TransactionContext to request headers.");

			if (!transactionContext.EventContext?
					.CorrelationGuid.IsNullOrWhiteSpace()
				?? false)
			{
				headers.Add(
					"X-CorrelationGuid",
					transactionContext
						.EventContext.CorrelationGuid);

				_logger.LogDebug("Adding CorrelationGuid to request headers.");
			}

			if (!transactionContext.EventContext?
					.ApplicationName.IsNullOrWhiteSpace()
				?? false)
			{
				headers.Add(
					"X-Source",
					transactionContext
						.EventContext.ApplicationName);

				_logger.LogDebug("Adding Source to request headers.");
			}
		}
	}
}
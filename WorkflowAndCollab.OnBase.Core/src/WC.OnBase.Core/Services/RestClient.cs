//  ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    OnBase.Core
//    RestClient.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace CareSource.WC.OnBase.Core.Services
{
	using System;
	using System.IO;
	using System.Linq;
	using System.Net.Http;
	using System.Net.Http.Headers;
	using System.Text;
	using System.Web;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.OnBase.Core.Helpers.Interfaces;
	using CareSource.WC.OnBase.Core.Services.Interfaces;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;

	/// <summary>
	/// </summary>
	public class RestClient : IRestClient
	{
		private readonly IJsonSerializerHelper _jsonSerializerHelper;

		public RestClient(IJsonSerializerHelper jsonSerializerHelper)
		{
			_jsonSerializerHelper = jsonSerializerHelper;
		}

		/// <summary>
		///    Gets the action.
		/// </summary>
		public string Action { get; set; }

		public HttpClient Client { get; set; } = new HttpClient();

		/// <summary>
		///    Gets the content type of the message.
		/// </summary>
		public string ContentType { get; set; }

		/// <summary>
		///    Gets the end point.
		/// </summary>
		public string EndPoint { get; set; }

		/// <summary>
		///    Gets or sets the password.
		/// </summary>
		public object Password { get; set; }

		/// <summary>
		///    Gets or sets the name of the user.
		/// </summary>
		public object UserName { get; set; }

		/// <summary>
		///    Gets the specified request data.
		/// </summary>
		/// <param name="requestData">
		///    The request data.
		/// </param>
		/// <returns>
		///    an object of type
		///    <typeparam name="TResponse"></typeparam>
		/// </returns>
		public TResponse Get<TRequest, TResponse>(string contentType,
			string endPoint,
			string action,
			string userName,
			string password,
			TRequest requestData)
		{
			Action = action;
			EndPoint = endPoint;
			ContentType = contentType;
			UserName = userName;
			Password = password;

			Client.DefaultRequestHeaders.Authorization = null;

			if (UserName != null
				&& Password != null)
			{
				Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
					"Basic",
					Convert.ToBase64String(
						Encoding.ASCII.GetBytes(
							$"{UserName}:{Password}")));
			}

			Client.DefaultRequestHeaders.Accept.Clear();
			Client.DefaultRequestHeaders.Accept
				.Add(new MediaTypeWithQualityHeaderValue(ContentType));

			HttpResponseMessage response = Client.GetAsync(
				$"{Path.Combine(EndPoint + Action)}?{GetQueryString(requestData)}").Result;

			string data = response.Content.ReadAsStringAsync().Result;

			return _jsonSerializerHelper.FromJson<TResponse>(data);
		}

		public TResponse Post<TRequest, TResponse>(
			string contentType,
			string endPoint,
			string action,
			string userName,
			string password,
			TRequest requestData)
		{
			Action = action;
			EndPoint = endPoint;
			ContentType = contentType;
			UserName = userName;
			Password = password;

			Client.DefaultRequestHeaders.Authorization = null;

			if (UserName != null
				&& Password != null)
			{
				Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
					"Basic",
					Convert.ToBase64String(
						Encoding.ASCII.GetBytes(
							$"{UserName}:{Password}")));
			}

			StringContent content = new StringContent(
				_jsonSerializerHelper.ToJson(requestData),
				Encoding.UTF8, ContentType);

			content.Headers.ContentType = new MediaTypeHeaderValue(ContentType);

			HttpResponseMessage response = Client.PostAsync(
					Path.Combine(EndPoint + Action),
					content
				)
				.Result;

			string results = response.Content.ReadAsStringAsync()
				.Result;

			return _jsonSerializerHelper.FromJson<TResponse>(results);
		}

		public string GetQueryString(object obj,
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
								return string.Join("&", jp.Value.ToList().Select(
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
									JsonConvert.DeserializeObject(
										jp.Value.ToString()),
									jp.Name);
							}

							string s = jp.Value.ToString();

							return
								s.IsNullOrWhiteSpace()
									? string.Empty
									: $"{(prefix.Length > 0 ? $"{prefix}.{jp.Name}" : jp.Name)}"
									+ $"={HttpUtility.UrlEncode(s)}";
						}
					));
		}
	}
}
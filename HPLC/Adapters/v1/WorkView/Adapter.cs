// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Hplc
//   Adapter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Adapters.v1.WorkView
{
	using System;
	using System.Collections.Generic;
	using System.Net;
	using System.Net.Http;
	using System.Security.Authentication;
	using CareSource.WC.OnBase.Core.Diagnostics.Interfaces;
	using Newtonsoft.Json;
	using WC.Services.Hplc.Adapters.v2.WorkView;
	using WC.Services.Hplc.Models;

	/// <summary>
	///    Functions defining a CareSource.WC.Services.Hplc.Adapters.v1.WorkViewAdapter object.
	/// </summary>
	/// <seealso cref="IAdapter" />
	public partial class Adapter
		: IAdapter
	{

		private readonly IHttpRequestResolver _httpRequestResolver;
		private readonly ILogger _logger;
		private readonly IRestClient _restClient;
		private readonly string _rootUrl;
		private bool _disposed;
		private readonly string _applicationName;

		/// <summary>
		///    Initializes a new instance of the <see cref="v2.WorkView.Adapter" /> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="httpRequestResolver">The HTTP request resolver.</param>
		/// <param name="client"></param>
		public Adapter(
			ILogger logger,
			IHttpRequestResolver httpRequestResolver,
			IRestClient client)
		{
			_logger = logger;
			_httpRequestResolver = httpRequestResolver;

			_rootUrl = System.Configuration.ConfigurationManager
				.AppSettings.Get("Services.WorkView.RootUrl");

			_applicationName = System.Configuration.ConfigurationManager
				.AppSettings.Get("Services.WorkView.ApplicationName");

			_restClient = client;
		}

		/// <summary>
		///    Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
		///    resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		///    Finalizes an instance of the <see cref="v2.WorkView.Adapter" /> class.
		/// </summary>
		~Adapter()
			=> Dispose(false);

		/// <summary>
		///    Gets the specified arguments.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="action"></param>
		/// <returns></returns>
		/// <exception cref="UnauthorizedAccessException"></exception>
		/// <exception cref="KeyNotFoundException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		private TReturnType Get<TRequestType, TReturnType>(
			TRequestType data,
			string action = "")
		{
			try
			{
				_logger.LogDebug($"DataSent:{JsonConvert.SerializeObject(data)}");
				_logger.LogDebug($"Action:{action}");
				_logger.LogDebug($"root:{_rootUrl}");

				(HttpStatusCode statusCode, string response, TReturnType model) =
					_restClient.Get<TRequestType, TReturnType>(
						"application/json",
						_rootUrl,
						action,
						_httpRequestResolver.BasicAuthUserName(),
						_httpRequestResolver.BasicAuthPassword(),
						data);

				if ((int)(statusCode) >= 500)
				{
					if (response.Contains("found for id"))
					{
						throw new KeyNotFoundException(response);
					}
					throw new Exception(response);
				}
				if ((int)(statusCode) == 401)
				{
					throw new AuthenticationException(response);
				}
				if ((int)(statusCode) >= 400)
				{
					throw new ArgumentException(response);
				}
				return model;
			}
			catch (HttpRequestException httpRequestException)
			{
				_logger.LogError(
					httpRequestException.Message,
					httpRequestException);

				throw;
			}
		}

		private TReturnType Post<TRequestType, TReturnType>(
			TRequestType data,
			string action = "")
		{
			try
			{
				_logger.LogDebug(
					"Data being sent to WorkView Service",
					new Dictionary<string, object>
					{
						{ "data", data },
					});

				(HttpStatusCode statusCode, string response, TReturnType model) = _restClient.Post<TRequestType, TReturnType>(
						"application/json",
						_rootUrl,
						action,
						_httpRequestResolver.BasicAuthUserName(),
						_httpRequestResolver.BasicAuthPassword(),
						data
					);

				if ((int)(statusCode) >= 500)
				{
					throw new Exception(response);
				}
				if ((int)(statusCode) == 401)
				{
					throw new AuthenticationException(response);
				}
				if ((int)(statusCode) >= 400)
				{
					throw new ArgumentException(response);
				}
				return model;
			}
			catch (HttpRequestException httpRequestException)
			{
				_logger.LogError(
					httpRequestException.Message,
					httpRequestException
					);

				throw;
			}
		}

		/// <summary>
		///    Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing">
		///    <c>true</c> to release both managed and unmanaged resources;
		///    <c>false</c> to release only unmanaged resources.
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
			{
				return;
			}

			_disposed = true;
		}
	}
}
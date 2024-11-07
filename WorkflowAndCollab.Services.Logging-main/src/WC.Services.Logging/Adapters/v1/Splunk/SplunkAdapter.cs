//  ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    WC.Services.Logging
//    SplunkAdapter.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace WC.Services.Logging.Adapters.v1.Splunk
{
	using System.Net.Http.Headers;
	using System.Text;
	using CareSource.WC.Core.Configuration.Interfaces;
	using CareSource.WC.Core.Extensions;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;
	using NLog;
	using WC.Services.Logging.Models.v1;
	using LogLevel = Microsoft.Extensions.Logging.LogLevel;

	public class SplunkAdapter : ILoggingAdapter
	{
		private const string ApplicationJson = "application/json";
		private readonly SplunkConfiguration _configuration;

		public SplunkAdapter(
			ISettingsAdapter settingsAdapter) =>
			_configuration = settingsAdapter.GetSection<SplunkConfiguration>("Logging:Splunk");

		public ILoggingResponse WriteLog(
			Message message,
			string token)
		{
			message.Fields.TryGetValue(
				Models.Constants.CorrelationGuid,
				out string? correlationGuid);

			if (!IsEnabled(message.Event.LogLevel))
			{
				return new SplunkResponse
				{
					Text =
						$"Log level of '{message.Event.LogLevel.ToString()}' is below the system " +
						$"level of '{_configuration.LogLevel}'",
					Code = 0,
					CorrelationGuid = correlationGuid
				};
			}

			HttpClient client = ConfigureHttpClient(token);

			HttpRequestMessage request = ConfigureHttpRequestMessage(message);
			
			try
			{
				HttpResponseMessage response = client.SendAsync(request).Result;
				string result = response.Content.ReadAsStringAsync().Result;

				SplunkResponse? sResponse = JsonConvert.DeserializeObject<SplunkResponse>(result);

				if (sResponse == null)
				{
					throw new Exception(
						"Service error",
						new Exception("Response was null."));
				}

				return (int)response.StatusCode switch
				{
					>= 404
						=> throw new Exception(
							"Service error",
							new Exception(sResponse.Text)),
					_ => sResponse
				};
			}
			catch (Exception exception)
			{
				Logger? logger = LogManager.GetCurrentClassLogger();
				logger.Log(
					NLog.LogLevel.Info,
					SerializeObject(message));

				Message error = new Message()
				{
					Host = Environment.MachineName,
					SourceType = "Service Exception",
					Source = "Logging Service",
					Fields = new Dictionary<string, string>
					{
						{ "Exception", JsonConvert.SerializeObject(exception) },
						{ "ExceptionType", exception.GetType().Name },
						{ "Correlation Guid", correlationGuid },
						{ "StackTrace", Environment.StackTrace }
					},
					Event = new Event
					{
						Message = exception.Message,
						LogLevel = LogLevel.Critical
					}
				};

				logger.Log(NLog.LogLevel.Error, SerializeObject(error));

				return new SplunkResponse
				{
					Code = -1,
					CorrelationGuid = correlationGuid,
					Message = string.Empty,
					Text = "An unhandled exception occurred: see Correlation Guid."
				};
			}
			finally
			{
				client.Dispose();
			}
		}

		public bool IsEnabled(LogLevel logLevel)
			=> logLevel >= _configuration.LogLevel;

		private void ConfigureBasicAuth(HttpClient client,
			string token) =>
			client.DefaultRequestHeaders.Authorization =
				new AuthenticationHeaderValue(
					"Basic",
					$"Splunk:{token}".Base64Encode());

		private HttpClient ConfigureHttpClient(string token)
		{
			HttpClient client = new() { BaseAddress = new Uri(_configuration.Url) };

			client.DefaultRequestHeaders.Clear();

			client.DefaultRequestHeaders.Accept.Add(
				new MediaTypeWithQualityHeaderValue(ApplicationJson));

			ConfigureBasicAuth(client, token);

			return client;
		}

		private static HttpRequestMessage ConfigureHttpRequestMessage(Message message)
			=> new(
				HttpMethod.Post,
				string.Empty)
			{
				Content = new StringContent(
					SerializeObject(message),
					Encoding.UTF8,
					ApplicationJson)
			};

		private static string SerializeObject(Message message) =>
			JsonConvert.SerializeObject(
				message,
				new JsonSerializerSettings
				{
					Converters = new List<JsonConverter> { new StringEnumConverter() }
				});
	}
}
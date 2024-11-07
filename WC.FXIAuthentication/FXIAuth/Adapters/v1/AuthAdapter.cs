// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    AuthAdapter.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Adapters.v1;

using System.Net.Http.Headers;
using System.Text;
using FXIAuthentication.Core.Configuration;
using FXIAuthentication.Core.Configuration.Models;
using FXIAuthentication.Models.v1;
using Newtonsoft.Json;
using FXIAuthentication.Core.Extensions;

public class AuthAdapter : IAuthAdapter
{
	private readonly FacetsConfiguration _configSection;
	private readonly ILogger<AuthAdapter> _logger;


	public AuthAdapter(
		ILogger<AuthAdapter> logger,
		ISettingsAdapter settingsAdapter)
	{
		_logger = logger;
		_configSection = settingsAdapter.GetSection<FacetsConfiguration>("FXIAuth");
	}


	public FxiResponse GetToken(
		string userName,
		Guid correlationGuid)
	{
		_logger.LogInformation(
			$"Starting {nameof(AuthAdapter)}.{nameof(GetToken)}",
			new Dictionary<string, object>()
			{
				{ nameof(correlationGuid), correlationGuid },
				{ nameof(userName),userName}
			});

		_logger.LogInformation(
			"ConfigData ",
			new Dictionary<string, object>()
			{
				{ nameof(correlationGuid), correlationGuid },
				{ nameof(_configSection), _configSection}
			});

		Uri baseAddress = new(_configSection.EndPoint);

		HttpClient client = new() { BaseAddress = baseAddress };

		client.DefaultRequestHeaders
			.Accept
			.Add(new MediaTypeWithQualityHeaderValue("application/json"));

		HttpRequestMessage request = new(HttpMethod.Post, "");

		request.Headers.Authorization = new AuthenticationHeaderValue(
			"Basic",
			CreateBasicAuth(userName));

		request.Headers.Add(
			"Host",
			$"{baseAddress.Host}:{baseAddress.Port}");

		var data = new
		{
			_configSection.Region, _configSection.FacetsIdentity, _configSection.SignonMethod
		};

		string payload = JsonConvert.SerializeObject(data);

		_logger.LogInformation(
			"Body ",
			new Dictionary<string, object>()
			{
				{ nameof(correlationGuid), correlationGuid },
				{ nameof(payload),data}
			});

		HttpContent content = new StringContent(
			payload,
			Encoding.UTF8,
			"application/json");

		request.Content = content;
		
		HttpResponseMessage postResult = client.SendAsync(request).Result;

		FxiResponse response =
			JsonConvert.DeserializeObject<FxiResponse>(postResult.Content.ReadAsStringAsync()
				.Result);

		_logger.LogDebug(
			"Result ",
			new Dictionary<string, object>()
			{
				{ nameof(correlationGuid), correlationGuid },
				{ nameof(response),response}
			});

		_logger.LogInformation(
			$"Ending {nameof(AuthAdapter)}.{nameof(GetToken)}",
			new Dictionary<string, object>()
			{
				{ nameof(correlationGuid), correlationGuid },
			});

		return response;
	}

	private string CreateBasicAuth(string userName)
	{
		byte[] bytes =
			Encoding.ASCII.GetBytes(userName + ":" + _configSection.Password);
		return Convert.ToBase64String(bytes);
	}
}
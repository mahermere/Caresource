// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    FxiResponse.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Models.v1;

using Newtonsoft.Json;

public class FxiResponse
{
	public Data Data { get; set; }
	public Status Status { get; set; }
}

public class Data
{
	[JsonProperty("access_token")] public string Token { get; set; }

	[JsonProperty("expires_in")] public int ExpiresIn { get; set; }

	[JsonProperty("token_type")] public string TokenType { get; set; }

	[JsonProperty("refresh_token")] public string RefreshToken { get; set; }

	[JsonProperty("scope")] public string Scope { get; set; }
}

public class Status
{
	public int HttpStatusCode { get; set; }
	public int StatusCode { get; set; }
	public string StatusMessage { get; set; }
	public int Level { get; set; }
}
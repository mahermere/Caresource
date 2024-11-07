// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    Claims
//    FxiResponse.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace Claims.Models
{
	using Newtonsoft.Json;

	public class FxiResponse
	{
		public Data Data
		{
			get;
			set;
		}

		public Status Status
		{
			get;
			set;
		}
	}

	public class Data
	{
		[JsonProperty("token")]
		public string Token
		{
			get;
			set;
		}

		[JsonProperty("expiresIn")]
		public int ExpiresIn
		{
			get;
			set;
		}

		[JsonProperty("tokenType")]
		public string TokenType
		{
			get;
			set;
		}

		[JsonProperty("refreshToken")]
		public string RefreshToken
		{
			get;
			set;
		}

		[JsonProperty("scope")]
		public string Scope
		{
			get;
			set;
		}
	}

	public class Status
	{
		public int HttpStatusCode
		{
			get;
			set;
		}

		public int StatusCode
		{
			get;
			set;
		}

		public string StatusMessage
		{
			get;
			set;
		}

		public int Level
		{
			get;
			set;
		}
	}
}
// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-$CURRENT_YEAR$.  All rights reserved.
//  
//    $PROJECT$
//    FXIAuthAdapter.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------
//

namespace Claims.Adapter
{
	using System;
	using System.Net;
	using System.Net.Http;
	using System.Net.Http.Headers;
	using Claims.Models;
	using Newtonsoft.Json;

	public abstract class FXIAuthAdapter
	{
		protected string GetAuthToken(FacetsConfiguration facetsConfig)
		{
			Uri baseUri = new Uri(
				string.Format(
					facetsConfig.AuthEndpoint,
					facetsConfig.AuthUser));

			HttpClient client = new HttpClient() { BaseAddress = baseUri };

			client.DefaultRequestHeaders
				.Accept
				.Add(new MediaTypeWithQualityHeaderValue("application/Xml"));

			HttpResponseMessage result = client.GetAsync("").Result;

			FxiResponse fxiAuth = JsonConvert.DeserializeObject<FxiResponse>(
				result.Content.ReadAsStringAsync().Result);
			if (fxiAuth != null
			    && fxiAuth.Status.HttpStatusCode != (int)HttpStatusCode.Created)
			{
				throw new UnauthorizedAccessException(fxiAuth.Status.StatusMessage);
			}

			return fxiAuth.Data.Token;
		}
	}
}

// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Hplc
//   HttpRequestResolver.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace HplcManagement.Models
{
	using System;
	using System.Linq;
	using System.Security.Authentication;
	using System.Text;
	using System.Web;
	using CareSource.WC.Entities.Transactions;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using Newtonsoft.Json;

	/// <summary>
	/// Data and functions describing a CareSource.WC.Services.Hplc.Models.HttpRequestResolver object.
	/// </summary>
	/// <remarks>
	/// Class used to Inject the Http context into classes do they can vase credentials through
	/// to service downstream
	/// </remarks>
	/// <seealso cref="IHttpRequestResolver" />
	public class HttpRequestResolver
		: IHttpRequestResolver
	{
		private readonly HttpContext _context;
		private readonly string _userName;
		private readonly string _password;

		/// <summary>
		/// Initializes a new instance of the <see cref="HttpRequestResolver"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		public HttpRequestResolver()
		{
			_context = HttpContext.Current;

			string auth = GetBasicAuth();
			auth = auth.Split(' ')
				.Last()
				.SafeTrim();

			byte[] bytes = Convert.FromBase64String(auth);

			string creds = Encoding.ASCII.GetString(bytes);

			string[] items = creds.Split(':');

			_userName = items.First();
			_password = items.Last();
		}

		/// <summary>
		/// Basics the authentication user name.
		/// </summary>
		/// <returns>
		/// Returns the Basic Auth User Name
		/// </returns>
		public string BasicAuthUserName()
			=> _userName;

		/// <summary>
		/// Basics the authentication password.
		/// </summary>
		/// <returns></returns>
		public string BasicAuthPassword()
			=> _password;

		/// <summary>
		/// Gets the basic authentication from the HTTP request.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="AuthenticationException">Basic Authorization is required.</exception>
		public string GetBasicAuth()
		{
			string auth = _context.Request.Headers["Authorization"];

			string basicAuth = auth.StartsWith(
				"basic",
				StringComparison.InvariantCultureIgnoreCase)
				? auth
				: null;

			if (basicAuth.IsNullOrWhiteSpace())
			{
				throw new AuthenticationException("Basic Authorization is required.");
			}

			return basicAuth;
		}

		/// <summary>
		/// Gets the transaction context.
		/// </summary>
		/// <returns></returns>
		public TransactionContext GetTransactionContext()
			=> JsonConvert.DeserializeObject<TransactionContext>(
				_context.Request.Headers["TransactionContext"]);
	}
}
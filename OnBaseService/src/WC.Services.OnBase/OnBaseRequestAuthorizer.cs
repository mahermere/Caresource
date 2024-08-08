// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.OnBase
//   OnBaseAuthorizeFilter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.OnBase
{
	using System;
	using System.Web.Http.Controllers;
	using CareSource.WC.Entities.Transactions;
	using CareSource.WC.OnBase.Core.Connection.Interfaces;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.OnBase.Core.Http.Interfaces;
	using CareSource.WC.OnBase.Core.Transaction.Interfaces;
	using Hyland.Unity;

	public class OnBaseRequestAuthorizer : IRequestAuthorizer
	{
		private readonly IApplicationConnectionAdapter<Application> _applicationConnectionAdapter;
		private readonly ITransactionContextManager _transactionContextManager;

		public OnBaseRequestAuthorizer(
			IApplicationConnectionAdapter<Application> applicationConnectionAdapter,
			ITransactionContextManager transactionContextManager)
		{
			_applicationConnectionAdapter = applicationConnectionAdapter;
			_transactionContextManager = transactionContextManager;
		}

		public bool Authorize(HttpActionContext actionContext)
		{
			if (actionContext?.Request?.Headers?.Authorization?.Scheme?.ToLower() != "basic")
				throw new UnauthorizedAccessException(
					"You must use Basic Auth in order to use this endpoint.");

			string basicAuthParameter = actionContext.Request.Headers.Authorization.Parameter;
			string[] decodedString = basicAuthParameter.Base64Decode()
				?
				.Split(
					new[] { ":" },
					StringSplitOptions.RemoveEmptyEntries);

			if (decodedString == null ||
			    decodedString.Length < 2)
				throw new UnauthorizedAccessException("Invalid format for Basic Auth value.");

			string username = decodedString[0];
			string password = decodedString[1];

			try
			{
				_applicationConnectionAdapter.Connect(
					username,
					password);
			}
			catch (Exception ex)
			{
				throw new UnauthorizedAccessException(
					$"Unable to Authorize username '{username}' in OnBase. Description: {ex.Message}");
			}

			_transactionContextManager.CurrentContext.SecurityContext = new SecurityContext
			{
				AuthenticatedUserId = username,
				Password = password,
				Domain = "CareSource",
				Type = "Basic Auth"
			};

			return true;
		}
	}
}
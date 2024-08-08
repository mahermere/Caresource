// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Hplc
//   IHttpRequestResolver.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Models
{
	using CareSource.WC.Entities.Transactions;

	/// <summary>
	/// 
	/// </summary>
	public interface IHttpRequestResolver
	{
		/// <summary>
		/// Gets the basic authentication.
		/// </summary>
		/// <returns></returns>
		string GetBasicAuth();

		/// <summary>
		/// Gets the transaction context.
		/// </summary>
		/// <returns></returns>
		TransactionContext GetTransactionContext();

		/// <summary>
		/// Basics the authentication user name.
		/// </summary>
		/// <returns></returns>
		string BasicAuthUserName();

		/// <summary>
		/// Basics the authentication password.
		/// </summary>
		/// <returns></returns>
		string BasicAuthPassword();
	}
}
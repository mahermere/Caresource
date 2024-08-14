// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   IAuthorizationManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Http.Interfaces
{
	using Microsoft.AspNetCore.Http;

	public interface IAuthorizationManager
	{
		string AuthType { get; }

		void Authorize(
			HttpContext context);
	}
}
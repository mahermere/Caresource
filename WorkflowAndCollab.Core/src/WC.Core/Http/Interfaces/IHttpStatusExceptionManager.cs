// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   IHttpStatusExceptionManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Http.Interfaces
{
	using System;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Http;

	public interface IHttpStatusExceptionManager
	{
		Task DetermineResponse(
			HttpContext context,
			Exception exception);
	}
}
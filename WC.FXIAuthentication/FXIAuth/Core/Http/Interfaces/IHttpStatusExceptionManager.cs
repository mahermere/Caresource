// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    IHttpStatusExceptionManager.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Http;

public interface IHttpStatusExceptionManager
{
	Task DetermineResponse(
		HttpContext context,
		Exception exception);
}
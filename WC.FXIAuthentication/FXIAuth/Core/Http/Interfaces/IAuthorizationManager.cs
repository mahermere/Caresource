// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    IAuthorizationManager.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Http;

public interface IAuthorizationManager
{
	string AuthType { get; }

	void Authorize(
		HttpContext context);
}
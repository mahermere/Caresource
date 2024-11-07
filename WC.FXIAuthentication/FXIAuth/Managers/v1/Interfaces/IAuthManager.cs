// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    IAuthManager.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Managers.v1;

using FXIAuthentication.Models.v1;

public interface IAuthManager
{
	FxiResponse GetToken(string userName,
		Guid correlationGuid);
}
// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    IAuthAdapter.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Adapters.v1;

using FXIAuthentication.Models.v1;

public interface IAuthAdapter
{
	FxiResponse GetToken(
		string userName,
		Guid correlationGuid);
}
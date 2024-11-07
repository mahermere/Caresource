// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    BusinessSecurityContext.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Transaction.Models;

public class BusinessSecurityContext
{
	public string ExternalUserId { get; set; }

	public string ExternalUserType { get; set; }
}
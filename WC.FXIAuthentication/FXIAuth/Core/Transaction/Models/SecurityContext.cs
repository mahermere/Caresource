// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    SecurityContext.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Transaction.Models;

public class SecurityContext
{
	public string AuthenticatedUserId { get; set; }

	public string Domain { get; set; }

	public string Password { get; set; }

	public string Policy { get; set; }

	public string Type { get; set; }
}
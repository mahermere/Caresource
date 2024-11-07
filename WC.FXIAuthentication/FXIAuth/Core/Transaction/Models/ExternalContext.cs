// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    ExternalContext.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Transaction.Models;

public class ExternalContext
{
	public BusinessSecurityContext BusinessSecurityContext { get; set; }

	public string ExternalMessageID { get; set; }
	public string ExternalTransactionID { get; set; }
}
// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    BusinessContext.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Transaction.Models;

public class BusinessContext
{
	public string BusinessStatus { get; set; }

	public List<ContextList> ContextList { get; set; }

	public ExternalContext ExternalContext { get; set; }

	public string MemberId { get; set; }

	public string ProviderId { get; set; }
	public string SubscriberId { get; set; }
}
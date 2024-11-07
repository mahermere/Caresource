// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    ProcessContext.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Transaction.Models;

using System.Runtime.Serialization;

[DataContract]
public class ProcessContext
{
	public ActivityContext ActivityContext { get; set; }

	public string ActivityName { get; set; }

	public List<ContextList> ContextList { get; set; }

	public ExceptionContext Exception { get; set; }
}
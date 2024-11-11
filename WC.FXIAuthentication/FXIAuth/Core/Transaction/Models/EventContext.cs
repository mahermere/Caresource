// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    EventContext.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.Transaction.Models;

using System.Xml.Serialization;
using Newtonsoft.Json;

public class EventContext
{
	[JsonProperty(PropertyName = "AppllicationName")]
	[XmlElement("AppllicationName")]
	public string ApplicationName { get; set; }

	public string CorrelationGuid { get; set; }

	public string CurrentGuid { get; set; }

	public string Destination { get; set; }

	public string Event { get; set; }

	public string ExecutionGuid { get; set; }

	public string ParentGuid { get; set; }

	public string Source { get; set; }

	public string Version { get; set; }
}
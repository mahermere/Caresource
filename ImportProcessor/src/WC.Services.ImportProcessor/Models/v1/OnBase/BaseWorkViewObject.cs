// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   ImportProcessor
//   BaseWorkViewObject.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace ImportProcessor.Models.v1.OnBase
{
	using System;
	using Newtonsoft.Json;

	//using CareSource.WC.Core.Extensions;
	//using WC.Services.ImportProcessor.Adapters.v1.OnBase;
	//using Attribute = WC.ImportProcessor.Api.Adapters.v1.OnBase.Attribute;

	public abstract class BaseWorkViewObject
	{
		/// <summary>
		///    Gets or sets the Application Object Identifier
		/// </summary>
		/// <remarks>
		///    The Object Identifier is the Unique Id Primary Key value from the data store
		/// </remarks>
		public long ObjectId { get; set; }

		/// <summary>
		///    Gets or sets the Base Work View Object Correlation Unique identifier
		/// </summary>
		/// <remarks>
		///    Guid used to track objects through the processes each new transaction gets a new one
		/// </remarks>
		[JsonIgnore]
		public Guid CorrelationGuid { get; set; }
	}
}
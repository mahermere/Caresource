// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   Workview
//   WorkViewObject.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.WorkView.Models.v5
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	public class WorkViewBaseObject
	{
		/// <summary>
		/// Gets or sets the Request Data Class Name
		/// </summary>
		/// <remarks>
		///	The class name is the name of the [Class] in Workview that you want to create.
		/// </remarks>
		[Required]
		public string ClassName { get; set; }

		/// <summary>
		/// Gets or sets the Request Data Properties
		/// </summary>
		/// <remarks>
		///	The properties are a key value pairs, the names must match exactly to the Attributes on
		///	the WorkView Class.
		///	Values will be validated, against data type and max length.
		/// </remarks>
		public IDictionary<string, string> Properties{ get; set; }

		/// <summary>
		/// Gets or sets the Work View Base Object Related
		/// </summary>
		public IEnumerable<WorkViewBaseObject> Related { get; set; }
	}
}

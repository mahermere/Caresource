// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Hplc
//   HplcResponse.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Models.v1
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	///    Data and functions describing a CareSource.WC.Services.Hplc.Models.v1.HplcResponse object.
	/// </summary>
	public class HplcResponse
	{
		/// <summary>
		///    Gets or sets the HPLC Response Application Identifier
		/// </summary>
		public int ApplicationId { get; set; }

		/// <summary>
		///    Gets or sets the HPLC Response Application Name
		/// </summary>
		public string ApplicationName { get; set; }

		/// <summary>
		///    Gets or sets the HPLC Response Attributes
		/// </summary>
		public IEnumerable<Attribute> Attributes { get; set; }

		/// <summary>
		///    Gets or sets the HPLC Response Class Identifier
		/// </summary>
		public int ClassId { get; set; }

		/// <summary>
		///    Gets or sets the HPLC Response Class Name
		/// </summary>
		public string ClassName { get; set; }

		/// <summary>
		///    Gets or sets the HPLC Response Created By
		/// </summary>
		public string CreatedBy { get; set; }

		/// <summary>
		///    Gets or sets the HPLC Response Created Date
		/// </summary>
		public DateTime CreatedDate { get; set; }

		/// <summary>
		///    Gets or sets the HPLC Response Filter Name
		/// </summary>
		public string FilterName { get; set; }

		/// <summary>
		///    Gets or sets the HPLC Response Filters
		/// </summary>
		public IEnumerable<Filter> Filters { get; set; }

		/// <summary>
		///    Gets or sets the HPLC Response Identifier
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		///    Gets or sets the HPLC Response Name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///    Gets or sets the HPLC Response Revision By
		/// </summary>
		public string RevisionBy { get; set; }

		/// <summary>
		///    Gets or sets the HPLC Response Revision Date
		/// </summary>
		public DateTime RevisionDate { get; set; }
	}
}
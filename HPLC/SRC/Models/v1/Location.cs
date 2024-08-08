// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Hplc
//   Location.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Models.v1
{
	using System.Collections.Generic;

	/// <summary>
	///    Data describing a CareSource.WC.Services.Hplc.Models.v1.Location object.
	/// </summary>
	public class Location
	{
		/// <summary>
		///    Gets or sets the action type for this instance.
		/// </summary>
		public string ActionType { get; set; }

		/// <summary>
		///    Gets or sets the Location Capacity
		/// </summary>
		public int Capacity { get; set; }

		/// <summary>
		///    Gets or sets the Location City
		/// </summary>
		public string City { get; set; }

		/// <summary>
		/// Gets or sets the Location County
		/// </summary>
		public string County { get; set; }

		/// <summary>
		///    Gets or sets a value indicating whether this instance is primary.
		/// </summary>
		/// <value>
		///    <c>true</c> if this instance is primary; otherwise, <c>false</c>.
		/// </value>
		public bool IsPrimary { get; set; }

		/// <summary>
		///    Gets or sets the Location Notes
		/// </summary>
		public string Notes { get; set; }

		/// <summary>
		///    Gets or sets the Location Postal Code
		/// </summary>
		public string PostalCode { get; set; }

		/// <summary>
		///    Gets or sets the Location State
		/// </summary>
		public string State { get; set; }

		/// <summary>
		///    Gets or sets the Location Street 1
		/// </summary>
		public string Street1 { get; set; }

		/// <summary>
		///    Gets or sets the Location Street 2
		/// </summary>
		public string Street2 { get; set; }

		/// <summary>
		///    Gets or sets the Location Type
		/// </summary>
		public IEnumerable<string> Type { get; set; }= new List<string>();

		/// <summary>
		/// Gets or sets the Location Phones
		/// </summary>
		public IEnumerable<Phone> Phones { get; set; } = new List<Phone>();

		/// <summary>
		/// Gets or sets the Location Minimum Age
		/// </summary>
		public int MinAge { get; set; }

		/// <summary>
		/// Gets or sets the Location Maximum Age
		/// </summary>
		public int MaxAge { get; set; }

		/// <summary>
		/// Gets or sets the Location Gender Restrictions
		/// </summary>
		public string GenderRestrictions { get; set; }
	}
}
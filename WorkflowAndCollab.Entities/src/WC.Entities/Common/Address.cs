// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CareSource.WC.Entities.WC.Entities
//   Address.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Common
{
	/// <summary>
	///    Represents the data used to define a the address
	/// </summary>
	public class Address
	{
		/// <summary>
		///    Gets or sets the city of the address class.
		/// </summary>
		public string City { get; set; }

		/// <summary>
		///    Gets or sets the line1 of the address class.
		/// </summary>
		public string Line1 { get; set; }

		/// <summary>
		///    Gets or sets the line2 of the address class.
		/// </summary>
		public string Line2 { get; set; }

		/// <summary>
		///    Gets or sets the line3 of the address class.
		/// </summary>
		public string Line3 { get; set; }

		/// <summary>
		///    Gets or sets the state of the address class.
		/// </summary>
		public string State { get; set; }

		/// <summary>
		///    Gets or sets the zip of the address class.
		/// </summary>
		public string Zip { get; set; }
	}
}
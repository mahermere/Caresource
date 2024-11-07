// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2023.  All rights reserved.
// 
//    WC.Services.Claims
//    Address.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace Claims.Models
{
	public class Address
	{
		public string City { get; set; } = string.Empty;

		public string Line1 { get; set; } = string.Empty;

		public string Line2 { get; set; } = string.Empty;

		public string Line3 { get; set; } = string.Empty;

		public string State { get; set; } = string.Empty;

		public string Zip { get; set; } = string.Empty;
	}
}
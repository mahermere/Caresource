// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   Attribute.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace HplcManagement.Models.v1
{
	using System;
	using CareSource.WC.OnBase.Core.ExtensionMethods;

	/// <summary>
	///    Data and functions describing a CareSource.WC.Services.Hplc.Models.v1.Attribute object.
	/// </summary>
	public class Attribute
	{
		/// <summary>
		///    Gets or sets the Attribute Name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///    Gets or sets the Attribute Value
		/// </summary>
		public string Value { get; set; }

		public bool HasValue
			=> !Value.IsNullOrWhiteSpace();

		public bool HasBooleanValue
			=> HasValue
				&& bool.TryParse(
					Value,
					out _);

		public bool GetBooleanValue
		{
			get
			{
				if (bool.TryParse(
						Value,
						out bool boolValue))
				{
					return boolValue;
				}

				switch (Value.SafeTrim().ToLower())
				{
					case "yes":
					case "y":
						return true;
					case "no":
					case "n":
						return false;
				}

				return default;
			}
		}

		public string GetStringValue
			=> Value.SafeTrim();

		public long GetLongValue
			=> Value.ToSafeLong().GetValueOrDefault();
		
		public DateTime GetDateTimeValue
			=> Value.ToSafeDateTime().GetValueOrDefault();

		public DateTime GetDateValue
			=> Value.ToSafeDateTime().GetValueOrDefault().Date;

		public double GetDoubleValue
			=> Value.ToSafeDouble().GetValueOrDefault();

		public decimal GetDecimalValue
			=> Value.ToSafeDecimal().GetValueOrDefault();
	}
}
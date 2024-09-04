// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   Attribute.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Dotnet8.Models
{
    using System;
    using System.Collections.Generic;
   

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
            => !String.IsNullOrWhiteSpace(Value);

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

                switch (Value.Trim().ToLower())
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
            => Value.Trim();

        public long GetLongValue
            => Convert.ToInt64(Value);//Value.ToSafeLong().GetValueOrDefault();

        public DateTime GetDateTimeValue
            => DateTime.Parse(Value);//Value.ToSafeDateTime().GetValueOrDefault();

        public DateTime GetDateValue
            => DateTime.Parse(Value).Date;//Value.ToSafeDateTime().GetValueOrDefault().Date;

        public double GetDoubleValue
            => Convert.ToDouble(Value);//Value.ToSafeDouble().GetValueOrDefault();

        public decimal GetDecimalValue
            => Convert.ToDecimal(Value); //Value.ToSafeDecimal().GetValueOrDefault();
    }
}
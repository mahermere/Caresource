// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   Workview
//   AttributeValue.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.WorkView.Dotnet8.Models.v5
{
	using System;
    using WC.Services.WorkView.Dotnet8.Extensions;

    //using CareSource.WC.OnBase.Core.ExtensionMethods;

    public static class AttributeValueHelper
	{
		public static bool GetBooleanValue(this string value)
			=> value.ToSafeBool().GetValueOrDefault();

		public static DateTime GetDateTimeValue(this string value)
			=> value.ToSafeDateTime().GetValueOrDefault();

		public static DateTime GetDateValue(this string value)
			=> value.ToSafeDateTime().GetValueOrDefault().Date;

		public static decimal GetDecimalValue(this string value)
			=> value.ToSafeDecimal().GetValueOrDefault();

		public static double GetDoubleValue(this string value)
			=> value.ToSafeDouble().GetValueOrDefault();

		public static long GetLongValue(this string value)
			=> value.ToSafeLong().GetValueOrDefault();

		public static string GetStringValue(this string value)
			=> value.Trim();
		
		public static bool HasValue(this string value)
			=> !string.IsNullOrWhiteSpace(value);

	}
}
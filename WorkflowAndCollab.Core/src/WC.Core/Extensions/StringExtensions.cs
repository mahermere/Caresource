// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   StringExtensions.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Extensions
{
	using System;
	using System.Globalization;
	using System.Net;
	using System.Text;

	public static class StringExtensions
	{
		public static string ReturnIfNotNullOrEmpty(
			this string text) => !string.IsNullOrEmpty(text)
			? text
			: null;

		public static bool? ToSafeBool(
			this string source)
		{
			if (bool.TryParse(
				source.ToLower(),
				out bool boolean))
			{
				return boolean;
			}

			return null;
		}

		public static byte? ToSafeByte(
			this string source)
		{
			if (byte.TryParse(
				source,
				out byte byteNumber))
			{
				return byteNumber;
			}

			return null;
		}

		public static char? ToSafeChar(
			this string source)
		{
			if (char.TryParse(
				source,
				out char charValue))
			{
				return charValue;
			}

			return null;
		}

		public static DateTime? ToSafeDateTime(
			this string source)
		{
			if (DateTime.TryParse(
				source,
				out DateTime dateTimeValue))
			{
				return dateTimeValue;
			}

			return null;
		}

		public static decimal? ToSafeDecimal(
			this string source)
		{
			if (decimal.TryParse(
				source,
				out decimal decimalNumber))
			{
				return decimalNumber;
			}

			return null;
		}

		public static double? ToSafeDouble(
			this string source)
		{
			if (double.TryParse(
				source,
				out double doubleNumber))
			{
				return doubleNumber;
			}

			return null;
		}

		public static TEnumType? ToSafeEnum<TEnumType>(
			this string source,
			bool ignoreCase = true)
			where TEnumType : struct
		{
			TEnumType typeEnum = default(TEnumType);

			if (!(typeEnum is Enum))
			{
				throw new ArgumentException("TTypeEnum must be an enumerated type");
			}

			if (Enum.TryParse(
				source,
				ignoreCase,
				out typeEnum))
			{
				return typeEnum;
			}

			return null;
		}

		public static DateTime? ToSafeExactDateTime(
			this string source,
			string format,
			IFormatProvider provider,
			DateTimeStyles styles)
		{
			if (DateTime.TryParseExact(
				source,
				format,
				provider,
				styles,
				out DateTime formattedDateTimeValue))
			{
				return formattedDateTimeValue;
			}

			return null;
		}

		public static DateTime? ToSafeExactDateTime(
			this string source,
			string[] formats,
			IFormatProvider provider,
			DateTimeStyles styles)
		{
			if (DateTime.TryParseExact(
				source,
				formats,
				provider,
				styles,
				out DateTime formattedDateTimeValue))
			{
				return formattedDateTimeValue;
			}

			return null;
		}

		public static float? ToSafeFloat(
			this string source)
		{
			if (float.TryParse(
				source,
				out float floatNumber))
			{
				return floatNumber;
			}

			return null;
		}

		public static int? ToSafeInt(
			this string source)
		{
			if (int.TryParse(
				source,
				out int integer))
			{
				return integer;
			}

			return null;
		}

		public static long? ToSafeLong(
			this string source)
		{
			if (long.TryParse(
				source,
				out long longNumber))
			{
				return longNumber;
			}

			return null;
		}

		public static short? ToSafeShort(
			this string source)
		{
			if (short.TryParse(
				source,
				out short shortNumber))
			{
				return shortNumber;
			}

			return null;
		}

		public static string Base64Encode(
			this string plainText)
		{
			byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
			return Convert.ToBase64String(plainTextBytes);
		}

		public static string Base64Decode(
			this string base64EncodedData)
		{
			byte[] base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
			return Encoding.UTF8.GetString(base64EncodedBytes);
		}

		public static string UrlEncode(
			this string plainText) => WebUtility.UrlEncode(plainText);

		public static string UrlDecode(
			this string urlEncodedData) => WebUtility.UrlDecode(urlEncodedData);

		/// <summary>
		///    Determines whether the value [is null or white space].
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///    <c>true</c> if [is null or white space] [the specified value]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsNullOrWhiteSpace(
			this string value)
			=> string.IsNullOrWhiteSpace(value);

		/// <summary>
		///    Safely trims the value and if null returns an empty string.
		///    Removes all leading and trailing white-space characters from the current System.String
		///    object.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///    The string that remains after all white-space characters are removed from the start and
		///    end of the current string. If no characters can be trimmed from the current instance,
		///    the method returns the current instance unchanged.
		/// </returns>
		public static string SafeTrim(
			this string value)
			=> value.IsNullOrWhiteSpace()
				? string.Empty
				: value.Trim();

		/// <summary>
		///    Safely trims the value and if null returns an empty string.
		///    Removes all trailing white-space characters from the current System.String object.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///    The string that remains after all white-space characters are removed from the end of the
		///    current string. If no characters can be trimmed from the current instance, the method
		///    returns the current instance unchanged.
		/// </returns>
		public static string SafeTrimEnd(
			this string value)
			=> value.IsNullOrWhiteSpace()
				? string.Empty
				: value.TrimEnd();

		/// <summary>
		///    Safely trims the value and if null returns an empty string.
		///    Removes all leading white-space characters from the current System.String object.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///    The string that remains after all white-space characters are removed from the start of
		///    the current string. If no characters can be trimmed from the current instance, the
		///    method returns the current instance unchanged.
		/// </returns>
		public static string SafeTrimStart(
			this string value)
			=> value.IsNullOrWhiteSpace()
				? string.Empty
				: value.TrimStart();
	}
}
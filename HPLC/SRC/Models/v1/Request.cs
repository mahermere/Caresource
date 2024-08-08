// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Hplc
//   Request.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Models.v1
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Linq;
	using System.Reflection;
	using CareSource.WC.OnBase.Core.ExtensionMethods;

	/// <summary>
	///    Data and functions describing a CareSource.WC.Services.Hplc.Models.v1.Request object.
	/// </summary>
	public class Request
	{
		private string _source;
		private string _type;

		/// <summary>
		///    Gets or sets the Request CareSource Received Date
		/// </summary>
		public DateTime? CareSourceReceivedDate { get; set; } = DateTime.Now;

		/// <summary>
		///    Gets or sets the Request Change Effective Date
		/// </summary>
		public DateTime? ChangeEffectiveDate { get; set; }

		/// <summary>
		///    Gets or sets the Request Entity Contact Email
		/// </summary>
		[EmailAddress]
		public string ContactEmail { get; set; }

		/// <summary>
		///    Gets or sets the Request Contact Name
		/// </summary>
		public string ContactName { get; set; }

		/// <summary>
		///    Gets or sets the Request Contact Phone
		/// </summary>
		public string ContactPhone { get; set; }

		/// <summary>
		///    Gets or sets the Request Date
		/// </summary>
		public DateTime Date { get; set; } = DateTime.Now;

		/// <summary>
		///    Gets or sets the Request Entity Name
		/// </summary>
		public string EntityName { get; set; }

		/// <summary>
		///    Gets or sets the Request Entity Tin
		/// </summary>
		public string EntityTin { get; set; }

		/// <summary>
		///    Gets or sets the Request Identifier
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		///    Gets or sets the Request Notes
		/// </summary>
		public string Notes { get; set; }

		/// <summary>
		///    Gets or sets the Request Primary State
		/// </summary>
		public string PrimaryState { get; set; }

		/// <summary>
		///    Gets or sets the Request Lines Of Business
		/// </summary>
		public IEnumerable<string> Products { get; set; } = new List<string>();

		/// <summary>
		///    Gets or sets the Request Providers
		/// </summary>
		public IEnumerable<Provider> Providers { get; set; } = new List<Provider>();

		/// <summary>
		/// Gets or sets the Request Status
		/// </summary>
		public string Status { get; internal set; }

		/// <summary>
		///    Gets or sets the Request Signatory Email
		/// </summary>
		[Required]
		[EmailAddress]
		public string SignatoryEmail { get; set; }

		/// <summary>
		///    Gets or sets the Request Signatory Name
		/// </summary>
		[Required]
		public string SignatoryName { get; set; }

		/// <summary>
		///    Gets or sets the Request Signatory Title
		/// </summary>
		[Required]
		public string SignatoryTitle { get; set; }

		/// <summary>
		///    Gets or sets the Request Source
		/// </summary>
		public string Source
		{
			get => _source?.ToUpper();
			set => _source = value?.ToUpper();
		}

		/// <summary>
		///    Gets or sets the Request Type
		/// </summary>
		[Required]
		public string Type
		{
			get => _type?.ToUpper();
			set => _type = value?.ToUpper();
		}
	}

	/// <summary>
	///    Data and functions describing a CareSource.WC.Services.Hplc.Models.v1.ExtensionMethods object.
	/// </summary>
	public static class ExtensionMethods
	{
		/// <summary>
		///    Trims all strings.
		/// </summary>
		/// <typeparam name="TSelf">The type of the self.</typeparam>
		/// <param name="obj">The object.</param>
		public static void TrimAllStrings<TSelf>(this TSelf obj)
		{
			BindingFlags flags = BindingFlags.Instance
				| BindingFlags.Public
				| BindingFlags.NonPublic
				| BindingFlags.FlattenHierarchy;

			foreach (PropertyInfo prop in obj.GetType()
				.GetProperties(flags)
				.Where(p => p.PropertyType == typeof(string) && p.CanWrite))
			{
				string currentValue = (string)prop.GetValue(
					obj,
					null);

				prop.SetValue(
					obj,
					currentValue.SafeTrim(),
					null);
			}
		}
	}
}
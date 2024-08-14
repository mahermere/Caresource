// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   CareSource.WC.Entities
//   Filter.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Entities.Common
{
	public class Filter
	{
		public Filter()
		{ }

		public Filter(string name, string value)
		{
			Name = name;
			Value = value;
		}

		public Filter(string name, string value, CompareOperator compareOperator)
		: this(name, value)
		{
			CompareOperator = compareOperator;
		}
		/// <summary>
		///    Initializes a new instance of the <see cref="Filter" /> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		/// <param name="includeNull">
		///    if set to <c>true</c> the results will include values that
		///    have not been set fopr this filter.
		/// </param>
		public Filter(
			string name,
			string value,
			bool includeNull = false)
			: this(name, value)
		{
			IncludeNull = includeNull;
		}

		/// <summary>
		///    Gets or sets a value indicating whether [include null].
		/// </summary>
		public bool IncludeNull { get; set; }

		/// <summary>
		///    Gets or sets the name of the filter class.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///    Gets or sets the value of the filter class.
		/// </summary>
		public string Value { get; set; }

		/// <summary>
		///    Gets or sets the operator of the filter class.
		/// </summary>
		public CompareOperator CompareOperator { get; set; }
	}
}
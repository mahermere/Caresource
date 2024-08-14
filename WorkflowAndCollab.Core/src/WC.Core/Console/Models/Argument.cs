// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   Argument.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Console.Models
{
	public class Argument
	{
		public Argument(
			string argumentText,
			object value)
		{
			ArgumentText = argumentText;
			Value = value;
		}

		public string ArgumentText { get; set; }

		public object Value { get; set; }

		public bool Processed { get; set; } = false;
	}
}
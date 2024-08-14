// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   CommandArgumentAttribute.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.Console.Models
{
	using System;

	[AttributeUsage(AttributeTargets.Property)]
	public class CommandArgumentAttribute : Attribute
	{
		public CommandArgumentAttribute(
			string argumentName,
			string argumentAbbreviation,
			string description = null)
		{
			ArgumentName = argumentName;
			ArgumentAbbreviation = argumentAbbreviation;
			Description = description;
		}

		public string ArgumentName { get; set; }

		public string ArgumentAbbreviation { get; set; }

		public string Description { get; set; }

		public bool Required { get; set; } = false;
	}
}
﻿// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   WorkViewAttribute.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Models.v2
{
	public class WorkViewNameAttribute
		: System.Attribute
	{
		public WorkViewNameAttribute(string name)
			=> Name = name;

		public string Name { get; }
	}
}
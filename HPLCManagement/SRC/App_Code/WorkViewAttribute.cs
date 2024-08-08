// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   WorkViewAttribute.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace HplcManagement
{
	public class WorkViewNameAttribute
		: System.Attribute
	{
		public WorkViewNameAttribute(string name)
			=> Name = name;

		public string Name { get; }
	}
}
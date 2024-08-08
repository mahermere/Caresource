// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   ImportProcessor
//   WorkviewSettings.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace ImportProcessor.Models.v1.OnBase
{
	public class WorkViewSettings
	{
		public Applications Applications { get; set; }
		public Service Service { get; set; }
	}

	public class WorkView
	{ }

	public class Applications
	{
		public ClaimsPerformanceSolution ClaimsPerformanceSolution { get; set; }
	}

	public class ClaimsPerformanceSolution
	{
		public string Name { get; set; }
		public Classes Classes { get; set; }
	}

	public class Classes
	{
		public Class Claims { get; set; }
		public Class Documents { get; set; }
	}

	public class Class
	{
		public string Name { get; set; }
		public string[] Attributes { get; set; }
	}

	public class Service
	{
		public string BaseUrl { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
	}
}
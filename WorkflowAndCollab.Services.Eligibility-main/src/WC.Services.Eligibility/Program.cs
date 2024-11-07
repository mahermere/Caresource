// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2018. All rights reserved.
// 
//   WorkFlowAndCollab.API.Eligibility
//   Program.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Eligibility
{
	using Microsoft.AspNetCore;
	using Microsoft.AspNetCore.Hosting;

	public class Program
	{
		public static void Main(
			string[] args) => CreateWebHostBuilder(args)
			.Build()
			.Run();

		public static IWebHostBuilder CreateWebHostBuilder(
			string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>();
	}
}
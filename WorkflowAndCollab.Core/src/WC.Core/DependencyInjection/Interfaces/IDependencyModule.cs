// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   IDependencyModule.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.DependencyInjection.Interfaces
{
	public interface IDependencyModule
	{
		/// <summary>
		///    Order that the DependencyModule should be loaded.
		///    1-200   : Core / Foundational Tier Projects
		///    201-500 : Middle Tier Projects
		///    501-900 : Application / User Interface Tier Projects
		///    900+    : Override Tier Projects
		/// </summary>
		ushort LoadOrder { get; }

		void Load(
			DependencyCollection dependencyCollection,
			IEnvironmentContext environmentContext);
	}
}
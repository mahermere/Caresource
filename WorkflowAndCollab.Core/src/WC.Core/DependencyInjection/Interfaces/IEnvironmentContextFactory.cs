// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   IEnvironmentContextFactory.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.DependencyInjection.Interfaces
{
	public interface IEnvironmentContextFactory
	{
		IEnvironmentContext CreateEnvironmentContext();
	}
}
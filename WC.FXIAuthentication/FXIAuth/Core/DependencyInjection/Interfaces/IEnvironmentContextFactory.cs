// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    IEnvironmentContextFactory.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.DependencyInjection;

public interface IEnvironmentContextFactory
{
	IEnvironmentContext CreateEnvironmentContext();
}
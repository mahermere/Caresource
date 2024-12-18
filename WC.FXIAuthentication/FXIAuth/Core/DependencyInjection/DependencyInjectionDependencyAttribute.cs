﻿// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    DependencyInjectionDependencyAttribute.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.DependencyInjection;

/// <summary>
///    Currently Contains No Functionality
///    This is used to mark classes that the DependencyInjection System is dependent on.
///    Classes that the Dependency Injection System is dependent on should be light weight and not
///    require too many dependencies as they need to be newed up in ServiceCollectionExtensions.
///    HINT: No Applogging in these classes!
/// </summary>
public class DependencyInjectionDependencyAttribute : Attribute { }
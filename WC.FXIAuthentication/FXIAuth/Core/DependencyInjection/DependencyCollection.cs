// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    DependencyCollection.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.DependencyInjection;

public class DependencyCollection
{
	internal IServiceCollection ServiceCollection;

	public DependencyCollection(
		IServiceCollection serviceCollection) => ServiceCollection = serviceCollection;

	public void AddDependency<TService>()
		where TService : class => AddSingletonDependency<TService>();

	public void AddDependency<TService, TImplementation>()
		where TService : class
		where TImplementation : class, TService
		=> AddSingletonDependency<TService, TImplementation>();

	public void AddTransientDependency<TService>()
		where TService : class => ServiceCollection.AddTransient<TService>();

	public void AddTransientDependency<TService, TImplementation>()
		where TService : class
		where TImplementation : class, TService
		=> ServiceCollection.AddTransient<TService, TImplementation>();

	public void AddSingletonDependency<TService>()
		where TService : class => ServiceCollection.AddSingleton<TService>();

	public void AddSingletonDependency<TService, TImplementation>()
		where TService : class
		where TImplementation : class, TService
		=> ServiceCollection.AddSingleton<TService, TImplementation>();

	public void AddScopedDependency<TService>()
		where TService : class => ServiceCollection.AddScoped<TService>();

	public void AddScopedDependency<TService, TImplementation>()
		where TService : class
		where TImplementation : class, TService
		=> ServiceCollection.AddScoped<TService, TImplementation>();
}
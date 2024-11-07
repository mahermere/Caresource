// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    FXIAuthentication
//    CoreDependencyModule.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace FXIAuthentication.Core.DependencyInjection;

using FXIAuthentication.Adapters.v1;
using FXIAuthentication.Core.Configuration;
using FXIAuthentication.Core.Helpers;
using FXIAuthentication.Core.Http;
using FXIAuthentication.Core.Http.Swagger;
using FXIAuthentication.Core.Http.Swagger.Interfaces;
using FXIAuthentication.Core.Transaction;
using FXIAuthentication.Core.Transaction.Models;
using FXIAuthentication.Managers.v1;

public class CoreDependencyModule : IDependencyModule
{
	public ushort LoadOrder => 50;

	public void Load(
		DependencyCollection dependencyCollection,
		IEnvironmentContext environmentContext)
	{
		dependencyCollection.AddDependency<IAuthManager, AuthManager>();
		dependencyCollection.AddDependency<IAuthAdapter, AuthAdapter>();

		dependencyCollection.AddDependency<IFileHelper, FileHelper>();
		dependencyCollection.AddDependency<IJsonSerializerHelper, JsonSerializerHelper>();
		dependencyCollection.AddDependency<TransactionContext>();
		dependencyCollection
			.AddDependency<ITransactionContextManager, TransactionContextManager>();

		dependencyCollection
			.AddDependency<IEnvironmentVariableHelper, EnvironmentVariableHelper>();
		dependencyCollection
			.AddDependency<ICareSourceEnvironmentHelper, CareSourceEnvironmentHelper>();
		dependencyCollection.AddDependency<IAssemblyHelper, AssemblyHelper>();
		dependencyCollection.AddDependency<IReflectionHelper, ReflectionHelper>();

		dependencyCollection
			.AddDependency<IEnvironmentContextFactory, EnvironmentContextFactory>();

		dependencyCollection.AddDependency<ISettingsAdapter, AppSettingsAdapter>();

		dependencyCollection
			.AddDependency<ISwaggerConfigurationManager, DefaultSwaggerConfigurationManager>();

		dependencyCollection
			.AddScopedDependency<IEventContextManager, HttpEventContextManager>();
		dependencyCollection
			.AddDependency<IHttpStatusExceptionManager, HttpStatusExceptionManager>();
	}
}
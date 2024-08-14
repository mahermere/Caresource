// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   CoreDependencyModule.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Core.DependencyInjection
{
	using CareSource.WC.Core.Configuration;
	using CareSource.WC.Core.Configuration.Interfaces;
	using CareSource.WC.Core.DependencyInjection.Interfaces;
	using CareSource.WC.Core.Helpers;
	using CareSource.WC.Core.Helpers.Interfaces;
	using CareSource.WC.Core.Http;
	using CareSource.WC.Core.Http.Interfaces;
	using CareSource.WC.Core.Http.Swagger;
	using CareSource.WC.Core.Http.Swagger.Interfaces;
	using CareSource.WC.Core.Transaction;
	using CareSource.WC.Core.Transaction.Interfaces;

	public class CoreDependencyModule : IDependencyModule
	{
		public ushort LoadOrder => 50;

		public void Load(
			DependencyCollection dependencyCollection,
			IEnvironmentContext environmentContext)
		{
			dependencyCollection
				.AddDependency<IEnvironmentVariableHelper, EnvironmentVariableHelper>();
			dependencyCollection
				.AddDependency<ICareSourceEnvironmentHelper, CareSourceEnvironmentHelper>();
			dependencyCollection.AddDependency<IAssemblyHelper, AssemblyHelper>();
			dependencyCollection.AddDependency<IReflectionHelper, ReflectionHelper>();
			dependencyCollection.AddDependency<IFileHelper, FileHelper>();
			dependencyCollection.AddDependency<IJsonSerializerHelper, JsonSerializerHelper>();
			dependencyCollection.AddDependency<IXmlSerializerHelper, XmlSerializerHelper>();

			dependencyCollection.AddDependency<IRestClient, RestClient>();

			dependencyCollection
				.AddDependency<IEnvironmentContextFactory, EnvironmentContextFactory>();

			dependencyCollection.AddDependency<ISettingsAdapter, AppSettingsAdapter>();

			dependencyCollection
				.AddDependency<ISwaggerConfigurationManager, DefaultSwaggerConfigurationManager>();
			dependencyCollection
				.AddDependency<ITransactionContextManager, TransactionContextManager>();
			dependencyCollection
				.AddScopedDependency<IEventContextManager, HttpEventContextManager>();
			dependencyCollection
				.AddDependency<IHttpStatusExceptionManager, HttpStatusExceptionManager>();
		}
	}
}
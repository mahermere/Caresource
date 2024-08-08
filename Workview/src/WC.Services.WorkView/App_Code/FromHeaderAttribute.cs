// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2022. All rights reserved.
// 
//   Workview
//   FromHeaderAttribute.cs
// </copyright>
// ------------------------------------------------------------------------------------------------
namespace CareSource.WC.Services.WorkView
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using System.Web.Http;
	using System.Web.Http.Controllers;
	using System.Web.Http.Metadata;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using CareSource.WC.Services.WorkView.Models.v5;

	public class FromHeaderBinding : HttpParameterBinding
	{
		private readonly string _name;

		public FromHeaderBinding(HttpParameterDescriptor parameter, string headerName)
			: base(parameter)
		{
			if (headerName.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException(nameof(headerName));
			}

			_name = headerName;
		}

		public override Task ExecuteBindingAsync(
			ModelMetadataProvider metadataProvider,
			HttpActionContext actionContext,
			CancellationToken cancellationToken)
		{
			if (actionContext.Request.Headers.TryGetValues(
				_name,
				out IEnumerable<string> values))
			{
				actionContext.ActionArguments[Descriptor.ParameterName] = values.FirstOrDefault();
			}

			TaskCompletionSource<object> taskSource = new TaskCompletionSource<object>();

			taskSource.SetResult(null);
			return taskSource.Task;
		}
	}

	public abstract class FromHeaderAttribute : ParameterBindingAttribute
	{
		private readonly string _name;

		protected FromHeaderAttribute(string headerName)
			=> _name = headerName;

		public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
			=> new FromHeaderBinding(
				parameter,
				_name);
	}

	public class FromHeadersAttribute : FromHeaderAttribute
	{
		public FromHeadersAttribute(string headerName)
			: base(headerName)
		{ }
	}
}
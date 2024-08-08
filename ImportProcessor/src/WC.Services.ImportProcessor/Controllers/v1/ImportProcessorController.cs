// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   ImportProcessor
//   ImportProcessorController.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace ImportProcessor.Controllers.v1
{
	using System;
	using System.Net;
	using System.Web.Http;
	using CareSource.WC.OnBase.Core.Configuration.Interfaces;
	using CareSource.WC.OnBase.Core.Helpers.Interfaces;
	using ImportProcessor.Managers.v1.Interfaces;
	using ImportProcessor.Models.v1;
	using Microsoft.Extensions.Logging;
	using Microsoft.Web.Http;
	using Swashbuckle.Swagger.Annotations;

	//[OnBaseAuthorizeFilter]
	[ApiVersion("1")]
	[RoutePrefix("api/v{version:apiVersion}")]
	[SwaggerResponse(
		HttpStatusCode.Unauthorized,
		"When unable to authenticate using the basic Authentication.",
		typeof(ImportProcessorResponse))]
	public partial class ImportProcessorController : ApiController
	{
		private readonly ILogger _logger;
		private readonly IImportProcessorManager<ImportProcessorResponse> _manager;

		private readonly IJsonSerializerHelper _serializer;
		private readonly ISettingsAdapter _settingsAdapter;
		private readonly Guid correlationGuid = Guid.NewGuid();

		public ImportProcessorController(
			IImportProcessorManager<ImportProcessorResponse> manager,
			ILogger logger,
			IJsonSerializerHelper serializer,
			ISettingsAdapter settingsAdapter)
		{
			_manager = manager;
			_logger = logger;
			_serializer = serializer;
			_settingsAdapter = settingsAdapter;
		}
	}
}
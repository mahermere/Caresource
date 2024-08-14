// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   Core
//   HttpEventContextManager.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Http.Controllers;

namespace CareSource.WC.OnBase.Core.Http
{

    using CareSource.WC.Entities.Transactions;
    using CareSource.WC.OnBase.Core.Diagnostics.Interfaces;
    using CareSource.WC.OnBase.Core.ExtensionMethods;
    using CareSource.WC.OnBase.Core.Helpers.Interfaces;
    using CareSource.WC.OnBase.Core.Transaction.Interfaces;

    public class HttpEventContextManager : IEventContextManager
    {
        private readonly IAssemblyHelper _assemblyHelper;
        private readonly ILogger _logger;

        public HttpEventContextManager(IAssemblyHelper assemblyHelper
            , ILogger logger)
        {
            _assemblyHelper = assemblyHelper;
            _logger = logger;
        }

        public void SetEventContext(
            TransactionContext context,
            params object[] eventArgs)
        {
            HttpActionContext httpContext = eventArgs[0] as HttpActionContext;
            if (httpContext == null)
            {
                throw new Exception(
                    "HttpContext is required to set EventContext on TransactionContext.");
            }

            string source = context.EventContext?.Source;
            _logger.LogDebug("Attempting to pull source from 'X-Source' header.");

            IEnumerable<string> sourceHeaders = new List<string>();
            httpContext.Request?.Headers.TryGetValues("X-Source", out sourceHeaders);
            var sourceHeader = sourceHeaders?.FirstOrDefault();
            if (!sourceHeader.IsNullOrWhiteSpace())
            {
                source = sourceHeader;
            }

            string correlationGuid = context.EventContext?.CorrelationGuid;
            _logger.LogDebug("Attempting to pull correlation guid from 'X-CorrelationGuid' header.");

            IEnumerable<string> correlationGuidHeaders = new List<string>();
            httpContext.Request?.Headers.TryGetValues("X-CorrelationGuid", out correlationGuidHeaders);
            var correlationGuidHeader = correlationGuidHeaders?.FirstOrDefault();
            if (!correlationGuidHeader.IsNullOrWhiteSpace())
            {
                correlationGuid = correlationGuidHeader;
            }

            string version = "1";
            Match versionMatch = Regex.Match(
                httpContext.Request.RequestUri.AbsolutePath, ".*\\/v([0-9]+).*");
            if (versionMatch.Success)
            {
                version = versionMatch.Groups[1]
                    .Value;
            }

            context.EventContext = new EventContext
            {
                CorrelationGuid = correlationGuid ??
                                  Guid.NewGuid().ToString(),
                ParentGuid = context.EventContext?.ParentGuid,
                CurrentGuid = context.EventContext?.CurrentGuid
                              ?? Guid.NewGuid().ToString(),
                Destination = "OnBase",
                Source = source ?? "None Given",
                ApplicationName = _assemblyHelper.GetWebEntryAssembly()?.FullName?
                    .Split(new[] { "," }, StringSplitOptions.None)?[0],
                Event = $"{httpContext.Request.Method.Method} {httpContext.Request.RequestUri.PathAndQuery}",
                Version = version
            };
        }
    }
}
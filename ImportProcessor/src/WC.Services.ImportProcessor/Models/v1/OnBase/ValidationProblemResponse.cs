// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2019. All rights reserved.
// 
//   WC.Services.OnBase
//   ValidationProblemResponse.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

// ReSharper disable once CheckNamespace

namespace ImportProcessor.Models.v1.OnBase
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Web.Http.ModelBinding;
	//using CareSource.WC.Services.Models;
	using Newtonsoft.Json;

	public class ValidationProblemResponse : BaseResponse<ModelStateDictionary>
	{
		private readonly List<Failure> _errors;

		public ValidationProblemResponse(
			Guid correlationGuid,
			ModelStateDictionary result)
			: base(
				"One or more model validation errors occurred.",
				(int)HttpStatusCode.BadRequest,
				correlationGuid)
		{
			_errors = new List<Failure>();

			if (result == null)
			{
				return;
			}

			foreach (KeyValuePair<string, ModelState> m in result)
			{
				if (m.Value.Errors.Any())
				{
					_errors.Add(
						new Failure
						{
							Id = m.Key,
							Messages = m.Value.Errors.Select(e => e.ErrorMessage)
								.ToArray()
						});
				}
			}
		}

		public ValidationProblemResponse(
			Guid correlationGuid)
			: base(
				"One or more model validation errors occurred.",
				(int)HttpStatusCode.BadRequest,
				correlationGuid)
		{ }

		/// <summary>
		///    Gets or sets the Base Response{ T Model} Errors
		/// </summary>
		/// <remarks>
		///    If there are any errors then they will be here.
		/// </remarks>
		[JsonProperty(PropertyName = "errors")]
		public IEnumerable<Failure> Errors => _errors;
	}
}
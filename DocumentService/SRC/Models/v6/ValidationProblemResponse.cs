// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2020. All rights reserved.
// 
//   WC.Services.Document
//   ValidationProblemResponse.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace CareSource.WC.Services.Document.Models.v6
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Web.Http.ModelBinding;
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
		///    If there are any errors then they will be here.  Overriding the base class errors so
		///    the property can be ignored if null
		/// </remarks>
		[JsonProperty(PropertyName = "errors")]
		public IEnumerable<Failure> Errors => _errors;
	}
}
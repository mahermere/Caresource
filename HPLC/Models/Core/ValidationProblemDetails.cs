// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   ValidationProblemDetails.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.Hplc.Models.Core
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Http.ModelBinding;
	using CareSource.WC.OnBase.Core.ExtensionMethods;
	using Newtonsoft.Json;

	/// <summary>
	/// Data and functions describing a ValidationProblemDetails object.
	/// </summary>
	public class ValidationProblemDetails : ProblemDetails
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ValidationProblemDetails"/> class.
		/// </summary>
		public ValidationProblemDetails()
			=> this.Title = "One or more validation errors occurred.";

		/// <summary>
		/// Initializes a new instance of the <see cref="ValidationProblemDetails"/> class.
		/// using the specified modelState.
		/// </summary>
		/// <param name="modelState">ModelStateDictionary containing the validation errors.</param>
		public ValidationProblemDetails(ModelStateDictionary modelState)
		{
			if (modelState == null)
			{
				throw new ArgumentNullException(nameof(modelState));
			}

			foreach( KeyValuePair<string, ModelState> keyValuePair in modelState)
			{
				string key = keyValuePair.Key;
				ModelErrorCollection errors = keyValuePair.Value.Errors;

				if (errors.SafeAny())
				{
					if (errors.Count == 1)
					{
						string errorMessage = GetErrorMessage(errors[0]);
						Errors.Add(key, new string[1]
						{
							errorMessage
						});
					}
					else
					{
						string[] strArray = new string[errors.Count];
						for (int index = 0; index < errors.Count; ++index)
						{
							strArray[index] = GetErrorMessage(errors[index]);
						}

						Errors.Add(key, strArray);
					}
				}
			}

			string GetErrorMessage(ModelError error)
				=> error.ErrorMessage.IsNullOrWhiteSpace()
					? "An unspecified error has occurred."
					: error.ErrorMessage;

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ValidationProblemDetails"/> class using the specified errors.
		/// </summary>
		/// <param name="errors">The validation errors.</param>
		public ValidationProblemDetails(IDictionary<string, string[]> errors)
			=> Errors = errors;

		/// <summary>
		/// Gets the validation errors associated with this instance of ValidationProblemDetails.
		/// </summary>
		/// <value>
		/// The errors.
		/// </value>
		//[JsonProperty("errors")]
		public IDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>();
	}
}

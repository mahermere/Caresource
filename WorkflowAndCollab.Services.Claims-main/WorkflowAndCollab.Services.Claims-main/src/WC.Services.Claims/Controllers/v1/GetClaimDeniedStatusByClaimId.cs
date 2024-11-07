// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    Claims
//    GetClaimDeniedStatusByClaimId.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace Claims.Controllers.v1
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.Net;
	using Claims.OnBase.Utilities;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Logging;

	public partial class ClaimsController
	{
		/// <summary>
		///    Get Claim Denied Status for a given Claim Id from Facets.
		/// </summary>
		/// <param name="claimId">Claim Id</param>
		/// <returns>Returns the denied status for a given claim id.</returns>
		/// <response code="200">Returns the data from the claim record.</response>
		/// <response code="400">If the given Claim Id is not valid.</response>
		/// <response code="401">If the given authorization is not valid for OnBase.</response>
		/// <response code="404">If the given Claim Id is not found.</response>
		/// <response code="500">If there is an uncaught server exception.</response>
		[HttpGet("DeniedStatus/{claimId}")]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
		public IActionResult GetClaimDeniedStatus(
			[Required]
			string claimId)
		{
			try
			{
				if (claimId.IsNullOrWhiteSpace())
				{
					_logger.LogError("Claim Id is required.");
					return BadRequest("Claim Id is required.");
				}

				_logger.LogInformation("Successfully validated request!");

				string ExCode = "000";
				ExCode = _claimsManager.GetClaimDeniedStatus(claimId);

				_logger.LogInformation($"Successfully returned Status for {claimId} = {ExCode}.");

				return Ok(ExCode);
			}
			catch (Exception e)
			{
				_logger.LogError("Unknown Exception for Claim Denied Status.");
				return StatusCode(500, e.Message);
			}
		}
	}
}
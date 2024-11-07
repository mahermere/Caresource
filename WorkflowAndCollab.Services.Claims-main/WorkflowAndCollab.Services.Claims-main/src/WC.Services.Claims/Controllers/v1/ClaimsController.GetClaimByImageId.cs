// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    Claims
//    ClaimsController.GetClaimByImageId.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace Claims.Controllers.v1
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.Net;
	using Claims.Models;
	using Claims.OnBase.Utilities;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Logging;

	public partial class ClaimsController
	{
		/// <summary>
		///    Get Data for a given DCN (MICRO ID) from Facets.
		/// </summary>
		/// <param name="microId">DCN (MICRO ID)</param>
		/// <returns>Member Id, Subscriber Id, Suffix, and Claim Numberfor a given DCN (MICRO ID).</returns>
		/// <response code="200">Returns the data from the claim record.</response>
		/// <response code="400">If the given DCN (MICRO ID) is not valid.</response>
		/// <response code="401">If the given authorization is not valid for OnBase.</response>
		/// <response code="404">If the given DCN (MICRO ID) is not found.</response>
		/// <response code="500">If there is an uncaught server exception.</response>
		[HttpGet("DCN/{microId}")]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
		public IActionResult GetDataByDCN(
			[Required]
			string microId)
		{
			try
			{
				if (microId.IsNullOrWhiteSpace())
				{
					_logger.LogError("Micro Id is required.");
					return BadRequest("Micro Id is required.");
				}

				_logger.LogInformation("Successfully validated request!");

				DCNClaimData dcnClaimData = _claimsManager.GetDataByDCN(microId);

				if (dcnClaimData.ClaimId == null)
				{
					_logger.LogError($"Image Id not found for DCN {microId}.");
					return NotFound($"Image Id not found for DCN {microId}.");
				}

				_logger.LogInformation($"Successfully returned data for DCN {microId}.");

				return Ok(dcnClaimData);
			}
			catch (Exception e)
			{
				_logger.LogError($"Unknown Exception for DCN {microId}.");
				return StatusCode(500, e.Message);
			}
		}
	}
}
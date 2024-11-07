// ------------------------------------------------------------------------------------------------
//  <copyright>
//    Copyright (c) CareSource, 2020-2022.  All rights reserved.
// 
//    Claims
//    ClaimsController.cs
//  </copyright>
//  ------------------------------------------------------------------------------------------------

namespace Claims.Controllers.v1
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Net;
	using Asp.Versioning;
	using Claims.Managers.Interfaces;
	using Claims.Models;
	using Claims.OnBase.Utilities;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Logging;

	[Produces("application/json")]
	[ApiVersion("1")]
	[Route("api/v{version:int:apiVersion}/")]
	[ApiController]
	public partial class ClaimsController : ControllerBase
	{
		private readonly IClaimsManager _claimsManager;
		private readonly ILogger<ClaimsController> _logger;

		public ClaimsController(
			IClaimsManager claimsManager,
			ILogger<ClaimsController> logger)
		{
			_claimsManager = claimsManager;
			_logger = logger;
		}

		/// <summary>
		///    Get a Claim for a given claim id from Facets.
		/// </summary>
		/// <param name="claimId">Facets Claim Id</param>
		/// <returns>Formatted claim for a given claim id.</returns>
		/// <response code="200">Returns the found claim.</response>
		/// <response code="400">If the given Claim Id is not valid.</response>
		/// <response code="401">If the given authorization is not valid for OnBase.</response>
		/// <response code="500">If there is an uncaught server exception.</response>
		[HttpGet("{claimId}")]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
		public IActionResult Get(string claimId)
		{
			if (claimId.IsNullOrWhiteSpace())
			{
				_logger.LogError("Claim Id is required.");
				return BadRequest("Claim Id is required.");
			}

			if (claimId == "DCN")
			{
				_logger.LogError("DCN is required.");
				return BadRequest("DCN is required.");
			}

			if (claimId == "DeniedStatus")
			{
				_logger.LogError("Claim Id is required.");
				return BadRequest("Claim Id is required.");
			}

			

			_logger.LogInformation("Successfully validated request!");

			Claim claim = _claimsManager.GetById(claimId);

			_logger.LogInformation("Successfully created returned claim id.");

			return Ok(claim);
		}

		/// <summary>
		///    Get Claims for a given subscriber id from Facets.
		/// </summary>
		/// <param name="subscriberId">A member subscriber id</param>
		/// <param name="subscriberSuffix">A member subscriber suffix, this is defaulted to 0</param>
		/// <param name="startServiceDate">start date of the service covered by the claim</param>
		/// <param name="endServiceDate">end date of the service covered by the claim</param>
		/// <param name="page">Current page of claims</param>
		/// <param name="pageSize">size for each page returned</param>
		/// <returns>Formatted claim for a given parameters.</returns>
		/// <response code="200">Returns the found claims.</response>
		/// <response code="204">No claims where found.</response>
		/// <response code="400">If the given parameters is not valid.</response>
		/// <response code="401">If the given authorization is not valid for OnBase.</response>
		/// <response code="500">If there is an uncaught server exception.</response>
		[HttpGet("subscriber/{subscriberId}")]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
		public IActionResult Get(
			[Required] string subscriberId,
			[FromQuery] int? subscriberSuffix,
			[FromQuery] [Required] DateTime? startServiceDate,
			[FromQuery] [Required] DateTime? endServiceDate,
			[FromQuery] int? page,
			[FromQuery] int? pageSize)
		{
			if (subscriberId.Length != 9)
			{
				return BadRequest("Subscriber Id must be 9 digits to be valid.");
			}

			if (!subscriberSuffix.HasValue)
			{
				subscriberSuffix = 0;
			}

			_logger.LogInformation("Successfully validated request!");

			List<Claim> claims = _claimsManager.GetByFilter(
				page,
				pageSize,
				new Claim
				{
					SubscriberId = subscriberId,
					SubscriberSuffix = subscriberSuffix,
					EarliestDateOfService = startServiceDate,
					LatestDateOfService = endServiceDate
				});

			_logger.LogInformation("Successfully created returned claim id.");

			if ((claims?.Count ?? 0) < 1)
			{
				return NoContent();
			}

			return Ok(
				new
				{
					Page = page,
					PageSize = pageSize,
					TotalResults = claims?.Count ?? 0,
					Results = claims
				});
		}
	}
}
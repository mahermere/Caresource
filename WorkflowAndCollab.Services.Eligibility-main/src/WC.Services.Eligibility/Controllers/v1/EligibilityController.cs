using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CareSource.WC.Entities.Members;
using Microsoft.AspNetCore.Mvc;
using CareSource.WC.Services.Eligibility.Managers.Interfaces;

namespace CareSource.WC.Services.Eligibility.Controllers.v1
{
    using CareSource.WC.Entities.Eligibility;
    using System.Net;

    [Produces("application/json")]
    [ApiVersion("1")]
    [Route("api/v{version:int:apiVersion}/")]
    [ApiController]
    public class EligibilityController : ControllerBase
	{
        private readonly IMemberManager<Member> _memberManager;
        private readonly IEligibilityManager<Eligibility> _eligibilityManager;

        public EligibilityController(
            IMemberManager<Member> memberManager,
			IEligibilityManager<Eligibility> eligibilityManager)
		{
            _memberManager = memberManager;
            _eligibilityManager = eligibilityManager;
		}

        /// <summary>
        /// Get a members eligibility from Facets.
        /// </summary>
        /// <param name="memberId">Member Id to use for searching for eligibility.</param>
        /// <returns>Member eligibility</returns>
        /// <response code="200">Successfully found member's eligibility</response>
        /// <response code="400">If the given request is not valid.</response>
        /// <response code="401">If the given authorization is not valid for OnBase.</response>
        /// <response code="500">If there is an uncaught server exception.</response>
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [Route("{memberId}")]
        [HttpGet]
        public IActionResult Get([Required]string memberId)
		{
            if (!_memberManager.ValidateMemberId(memberId, ModelState))
            {
                return BadRequest(ModelState);
            }

			var eligibilities = _eligibilityManager.GetEligibilities(memberId);

			if (eligibilities == null)
			{
				throw new ArgumentException($"No Eligibility found for id: {memberId}");
			}

			return Ok(eligibilities);
		}
	}
}
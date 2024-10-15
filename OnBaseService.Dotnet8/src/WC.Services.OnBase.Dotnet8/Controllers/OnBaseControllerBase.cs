using CareSource.WC.Entities.Requests.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using WC.Services.OnBase.Dotnet8.Models;
using WC.Services.OnBase.Dotnet8.OnBase;

namespace WC.Services.OnBase.Dotnet8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OnBaseControllerBase : ControllerBase
    {
        private readonly log4net.ILog _logger;

        public OnBaseControllerBase(log4net.ILog logger)
        {
            _logger = logger;
        }

        protected IActionResult HandleUnknownErrors(
            Exception exception)
        {
            _logger.Error(
                exception.Message,
                exception);

            return Content(
                HttpStatusCode.InternalServerError.ToString(),
                new BaseResponse<string>(
                    "That is weird, I tried to complete your request; Please see my logs for what " +
                    "really happened.",
                    (int)HttpStatusCode.InternalServerError,
                    null
                ).ToString());
        }

        protected IActionResult HandleValidationErrors(
            BaseRequest request,
            ValidationException exception)
        {
            _logger.Error(
                "Bad Request",
                exception);

           
            return BadRequest(new ValidationProblemDetails(ModelState)
            {
                Instance = request.CorrelationGuid.ToString()
            });
        }
    }
}

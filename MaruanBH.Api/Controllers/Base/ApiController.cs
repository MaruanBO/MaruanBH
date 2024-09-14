using MediatR;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace MaruanBH.Api.Controllers

{
    [Route("[controller]")]
    public class ApiController : Controller
    {
        public ApiController(IMediator mediator, ILogger<ApiController> logger)
        {
            Mediator = mediator;
            Logger = logger;
        }

        protected IMediator Mediator { get; }
        protected ILogger<ApiController> Logger { get; }

        protected IActionResult? ValidateRequest<T>(T request, IValidator<T> validator)
        {
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            return null;
        }
    }
}
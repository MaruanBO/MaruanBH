using Microsoft.AspNetCore.Mvc;
using MaruanBH.Core.CustomerContext.Queries;
using MaruanBH.Core.CustomerContext.Commands;
using MediatR;
using FluentValidation;
using MaruanBH.Domain.Base.Error;
using MaruanBH.Core.Base.Exceptions;
using MaruanBH.Core.CustomerContext.DTOs;
using CSharpFunctionalExtensions;

namespace MaruanBH.Api.Controllers
{
    [ApiController]
    [Route("api/customer")]
    public class CustomerController : ApiController
    {
        private readonly IValidator<GetCustomerDetailsQuery> _getCustomerDetailsQueryValidator;
        private readonly IValidator<CreateCustomerDto> _createCustomerValidator;

        public CustomerController(IMediator mediator, IValidator<GetCustomerDetailsQuery> validator, ILogger<CustomerController> logger, IValidator<CreateCustomerDto> createCustomerValidator) : base(mediator, logger)
        {
            _getCustomerDetailsQueryValidator = validator;
            _createCustomerValidator = createCustomerValidator;
        }

        /// <summary>
        /// Creates a new customer.
        /// </summary>
        /// <param name="dto">The customer data to create.</param>
        /// <returns>The unique identifier of the newly created customer.</returns>
        /// <response code="201">Returns the Guid of the created customer.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpPost("create")]
        [ProducesResponseType(typeof(CreateCustomerResponseDto), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerDto dto)
        {
            var validationResult = _createCustomerValidator.Validate(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            try
            {
                Logger.LogInformation("Processing CreateCustomer request for customer with name {Name}", dto.Name);
                var command = new CreateCustomerCommand(dto);
                var customerId = await Mediator.Send(command);
                return Ok(new CreateCustomerResponseDto { CustomerId = customerId });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An error occurred while creating the customer.");
                return StatusCode(500, new { message = "An error occurred while creating the customer." });
            }
        }

        /// <summary>
        /// Retrieves the details of a customer by their ID.
        /// This endpoint is primarily for developer experience (DX) purposes. 
        /// For retrieving customer information including Name, Surname, Balance, and transactions of the accounts, 
        /// please refer to the account/details/{id} endpoint in the AccountController for comprehensive account details.
        /// </summary>
        /// <param name="id">The unique identifier of the customer.</param>
        /// <returns>Customer details including Name, Surname, Balance, and transactions of the accounts.</returns>
        /// <response code="200">Returns customer details successfully.</response>
        /// <response code="404">If the customer with the given ID is not found.</response>
        /// <response code="500">If an internal server error occurs.</response>
        /// <response code="400">If the request is invalid.</response>
        [HttpGet("details/{id}")]
        [ProducesResponseType(typeof(CustomerDetailDto), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [ProducesResponseType(typeof(string[]), 400)]
        public async Task<IActionResult> GetCustomerDetails(Guid id)
        {
            var query = new GetCustomerDetailsQuery(id);

            var validationResult = ValidateRequest(query, _getCustomerDetailsQueryValidator);
            if (validationResult != null)
            {
                return validationResult;
            }

            try
            {
                Logger.LogInformation("Processing GetCustomerDetails query for customer ID {Id}", id);
                Result<CustomerDetailDto, Error> result = await Mediator.Send(query);
                if (result.IsSuccess)
                {
                    return Ok(result.Value);
                }
                else
                {
                    // not all code paths return a value
                    Logger.LogWarning("Customer not found for ID {Id}", id);
                    return NotFound(new { message = result.Error.ToString() });
                }
            }
            catch (CustomException ex)
            {
                Logger.LogWarning(ex, "Customer not found for ID {Id}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An error occurred while processing the request for customer ID {Id}", id);
                return StatusCode(500, new { message = "An error occurred while processing the request." });
            }
        }
    }
}

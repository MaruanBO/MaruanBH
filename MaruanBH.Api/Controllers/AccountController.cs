using MediatR;
using Microsoft.AspNetCore.Mvc;
using MaruanBH.Core.AccountContext.Queries;
using MaruanBH.Core.Base.Exceptions;
using MaruanBH.Core.AccountContext.Commands;
using FluentValidation;
using MaruanBH.Core.AccountContext.DTOs;
using MaruanBH.Domain.Base.Error;

namespace MaruanBH.Api.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ApiController
    {

        private readonly IValidator<CreateAccountDto> _createAccountValidator;

        public AccountController(IMediator mediator, ILogger<AccountController> logger, IValidator<CreateAccountDto> validator) : base(mediator, logger)
        {
            _createAccountValidator = validator;
        }

        /// <summary>
        /// Creates a new account and associates it with an existing customer. 
        /// </summary>
        /// <remarks>
        /// This method handles the creation of a new account linked to a customer identified by their customerID. 
        /// The API exposes an endpoint that accepts the customerID and initialCredit as input parameters. 
        /// 
        /// If the initialCredit is greater than 0, a transaction will be automatically created and associated with the newly created account. 
        /// 
        /// Workflow:
        /// 1. A new account is created for the customer.
        /// 2. If the initialCredit is greater than 0, we add a new transaction for the account.
        /// </remarks>
        /// <param name="dto">The customer data to create.</param>
        /// <returns>The unique identifier of the newly created customer.</returns>
        /// <response code="201">Returns the Guid of the created customer.</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateAccountResponseDto))]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [ProducesResponseType(typeof(string[]), 400)]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDto dto)
        {

            var validationResult = _createAccountValidator.Validate(dto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            try
            {
                Logger.LogInformation("Processing CreateAccount");
                var command = new CreateAccountCommand(dto);
                var id = await Mediator.Send(command);
                return Ok(new CreateAccountResponseDto { AccountId = id });
            }
            catch (CustomException ex)
            {
                Logger.LogWarning(ex, "Something went wrong while creating the account");
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An error occurred while processing the request to create the user");
                return StatusCode(500, new { message = "An error occurred while processing the request." });
            }
        }

        /// <summary>
        /// Retrieves the details of an account based on the provided account ID.
        /// </summary>
        /// <remarks>
        /// This method fetches and returns the account details, including the associated customer's information, account balance, and transaction history.
        /// 
        /// The output will display:
        /// - Customer's Name and Surname
        /// - Account Balance
        /// - List of Transactions for the account
        /// </remarks>
        /// <param name="id">The unique identifier of the account.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains an <see cref="IActionResult"/> that represents the result of the operation. 
        /// If the account is found, it returns an <see cref="OkObjectResult"/> with the account details. 
        /// If the account is not found, it returns a <see cref="NotFoundObjectResult"/> with an error message. 
        /// If an unexpected error occurs, it returns a <see cref="StatusCodeResult"/> with a 500 status code and an error message.
        /// </returns>
        /// <response code="200">Returns the details of the account if found.</response>
        /// <response code="404">If the account is not found.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [ProducesResponseType(typeof(AccountDetailsDto), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [ProducesResponseType(typeof(string[]), 400)]
        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetAccountDetails(Guid id)
        {
            try
            {
                var accountDetails = await Mediator.Send(new GetAccountDetailsQuery(id));
                return Ok(accountDetails);
            }
            catch (CustomException ex)
            {
                Logger.LogWarning(ex, "Something went wrong while getting the account details for customer ID {Id}", id);
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

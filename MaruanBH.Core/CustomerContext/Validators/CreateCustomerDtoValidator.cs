using FluentValidation;
using MaruanBH.Core.CustomerContext.DTOs;

namespace MaruanBH.Core.CustomerContext.Validators
{
    public class CreateCustomerDtoValidator : AbstractValidator<CreateCustomerDto>
    {
        public CreateCustomerDtoValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(c => c.Surname).NotEmpty().WithMessage("Surname is required.");
            RuleFor(c => c.Balance).GreaterThanOrEqualTo(0).WithMessage("Balance must be a positive value.");
        }
    }
}

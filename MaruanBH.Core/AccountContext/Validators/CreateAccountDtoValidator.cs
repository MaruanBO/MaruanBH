using FluentValidation;
using MaruanBH.Core.AccountContext.DTOs;

namespace MaruanBH.Core.AccountContext.Validators

{
    public class CreateAccountDtoValidator : AbstractValidator<CreateAccountDto>
    {
        public CreateAccountDtoValidator()
        {
            RuleFor(c => c.CustomerId).NotEmpty().WithMessage("CustomerId is required.");
            RuleFor(c => c.InitialCredit).GreaterThanOrEqualTo(0).WithMessage("InitialCredit must be greater than or equal to 0.");
        }
    }
}

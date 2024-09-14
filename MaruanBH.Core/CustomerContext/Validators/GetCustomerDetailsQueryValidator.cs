using FluentValidation;
using MaruanBH.Core.CustomerContext.Queries;

namespace MaruanBH.Core.CustomerContext.Validators
{
    public class GetCustomerDetailsQueryValidator : AbstractValidator<GetCustomerDetailsQuery>
    {
        public GetCustomerDetailsQueryValidator()
        {
            RuleFor(query => query.Id)
                .NotNull().WithMessage("Customer ID must be provided.")
                .NotEmpty().WithMessage("Customer ID must not be an empty GUID.");
        }
    }
}

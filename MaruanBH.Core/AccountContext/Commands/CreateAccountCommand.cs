using MediatR;
using MaruanBH.Core.AccountContext.DTOs;

namespace MaruanBH.Core.AccountContext.Commands
{
    public class CreateAccountCommand : IRequest<Guid>
    {
        public CreateAccountDto AccountDto { get; }

        public CreateAccountCommand(CreateAccountDto accountDto)
        {
            AccountDto = accountDto;
        }
    }
}

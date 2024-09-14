
using MediatR;
using MaruanBH.Core.CustomerContext.DTOs;

namespace MaruanBH.Core.CustomerContext.Commands
{
    public class CreateCustomerCommand : IRequest<Guid>
    {
        public CreateCustomerDto CustomerDto { get; }

        public CreateCustomerCommand(CreateCustomerDto customerDto)
        {
            CustomerDto = customerDto;
        }
    }
}
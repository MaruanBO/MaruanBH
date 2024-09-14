using MediatR;
using MaruanBH.Core.AccountContext.DTOs;

namespace MaruanBH.Core.AccountContext.Queries
{
    public class GetAccountDetailsQuery : IRequest<AccountDetailsDto>
    {
        public Guid AccountId { get; }

        public GetAccountDetailsQuery(Guid accountId)
        {
            AccountId = accountId;
        }
    }
}

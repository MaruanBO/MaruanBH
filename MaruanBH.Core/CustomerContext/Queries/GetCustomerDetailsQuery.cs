using System;
using MediatR;
using MaruanBH.Core.CustomerContext.DTOs;
using CSharpFunctionalExtensions;
using MaruanBH.Domain.Base.Error;

namespace MaruanBH.Core.CustomerContext.Queries
{
    public class GetCustomerDetailsQuery : IRequest<Result<CustomerDetailDto, Error>>
    {
        public Guid Id { get; }

        public GetCustomerDetailsQuery(Guid id)
        {
            Id = id;
        }
    }
}

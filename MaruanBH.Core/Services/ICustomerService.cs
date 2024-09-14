using MaruanBH.Domain.Entities;
using MaruanBH.Core.AccountContext.DTOs;
using CSharpFunctionalExtensions;
using MaruanBH.Domain.Base.Error;

namespace MaruanBH.Core.Services
{
    public interface ICustomerService
    {
        Task<Result<Customer, Error>> GetCustomerByIdAsync(Guid id);
        Task<Result<Guid>> CreateCustomerAsync(Customer customer);
    }
}

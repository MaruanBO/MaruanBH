using MaruanBH.Domain.Entities;
using CSharpFunctionalExtensions;

namespace MaruanBH.Domain.Repositories
{
    public interface ICustomerRepository
    {
        Task<Maybe<Customer>> GetCustomerByIdAsync(Guid id);
        Task<Result> AddAsync(Customer customer);
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using MaruanBH.Domain.Entities;
using MaruanBH.Domain.Repositories;
using CSharpFunctionalExtensions;

namespace MaruanBH.Persistance.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IMemoryCache _cache;
        private const string CustomerCacheKey = "Customers";

        public CustomerRepository(IMemoryCache cache)
        {
            _cache = cache;
            InitializeCache();
        }

        public Task<Maybe<Customer>> GetCustomerByIdAsync(Guid id) =>
            Task.FromResult(
                Maybe<Dictionary<Guid, Customer>>
                    .From(_cache.Get<Dictionary<Guid, Customer>>(CustomerCacheKey))
                    .Bind(customers => customers.TryGetValue(id, out var customer)
                        ? Maybe<Customer>.From(customer)
                        : Maybe<Customer>.None)
            );

        public Task<Result> AddAsync(Customer customer) =>
            Task.FromResult(
                Result.Success()
                    .Tap(() =>
                    {
                        var customers = _cache.Get<Dictionary<Guid, Customer>>(CustomerCacheKey)
                            ?? new Dictionary<Guid, Customer>();
                        customers[customer.Id] = customer;
                        _cache.Set(CustomerCacheKey, customers);
                    })
            );

        private void InitializeCache() =>
            _cache.GetOrCreate(CustomerCacheKey, _ => new Dictionary<Guid, Customer>());
    }
}

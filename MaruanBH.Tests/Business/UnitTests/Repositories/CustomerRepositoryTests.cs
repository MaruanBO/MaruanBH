using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MaruanBH.Domain.Entities;
using MaruanBH.Persistance.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace MaruanBH.Tests.Repositories
{
    public class CustomerRepositoryTests
    {
        private readonly Mock<IMemoryCache> _mockCache;
        private readonly Mock<ICacheEntry> _mockCacheEntry;
        private readonly string _customerCacheKey = "Customers";

        public CustomerRepositoryTests()
        {
            _mockCache = new Mock<IMemoryCache>();
            _mockCacheEntry = new Mock<ICacheEntry>();

            _mockCache
                .Setup(m => m.CreateEntry(It.IsAny<object>()))
                .Returns(_mockCacheEntry.Object);
        }

        [Fact]
        public void Constructor_InitializesCache()
        {
            object? value = null;
            _mockCache.Setup(m => m.TryGetValue(It.IsAny<object>(), out value)).Returns(false);

            var repository = new CustomerRepository(_mockCache.Object);

            _mockCache.Verify(m => m.CreateEntry(_customerCacheKey), Times.Once);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ReturnsCustomer_WhenCustomerExists()
        {
            var customerId = Guid.NewGuid();
            var customer = new Customer("Marouane", " Boukhriss Ouchab", 0);
            var customers = new Dictionary<Guid, Customer> { { customerId, customer } };
            object? outValue = customers;

            _mockCache.Setup(m => m.TryGetValue(_customerCacheKey, out outValue)).Returns(true);

            var repository = new CustomerRepository(_mockCache.Object);

            var result = await repository.GetCustomerByIdAsync(customerId);

            Assert.True(result.HasValue);
            Assert.Equal(customer, result.Value);
        }

        [Fact]
        public async Task AddAsync_AddsCustomerToCache()
        {
            var customer = new Customer("Marouane ", "Boukhriss Ouchab", 0);
            var customers = new Dictionary<Guid, Customer>();
            object? outValue = customers;

            _mockCache.Setup(m => m.TryGetValue(_customerCacheKey, out outValue)).Returns(true);
            _mockCache.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(_mockCacheEntry.Object);

            var repository = new CustomerRepository(_mockCache.Object);

            await repository.AddAsync(customer);

            _mockCacheEntry.VerifySet(m => m.Value = It.Is<Dictionary<Guid, Customer>>(d => d.ContainsKey(customer.Id)), Times.Once);
        }
    }
}

using System;

namespace MaruanBH.Domain.Entities
{

    /// <summary>
    /// Immutable entity with readonly properties
    /// </summary>
    public class Account
    {
        public Guid Id { get; init; }
        public Guid CustomerId { get; init; } // foreign key
        public decimal InitialCredit { get; init; }

        public Account(Guid customerId, decimal initialCredit)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
            InitialCredit = initialCredit;
        }
    }
}

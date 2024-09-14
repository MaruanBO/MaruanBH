namespace MaruanBH.Domain.Entities
{

    /// <summary>
    /// Immutable entity with readonly properties
    /// </summary>
    public class Account
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; } // foreign key
        public decimal InitialCredit { get; set; }

        public Account(Guid customerId, decimal initialCredit)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
            InitialCredit = initialCredit;
        }
    }
}

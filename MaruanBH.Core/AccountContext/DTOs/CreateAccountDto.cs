namespace MaruanBH.Core.AccountContext.DTOs
{
    public class CreateAccountDto
    {
        public Guid CustomerId { get; set; }
        public decimal InitialCredit { get; set; }
    }
}
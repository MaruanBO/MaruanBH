namespace MaruanBH.Core.CustomerContext.DTOs
{
    public class CreateCustomerDto
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public decimal Balance { get; set; }
    }
}

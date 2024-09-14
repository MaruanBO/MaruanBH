namespace MaruanBH.Domain.Base.Error
{
    public class ErrorResponse
    {
        public string Type { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public IReadOnlyList<string> Messages { get; set; } = new List<string>();
    }
}
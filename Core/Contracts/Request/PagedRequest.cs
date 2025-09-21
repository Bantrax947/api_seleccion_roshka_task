namespace Core.Contracts.Request
{
    public class PagedRequest
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
        public string? Order { get; set; } = "asc";
    }
}
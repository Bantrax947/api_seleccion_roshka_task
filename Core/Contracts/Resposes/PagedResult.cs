namespace Core.Contracts.Resposes
{
    public class PagedResult<T>
    {
        public List<T> Data { get; set; }

        public Meta Meta { get; set; }
    }

    public class Meta
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
    }
}
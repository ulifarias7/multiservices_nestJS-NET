namespace Document.API.Common.filters
{
    public class BaseFilter
    {
        public List<int>? IdList { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? OrderBy { get; set; }
        public bool OrderDescending { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public BaseFilter()
        {
            Page = 1;
            PageSize = 10;
        }
    }
}

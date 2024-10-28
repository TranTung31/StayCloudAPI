namespace StayCloudAPI.Application.DTOs
{
    public abstract class PagedResultBase
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages 
        {
            get
            {
                if (TotalCount == 0) return 0;
                var totalPages = (double)TotalCount / PageSize;
                return (int)Math.Ceiling(totalPages);
            }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException();
                TotalPages = value;
            }
        }
    }
}

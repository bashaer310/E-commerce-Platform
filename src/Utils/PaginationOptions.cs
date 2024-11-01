
namespace Backend_Teamwork.src.Utils
{
    public class PaginationOptions
    {
        // Pagination
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;

        // Search
        public string? Search { get; set; } = null;

        // Sort ("", "name_desc", "date_desc", "date", "price_desc", "price")
        public string? SortOrder { get; set; } = null; 

        // Price range
        public decimal? LowPrice { get; set; } = 0;
        public decimal? HighPrice { get; set; } = 10000;

        //  Availability
        public bool? IsAvailable { get; set; } = false;


    }
}

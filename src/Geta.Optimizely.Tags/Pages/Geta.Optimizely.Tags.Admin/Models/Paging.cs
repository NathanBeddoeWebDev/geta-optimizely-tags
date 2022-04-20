using Microsoft.AspNetCore.Mvc;

namespace Geta.Optimizely.Tags.Pages.Geta.Optimizely.Tags.Admin.Models
{
    public class Paging
    {
        public const int DefaultPageSize = 50;

        [FromQuery(Name = "page")]
        public int PageNumber { get; set; } = 1;

        [FromQuery(Name = "page-size")]
        public int PageSize { get; set; } = DefaultPageSize;
    }
}

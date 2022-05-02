using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Geta.Optimizely.Tags.Pages.Geta.Optimizely.Tags.Components.Pager
{
    public class PagerViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IPagedList items)
        {
            return View(new PagerViewModel
            {
                HasPreviousPage = items.HasPreviousPage,
                HasNextPage = items.HasNextPage,
                PageNumber = items.PageNumber,
                PageCount = items.PageCount,
                QueryString = HttpContext.Request.QueryString.ToString() ?? string.Empty
            });
        }
    }
}

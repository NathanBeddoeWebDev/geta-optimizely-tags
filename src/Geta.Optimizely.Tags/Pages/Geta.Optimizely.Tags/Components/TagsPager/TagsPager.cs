using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Geta.Optimizely.Tags.Pages.Geta.Optimizely.Tags.Components.TagsPager
{
    public class TagsPager : ViewComponent
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public TagsPager(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public IViewComponentResult Invoke(IPagedList items)
        {
            var context = _contextAccessor.HttpContext;
            return View(new TagsPagerViewModel
            {
                HasPreviousPage = items.HasPreviousPage,
                HasNextPage = items.HasNextPage,
                PageNumber = items.PageNumber,
                PageCount = items.PageCount,
                QueryString = context.Request.QueryString.ToString()
            });
        }
    }
}

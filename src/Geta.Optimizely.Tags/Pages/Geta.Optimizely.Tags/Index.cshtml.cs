using System.Collections.Generic;
using System.Linq;
using Geta.Optimizely.Tags.Core;
using Geta.Optimizely.Tags.Pages.Geta.Optimizely.Tags.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using X.PagedList;

namespace Geta.Optimizely.Tags.Pages.Geta.Optimizely.Tags
{
    public class Index : PageModel
    {
        private readonly ITagRepository _tagRepository;

        public Index(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public IPagedList<Tag> Items { get; set; } = Enumerable.Empty<Tag>().ToPagedList();

        [BindProperty]
        public TagEditModel Tag { get; set; }

        [BindProperty(SupportsGet = true)]
        public Paging Paging { get; set; }

        [BindProperty(SupportsGet = true, Name = "q")]
        public string Query { get; set; }

        public bool HasQuery => !string.IsNullOrEmpty(Query);

        public void OnGet()
        {
            Load();
        }

        private void Load()
        {
            var items = FindTags().ToPagedList(Paging.PageNumber, Paging.PageSize);
            Items = items;
        }

        private IEnumerable<Tag> FindTags()
        {
            return _tagRepository.GetAllTags().ToList();
        }
    }
}

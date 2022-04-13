using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.Web;
using Geta.Optimizely.Tags.Sample.Models.Pages;
using Microsoft.AspNetCore.Html;

namespace Geta.Optimizely.Tags.Sample.Models.ViewModels
{
    public class ContactBlockModel
    {
        [UIHint(UIHint.Image)]
        public ContentReference Image { get; set; }
        public string Heading { get; set; }
        public string LinkText { get; set; }
        public IHtmlContent LinkUrl { get; set; }
        public bool ShowLink { get; set; }
        public ContactPage ContactPage { get; set; }
    }
}
using System;
using EPiServer;
using EPiServer.Core;

namespace Geta.Optimizely.Tags.Core.Events
{
    public class TagsInitializer
    {
        private readonly IContentEvents _contentEvents;
        private readonly Func<ITagService> _tagServiceFactory;

        public TagsInitializer(IContentEvents contentEvents, Func<ITagService> tagServiceFactory)
        {
            _contentEvents = contentEvents;
            _tagServiceFactory = tagServiceFactory;
        }

        public void Initialize()
        {
            _contentEvents.PublishedContent += OnPublishedContent;
        }

        private void OnPublishedContent(object sender, ContentEventArgs e)
        {
            var tagService = _tagServiceFactory();
            tagService.UpdateContentTags(e.Content);
        }
    }
}

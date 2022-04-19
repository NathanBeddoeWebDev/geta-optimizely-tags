// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using EPiServer;
using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Geta.Optimizely.Tags.Core;

namespace Geta.Optimizely.Tags
{
    [ModuleDependency(typeof(ServiceContainerInitialization))]
    public class TagsModule : IInitializableModule
    {
        private ITagService _tagService;
        private IContentEvents _contentEvents;

        private void OnPublishedContent(object sender, ContentEventArgs e)
        {
            _tagService.UpdateContentTags(e.Content);
        }

        public void Initialize(InitializationEngine context)
        {
            _tagService = ServiceLocator.Current.GetInstance<ITagService>();
            _contentEvents = ServiceLocator.Current.GetInstance<IContentEvents>();

            _contentEvents.PublishedContent += OnPublishedContent;
        }

        public void Uninitialize(InitializationEngine context)
        {
            _contentEvents.PublishedContent -= OnPublishedContent;
        }
    }
}

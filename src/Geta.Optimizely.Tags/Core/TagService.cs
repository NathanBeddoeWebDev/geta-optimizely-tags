// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EPiServer.Cms.Shell;
using EPiServer.Core;
using EPiServer.Data;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Geta.Optimizely.Tags.Core.Attributes;

namespace Geta.Optimizely.Tags.Core
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;
        private readonly IContentTypeRepository _contentTypeRepository;

        public TagService(ITagRepository tagRepository, IContentTypeRepository contentTypeRepository)
        {
            _tagRepository = tagRepository;
            _contentTypeRepository = contentTypeRepository;
        }

        public Tag GetTagById(Identity id)
        {
            return _tagRepository.GetTagById(id);
        }

        public IEnumerable<Tag> GetTagsByContent(Guid contentGuid)
        {
            return _tagRepository.GetTagsByContent(contentGuid);
        }

        public Tag GetTagByNameAndGroup(string name, string groupKey)
        {
            return _tagRepository.GetTagByNameAndGroup(name, groupKey);
        }

        public IEnumerable<Tag> GetTagsByName(string name)
        {
            return _tagRepository.GetTagsByName(name);
        }

        public IQueryable<Tag> GetAllTags()
        {
            return _tagRepository.GetAllTags();
        }

        public Identity Save(Tag tag)
        {
            return _tagRepository.Save(tag);
        }

        public Tag Save(Guid contentGuid, string name, string groupKey = "")
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            var tag = GetTagByNameAndGroup(name, groupKey)
                      ?? new Tag
                      {
                          Name = name,
                          GroupKey = groupKey
                      };

            if (tag.PermanentLinks == null)
            {
                tag.PermanentLinks = new List<Guid> { contentGuid };
            }
            else
            {
                if (!tag.PermanentLinks.Contains(contentGuid))
                {
                    tag.PermanentLinks.Add(contentGuid);
                }
            }

            Save(tag);

            return tag;
        }

        public void Save(Guid contentGuid, IEnumerable<string> names, string groupKey)
        {
            foreach (var name in names)
            {
                Save(contentGuid, name, groupKey);
            }
        }

        public void Delete(string name)
        {
            var tags = _tagRepository.GetTagsByName(name);
            foreach (var tag in tags)
            {
                _tagRepository.Delete(tag);
            }
        }

        public void Delete(Identity id)
        {
            var tag = _tagRepository.GetTagById(id);

            if (tag == null)
            {
                return;
            }

            _tagRepository.Delete(tag);
        }

        public void UpdateContentTags(IContent content)
        {
            RemoveOldContentTags(content);
            AddContentTags(content);
        }

        private void AddContentTags(IContent content)
        {
            var contentType = _contentTypeRepository.Load(content.ContentTypeID);

            var tagProperties = contentType.PropertyDefinitions.Where(p => p.TemplateHint == "Tags").ToArray();

            if (!tagProperties.Any())
            {
                return;
            }

            foreach (var tagProperty in tagProperties)
            {
                var tagPropertyInfo = contentType.ModelType.GetProperty(tagProperty.Name);
                var tags = GetPropertyTags(content as ContentData, tagProperty);

                if (tagPropertyInfo == null)
                {
                    return;
                }

                var groupKeyAttribute =
                    tagPropertyInfo.GetCustomAttribute(typeof(TagsGroupKeyAttribute)) as TagsGroupKeyAttribute;
                var cultureSpecificAttribute
                    = tagPropertyInfo.GetCustomAttribute(typeof(CultureSpecificAttribute)) as CultureSpecificAttribute;

                var groupKey = TagsHelper.GetGroupKeyFromAttributes(groupKeyAttribute, cultureSpecificAttribute, content);

                Save(content.ContentGuid, tags, groupKey);
            }
        }

        private void RemoveOldContentTags(IContent content)
        {
            var contentGuid = content.ContentGuid;
            var oldTags = GetTagsByContent(contentGuid);
            var language = content.LanguageBranch();

            foreach (var tag in oldTags)
            {
                if (tag.PermanentLinks == null || !tag.PermanentLinks.Contains(contentGuid) || !tag.GroupKey.EndsWith(language))
                {
                    continue;
                }

                tag.PermanentLinks.Remove(contentGuid);

                Save(tag);
            }
        }

        private static IEnumerable<string> GetPropertyTags(ContentData content, PropertyDefinition propertyDefinition)
        {
            var tagNames = content[propertyDefinition.Name] as string;
            return tagNames?.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries) ?? Enumerable.Empty<string>();
        }
    }
}

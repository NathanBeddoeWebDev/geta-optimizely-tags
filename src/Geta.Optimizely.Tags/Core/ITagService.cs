// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Core;
using EPiServer.Data;

namespace Geta.Optimizely.Tags.Core
{
    public interface ITagService
    {
        Tag GetTagById(Identity id);
        IEnumerable<Tag> GetTagsByContent(Guid contentGuid);
        Tag GetTagByNameAndGroup(string name, string groupKey);
        IEnumerable<Tag> GetTagsByName(string name);
        IQueryable<Tag> GetAllTags();
        Identity Save(Tag tag);
        Tag Save(Guid contentGuid, string name, string groupKey);
        void Save(Guid contentGuid, IEnumerable<string> names, string groupKey);
        void Delete(string name);
        void Delete(Identity id);
        void UpdateContentTags(IContent content);
    }
}

// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Data;

namespace Geta.Optimizely.Tags.Core
{
    public interface ITagRepository
    {
        Tag GetTagById(Identity id);
        Tag GetTagByNameAndGroup(string name, string groupKey);
        IEnumerable<Tag> GetTagsByName(string name);
        IEnumerable<Tag> GetTagsByContent(Guid contentGuid);
        IQueryable<Tag> GetAllTags();
        Identity Save(Tag tag);
        void Delete(Tag tag);
    }
}

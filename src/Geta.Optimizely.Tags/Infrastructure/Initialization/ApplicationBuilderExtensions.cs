// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information

using Geta.Optimizely.Tags.Core.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Geta.Optimizely.Tags.Infrastructure.Initialization
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseGetaTags(this IApplicationBuilder app)
        {
            var services = app.ApplicationServices;

            var initializer = services.GetRequiredService<TagsInitializer>();
            initializer.Initialize();

            return app;
        }
    }
}

using System;
using System.Linq;
using EPiServer.Shell.Modules;
using Geta.Optimizely.Tags.Core;
using Geta.Optimizely.Tags.Core.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Geta.Optimizely.Tags.Infrastructure.Configuration
{
    public static class ServiceCollectionExtensions
    {
        private const string ModuleName = "Geta.Optimizely.Tags";

        public static IServiceCollection AddGetaTags(this IServiceCollection services)
        {
            services.AddTransient<ITagService, TagService>();
            services.AddTransient<ITagRepository, TagRepository>();
            services.AddTransient<ITagEngine, TagEngine>();
            services.AddSingleton<Func<ITagService>>(x => x.GetRequiredService<ITagService>);
            services.AddSingleton<TagsInitializer>();

            services.Configure<ProtectedModuleOptions>(
                pm =>
                {
                    if (!pm.Items.Any(i => i.Name.Equals(ModuleName, StringComparison.OrdinalIgnoreCase)))
                    {
                        pm.Items.Add(new()
                        {
                            Name = ModuleName
                        });
                    }
                });

            return services;
        }
    }
}

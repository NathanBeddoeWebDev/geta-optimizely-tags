using System.Linq;
using EPiServer.Shell.Modules;
using Geta.Optimizely.Tags.Implementations;
using Geta.Optimizely.Tags.Interfaces;
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

            services.Configure<ProtectedModuleOptions>(
                pm =>
                {
                    if (!pm.Items.Any(i => i.Name.Equals(ModuleName, System.StringComparison.OrdinalIgnoreCase)))
                    {
                        pm.Items.Add(new ModuleDetails
                        {
                            Name = ModuleName
                        });
                    }
                });

            return services;
        }
    }
}

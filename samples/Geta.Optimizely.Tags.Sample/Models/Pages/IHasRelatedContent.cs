using EPiServer.Core;

namespace Geta.Optimizely.Tags.Sample.Models.Pages
{
    public interface IHasRelatedContent
    {
        ContentArea RelatedContentArea { get; }
    }
}
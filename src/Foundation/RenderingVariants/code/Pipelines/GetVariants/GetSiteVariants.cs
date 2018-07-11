using System.Linq;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.Presentation;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
using Sitecore.XA.Foundation.Variants.Abstractions.Pipelines.GetVariants;
using VariantsGrouping = Sitecore.XA.Foundation.Variants.Abstractions.Templates.VariantsGrouping;

namespace XA.Extensions.Foundation.RenderingVariants.Pipelines.GetVariants
{
    public class GetSiteVariants : GetVariantsBase
    {
        public readonly IPresentationContext PresentationContext;

        public GetSiteVariants(IPresentationContext presentationContext)
        {
            PresentationContext = presentationContext;
        }

        public void Process(GetVariantsArgs args)
        {
            Item presentationItem = PresentationContext.GetPresentationItem(args.ContextItem);
            if (presentationItem != null)
            {
                Item variantsGrouping = presentationItem.FirstChildInheritingFrom(VariantsGrouping.ID);
                Item renderingVariantsRoot = variantsGrouping?.Children.FirstOrDefault(item => item.Name.Equals(args.RenderingName));
                if (renderingVariantsRoot != null)
                {
                    args.Variants.AddRange(GetVariantsFromRoot(renderingVariantsRoot));
                }
            }
        }
    }
}
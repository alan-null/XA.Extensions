using Sitecore.Diagnostics;
using Sitecore.Pipelines.GetMasters;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;

namespace XA.Extensions.Foundation.RenderingVariants.Processors.UiGetMasters
{
    public class GetItemMasters
    {
        public void Process(GetMastersArgs args)
        {
            Assert.ArgumentNotNull(args, nameof(args));
            if (args.Item.InheritsFrom(Sitecore.XA.Foundation.Variants.Abstractions.Templates.ICompatibleRenderings.ID))
            {
                args.Masters.Add(args.Item.Template);
            }
        }
    }
}
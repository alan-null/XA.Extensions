using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
using Templates = Sitecore.XA.Foundation.Variants.Abstractions.Templates;

namespace XA.Extensions.Foundation.RenderingVariants.Pipelines.GetVariants
{
    public class GetVariantsBase
    {
        public IEnumerable<Item> GetVariantsFromRoot(Item root)
        {
            return GetVariantsRecurse(root);
        }

        protected virtual IEnumerable<Item> GetVariantsRecurse(Item root)
        {
            foreach (Item child in root.Children)
            {
                if (child.InheritsFrom(Templates.IVariantDefinition.ID))
                {
                    yield return child;
                }
                if (child.InheritsFrom(Templates.ICompatibleRenderings.ID))
                {
                    foreach (var item in GetVariantsRecurse(child))
                    {
                        yield return item;
                    }
                }
            }
        }
    }
}

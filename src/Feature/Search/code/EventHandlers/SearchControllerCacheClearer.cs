using System;
using System.Linq;
using Sitecore.Caching;

namespace XA.Extensions.Feature.Search.EventHandlers
{
    public class SearchControllerCacheClearer
    {
        public void OnPublishEnd(object sender, EventArgs args)
        {
            ClearCache();
        }

        protected virtual void ClearCache()
        {
            GetCacheByName(Constants.SearchControllerCacheKey)?.Clear();
        }

        protected ICacheInfo GetCacheByName(string name)
        {
            return CacheManager.GetAllCaches().FirstOrDefault(c => c.Name == name);
        }
    }
}

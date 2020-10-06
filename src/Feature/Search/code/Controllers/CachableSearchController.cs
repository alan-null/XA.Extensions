using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Sitecore;
using Sitecore.DependencyInjection;
using Sitecore.Web;
using Sitecore.XA.Feature.Search.Filters;
using Sitecore.XA.Foundation.Abstractions;
using Sitecore.XA.Foundation.Caching;
using XA.Extensions.Feature.Search.Models;

namespace XA.Extensions.Feature.Search.Controllers
{
    public class CachableSearchController : Sitecore.XA.Feature.Search.Controllers.SearchController
    {
        public CachableSearchController()
        {
            PageMode = ServiceLocator.ServiceProvider.GetService<IPageMode>();
        }

        public IPageMode PageMode { get; }
        protected static readonly DictionaryCache Cache = new DictionaryCache(Constants.SearchControllerCacheKey, GetCacheSize());

        [ActionName("CachedResults")]
        [RegisterSearchEvent]
        public SerializableResultSet GetCachedResults(string v = null, string q = null, string s = null, string l = null, string g = null, string o = null, int e = 0, int p = 20, string sig = null, string site = null)
        {
            SerializableResultSet result;
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            if (PageMode.IsNormal)
            {
                var cacheKey = BuildCacheKey();
                var cacheValue = Cache.Get(cacheKey);
                if (cacheValue == null)
                {
                    result = new SerializableResultSet(GetResults(v, q, s, l, g, o, e, p, sig, site));
                    StoreInCache(result, cacheKey);
                }
                else
                {
                    result = JsonConvert.DeserializeObject<SerializableResultSet>(cacheValue.Value);
                }
            }
            else
            {
                result = new SerializableResultSet(GetResults(v, q, s, l, g, o, e, p, sig, site));
            }
            stopwatch.Stop();
            result.TotalTime = stopwatch.ElapsedMilliseconds;
            return result;
        }

        protected virtual string BuildCacheKey()
        {
            var qs = HttpContext.Current.Request.QueryString;
            var enumerable = qs.AllKeys.OrderBy(s => s).Select(k => $"{k}:{qs.Get(k)}").ToList();
            enumerable.Add($"site:{Context.Site?.SiteInfo?.Name}");
            var s2 = String.Join("/", enumerable).ToLowerInvariant();

            var buildCacheKey = HttpContext.Current.Request.QueryString.AllKeys.OrderBy(x => x)
                .Select(key => $"{key}:{HttpContext.Current.Request.Params[key]}")
                .Union(new[] { $"site:{Context.Site?.SiteInfo?.Name}" })
                .Aggregate(string.Empty, (a, b) => string.Concat(a, "/", b))
                .ToLowerInvariant();
            return buildCacheKey;
        }

        protected void StoreInCache(SerializableResultSet args, string cacheKey)
        {
            var value = new DictionaryCacheValue { Value = JsonConvert.SerializeObject(args) };
            Cache.Set(cacheKey, value);
        }

        protected static long GetCacheSize()
        {
            var defaultValue = "50MB";
            var setting = "XA.Extensions.Feature.Search.CachableSearchController";
            return StringUtil.ParseSizeString(Sitecore.Configuration.Settings.GetSetting(setting, defaultValue));
        }
    }
}
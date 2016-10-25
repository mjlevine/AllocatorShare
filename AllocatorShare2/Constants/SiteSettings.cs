using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AllocatorShare2.Constants
{
    public class SiteSettings
    {
        public static TimeSpan DefaultCacheTimeSpan = new TimeSpan(4, 0, 0);
        public static TimeSpan TemplateCacheTimeSpan = new TimeSpan(0, 30, 0);
        public const string RootListCacheKey = "RootList";
        public const string CacheTreeListPrefix = "TreeList_";

    }
}
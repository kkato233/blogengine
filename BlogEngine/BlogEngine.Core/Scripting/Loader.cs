using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;

namespace BlogEngine.Core.Scripting
{
    public static class Loader
    {
        const string ScriptsKey = "Script.Loader-Scripts";
        const string StylesKey = "Script.Loader-Styles";

        public static IEnumerable<ContentItem> Scripts
        {
            get
            {
                if (Blog.CurrentInstance.Cache[ScriptsKey] == null)
                {
                    Blog.CurrentInstance.Cache.Add(
                        ScriptsKey,
                        Repository.GetScripts(),
                        null,
                        Cache.NoAbsoluteExpiration,
                        new TimeSpan(0, 15, 0),
                        CacheItemPriority.Low,
                        null);
                }
                return (IEnumerable<ContentItem>)Blog.CurrentInstance.Cache[ScriptsKey];
            }
        }

        public static IEnumerable<ContentItem> Styles
        {
            get
            {
                if (Blog.CurrentInstance.Cache[StylesKey] == null)
                {
                    Blog.CurrentInstance.Cache.Add(
                        StylesKey,
                        Repository.GetStyles(),
                        null,
                        Cache.NoAbsoluteExpiration,
                        new TimeSpan(0, 15, 0),
                        CacheItemPriority.Low,
                        null);
                }
                return (IEnumerable<ContentItem>)Blog.CurrentInstance.Cache[StylesKey];
            }
        }
    }
}

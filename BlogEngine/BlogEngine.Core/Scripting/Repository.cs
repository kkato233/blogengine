using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;

namespace BlogEngine.Core.Scripting
{
    public static class Repository
    {
        static List<ContentItem> scripts;
        static List<ContentItem> styles;

        public static void AddScript(ContentItem item)
        {
            foreach (var script in scripts)
            {
                if (script.Source == item.Source)
                    return;
            }
            scripts.Add(item);
        }

        public static void AddStyle(ContentItem item)
        {
            foreach (var style in styles)
            {
                if (style.Source == item.Source)
                    return;
            }
            styles.Add(item);
        }

        public static IEnumerable<ContentItem> GetScripts()
        {
            if (scripts == null)
                scripts = new List<ContentItem>();

            var path = HttpContext.Current.Server.MapPath(Utils.ApplicationRelativeWebRoot + "Scripts");

            var fileEntries = Directory.GetFiles(path)
                .Where(file => file.EndsWith(".js", StringComparison.OrdinalIgnoreCase) &&
                    !file.EndsWith("-vsdoc.js", StringComparison.OrdinalIgnoreCase) &&
                    !file.Contains("combined_")).ToList();

            foreach (var s in fileEntries)
            {
                ContentItem ci = new ContentItem();
                var src = s.Replace(path, "").TrimStart('\\').TrimStart('/');
                ci.Source = src;

                AddScript(ci);
            }
            return scripts.OrderBy(s => s.Source);
        }

        public static IEnumerable<ContentItem> GetStyles()
        {
            if (styles == null)
                styles = new List<ContentItem>();

            var path = HttpContext.Current.Server.MapPath(Utils.ApplicationRelativeWebRoot + "Styles");

            var fileEntries = Directory.GetFiles(path)
                .Where(file => file.EndsWith(".css", StringComparison.OrdinalIgnoreCase) &&
                    !file.Contains("combined_")).ToList();

            foreach (var s in fileEntries)
            {
                ContentItem ci = new ContentItem();
                var src = s.Replace(path, "").TrimStart('\\').TrimStart('/');
                ci.Source = src;

                AddStyle(ci);
            }
            return styles.OrderBy(s => s.Source);
        }
    }
}

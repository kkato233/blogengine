#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;

using BlogEngine.Core;

#endregion

/// <summary>
/// The widgets_ tag_cloud_widget.
/// </summary>
public partial class WidgetsTagCloudWidget : WidgetBase
{
    /// <summary>
    /// This method works as a substitute for Page_Load. You should use this method for
    /// data binding etc. instead of Page_Load.
    /// </summary>
    public override void LoadWidget()
    {
        foreach (var key in this.WeightedList.Keys)
        {
            using (var li = new HtmlGenericControl("li"))
            {
                li.InnerHtml = string.Format(
                    Link, 
                    string.Format("{0}?tag=/{1}", Utils.RelativeWebRoot, Utils.RemoveIllegalCharacters(key)), 
                    this.WeightedList[key], 
                    "Tag: " + key, 
                    key);
                this.ulTags.Controls.Add(li);
            }
        }
    }

    /// <summary>
    /// Initializes static members of the <see cref="WidgetsTagCloudWidget"/> class. 
    /// </summary>
    static WidgetsTagCloudWidget()
    {
        Post.Saved += delegate { Reload(); };
    }

    /// <summary>
    /// Reloads this instance.
    /// </summary>
    public static void Reload()
    {
        weightedList = null;
    }

    #region Private fields

    /// <summary>
    /// The link.
    /// </summary>
    private const string Link = "<a href=\"{0}\" class=\"{1}\" title=\"{2}\">{3}</a> ";

    /// <summary>
    /// The _ weighted list.
    /// </summary>
    private static Dictionary<string, string> weightedList;

    /// <summary>
    /// The _ sync root.
    /// </summary>
    private static readonly object SyncRoot = new object();

    #endregion

    /// <summary>
    /// The _ minimum posts.
    /// </summary>
    private int minimumPosts = 1;

    /// <summary>
    /// The _ tag cloud size.
    /// </summary>
    private int tagCloudSize = -1;

    /// <summary>
    /// Gets MinimumPosts.
    /// </summary>
    private int MinimumPosts
    {
        get
        {
            var settings = this.GetSettings();
            if (settings.ContainsKey("minimumposts"))
            {
                int.TryParse(settings["minimumposts"], out this.minimumPosts);
            }

            return this.minimumPosts;
        }
    }

    /// <summary>
    /// Gets TagCloudSize.
    /// </summary>
    private int TagCloudSize
    {
        get
        {
            var settings = this.GetSettings();
            if (settings.ContainsKey("tagcloudsize"))
            {
                int.TryParse(settings["tagcloudsize"], out this.tagCloudSize);
            }

            return this.tagCloudSize;
        }
    }

    /// <summary>
    /// Gets WeightedList.
    /// </summary>
    private Dictionary<string, string> WeightedList
    {
        get
        {
            if (weightedList == null)
            {
                lock (SyncRoot)
                {
                    if (weightedList == null)
                    {
                        weightedList = new Dictionary<string, string>();
                        this.SortList();
                    }
                }
            }

            return weightedList;
        }
    }

    /// <summary>
    /// Builds a raw list of all tags and the number of times
    ///     they have been added to a post.
    /// </summary>
    private static SortedDictionary<string, int> CreateRawList()
    {
        var dic = new SortedDictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
        foreach (var tag in Post.Posts.Where(post => post.VisibleToPublic).SelectMany(post => post.Tags))
        {
            if (dic.ContainsKey(tag))
            {
                dic[tag]++;
            }
            else
            {
                dic[tag] = 1;
            }
        }

        return dic;
    }

    /// <summary>
    /// Sorts the list of tags based on how much they are used.
    /// </summary>
    private void SortList()
    {
        var dic = CreateRawList();
        var max = dic.Values.Max();
        
        var currentTagCount = 0;

        foreach (var key in
            dic.Keys.Where(key => dic[key] >= this.MinimumPosts).Where(key => this.TagCloudSize <= 0 || currentTagCount < this.TagCloudSize))
        {
            currentTagCount++;

            var weight = ((double)dic[key] / max) * 100;
            if (weight >= 99)
            {
                weightedList.Add(key, "biggest");
            }
            else if (weight >= 70)
            {
                weightedList.Add(key, "big");
            }
            else if (weight >= 40)
            {
                weightedList.Add(key, "medium");
            }
            else if (weight >= 20)
            {
                weightedList.Add(key, "small");
            }
            else if (weight >= 3)
            {
                weightedList.Add(key, "smallest");
            }
        }
    }

    /// <summary>
    ///     Gets the name. It must be exactly the same as the folder that contains the widget.
    /// </summary>
    /// <value></value>
    public override string Name
    {
        get
        {
            return "Tag cloud";
        }
    }

    /// <summary>
    ///     Gets wether or not the widget can be edited.
    ///     <remarks>
    ///         The only way a widget can be editable is by adding a edit.ascx file to the widget folder.
    ///     </remarks>
    /// </summary>
    /// <value></value>
    public override bool IsEditable
    {
        get
        {
            return true;
        }
    }
}
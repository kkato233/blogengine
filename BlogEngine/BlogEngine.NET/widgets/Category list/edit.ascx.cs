﻿#region Using

using System;

#endregion

/// <summary>
/// The widgets categories edit.
/// </summary>
public partial class WidgetsCategoriesEdit : WidgetEditBase
{
    #region Public Methods

    /// <summary>
    /// Saves this the basic widget settings such as the Title.
    /// </summary>
    public override void Save()
    {
        var settings = this.GetSettings();
        settings["showrssicon"] = this.cbShowRssIcon.Checked.ToString();
        settings["showpostcount"] = this.cbShowPostCount.Checked.ToString();
        this.SaveSettings(settings);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
    /// </summary>
    /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        if (this.Page.IsPostBack)
        {
            return;
        }

        var settings = this.GetSettings();
        var showRssIcon = true;
        var showPostCount = true;
        if (settings.ContainsKey("showrssicon"))
        {
            bool.TryParse(settings["showrssicon"], out showRssIcon);
            bool.TryParse(settings["showpostcount"], out showPostCount);
        }

        this.cbShowRssIcon.Checked = showRssIcon;
        this.cbShowPostCount.Checked = showPostCount;
    }

    #endregion
}
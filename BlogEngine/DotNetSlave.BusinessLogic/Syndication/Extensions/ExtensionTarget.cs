/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/25/2007	brian.kuhn		Created ExtensionTarget Enumeration
****************************************************************************/
using System;

namespace BlogEngine.Core.Syndication.Extensions
{
    /// <summary>
    /// Represents permissible syndication extension targets.
    /// </summary>
    /// <remarks>This enumeration is used to describe what syndication specification elements an extension can extend.</remarks>
    public enum ExtensionTarget
    {
        /// <summary>
        /// No extension target specified.
        /// </summary>
        None            = 0,
        /// <summary>
        /// Indicates that extension can extend the Atom category element.
        /// </summary>
        AtomCategory    = 1,
        /// <summary>
        /// Indicates that extension can extend the Atom entry element.
        /// </summary>
        AtomEntry       = 2,
        /// <summary>
        /// Indicates that extension can extend the Atom feed element.
        /// </summary>
        AtomFeed        = 3,
        /// <summary>
        /// Indicates that extension can extend the Atom generator element.
        /// </summary>
        AtomGenerator   = 4,
        /// <summary>
        /// Indicates that extension can extend the Atom link element.
        /// </summary>
        AtomLink        = 5,
        /// <summary>
        /// Indicates that extension can extend the Atom person element.
        /// </summary>
        AtomPerson      = 6,
        /// <summary>
        /// Indicates that extension can extend the Atom text element.
        /// </summary>
        AtomText        = 7,
        /// <summary>
        /// Indicates that extension extends the OPML body element.
        /// </summary>
        OpmlBody        = 8,
        /// <summary>
        /// Indicates that extension extends the root OPML document.
        /// </summary>
        OpmlDocument    = 9,
        /// <summary>
        /// Indicates that extension extends the OPML head element.
        /// </summary>
        OpmlHead        = 10,
        /// <summary>
        /// Indicates that extension extends the OPML outline element.
        /// </summary>
        OpmlOutline     = 11,
        /// <summary>
        /// Indicates that extension can extend the RSS category element.
        /// </summary>
        RssCategory     = 12,
        /// <summary>
        /// Indicates that extension can extend the RSS channel element.
        /// </summary>
        RssChannel      = 13,
        /// <summary>
        /// Indicates that extension can extend the RSS cloud element.
        /// </summary>
        RssCloud        = 14,
        /// <summary>
        /// Indicates that extension can extend the RSS enclosure element.
        /// </summary>
        RssEnclosure    = 15,
        /// <summary>
        /// Indicates that extension can extend the RSS root element.
        /// </summary>
        RssFeed         = 16,
        /// <summary>
        /// Indicates that extension can extend the RSS guid element.
        /// </summary>
        RssGuid         = 17,
        /// <summary>
        /// Indicates that extension can extend the RSS image element.
        /// </summary>
        RssImage        = 18,
        /// <summary>
        /// Indicates that extension can extend the RSS item element.
        /// </summary>
        RssItem         = 19,
        /// <summary>
        /// Indicates that extension can extend the RSS source element.
        /// </summary>
        RssSource       = 20,
        /// <summary>
        /// Indicates that extension can extend the RSS text input element.
        /// </summary>
        RssTextInput    = 21
    };
}
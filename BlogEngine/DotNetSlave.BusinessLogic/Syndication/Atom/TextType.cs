/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/18/2007	brian.kuhn		Created TextType Enumeration
****************************************************************************/
using System;

namespace BlogEngine.Core.Syndication.Atom
{
    /// <summary>
    /// Represents common types of encoding of human-readable text.
    /// </summary>
    public enum TextType
    {
        /// <summary>
        /// No texy type specified.
        /// </summary>
        None    = 0,
        /// <summary>
        ///  Signifies that the text is intended to be presented to humans in a readable fashion.
        /// </summary>
        Html    = 1,
        /// <summary>
        /// Signifies that the text represents HTML markup.
        /// </summary>
        Text    = 2,
        /// <summary>
        /// Signifies that the text represents XHTML markup.
        /// </summary>
        Xhtml   = 3
    };
}
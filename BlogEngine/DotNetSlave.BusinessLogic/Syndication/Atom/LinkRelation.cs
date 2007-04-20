/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/17/2007	brian.kuhn		Created LinkRelation Enumeration
****************************************************************************/
using System;

namespace DotNetSlave.BlogEngine.BusinessLogic.Syndication.Atom
{
    /// <summary>
    /// Represents common types of link relations.
    /// </summary>
    public enum LinkRelation
    {
        /// <summary>
        /// No link relation specified.
        /// </summary>
        None        = 0,
        /// <summary>
        ///  Signifies that the link identifies an alternate version of the resource.
        /// </summary>
        Alternate   = 1,
        /// <summary>
        /// Signifies that the link identifies a related resource that is potentially large in size and might require special handling.
        /// </summary>
        Enclosure   = 2,
        /// <summary>
        /// Signifies that the link identifies a related resource.
        /// </summary>
        Related     = 3,
        /// <summary>
        /// Signifies that the link identifies an equivalent resource.
        /// </summary>
        Self        = 4,
        /// <summary>
        /// Signifies that the link identifies the source resource.
        /// </summary>
        Via         = 5
    };
}
/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/13/2007	brian.kuhn		Created CloudProtocol Enumeration
****************************************************************************/
using System;

namespace DotNetSlave.BlogEngine.BusinessLogic.Syndication.Rss
{
    /// <summary>
    /// Represents common types of cloud protocols.
    /// </summary>
    public enum CloudProtocol
    {
        /// <summary>
        /// No cloud protocol specified.
        /// </summary>
        None        = 0,
        /// <summary>
        ///  HTTP-POST protocol.
        /// </summary>
        HttpPost    = 1,
        /// <summary>
        /// SOAP 1.1 protocol.
        /// </summary>
        Soap        = 2,
        /// <summary>
        /// XML-RPC protocol.
        /// </summary>
        XmlRpc      = 3
    };
}
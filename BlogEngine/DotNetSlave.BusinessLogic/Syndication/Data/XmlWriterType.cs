/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/20/2007	brian.kuhn		Created XmlWriterType Enumeration
****************************************************************************/
using System;
using System.Xml;
using System.Xml.Serialization;

namespace DotNetSlave.BlogEngine.BusinessLogic.Syndication.Data
{
    /// <summary>
    /// Represents common types of XML writers.
    /// </summary>
    /// <remarks>THis enumeration is used internally to determine if an <see cref="XmlWriter"/> was created by an <see cref="XmlSerializer"/>.</remarks>
    public enum XmlWriterType
    {
        /// <summary>
        /// No XML writer type specified.
        /// </summary>
        None        = 0,
        /// <summary>
        /// Indicates the XML writer was created by an XmlSerializer.
        /// </summary>
        Serialized  = 1,
        /// <summary>
        ///  Indicates a standard XML writer is being utilized.
        /// </summary>
        Standard    = 2        
    };
}
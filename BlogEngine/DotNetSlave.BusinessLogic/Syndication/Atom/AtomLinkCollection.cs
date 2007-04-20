/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/18/2007	brian.kuhn		Created AtomLinkCollection Class
****************************************************************************/
using System;
using System.Collections.ObjectModel;
using System.Globalization;

using DotNetSlave.BlogEngine.BusinessLogic.Properties;

namespace DotNetSlave.BlogEngine.BusinessLogic.Syndication.Atom
{
    /// <summary>
    /// Represents a collection of <see cref="AtomLink"/> instances that can be associated to <see cref="AtomFeed"/> and <see cref="AtomEntry"/> entities.
    /// </summary>
    [Serializable()]
    public class AtomLinkCollection : Collection<AtomLink>
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region AtomLinkCollection()
        /// <summary>
        /// Initializes a new instance of the <see cref="AtomLinkCollection"/> class.
        /// </summary>
        public AtomLinkCollection() : base()
        {
            //------------------------------------------------------------
            //	Attempt to initialize class state
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	
                //------------------------------------------------------------
                
            }
            catch
            {
                //------------------------------------------------------------
                //	Rethrow exception
                //------------------------------------------------------------
                throw;
            }
        }
        #endregion

        //============================================================
        //	PUBLIC ROUTINES
        //============================================================
        #region Clear(string type)
        /// <summary>
        /// Removes all <see cref="AtomLink"/> elements from the collection that have the specified media type.
        /// </summary>
        /// <param name="type">The advisory media type for the link.</param>
        /// <remarks>Removal of links based on media type is case sensitive.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="type"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="type"/> is an empty string.</exception>
        public void Clear(string type)
        {
            //------------------------------------------------------------
            //	Attempt to clear collection for specific type
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameter
                //------------------------------------------------------------
                if (type == null)
                {
                    throw new ArgumentNullException("type");
                }
                else if (String.IsNullOrEmpty(type.Trim()))
                {
                    throw new ArgumentException(Resources.ExceptionEmptyStringMessage, "type");
                }

                //------------------------------------------------------------
                //	Iterate through collection
                //------------------------------------------------------------
                for (int i = 0; i < this.Count; i++)
                {
                    //------------------------------------------------------------
                    //	Get current link
                    //------------------------------------------------------------
                    AtomLink link   = this[i];

                    //------------------------------------------------------------
                    //	Determine if link media type matches specified media type
                    //------------------------------------------------------------
                    if (String.Compare(link.Type, type, false, CultureInfo.InvariantCulture) == 0)
                    {
                        //------------------------------------------------------------
                        //	Remove category from collection
                        //------------------------------------------------------------
                        this.Remove(link);
                    }
                }
            }
            catch
            {
                //------------------------------------------------------------
                //	Rethrow exception
                //------------------------------------------------------------
                throw;
            }
        }
        #endregion

        #region FindAll(string type)
        /// <summary>
        /// Finds all <see cref="AtomLink"/> instances contained in this collection that have the specified media type.
        /// </summary>
        /// <param name="type">The advisory media type of the collection elements to find.</param>
        /// <returns>A collection of all <see cref="AtomLink"/> instances contained in this collection that have the specified <paramref name="type"/>. If no results found, returns an empty collection.</returns>
        /// <remarks>Search of links based on media type is case sensitive.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="type"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="type"/> is an empty string.</exception>
        public AtomLinkCollection FindAll(string type)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            AtomLinkCollection links    = new AtomLinkCollection();

            //------------------------------------------------------------
            //	Attempt to find links that match specified media type
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameter
                //------------------------------------------------------------
                if (type == null)
                {
                    throw new ArgumentNullException("type");
                }
                else if (String.IsNullOrEmpty(type.Trim()))
                {
                    throw new ArgumentException(Resources.ExceptionEmptyStringMessage, "type");
                }

                //------------------------------------------------------------
                //	Iterate through collection
                //------------------------------------------------------------
                for (int i = 0; i < this.Count; i++)
                {
                    //------------------------------------------------------------
                    //	Get current link
                    //------------------------------------------------------------
                    AtomLink link   = this[i];

                    //------------------------------------------------------------
                    //	Determine if link media type matches specified media type
                    //------------------------------------------------------------
                    if (String.Compare(link.Type, type, false, CultureInfo.InvariantCulture) == 0)
                    {
                        //------------------------------------------------------------
                        //	Add link to result collection
                        //------------------------------------------------------------
                        links.Add(link);
                    }
                }
            }
            catch
            {
                //------------------------------------------------------------
                //	Rethrow exception
                //------------------------------------------------------------
                throw;
            }

            //------------------------------------------------------------
            //	Return result
            //------------------------------------------------------------
            return links;
        }
        #endregion
    }
}

/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/13/2007	brian.kuhn		Created RssItemCollection Class
****************************************************************************/
using System;
using System.Collections.ObjectModel;
using System.Globalization;

using DotNetSlave.BlogEngine.BusinessLogic.Properties;

namespace DotNetSlave.BlogEngine.BusinessLogic.Syndication.Rss
{
    /// <summary>
    /// Represents a collection of <see cref="RssItem"/> instances that can be associated to an <see cref="RssChannel"/>.
    /// </summary>
    [Serializable()]
    public class RssItemCollection : Collection<RssItem>
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region RssItemCollection()
        /// <summary>
        /// Initializes a new instance of the <see cref="RssItemCollection"/> class.
        /// </summary>
        public RssItemCollection() : base()
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
        #region Find(RssGuid guid)
        /// <summary>
        /// Returns the <see cref="RssItem"/> contained in this collection that has the specified <see cref="RssGuid"/> value.
        /// </summary>
        /// <param name="guid">The globally unique identifier for the item to search for.</param>
        /// <returns>A <see cref="RssItem"/> if a match is found, otherwise returns a null reference (Nothing in Visual Basic).</returns>
        /// <remarks>Search is based on a case-insensitive comparison of the specified <see cref="RssGuid.Value"/> against guid value of items in collection.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="guid"/> is a null reference (Nothing in Visual Basic).</exception>
        public RssItem Find(RssGuid guid)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            RssItem item    = null;

            //------------------------------------------------------------
            //	Attempt to find item in collection
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameter
                //------------------------------------------------------------
                if (guid == null)
                {
                    throw new ArgumentNullException("guid");
                }

                //------------------------------------------------------------
                //	Enumerate through the collection
                //------------------------------------------------------------
                foreach(RssItem currentItem in this)
                {
                    //------------------------------------------------------------
                    //	Determine if match found
                    //------------------------------------------------------------
                    if (String.Compare(currentItem.Guid.Value, guid.Value, true, CultureInfo.InvariantCulture) == 0)
                    {
                        item    = currentItem;
                        break;
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
            return item;
        }
        #endregion

        #region FindAll(RssCategory category)
        /// <summary>
        /// Finds all <see cref="RssItem"/> instances contained in this collection that have the specified <see cref="RssCategory"/>.
        /// </summary>
        /// <param name="category">The categorization taxonomy of the items to search for.</param>
        /// <returns>A collection of all <see cref="RssItem"/> instances contained in this collection that have the specified <paramref name="category"/>. If no results found, returns an empty collection.</returns>
        /// <remarks>Search based on whether item contains the specified category.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="category"/> is a null reference (Nothing in Visual Basic).</exception>
        public RssItemCollection FindAll(RssCategory category)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            RssItemCollection items = new RssItemCollection();

            //------------------------------------------------------------
            //	Attempt to find categories that match specified domain
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameter
                //------------------------------------------------------------
                if (category == null)
                {
                    throw new ArgumentNullException("category");
                }

                //------------------------------------------------------------
                //	Enumerate through the collection
                //------------------------------------------------------------
                foreach (RssItem currentItem in this)
                {
                    //------------------------------------------------------------
                    //	Determine if match found
                    //------------------------------------------------------------
                    if (currentItem.Categories.Contains(category))
                    {
                        //------------------------------------------------------------
                        //	Add item to return collection
                        //------------------------------------------------------------
                        items.Add(currentItem);
                        break;
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
            return items;
        }
        #endregion
    }
}

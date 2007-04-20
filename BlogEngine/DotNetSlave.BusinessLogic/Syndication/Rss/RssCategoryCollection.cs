/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/13/2007	brian.kuhn		Created RssCategoryCollection Class
****************************************************************************/
using System;
using System.Collections.ObjectModel;
using System.Globalization;

using DotNetSlave.BlogEngine.BusinessLogic.Properties;

namespace DotNetSlave.BlogEngine.BusinessLogic.Syndication.Rss
{
    /// <summary>
    /// Represents a collection of <see cref="RssCategory"/> instances that can be associated to <see cref="RssChannel"/> and <see cref="RssItem"/> entities.
    /// </summary>
    [Serializable()]
    public class RssCategoryCollection : Collection<RssCategory>
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region RssCategoryCollection()
        /// <summary>
        /// Initializes a new instance of the <see cref="RssCategoryCollection"/> class.
        /// </summary>
        public RssCategoryCollection() : base()
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
        #region Clear(string domain)
        /// <summary>
        /// Removes all <see cref="RssCategory"/> elements from the collection that have the specified domain.
        /// </summary>
        /// <param name="domain">The categorization taxonomy identifier.</param>
        /// <remarks>Removal of categories based on domain is case sensitive.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="domain"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="domain"/> is an empty string.</exception>
        public void Clear(string domain)
        {
            //------------------------------------------------------------
            //	Attempt to clear collection for specific domain
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameter
                //------------------------------------------------------------
                if (domain == null)
                {
                    throw new ArgumentNullException("domain");
                }
                else if (String.IsNullOrEmpty(domain.Trim()))
                {
                    throw new ArgumentException(Resources.ExceptionEmptyStringMessage, "domain");
                }

                //------------------------------------------------------------
                //	Iterate through collection
                //------------------------------------------------------------
                for (int i = 0; i < this.Count; i++)
                {
                    //------------------------------------------------------------
                    //	Get current category
                    //------------------------------------------------------------
                    RssCategory category    = this[i];

                    //------------------------------------------------------------
                    //	Determine if category domain matches specified domain
                    //------------------------------------------------------------
                    if (String.Compare(category.Domain, domain, false, CultureInfo.InvariantCulture) == 0)
                    {
                        //------------------------------------------------------------
                        //	Remove category from collection
                        //------------------------------------------------------------
                        this.Remove(category);
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

        #region FindAll(string domain)
        /// <summary>
        /// Finds all <see cref="RssCategory"/> instances contained in this collection that have the specified domain.
        /// </summary>
        /// <param name="domain">The categorization taxonomy identifier of the collection elements to find.</param>
        /// <returns>A collection of all <see cref="RssCategory"/> instances contained in this collection that have the specified <paramref name="domain"/>. If no results found, returns an empty collection.</returns>
        /// <remarks>Search of categories based on domain is case sensitive.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="domain"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="domain"/> is an empty string.</exception>
        public RssCategoryCollection FindAll(string domain)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            RssCategoryCollection categories    = new RssCategoryCollection();

            //------------------------------------------------------------
            //	Attempt to find categories that match specified domain
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameter
                //------------------------------------------------------------
                if (domain == null)
                {
                    throw new ArgumentNullException("domain");
                }
                else if (String.IsNullOrEmpty(domain.Trim()))
                {
                    throw new ArgumentException(Resources.ExceptionEmptyStringMessage, "domain");
                }

                //------------------------------------------------------------
                //	Iterate through collection
                //------------------------------------------------------------
                for (int i = 0; i < this.Count; i++)
                {
                    //------------------------------------------------------------
                    //	Get current category
                    //------------------------------------------------------------
                    RssCategory category = this[i];

                    //------------------------------------------------------------
                    //	Determine if category domain matches specified domain
                    //------------------------------------------------------------
                    if (String.Compare(category.Domain, domain, false, CultureInfo.InvariantCulture) == 0)
                    {
                        //------------------------------------------------------------
                        //	Add category to result collection
                        //------------------------------------------------------------
                        categories.Add(category);
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
            return categories;
        }
        #endregion
    }
}

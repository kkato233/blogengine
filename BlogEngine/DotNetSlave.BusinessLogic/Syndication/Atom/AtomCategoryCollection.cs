/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/18/2007	brian.kuhn		Created AtomCategoryCollection Class
****************************************************************************/
using System;
using System.Collections.ObjectModel;
using System.Globalization;

using DotNetSlave.BlogEngine.BusinessLogic.Properties;

namespace DotNetSlave.BlogEngine.BusinessLogic.Syndication.Atom
{
    /// <summary>
    /// Represents a collection of <see cref="AtomCategory"/> instances that can be associated to <see cref="AtomFeed"/> and <see cref="AtomEntry"/> entities.
    /// </summary>
    [Serializable()]
    public class AtomCategoryCollection : Collection<AtomCategory>
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region AtomCategoryCollection()
        /// <summary>
        /// Initializes a new instance of the <see cref="AtomCategoryCollection"/> class.
        /// </summary>
        public AtomCategoryCollection() : base()
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
        #region Clear(Uri scheme)
        /// <summary>
        /// Removes all <see cref="AtomCategory"/> elements from the collection that have the specified scheme.
        /// </summary>
        /// <param name="scheme">The categorization scheme.</param>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="scheme"/> is a null reference (Nothing in Visual Basic).</exception>
        public void Clear(Uri scheme)
        {
            //------------------------------------------------------------
            //	Attempt to clear collection for specific scheme
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameter
                //------------------------------------------------------------
                if (scheme == null)
                {
                    throw new ArgumentNullException("scheme");
                }

                //------------------------------------------------------------
                //	Iterate through collection
                //------------------------------------------------------------
                for (int i = 0; i < this.Count; i++)
                {
                    //------------------------------------------------------------
                    //	Get current category
                    //------------------------------------------------------------
                    AtomCategory category    = this[i];

                    //------------------------------------------------------------
                    //	Determine if category scheme matches specified scheme
                    //------------------------------------------------------------
                    if (category.Scheme == scheme)
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

        #region FindAll(Uri scheme)
        /// <summary>
        /// Finds all <see cref="AtomCategory"/> instances contained in this collection that have the specified scheme.
        /// </summary>
        /// <param name="scheme">The categorization scheme of the collection elements to find.</param>
        /// <returns>A collection of all <see cref="AtomCategory"/> instances contained in this collection that have the specified <paramref name="scheme"/>. If no results found, returns an empty collection.</returns>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="scheme"/> is a null reference (Nothing in Visual Basic).</exception>
        public AtomCategoryCollection FindAll(Uri scheme)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            AtomCategoryCollection categories    = new AtomCategoryCollection();

            //------------------------------------------------------------
            //	Attempt to find categories that match specified scheme
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameter
                //------------------------------------------------------------
                if (scheme == null)
                {
                    throw new ArgumentNullException("scheme");
                }

                //------------------------------------------------------------
                //	Iterate through collection
                //------------------------------------------------------------
                for (int i = 0; i < this.Count; i++)
                {
                    //------------------------------------------------------------
                    //	Get current category
                    //------------------------------------------------------------
                    AtomCategory category = this[i];

                    //------------------------------------------------------------
                    //	Determine if category scheme matches specified scheme
                    //------------------------------------------------------------
                    if (category.Scheme == scheme)
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

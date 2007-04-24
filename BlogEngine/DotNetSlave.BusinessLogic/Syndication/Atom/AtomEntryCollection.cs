/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/18/2007	brian.kuhn		Created AtomEntryCollection Class
****************************************************************************/
using System;
using System.Collections.ObjectModel;
using System.Globalization;

using BlogEngine.Core.Properties;

namespace BlogEngine.Core.Syndication.Atom
{
    /// <summary>
    /// Represents a collection of <see cref="AtomEntry"/> instances that can be associated to a <see cref="AtomFeed"/>entity.
    /// </summary>
    [Serializable()]
    public class AtomEntryCollection : Collection<AtomEntry>
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region AtomEntryCollection()
        /// <summary>
        /// Initializes a new instance of the <see cref="AtomEntryCollection"/> class.
        /// </summary>
        public AtomEntryCollection() : base()
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
        #region FindAll(Uri id)
        /// <summary>
        /// Finds all <see cref="AtomEntry"/> instances contained in this collection that have the specified universally unique identifier.
        /// </summary>
        /// <param name="id">The universally unique identifier of the collection elements to find.</param>
        /// <returns>A collection of all <see cref="AtomEntry"/> instances contained in this collection that have the specified <paramref name="id"/>. If no results found, returns an empty collection.</returns>
        /// <remarks></remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="id"/> is a null reference (Nothing in Visual Basic).</exception>
        public AtomEntryCollection FindAll(Uri id)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            AtomEntryCollection entries = new AtomEntryCollection();

            //------------------------------------------------------------
            //	Attempt to find entries that match specified identifier
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameter
                //------------------------------------------------------------
                if (id == null)
                {
                    throw new ArgumentNullException("id");
                }

                //------------------------------------------------------------
                //	Iterate through collection
                //------------------------------------------------------------
                for (int i = 0; i < this.Count; i++)
                {
                    //------------------------------------------------------------
                    //	Get current entry
                    //------------------------------------------------------------
                    AtomEntry entry = this[i];

                    //------------------------------------------------------------
                    //	Determine if entry identifier matches specified identifier
                    //------------------------------------------------------------
                    if (entry.Id == id)
                    {
                        //------------------------------------------------------------
                        //	Add entry to result collection
                        //------------------------------------------------------------
                        entries.Add(entry);
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
            return entries;
        }
        #endregion
    }
}

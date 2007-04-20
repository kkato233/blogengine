/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/13/2007	brian.kuhn		Created AtomPersonCollection Class
****************************************************************************/
using System;
using System.Collections.ObjectModel;
using System.Globalization;

using DotNetSlave.BlogEngine.BusinessLogic.Properties;

namespace DotNetSlave.BlogEngine.BusinessLogic.Syndication.Atom
{
    /// <summary>
    /// Represents a collection of <see cref="AtomPerson"/> instances that can be associated to an <see cref="AtomFeed"/> or <see cref="AtomEntry"/> entities.
    /// </summary>
    [Serializable()]
    public class AtomPersonCollection : Collection<AtomPerson>
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region AtomPersonCollection()
        /// <summary>
        /// Initializes a new instance of the <see cref="AtomPersonCollection"/> class.
        /// </summary>
        public AtomPersonCollection() : base()
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
        #region Find(string name)
        /// <summary>
        /// Returns the <see cref="AtomPerson"/> contained in this collection that has the specified name.
        /// </summary>
        /// <param name="name">The human-readable name for the person.</param>
        /// <returns>A <see cref="AtomPerson"/> if a match is found, otherwise returns a null reference (Nothing in Visual Basic).</returns>
        /// <remarks>Search is based on a case-insensitive comparison of the specified name against name value of items in collection.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="name"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="name"/> is an empty string.</exception>
        public AtomPerson Find(string name)
        {
            //------------------------------------------------------------
            //	Local members
            //------------------------------------------------------------
            AtomPerson person   = null;

            //------------------------------------------------------------
            //	Attempt to find item in collection
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameter
                //------------------------------------------------------------
                if (name == null)
                {
                    throw new ArgumentNullException("name");
                }
                else if (String.IsNullOrEmpty(name.Trim()))
                {
                    throw new ArgumentException(Resources.ExceptionEmptyStringMessage, "name");
                }

                //------------------------------------------------------------
                //	Enumerate through the collection
                //------------------------------------------------------------
                foreach(AtomPerson currentPerson in this)
                {
                    //------------------------------------------------------------
                    //	Determine if match found
                    //------------------------------------------------------------
                    if (String.Compare(currentPerson.Name, name, true, CultureInfo.InvariantCulture) == 0)
                    {
                        person  = currentPerson;
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
            return person;
        }
        #endregion
    }
}

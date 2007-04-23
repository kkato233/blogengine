/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
04/11/2007	brian.kuhn		Created SyndicationExtensionDictionary Class
****************************************************************************/
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace DotNetSlave.BlogEngine.Syndication.Extensions
{
    /// <summary>
    /// Represents a collection of <see cref="ISyndicationExtension"/> instances keyed by their unique namespace.
    /// </summary>
    [Serializable()]
    public class SyndicationExtensionDictionary : Dictionary<string, ISyndicationExtension>
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        
        #endregion

        //============================================================
        //	CONSTRUCTORS
        //============================================================
        #region SyndicationExtensionDictionary()
        /// <summary>
        /// Initializes a new instance of the <see cref="SyndicationExtensionDictionary"/> class.
        /// </summary>
        public SyndicationExtensionDictionary() : base()
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

        #region SyndicationExtensionDictionary(SerializationInfo info, StreamingContext context)
        /// <summary>
        /// Initializes a new instance of the <see cref="SyndicationExtensionDictionary"/> class.
        /// </summary>
        /// <param name="info">A <see cref="SerializationInfo"/> instance that store the serialization information.</param>
        /// <param name="context">A <see cref="StreamingContext"/> that describes the source, destination and context for a given serialized stream.</param>
        protected SyndicationExtensionDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            //------------------------------------------------------------
            //	Attempt to initialize class state
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Initialization handled by base class
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
        #region Add(ISyndicationExtension extension)
        /// <summary>
        /// Adds the specified <see cref="ISyndicationExtension"/> to the dictionary using its Namespace property as the key.
        /// </summary>
        /// <param name="extension">The <see cref="ISyndicationExtension"/> to add to the dictionary.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="extension"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentException">The <paramref name="extension"/>'s Namespace property is a null or empty string.</exception>
        public void Add(ISyndicationExtension extension)
        {
            //------------------------------------------------------------
            //	Attempt to initialize class state
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameters
                //------------------------------------------------------------
                if (extension == null)
                {
                    throw new ArgumentNullException("extension");
                }

                if (String.IsNullOrEmpty(extension.Namespace))
                {
                    throw new ArgumentException(String.Format(null, "The {0} extension has a Namespace that is a null/empty string.", extension.Title), "extension");
                }
                
                //------------------------------------------------------------
                //	Add extension to the dictionary
                //------------------------------------------------------------
                base.Add(extension.Namespace.ToLowerInvariant(), extension);
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

        #region Add(Type extensionType)
        /// <summary>
        /// Adds the specified syndication extension to the dictionary using its <see cref="Type"/>.
        /// </summary>
        /// <param name="extensionType">The <see cref="Type"/> of the syndication extension to add to the dictionary.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="extensionType"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="MissingMethodException">No matching constructor was found.</exception>
        /// <exception cref="System.IO.FileNotFoundException">The assembly containing the specified type was not found.</exception>
        public void Add(Type extensionType)
        {
            //------------------------------------------------------------
            //	Attempt to initialize class state
            //------------------------------------------------------------
            try
            {
                //------------------------------------------------------------
                //	Validate parameter
                //------------------------------------------------------------
                if (extensionType == null)
                {
                    throw new ArgumentNullException("extensionType");
                }

                //------------------------------------------------------------
                //	Extract assembly name from specified type
                //------------------------------------------------------------
                AssemblyName typeAssemblyName   = extensionType.Assembly.GetName();

                //------------------------------------------------------------
                //	Load assembly for syndication extension
                //------------------------------------------------------------
                Assembly extensionAssembly      = Assembly.Load(typeAssemblyName, extensionType.Assembly.Evidence);

                //------------------------------------------------------------
                //	Create an instance of the syndication extension
                //------------------------------------------------------------
                ISyndicationExtension extension = extensionAssembly.CreateInstance(extensionType.FullName) as ISyndicationExtension;

                //------------------------------------------------------------
                //	Add extension to the dictionary
                //------------------------------------------------------------
                base.Add(extension.Namespace.ToLowerInvariant(), extension);
            }
            catch (MissingMethodException)
            {
                //------------------------------------------------------------
                //	Rethrow missing method exception
                //------------------------------------------------------------
                throw;
            }
            catch (System.IO.FileNotFoundException)
            {
                //------------------------------------------------------------
                //	Rethrow file not found exception
                //------------------------------------------------------------
                throw;
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
    }
}

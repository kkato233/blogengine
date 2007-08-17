using System;
using System.IO;
using System.Threading;
using System.Xml.Serialization;

using System.Web;
using System.Web.Caching;

namespace BlogEngine.Core.Providers{


    /// <summary>
    /// Generic class for a perssitable object. 
    /// Encapsulates the logic to load/save data from/to the filesystem. 
    /// To speed up the acces, caching is used.
    /// </summary>
    /// <typeparam name="T">class or struct with all the data-fields that must be persisted</typeparam>
    public class Persistable<T> where T : new() {

        #region Fields  

        string _fileName;
        XmlSerializer _serializer;
        T _value;
        ReaderWriterLock _lock;

        #endregion

        #region Properties  

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        protected virtual T Value {
            get { return _value; }
        }
        #endregion

        #region Construct  

        /// <summary>
        /// Creates a instance of Persistable. Also creates a instance of T
        /// </summary>
        protected Persistable(string fileName) {
            _fileName = fileName;
            _value = new T();
            _serializer = new XmlSerializer(typeof(T));
            _lock = new ReaderWriterLock();
            ///
            Load();
        }
        #endregion

        #region Methods 

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        /// <returns></returns>
        public virtual bool Delete() {
            return Delete(_fileName);
        }

        /// <summary>
        /// Deletes the data from the cache and filesystem
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        protected bool Delete(string fileName) {

            bool success = true;
            _lock.AcquireWriterLock(Timeout.Infinite);
            try {
                if (File.Exists(fileName)) {
                    HttpContext context = HttpContext.Current;
                    try {
                        File.Delete(fileName);
                        if (context != null)
                            context.Cache.Remove(fileName);
                    }
                    catch { success = false; }
                }
                return success;
            }
            finally {
                _lock.ReleaseWriterLock();
            }
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public virtual void Load() {
            Load(_fileName);
        }

        /// <summary>
        /// Loads the data from the filesystem. For deserialization a XmlSeralizer is used.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        protected void Load(string fileName) {

            _lock.AcquireReaderLock(Timeout.Infinite);
            try {
                HttpContext context = HttpContext.Current;
                object value = (context != null) ? context.Cache[fileName] : null;
                if (value != null) {
                    _value = (T)value;
                }
                else if (System.IO.File.Exists(fileName)) {
                    using (FileStream reader = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                        _value = (T)_serializer.Deserialize(reader);
                    }
                    if (_value != null && context != null) {
                        context.Cache.Insert(fileName, _value);
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception(
                    string.Format("Unable to load persitable object from file {0}", fileName), ex);
            }
            finally {
                _lock.ReleaseReaderLock();
            }
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public virtual void Save() {
            Save(_fileName);
        }

        /// <summary>
        /// Persists the data back to the filesystem
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        protected void Save(string fileName) {

            _lock.AcquireWriterLock(Timeout.Infinite);
            try {
                HttpContext context = HttpContext.Current;
                if (context != null)
                    context.Cache.Insert(fileName, _value,
                        null, DateTime.MaxValue, TimeSpan.FromHours(1), CacheItemPriority.Normal, null);
                if (!Directory.Exists(Path.GetDirectoryName(fileName)))
                    Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                using (FileStream writer = File.Create(fileName)) {
                    _serializer.Serialize(writer, _value);
                }
            }
            catch (Exception ex) {
                throw new Exception(
                    string.Format("Unable to save persitable object to file {0}", fileName), ex);
            }
            finally {
                _lock.ReleaseWriterLock();
            }
        }
        #endregion
    }
}

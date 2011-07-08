using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogEngine.Core.FileSystem
{
    using System.Data.Linq;
    using System.Data.Linq.Mapping;
    using System.Data;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Linq;
    using System.Linq.Expressions;
    using System.ComponentModel;
    using System;


    [global::System.Data.Linq.Mapping.DatabaseAttribute(Name = "BlogEngineFileStore")]
    public partial class FileStoreDb : System.Data.Linq.DataContext
    {

        private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();

        #region Extensibility Method Definitions
        partial void OnCreated();
        partial void InsertFileStoreDirectory(FileStoreDirectory instance);
        partial void UpdateFileStoreDirectory(FileStoreDirectory instance);
        partial void DeleteFileStoreDirectory(FileStoreDirectory instance);
        partial void InsertFileStoreFile(FileStoreFile instance);
        partial void UpdateFileStoreFile(FileStoreFile instance);
        partial void DeleteFileStoreFile(FileStoreFile instance);
        #endregion

        public FileStoreDb(string connection) :
            base(connection, mappingSource)
        {
            OnCreated();
        }

        public FileStoreDb(System.Data.IDbConnection connection) :
            base(connection, mappingSource)
        {
            OnCreated();
        }

        public FileStoreDb(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) :
            base(connection, mappingSource)
        {
            OnCreated();
        }

        public FileStoreDb(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) :
            base(connection, mappingSource)
        {
            OnCreated();
        }

        public System.Data.Linq.Table<FileStoreDirectory> FileStoreDirectories
        {
            get
            {
                return this.GetTable<FileStoreDirectory>();
            }
        }

        public System.Data.Linq.Table<FileStoreFile> FileStoreFiles
        {
            get
            {
                return this.GetTable<FileStoreFile>();
            }
        }
    }

    [global::System.Data.Linq.Mapping.TableAttribute(Name = "dbo.be_FileStoreDirectory")]
    public partial class FileStoreDirectory : INotifyPropertyChanging, INotifyPropertyChanged
    {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        private System.Guid _Id;

        private System.Nullable<System.Guid> _ParentID;

        private System.Guid _BlogID;

        private string _Name;

        private string _FullPath;

        private System.DateTime _CreateDate;

        private System.DateTime _LastAccess;

        private System.DateTime _LastModify;

        private EntitySet<FileStoreFile> _FileStoreFiles;

        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnIdChanging(System.Guid value);
        partial void OnIdChanged();
        partial void OnParentIDChanging(System.Nullable<System.Guid> value);
        partial void OnParentIDChanged();
        partial void OnBlogIDChanging(System.Guid value);
        partial void OnBlogIDChanged();
        partial void OnNameChanging(string value);
        partial void OnNameChanged();
        partial void OnFullPathChanging(string value);
        partial void OnFullPathChanged();
        partial void OnCreateDateChanging(System.DateTime value);
        partial void OnCreateDateChanged();
        partial void OnLastAccessChanging(System.DateTime value);
        partial void OnLastAccessChanged();
        partial void OnLastModifyChanging(System.DateTime value);
        partial void OnLastModifyChanged();
        #endregion

        public FileStoreDirectory()
        {
            this._FileStoreFiles = new EntitySet<FileStoreFile>(new Action<FileStoreFile>(this.attach_FileStoreFiles), new Action<FileStoreFile>(this.detach_FileStoreFiles));
            OnCreated();
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Id", DbType = "UniqueIdentifier NOT NULL", IsPrimaryKey = true)]
        public System.Guid Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                if ((this._Id != value))
                {
                    this.OnIdChanging(value);
                    this.SendPropertyChanging();
                    this._Id = value;
                    this.SendPropertyChanged("Id");
                    this.OnIdChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ParentID", DbType = "UniqueIdentifier")]
        public System.Nullable<System.Guid> ParentID
        {
            get
            {
                return this._ParentID;
            }
            set
            {
                if ((this._ParentID != value))
                {
                    this.OnParentIDChanging(value);
                    this.SendPropertyChanging();
                    this._ParentID = value;
                    this.SendPropertyChanged("ParentID");
                    this.OnParentIDChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_BlogID", DbType = "UniqueIdentifier NOT NULL")]
        public System.Guid BlogID
        {
            get
            {
                return this._BlogID;
            }
            set
            {
                if ((this._BlogID != value))
                {
                    this.OnBlogIDChanging(value);
                    this.SendPropertyChanging();
                    this._BlogID = value;
                    this.SendPropertyChanged("BlogID");
                    this.OnBlogIDChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Name", DbType = "VarChar(255) NOT NULL", CanBeNull = false)]
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                if ((this._Name != value))
                {
                    this.OnNameChanging(value);
                    this.SendPropertyChanging();
                    this._Name = value;
                    this.SendPropertyChanged("Name");
                    this.OnNameChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_FullPath", DbType = "VarChar(1000) NOT NULL", CanBeNull = false)]
        public string FullPath
        {
            get
            {
                return this._FullPath;
            }
            set
            {
                if ((this._FullPath != value))
                {
                    this.OnFullPathChanging(value);
                    this.SendPropertyChanging();
                    this._FullPath = value;
                    this.SendPropertyChanged("FullPath");
                    this.OnFullPathChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CreateDate", DbType = "DateTime NOT NULL")]
        public System.DateTime CreateDate
        {
            get
            {
                return this._CreateDate;
            }
            set
            {
                if ((this._CreateDate != value))
                {
                    this.OnCreateDateChanging(value);
                    this.SendPropertyChanging();
                    this._CreateDate = value;
                    this.SendPropertyChanged("CreateDate");
                    this.OnCreateDateChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_LastAccess", DbType = "DateTime NOT NULL")]
        public System.DateTime LastAccess
        {
            get
            {
                return this._LastAccess;
            }
            set
            {
                if ((this._LastAccess != value))
                {
                    this.OnLastAccessChanging(value);
                    this.SendPropertyChanging();
                    this._LastAccess = value;
                    this.SendPropertyChanged("LastAccess");
                    this.OnLastAccessChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_LastModify", DbType = "DateTime NOT NULL")]
        public System.DateTime LastModify
        {
            get
            {
                return this._LastModify;
            }
            set
            {
                if ((this._LastModify != value))
                {
                    this.OnLastModifyChanging(value);
                    this.SendPropertyChanging();
                    this._LastModify = value;
                    this.SendPropertyChanged("LastModify");
                    this.OnLastModifyChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "be_FileStoreDirectory_be_FileStoreFile", Storage = "_FileStoreFiles", ThisKey = "Id", OtherKey = "ParentDirectoryID")]
        public EntitySet<FileStoreFile> FileStoreFiles
        {
            get
            {
                return this._FileStoreFiles;
            }
            set
            {
                this._FileStoreFiles.Assign(value);
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanging()
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(String propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void attach_FileStoreFiles(FileStoreFile entity)
        {
            this.SendPropertyChanging();
            entity.FileStoreDirectory = this;
        }

        private void detach_FileStoreFiles(FileStoreFile entity)
        {
            this.SendPropertyChanging();
            entity.FileStoreDirectory = null;
        }
    }

    [global::System.Data.Linq.Mapping.TableAttribute(Name = "dbo.be_FileStoreFiles")]
    public partial class FileStoreFile : INotifyPropertyChanging, INotifyPropertyChanged
    {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        private System.Guid _FileID;

        private System.Guid _ParentDirectoryID;

        private string _Name;

        private string _FullPath;

        private System.Data.Linq.Binary _Contents;

        private int _Size;

        private System.DateTime _CreateDate;

        private System.DateTime _LastAccess;

        private System.DateTime _LastModify;

        private EntityRef<FileStoreDirectory> _FileStoreDirectory;

        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnFileIDChanging(System.Guid value);
        partial void OnFileIDChanged();
        partial void OnParentDirectoryIDChanging(System.Guid value);
        partial void OnParentDirectoryIDChanged();
        partial void OnNameChanging(string value);
        partial void OnNameChanged();
        partial void OnFullPathChanging(string value);
        partial void OnFullPathChanged();
        partial void OnContentsChanging(System.Data.Linq.Binary value);
        partial void OnContentsChanged();
        partial void OnSizeChanging(int value);
        partial void OnSizeChanged();
        partial void OnCreateDateChanging(System.DateTime value);
        partial void OnCreateDateChanged();
        partial void OnLastAccessChanging(System.DateTime value);
        partial void OnLastAccessChanged();
        partial void OnLastModifyChanging(System.DateTime value);
        partial void OnLastModifyChanged();
        #endregion

        public FileStoreFile()
        {
            this._FileStoreDirectory = default(EntityRef<FileStoreDirectory>);
            OnCreated();
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_FileID", DbType = "UniqueIdentifier NOT NULL", IsPrimaryKey = true)]
        public System.Guid FileID
        {
            get
            {
                return this._FileID;
            }
            set
            {
                if ((this._FileID != value))
                {
                    this.OnFileIDChanging(value);
                    this.SendPropertyChanging();
                    this._FileID = value;
                    this.SendPropertyChanged("FileID");
                    this.OnFileIDChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ParentDirectoryID", DbType = "UniqueIdentifier NOT NULL")]
        public System.Guid ParentDirectoryID
        {
            get
            {
                return this._ParentDirectoryID;
            }
            set
            {
                if ((this._ParentDirectoryID != value))
                {
                    if (this._FileStoreDirectory.HasLoadedOrAssignedValue)
                    {
                        throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
                    }
                    this.OnParentDirectoryIDChanging(value);
                    this.SendPropertyChanging();
                    this._ParentDirectoryID = value;
                    this.SendPropertyChanged("ParentDirectoryID");
                    this.OnParentDirectoryIDChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Name", DbType = "VarChar(255) NOT NULL", CanBeNull = false)]
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                if ((this._Name != value))
                {
                    this.OnNameChanging(value);
                    this.SendPropertyChanging();
                    this._Name = value;
                    this.SendPropertyChanged("Name");
                    this.OnNameChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_FullPath", DbType = "VarChar(255) NOT NULL", CanBeNull = false)]
        public string FullPath
        {
            get
            {
                return this._FullPath;
            }
            set
            {
                if ((this._FullPath != value))
                {
                    this.OnFullPathChanging(value);
                    this.SendPropertyChanging();
                    this._FullPath = value;
                    this.SendPropertyChanged("FullPath");
                    this.OnFullPathChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Contents", DbType = "VarBinary(MAX) NOT NULL", CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
        public System.Data.Linq.Binary Contents
        {
            get
            {
                return this._Contents;
            }
            set
            {
                if ((this._Contents != value))
                {
                    this.OnContentsChanging(value);
                    this.SendPropertyChanging();
                    this._Contents = value;
                    this.SendPropertyChanged("Contents");
                    this.OnContentsChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Size", DbType = "Int NOT NULL")]
        public int Size
        {
            get
            {
                return this._Size;
            }
            set
            {
                if ((this._Size != value))
                {
                    this.OnSizeChanging(value);
                    this.SendPropertyChanging();
                    this._Size = value;
                    this.SendPropertyChanged("Size");
                    this.OnSizeChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CreateDate", DbType = "DateTime NOT NULL")]
        public System.DateTime CreateDate
        {
            get
            {
                return this._CreateDate;
            }
            set
            {
                if ((this._CreateDate != value))
                {
                    this.OnCreateDateChanging(value);
                    this.SendPropertyChanging();
                    this._CreateDate = value;
                    this.SendPropertyChanged("CreateDate");
                    this.OnCreateDateChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_LastAccess", DbType = "DateTime NOT NULL")]
        public System.DateTime LastAccess
        {
            get
            {
                return this._LastAccess;
            }
            set
            {
                if ((this._LastAccess != value))
                {
                    this.OnLastAccessChanging(value);
                    this.SendPropertyChanging();
                    this._LastAccess = value;
                    this.SendPropertyChanged("LastAccess");
                    this.OnLastAccessChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_LastModify", DbType = "DateTime NOT NULL")]
        public System.DateTime LastModify
        {
            get
            {
                return this._LastModify;
            }
            set
            {
                if ((this._LastModify != value))
                {
                    this.OnLastModifyChanging(value);
                    this.SendPropertyChanging();
                    this._LastModify = value;
                    this.SendPropertyChanged("LastModify");
                    this.OnLastModifyChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "be_FileStoreDirectory_be_FileStoreFile", Storage = "_FileStoreDirectory", ThisKey = "ParentDirectoryID", OtherKey = "Id", IsForeignKey = true, DeleteOnNull = true, DeleteRule = "CASCADE")]
        public FileStoreDirectory FileStoreDirectory
        {
            get
            {
                return this._FileStoreDirectory.Entity;
            }
            set
            {
                FileStoreDirectory previousValue = this._FileStoreDirectory.Entity;
                if (((previousValue != value)
                            || (this._FileStoreDirectory.HasLoadedOrAssignedValue == false)))
                {
                    this.SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        this._FileStoreDirectory.Entity = null;
                        previousValue.FileStoreFiles.Remove(this);
                    }
                    this._FileStoreDirectory.Entity = value;
                    if ((value != null))
                    {
                        value.FileStoreFiles.Add(this);
                        this._ParentDirectoryID = value.Id;
                    }
                    else
                    {
                        this._ParentDirectoryID = default(System.Guid);
                    }
                    this.SendPropertyChanged("FileStoreDirectory");
                }
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanging()
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(String propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}
#pragma warning restore 1591
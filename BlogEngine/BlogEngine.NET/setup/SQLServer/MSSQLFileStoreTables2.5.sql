/****** Object:  Table [dbo].[be_FileStoreDirectory]    Script Date: 07/08/2011 12:42:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[be_FileStoreDirectory](
	[Id] [uniqueidentifier] NOT NULL,
	[ParentID] [uniqueidentifier] NULL,
	[BlogID] [uniqueidentifier] NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[FullPath] [varchar](1000) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[LastAccess] [datetime] NOT NULL,
	[LastModify] [datetime] NOT NULL,
 CONSTRAINT [PK_be_FileStoreDirectory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[be_FileStoreFiles]    Script Date: 07/08/2011 12:42:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[be_FileStoreFiles](
	[FileID] [uniqueidentifier] NOT NULL,
	[ParentDirectoryID] [uniqueidentifier] NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[FullPath] [varchar](255) NOT NULL,
	[Contents] [varbinary](max) NOT NULL,
	[Size] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[LastAccess] [datetime] NOT NULL,
	[LastModify] [datetime] NOT NULL,
 CONSTRAINT [PK_be_FileStoreFiles] PRIMARY KEY CLUSTERED 
(
	[FileID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[be_FileStoreFiles]  WITH CHECK ADD  CONSTRAINT [FK_be_FileStoreFiles_be_FileStoreDirectory] FOREIGN KEY([ParentDirectoryID])
REFERENCES [dbo].[be_FileStoreDirectory] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[be_FileStoreFiles] CHECK CONSTRAINT [FK_be_FileStoreFiles_be_FileStoreDirectory]
GO



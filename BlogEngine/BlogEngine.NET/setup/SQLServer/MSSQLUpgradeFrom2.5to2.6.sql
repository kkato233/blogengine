

SET CONCAT_NULL_YIELDS_NULL, ANSI_NULLS, ANSI_PADDING, QUOTED_IDENTIFIER, ANSI_WARNINGS, ARITHABORT ON
SET NUMERIC_ROUNDABORT, IMPLICIT_TRANSACTIONS, XACT_ABORT OFF
GO

--
-- Create table "dbo.be_Packages"
--
CREATE TABLE dbo.be_Packages (
  [PackageId] nvarchar(128) NOT NULL,
  [Version] nvarchar(128) NOT NULL
)
GO

--
-- Create table "dbo.be_PackageFiles"
--
CREATE TABLE dbo.be_PackageFiles (
  [PackageId] nvarchar(128) NOT NULL,
  [FileOrder] int NOT NULL,
  [FilePath] nvarchar(255) NOT NULL,
  [IsDirectory] bit NOT NULL
)
GO

--
-- QuickNotes
--

CREATE TABLE [dbo].[be_QuickNotes](
	[NoteID] [uniqueidentifier] NOT NULL,
	[BlogID] [uniqueidentifier] NOT NULL,
	[UserName] [nvarchar](100) NOT NULL,
	[Note] [nvarchar](max) NOT NULL,
	[Updated] [datetime] NULL
)
GO

CREATE TABLE [dbo].[be_QuickSettings](
	[BlogID] [uniqueidentifier] NOT NULL,
	[UserName] [nvarchar](100) NOT NULL,
	[SettingName] [nvarchar](255) NOT NULL,
	[SettingValue] [nvarchar](255) NOT NULL
)
GO

CREATE NONCLUSTERED INDEX [idx_be_NoteId_BlogId_UserName] ON [dbo].[be_QuickNotes] 
(
	[NoteID] ASC,
	[BlogID] ASC,
	[UserName] ASC
)
GO

ALTER TABLE dbo.be_Blogs ADD
	IsSiteAggregation bit NOT NULL CONSTRAINT DF_be_Blogs_IsSiteAggregation DEFAULT 0
GO






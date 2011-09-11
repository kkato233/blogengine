

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
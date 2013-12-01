
--
-- be_CustomFields
--
CREATE TABLE [dbo].[be_CustomFields](
	[CustomType] [nvarchar](100) NOT NULL,
	[ObjectId] [nvarchar](250) NOT NULL,
	[BlogId] [uniqueidentifier] NOT NULL,
	[Key] [nvarchar](250) NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
	[Attribute] [nvarchar](250) NULL
)
GO

CREATE NONCLUSTERED INDEX [idx_be_CustomType_ObjectId_BlogId_Key] ON [dbo].[be_CustomFields] 
(
	[CustomType] ASC,
	[ObjectId] ASC,
	[BlogId] ASC,
	[Key] ASC
)
GO






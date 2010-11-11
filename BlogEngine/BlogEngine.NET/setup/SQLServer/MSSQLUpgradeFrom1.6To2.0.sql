

ALTER TABLE dbo.be_PostComment ADD IsSpam bit NULL
GO

ALTER TABLE dbo.be_PostComment ADD IsDeleted bit NULL
GO

CREATE TABLE [dbo].[be_Rights](
	[RightName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_be_Rights] PRIMARY KEY CLUSTERED 
(
	[RightName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[be_RightRoles](
	[RightName] [nvarchar](100) NOT NULL,
	[Role] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_be_RightRoles] PRIMARY KEY CLUSTERED 
(
	[RightName] ASC,
	[Role] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


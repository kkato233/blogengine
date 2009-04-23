
CREATE TABLE [dbo].[be_BlogRollItems](
	[BlogRollId] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[BlogUrl] [varchar](255) NOT NULL,
	[FeedUrl] [varchar](255) NULL,
	[Xfn] [varchar](255) NULL,
	[SortIndex] [int] NOT NULL,
 CONSTRAINT [PK_be_BlogRollItems] PRIMARY KEY CLUSTERED 
(
	[BlogRollId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

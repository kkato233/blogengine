/****** BlogEngine.NET 1.4 SQL Upgrade Script ******/

/* be_Categories update */
ALTER TABLE [dbo].[be_Categories]
	ADD
		[ParentID] [uniqueidentifier] NULL
GO

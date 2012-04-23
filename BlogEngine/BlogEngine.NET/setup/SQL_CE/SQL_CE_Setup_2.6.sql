
CREATE TABLE [be_BlogRollItems] (
  [BlogRollRowId] int NOT NULL  IDENTITY (7,1)
, [BlogId] uniqueidentifier NOT NULL
, [BlogRollId] uniqueidentifier NOT NULL
, [Title] nvarchar(255) NOT NULL DEFAULT ''
, [Description] nvarchar(255) NULL DEFAULT ''
, [BlogUrl] nvarchar(255) NOT NULL DEFAULT ''
, [FeedUrl] nvarchar(255) NULL DEFAULT ''
, [Xfn] nvarchar(255) NULL DEFAULT ''
, [SortIndex] int NOT NULL
);
GO
CREATE TABLE [be_Blogs] (
  [BlogRowId] int NOT NULL  IDENTITY (2,1)
, [BlogId] uniqueidentifier NOT NULL
, [BlogName] nvarchar(255) NOT NULL
, [Hostname] nvarchar(255) NOT NULL
, [IsAnyTextBeforeHostnameAccepted] bit NOT NULL
, [StorageContainerName] nvarchar(255) NOT NULL
, [VirtualPath] nvarchar(255) NOT NULL
, [IsPrimary] bit NOT NULL
, [IsActive] bit NOT NULL
, [IsSiteAggregation] bit NOT NULL
);
GO
CREATE TABLE [be_Categories] (
  [CategoryRowID] int NOT NULL  IDENTITY (2,1)
, [BlogID] uniqueidentifier NOT NULL
, [CategoryID] uniqueidentifier NOT NULL DEFAULT newid()
, [CategoryName] nvarchar(50) NULL DEFAULT ''
, [Description] nvarchar(200) NULL DEFAULT ''
, [ParentID] uniqueidentifier NULL
);
GO
CREATE TABLE [be_DataStoreSettings] (
  [DataStoreSettingRowId] int NOT NULL  IDENTITY (65,1)
, [BlogId] uniqueidentifier NOT NULL
, [ExtensionType] nvarchar(50) NOT NULL DEFAULT ''
, [ExtensionId] nvarchar(100) NOT NULL DEFAULT ''
, [Settings] ntext NOT NULL DEFAULT ''
);
GO
CREATE TABLE [be_FileStoreDirectory] (
  [Id] uniqueidentifier NOT NULL
, [ParentID] uniqueidentifier NULL
, [BlogID] uniqueidentifier NOT NULL
, [Name] nvarchar(255) NOT NULL
, [FullPath] nvarchar(1000) NOT NULL
, [CreateDate] datetime NOT NULL
, [LastAccess] datetime NOT NULL
, [LastModify] datetime NOT NULL
);
GO
CREATE TABLE [be_FileStoreFiles] (
  [FileID] uniqueidentifier NOT NULL
, [ParentDirectoryID] uniqueidentifier NOT NULL
, [Name] nvarchar(255) NOT NULL
, [FullPath] nvarchar(255) NOT NULL
, [Contents] image NOT NULL
, [Size] int NOT NULL
, [CreateDate] datetime NOT NULL
, [LastAccess] datetime NOT NULL
, [LastModify] datetime NOT NULL
);
GO
CREATE TABLE [be_FileStoreFileThumbs] (
  [thumbnailId] uniqueidentifier NOT NULL
, [FileId] uniqueidentifier NOT NULL
, [size] int NOT NULL
, [contents] image NOT NULL
);
GO
CREATE TABLE [be_PackageFiles] (
  [PackageId] nvarchar(128) NOT NULL
, [FileOrder] int NOT NULL
, [FilePath] nvarchar(255) NOT NULL
, [IsDirectory] bit NOT NULL
);
GO
CREATE TABLE [be_Packages] (
  [PackageId] nvarchar(128) NOT NULL
, [Version] nvarchar(128) NOT NULL
);
GO
CREATE TABLE [be_Pages] (
  [PageRowID] int NOT NULL  IDENTITY (1,1)
, [BlogID] uniqueidentifier NOT NULL
, [PageID] uniqueidentifier NOT NULL DEFAULT newid()
, [Title] nvarchar(255) NULL DEFAULT ''
, [Description] ntext NULL DEFAULT ''
, [PageContent] ntext NULL DEFAULT ''
, [Keywords] ntext NULL DEFAULT ''
, [DateCreated] datetime NULL
, [DateModified] datetime NULL
, [IsPublished] bit NULL
, [IsFrontPage] bit NULL
, [Parent] uniqueidentifier NULL
, [ShowInList] bit NULL
, [Slug] nvarchar(255) NULL DEFAULT ''
, [IsDeleted] bit NOT NULL DEFAULT 0
);
GO
CREATE TABLE [be_PingService] (
  [PingServiceID] int NOT NULL  IDENTITY (9,1)
, [BlogID] uniqueidentifier NOT NULL
, [Link] nvarchar(255) NULL DEFAULT ''
);
GO
CREATE TABLE [be_PostCategory] (
  [PostCategoryID] int NOT NULL  IDENTITY (3,1)
, [BlogID] uniqueidentifier NOT NULL
, [PostID] uniqueidentifier NOT NULL
, [CategoryID] uniqueidentifier NOT NULL
);
GO
CREATE TABLE [be_PostComment] (
  [PostCommentRowID] int NOT NULL  IDENTITY (1,1)
, [BlogID] uniqueidentifier NOT NULL
, [PostCommentID] uniqueidentifier NOT NULL DEFAULT newid()
, [PostID] uniqueidentifier NOT NULL
, [ParentCommentID] uniqueidentifier NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000'
, [CommentDate] datetime NOT NULL
, [Author] nvarchar(255) NULL DEFAULT ''
, [Email] nvarchar(255) NULL DEFAULT ''
, [Website] nvarchar(255) NULL DEFAULT ''
, [Comment] ntext NULL DEFAULT ''
, [Country] nvarchar(255) NULL DEFAULT ''
, [Ip] nvarchar(50) NULL DEFAULT ''
, [IsApproved] bit NULL
, [ModeratedBy] nvarchar(100) NULL DEFAULT ''
, [Avatar] nvarchar(255) NULL DEFAULT ''
, [IsSpam] bit NOT NULL DEFAULT 0
, [IsDeleted] bit NOT NULL DEFAULT 0
);
GO
CREATE TABLE [be_PostNotify] (
  [PostNotifyID] int NOT NULL  IDENTITY (1,1)
, [BlogID] uniqueidentifier NOT NULL
, [PostID] uniqueidentifier NOT NULL
, [NotifyAddress] nvarchar(255) NULL DEFAULT ''
);
GO
CREATE TABLE [be_Posts] (
  [PostRowID] int NOT NULL  IDENTITY (2,1)
, [BlogID] uniqueidentifier NOT NULL
, [PostID] uniqueidentifier NOT NULL DEFAULT newid()
, [Title] nvarchar(255) NULL DEFAULT ''
, [Description] ntext NULL DEFAULT ''
, [PostContent] ntext NULL DEFAULT ''
, [DateCreated] datetime NULL
, [DateModified] datetime NULL
, [Author] nvarchar(50) NULL DEFAULT ''
, [IsPublished] bit NULL
, [IsCommentEnabled] bit NULL
, [Raters] int NULL
, [Rating] real NULL
, [Slug] nvarchar(255) NULL DEFAULT ''
, [IsDeleted] bit NOT NULL DEFAULT 0
);
GO
CREATE TABLE [be_PostTag] (
  [PostTagID] int NOT NULL  IDENTITY (5,1)
, [BlogID] uniqueidentifier NOT NULL
, [PostID] uniqueidentifier NOT NULL
, [Tag] nvarchar(50) NULL DEFAULT ''
);
GO
CREATE TABLE [be_Profiles] (
  [ProfileID] int NOT NULL  IDENTITY (1,1)
, [BlogID] uniqueidentifier NOT NULL
, [UserName] nvarchar(100) NULL DEFAULT ''
, [SettingName] nvarchar(200) NULL DEFAULT ''
, [SettingValue] ntext NULL DEFAULT ''
);
GO
CREATE TABLE [be_QuickNotes] (
  [QuickNoteID] int NOT NULL  IDENTITY (1,1)
, [NoteID] uniqueidentifier NOT NULL
, [BlogID] uniqueidentifier NOT NULL
, [UserName] nvarchar(100) NOT NULL
, [Note] ntext NOT NULL
, [Updated] datetime NULL
);
GO
CREATE TABLE [be_QuickSettings] (
  [QuickSettingID] int NOT NULL  IDENTITY (1,1)
, [BlogID] uniqueidentifier NOT NULL
, [UserName] nvarchar(100) NOT NULL
, [SettingName] nvarchar(255) NOT NULL
, [SettingValue] nvarchar(255) NOT NULL
);
GO
CREATE TABLE [be_Referrers] (
  [ReferrerRowId] int NOT NULL  IDENTITY (1,1)
, [BlogId] uniqueidentifier NOT NULL
, [ReferrerId] uniqueidentifier NOT NULL
, [ReferralDay] datetime NOT NULL DEFAULT getdate()
, [ReferrerUrl] nvarchar(255) NOT NULL DEFAULT ''
, [ReferralCount] int NOT NULL
, [Url] nvarchar(255) NULL DEFAULT ''
, [IsSpam] bit NULL
);
GO
CREATE TABLE [be_RightRoles] (
  [RightRoleRowId] int NOT NULL  IDENTITY (66,1)
, [BlogId] uniqueidentifier NOT NULL
, [RightName] nvarchar(100) NOT NULL DEFAULT ''
, [Role] nvarchar(100) NOT NULL DEFAULT ''
);
GO
CREATE TABLE [be_Rights] (
  [RightRowId] int NOT NULL  IDENTITY (41,1)
, [BlogId] uniqueidentifier NOT NULL
, [RightName] nvarchar(100) NOT NULL DEFAULT ''
);
GO
CREATE TABLE [be_Roles] (
  [RoleID] int NOT NULL  IDENTITY (4,1)
, [BlogID] uniqueidentifier NOT NULL
, [Role] nvarchar(100) NOT NULL DEFAULT ''
);
GO
CREATE TABLE [be_Settings] (
  [SettingRowId] int NOT NULL  IDENTITY (64,1)
, [BlogId] uniqueidentifier NOT NULL
, [SettingName] nvarchar(50) NOT NULL DEFAULT ''
, [SettingValue] ntext NULL DEFAULT ''
);
GO
CREATE TABLE [be_StopWords] (
  [StopWordRowId] int NOT NULL  IDENTITY (109,1)
, [BlogId] uniqueidentifier NOT NULL
, [StopWord] nvarchar(50) NOT NULL DEFAULT ''
);
GO
CREATE TABLE [be_UserRoles] (
  [UserRoleID] int NOT NULL  IDENTITY (2,1)
, [BlogID] uniqueidentifier NOT NULL
, [UserName] nvarchar(100) NOT NULL
, [Role] nvarchar(100) NOT NULL
);
GO
CREATE TABLE [be_Users] (
  [UserID] int NOT NULL  IDENTITY (2,1)
, [BlogID] uniqueidentifier NOT NULL
, [UserName] nvarchar(100) NOT NULL DEFAULT ''
, [Password] nvarchar(255) NOT NULL DEFAULT ''
, [LastLoginTime] datetime NULL
, [EmailAddress] nvarchar(100) NULL DEFAULT ''
);
GO
SET IDENTITY_INSERT [be_BlogRollItems] ON;
GO
INSERT INTO [be_BlogRollItems] ([BlogRollRowId],[BlogId],[BlogRollId],[Title],[Description],[BlogUrl],[FeedUrl],[Xfn],[SortIndex]) VALUES (1,'27604f05-86ad-47ef-9e05-950bb762570c','25e4d8da-3278-4e58-b0bf-932496dabc96',N'Mads Kristensen',N'Full featured simplicity in ASP.NET and C#',N'http://www.madskristensen.dk/',N'http://feeds.feedburner.com/netslave',N'contact',0);
GO
INSERT INTO [be_BlogRollItems] ([BlogRollRowId],[BlogId],[BlogRollId],[Title],[Description],[BlogUrl],[FeedUrl],[Xfn],[SortIndex]) VALUES (2,'27604f05-86ad-47ef-9e05-950bb762570c','ccc817ef-e760-482b-b82f-a6854663110f',N'Al Nyveldt',N'Adventures in Code and Other Stories',N'http://www.nyveldt.com/blog/',N'http://feeds.feedburner.com/razorant',N'contact',1);
GO
INSERT INTO [be_BlogRollItems] ([BlogRollRowId],[BlogId],[BlogRollId],[Title],[Description],[BlogUrl],[FeedUrl],[Xfn],[SortIndex]) VALUES (3,'27604f05-86ad-47ef-9e05-950bb762570c','dcdaa78b-0b77-4691-99f0-1bb6418945a1',N'Ruslan Tur',N'.NET and Open Source: better together',N'http://rtur.net/blog/',N'http://feeds.feedburner.com/rtur',N'contact',2);
GO
INSERT INTO [be_BlogRollItems] ([BlogRollRowId],[BlogId],[BlogRollId],[Title],[Description],[BlogUrl],[FeedUrl],[Xfn],[SortIndex]) VALUES (4,'27604f05-86ad-47ef-9e05-950bb762570c','8a846489-b69e-4fde-b2b2-53bc6104a6fa',N'John Dyer',N'Technology and web development in ASP.NET, Flash, and JavaScript',N'http://johndyer.name/',N'http://johndyer.name/syndication.axd',N'contact',3);
GO
INSERT INTO [be_BlogRollItems] ([BlogRollRowId],[BlogId],[BlogRollId],[Title],[Description],[BlogUrl],[FeedUrl],[Xfn],[SortIndex]) VALUES (5,'27604f05-86ad-47ef-9e05-950bb762570c','7f906880-4316-47f1-a934-1a912fc02f8b',N'Russell van der Walt',N'an adventure in web technologies',N'http://blog.ruski.co.za/',N'http://feeds.feedburner.com/rusvdw',N'contact',4);
GO
INSERT INTO [be_BlogRollItems] ([BlogRollRowId],[BlogId],[BlogRollId],[Title],[Description],[BlogUrl],[FeedUrl],[Xfn],[SortIndex]) VALUES (6,'27604f05-86ad-47ef-9e05-950bb762570c','890f00e5-3a86-4cba-b85b-104063964a87',N'Ben Amada',N'adventures in application development',N'http://allben.net/',N'http://feeds.feedburner.com/allben',N'contact',5);
GO
SET IDENTITY_INSERT [be_BlogRollItems] OFF;
GO
SET IDENTITY_INSERT [be_Blogs] ON;
GO
INSERT INTO [be_Blogs] ([BlogRowId],[BlogId],[BlogName],[Hostname],[IsAnyTextBeforeHostnameAccepted],[StorageContainerName],[VirtualPath],[IsPrimary],[IsActive],[IsSiteAggregation]) VALUES (1,'27604f05-86ad-47ef-9e05-950bb762570c',N'Primary',N'',0,N'',N'~/',1,1,0);
GO
SET IDENTITY_INSERT [be_Blogs] OFF;
GO
SET IDENTITY_INSERT [be_Categories] ON;
GO
INSERT INTO [be_Categories] ([CategoryRowID],[BlogID],[CategoryID],[CategoryName],[Description],[ParentID]) VALUES (1,'27604f05-86ad-47ef-9e05-950bb762570c','05366547-5c8e-4643-ad94-377d3f809ab8',N'General',N'',null);
GO
SET IDENTITY_INSERT [be_Categories] OFF;
GO
SET IDENTITY_INSERT [be_DataStoreSettings] ON;
GO
INSERT INTO [be_DataStoreSettings] ([DataStoreSettingRowId],[BlogId],[ExtensionType],[ExtensionId],[Settings]) VALUES (1,'27604f05-86ad-47ef-9e05-950bb762570c',N'1',N'be_WIDGET_ZONE',N'<?xml version="1.0" encoding="utf-16"?>
<WidgetData xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <Settings>&lt;widgets&gt;&lt;widget id="d9ada63d-3462-4c72-908e-9d35f0acce40" title="TextBox" showTitle="True"&gt;TextBox&lt;/widget&gt;&lt;widget id="19baa5f6-49d4-4828-8f7f-018535c35f94" title="Administration" showTitle="True"&gt;Administration&lt;/widget&gt;&lt;widget id="d81c5ae3-e57e-4374-a539-5cdee45e639f" title="Search" showTitle="True"&gt;Search&lt;/widget&gt;&lt;widget id="77142800-6dff-4016-99ca-69b5c5ebac93" title="Tag cloud" showTitle="True"&gt;Tag cloud&lt;/widget&gt;&lt;widget id="4ce68ae7-c0c8-4bf8-b50f-a67b582b0d2e" title="RecentPosts" showTitle="True"&gt;RecentPosts&lt;/widget&gt;&lt;/widgets&gt;</Settings>
</WidgetData>');
GO
INSERT INTO [be_DataStoreSettings] ([DataStoreSettingRowId],[BlogId],[ExtensionType],[ExtensionId],[Settings]) VALUES (51,'27604f05-86ad-47ef-9e05-950bb762570c',N'0',N'SimpleCaptcha',N'<?xml version="1.0" encoding="utf-16"?>
<ManagedExtension xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" Name="SimpleCaptcha">
  <AdminPage />
  <Author>&lt;a href="http://www.aaronstannard.com"&gt;Aaron Stannard&lt;/a&gt;</Author>
  <Description>Settings for the SimpleCaptcha control</Description>
  <Enabled>false</Enabled>
  <Priority>0</Priority>
  <Settings>
    <Delimiter>44</Delimiter>
    <Help>To get started with SimpleCaptcha, just provide some captcha instructions for your users in the &lt;b&gt;CaptchaLabel&lt;/b&gt;
                                field and the value you require from your users in order to post a comment in the &lt;b&gt;CaptchaAnswer&lt;/b&gt; field.</Help>
    <Hidden>false</Hidden>
    <Index>0</Index>
    <IsScalar>true</IsScalar>
    <KeyField>CaptchaLabel</KeyField>
    <Name>SimpleCaptcha</Name>
    <Parameters>
      <KeyField>true</KeyField>
      <Label>Your captcha''s label</Label>
      <MaxLength>30</MaxLength>
      <Name>CaptchaLabel</Name>
      <ParamType>String</ParamType>
      <Required>true</Required>
      <SelectedValue />
      <Values>5+5 = </Values>
    </Parameters>
    <Parameters>
      <KeyField>true</KeyField>
      <Label>Your captcha''s expected value</Label>
      <MaxLength>30</MaxLength>
      <Name>CaptchaAnswer</Name>
      <ParamType>String</ParamType>
      <Required>true</Required>
      <SelectedValue />
      <Values>10</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Show Captcha For Authenticated Users</Label>
      <MaxLength>1</MaxLength>
      <Name>ShowForAuthenticatedUsers</Name>
      <ParamType>Boolean</ParamType>
      <Required>true</Required>
      <SelectedValue />
      <Values>False</Values>
    </Parameters>
    <ShowAdd>true</ShowAdd>
    <ShowDelete>true</ShowDelete>
    <ShowEdit>true</ShowEdit>
  </Settings>
  <ShowSettings>true</ShowSettings>
  <Version>1.0</Version>
</ManagedExtension>');
GO
INSERT INTO [be_DataStoreSettings] ([DataStoreSettingRowId],[BlogId],[ExtensionType],[ExtensionId],[Settings]) VALUES (52,'27604f05-86ad-47ef-9e05-950bb762570c',N'0',N'SendCommentMail',N'<?xml version="1.0" encoding="utf-16"?>
<ManagedExtension xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" Name="SendCommentMail">
  <AdminPage />
  <Author>BlogEngine.NET</Author>
  <Description>Sends an e-mail to the blog owner whenever a comment is added</Description>
  <Enabled>true</Enabled>
  <Priority>0</Priority>
  <ShowSettings>true</ShowSettings>
  <Version>1.3</Version>
</ManagedExtension>');
GO
INSERT INTO [be_DataStoreSettings] ([DataStoreSettingRowId],[BlogId],[ExtensionType],[ExtensionId],[Settings]) VALUES (53,'27604f05-86ad-47ef-9e05-950bb762570c',N'0',N'BBCode',N'<?xml version="1.0" encoding="utf-16"?>
<ManagedExtension xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" Name="BBCode">
  <AdminPage />
  <Author>&lt;a href="http://dotnetblogengine.net"&gt;BlogEngine.NET&lt;/a&gt;</Author>
  <Description>Converts BBCode to XHTML in the comments</Description>
  <Enabled>true</Enabled>
  <Priority>0</Priority>
  <ShowSettings>true</ShowSettings>
  <Version>1.0</Version>
</ManagedExtension>');
GO
INSERT INTO [be_DataStoreSettings] ([DataStoreSettingRowId],[BlogId],[ExtensionType],[ExtensionId],[Settings]) VALUES (54,'27604f05-86ad-47ef-9e05-950bb762570c',N'0',N'MediaElementPlayer',N'<?xml version="1.0" encoding="utf-16"?>
<ManagedExtension xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" Name="MediaElementPlayer">
  <AdminPage />
  <Author>&lt;a href="http://johndyer.me/"&gt;John Dyer&lt;/a&gt;</Author>
  <Description>HTML5 Video/Audio Player</Description>
  <Enabled>true</Enabled>
  <Priority>0</Priority>
  <Settings>
    <Delimiter>44</Delimiter>
    <Help>
&lt;p&gt;Build on &lt;a href="http://mediaelement.js.com/"&gt;MediaElement.js&lt;/a&gt;, the HTML5 video/audio player.&lt;/p&gt;

&lt;ol&gt;
	&lt;li&gt;Upload media files to your /media/ folder&lt;/li&gt;
	&lt;li&gt;Add short code to your media: [video src="myfile.mp4"] for video and [audio src="myfile.mp3"] for audio&lt;/li&gt;
	&lt;li&gt;Customize with the following parameters:
		&lt;ul&gt;
			&lt;li&gt;&lt;b&gt;width&lt;/b&gt;: The exact width of the video&lt;/li&gt;
			&lt;li&gt;&lt;b&gt;height&lt;/b&gt;: The exact height of the video&lt;/li&gt;
			&lt;li&gt;&lt;b&gt;autoplay&lt;/b&gt;: Plays the video as soon as the webpage loads&lt;/li&gt;
		&lt;/ul&gt;
	&lt;/li&gt;
	&lt;li&gt;You can also specify multiple file formats and codecs 
		&lt;ul&gt;
			&lt;li&gt;&lt;b&gt;mp4&lt;/b&gt;: H.264 encoded MP4 file&lt;/li&gt;
			&lt;li&gt;&lt;b&gt;webm&lt;/b&gt;: VP8/WebM encoded file&lt;/li&gt;
			&lt;li&gt;&lt;b&gt;ogg&lt;/b&gt;: Theora/Vorbis encoded file&lt;/li&gt;
		&lt;/ul&gt;
	&lt;/li&gt;
&lt;/ol&gt;

&lt;p&gt;A complete example:&lt;br /&gt;
[code mp4="myfile.mp4" webm="myfile.webm" ogg="myfile.ogg" width="480" height="360"]
&lt;/p&gt;

&lt;p&gt;Supported formats&lt;/p&gt;
&lt;ul&gt;
	&lt;li&gt;&lt;b&gt;MP4/MP3&lt;/b&gt;: Native HTML5 for IE9, Safari, Chrome; Flash in IE8, Firefox, Opera&lt;/li&gt;
	&lt;li&gt;&lt;b&gt;WebM&lt;/b&gt;: HTML5 for IE9, Chrome, Firefox, Opera; Flash in IE8 (coming in Flash 11)&lt;/li&gt;
	&lt;li&gt;&lt;b&gt;FLV&lt;/b&gt;: Flash fallback&lt;/li&gt;
	&lt;li&gt;&lt;b&gt;WMV/WMA&lt;/b&gt;: Silverlight fallback&lt;/li&gt;
&lt;/ul&gt;
</Help>
    <Hidden>false</Hidden>
    <Index>0</Index>
    <IsScalar>true</IsScalar>
    <KeyField>width</KeyField>
    <Name>MediaElementPlayer</Name>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Default Width</Label>
      <MaxLength>100</MaxLength>
      <Name>width</Name>
      <ParamType>String</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>480</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Default Height</Label>
      <MaxLength>100</MaxLength>
      <Name>height</Name>
      <ParamType>String</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>360</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Folder for Media (MP4, MP3, WMV, Ogg, WebM, etc.)</Label>
      <MaxLength>100</MaxLength>
      <Name>folder</Name>
      <ParamType>String</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>media</Values>
    </Parameters>
    <ShowAdd>true</ShowAdd>
    <ShowDelete>true</ShowDelete>
    <ShowEdit>true</ShowEdit>
  </Settings>
  <ShowSettings>true</ShowSettings>
  <Version>1.5</Version>
</ManagedExtension>');
GO
INSERT INTO [be_DataStoreSettings] ([DataStoreSettingRowId],[BlogId],[ExtensionType],[ExtensionId],[Settings]) VALUES (55,'27604f05-86ad-47ef-9e05-950bb762570c',N'0',N'AkismetFilter',N'<?xml version="1.0" encoding="utf-16"?>
<ManagedExtension xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" Name="AkismetFilter">
  <AdminPage />
  <Author>&lt;a href="http://dotnetblogengine.net"&gt;BlogEngine.NET&lt;/a&gt;</Author>
  <Description>Akismet anti-spam comment filter</Description>
  <Enabled>false</Enabled>
  <Priority>0</Priority>
  <Settings>
    <Delimiter>44</Delimiter>
    <Help />
    <Hidden>false</Hidden>
    <Index>0</Index>
    <IsScalar>true</IsScalar>
    <KeyField>SiteURL</KeyField>
    <Name>AkismetFilter</Name>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Site URL</Label>
      <MaxLength>100</MaxLength>
      <Name>SiteURL</Name>
      <ParamType>String</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>http://example.com/blog</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>API Key</Label>
      <MaxLength>100</MaxLength>
      <Name>ApiKey</Name>
      <ParamType>String</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>123456789</Values>
    </Parameters>
    <ShowAdd>true</ShowAdd>
    <ShowDelete>true</ShowDelete>
    <ShowEdit>true</ShowEdit>
  </Settings>
  <ShowSettings>true</ShowSettings>
  <Version>1.0</Version>
</ManagedExtension>');
GO
INSERT INTO [be_DataStoreSettings] ([DataStoreSettingRowId],[BlogId],[ExtensionType],[ExtensionId],[Settings]) VALUES (56,'27604f05-86ad-47ef-9e05-950bb762570c',N'0',N'ResolveLinks',N'<?xml version="1.0" encoding="utf-16"?>
<ManagedExtension xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" Name="ResolveLinks">
  <AdminPage />
  <Author>BlogEngine.NET</Author>
  <Description>Auto resolves URLs in the comments and turn them into valid hyperlinks.</Description>
  <Enabled>true</Enabled>
  <Priority>0</Priority>
  <ShowSettings>true</ShowSettings>
  <Version>1.5</Version>
</ManagedExtension>');
GO
INSERT INTO [be_DataStoreSettings] ([DataStoreSettingRowId],[BlogId],[ExtensionType],[ExtensionId],[Settings]) VALUES (57,'27604f05-86ad-47ef-9e05-950bb762570c',N'0',N'Logger',N'<?xml version="1.0" encoding="utf-16"?>
<ManagedExtension xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" Name="Logger">
  <AdminPage />
  <Author>BlogEngine.NET</Author>
  <Description>Subscribes to Log events and records the events in a file.</Description>
  <Enabled>true</Enabled>
  <Priority>0</Priority>
  <ShowSettings>true</ShowSettings>
  <Version>1.0</Version>
</ManagedExtension>');
GO
INSERT INTO [be_DataStoreSettings] ([DataStoreSettingRowId],[BlogId],[ExtensionType],[ExtensionId],[Settings]) VALUES (58,'27604f05-86ad-47ef-9e05-950bb762570c',N'0',N'Recaptcha',N'<?xml version="1.0" encoding="utf-16"?>
<ManagedExtension xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" Name="Recaptcha">
  <AdminPage />
  <Author>&lt;a href="http://www.bloodforge.com"&gt;Bloodforge.com&lt;/a&gt;</Author>
  <Description>Settings for the Recaptcha control</Description>
  <Enabled>false</Enabled>
  <Priority>0</Priority>
  <Settings>
    <Delimiter>44</Delimiter>
    <Help>
&lt;script type=''text/javascript''&gt;
function showRecaptchaLog() {
        window.scrollTo(0, 0);
        var width = document.documentElement.clientWidth + document.documentElement.scrollLeft;
        var height = document.documentElement.clientHeight + document.documentElement.scrollTop;

        var layer = document.createElement(''div'');
        layer.style.zIndex = 1002;
        layer.id = ''RecaptchaLogLayer'';
        layer.style.position = ''absolute'';
        layer.style.top = ''0px'';
        layer.style.left = ''0px'';
        layer.style.height = document.documentElement.scrollHeight + ''px'';
        layer.style.width = width + ''px'';
        layer.style.backgroundColor = ''black'';
        layer.style.opacity = ''.6'';
        layer.style.filter += (''progid:DXImageTransform.Microsoft.Alpha(opacity=60)'');
        document.body.style.position = ''static'';
        document.body.appendChild(layer);

        var size = { ''height'': 500, ''width'': 750 };
        var iframe = document.createElement(''iframe'');
        iframe.name = ''Recaptcha Log Viewer'';
        iframe.id = ''RecaptchaLogDetails'';
        iframe.src = ''../Pages/RecaptchaLogViewer.aspx'';
        iframe.style.height = size.height + ''px'';
        iframe.style.width = size.width + ''px'';
        iframe.style.position = ''fixed'';
        iframe.style.zIndex = 1003;
        iframe.style.backgroundColor = ''white'';
        iframe.style.border = ''4px solid silver'';
        iframe.frameborder = ''0'';

        iframe.style.top = ((height + document.documentElement.scrollTop) / 2) - (size.height / 2) + ''px'';
        iframe.style.left = (width / 2) - (size.width / 2) + ''px'';

        document.body.appendChild(iframe);
        return false;
    }
&lt;/script&gt;
You can create your own public key at &lt;a href=''http://www.Recaptcha.net''&gt;http://www.Recaptcha.net&lt;/a&gt;. This is used for communication between your website and the recapcha server.&lt;br /&gt;&lt;br /&gt;Please rememeber you need to &lt;span style="color:red"&gt;enable extension&lt;/span&gt; for reCaptcha to show up on the comments form.&lt;br /&gt;&lt;br /&gt;You can see some statistics on Captcha solving by storing successful attempts. If you''re getting spam, this should also confirm that the spammers are at least solving the captcha.&lt;br /&gt;&lt;br /&gt;&lt;a href=''../Pages/RecaptchaLogViewer.aspx'' target=''_blank'' onclick=''return showRecaptchaLog()''&gt;Click here to view the log&lt;/a&gt;</Help>
    <Hidden>false</Hidden>
    <Index>0</Index>
    <IsScalar>true</IsScalar>
    <KeyField>PublicKey</KeyField>
    <Name>Recaptcha</Name>
    <Parameters>
      <KeyField>true</KeyField>
      <Label>Public Key</Label>
      <MaxLength>50</MaxLength>
      <Name>PublicKey</Name>
      <ParamType>String</ParamType>
      <Required>true</Required>
      <SelectedValue />
      <Values>YOURPUBLICKEY</Values>
    </Parameters>
    <Parameters>
      <KeyField>true</KeyField>
      <Label>Private Key</Label>
      <MaxLength>50</MaxLength>
      <Name>PrivateKey</Name>
      <ParamType>String</ParamType>
      <Required>true</Required>
      <SelectedValue />
      <Values>YOURPRIVATEKEY</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Show Captcha For Authenticated Users</Label>
      <MaxLength>1</MaxLength>
      <Name>ShowForAuthenticatedUsers</Name>
      <ParamType>Boolean</ParamType>
      <Required>true</Required>
      <SelectedValue />
      <Values>False</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Logging: Maximum successful recaptcha attempts to store (set to 0 to disable logging)</Label>
      <MaxLength>4</MaxLength>
      <Name>MaxLogEntries</Name>
      <ParamType>Integer</ParamType>
      <Required>true</Required>
      <SelectedValue />
      <Values>50</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Theme</Label>
      <MaxLength>20</MaxLength>
      <Name>Theme</Name>
      <ParamType>DropDown</ParamType>
      <Required>true</Required>
      <SelectedValue>white</SelectedValue>
      <Values>red</Values>
      <Values>white</Values>
      <Values>blackglass</Values>
      <Values>clean</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Language</Label>
      <MaxLength>5</MaxLength>
      <Name>Language</Name>
      <ParamType>DropDown</ParamType>
      <Required>true</Required>
      <SelectedValue>en</SelectedValue>
      <Values>en|English</Values>
      <Values>nl|Dutch</Values>
      <Values>fr|French</Values>
      <Values>de|German</Values>
      <Values>pt|Portuguese</Values>
      <Values>ru|Russian</Values>
      <Values>es|Spanish</Values>
      <Values>tr|Turkish</Values>
    </Parameters>
    <ShowAdd>true</ShowAdd>
    <ShowDelete>true</ShowDelete>
    <ShowEdit>true</ShowEdit>
  </Settings>
  <Settings>
    <Delimiter>44</Delimiter>
    <Help />
    <Hidden>true</Hidden>
    <Index>1</Index>
    <IsScalar>false</IsScalar>
    <KeyField>Response</KeyField>
    <Name>RecaptchaLog</Name>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Response</Label>
      <MaxLength>100</MaxLength>
      <Name>Response</Name>
      <ParamType>String</ParamType>
      <Required>false</Required>
      <SelectedValue />
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Challenge</Label>
      <MaxLength>100</MaxLength>
      <Name>Challenge</Name>
      <ParamType>String</ParamType>
      <Required>false</Required>
      <SelectedValue />
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>CommentID</Label>
      <MaxLength>100</MaxLength>
      <Name>CommentID</Name>
      <ParamType>String</ParamType>
      <Required>false</Required>
      <SelectedValue />
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>TimeToComment</Label>
      <MaxLength>100</MaxLength>
      <Name>TimeToComment</Name>
      <ParamType>String</ParamType>
      <Required>false</Required>
      <SelectedValue />
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>TimeToSolveCapcha</Label>
      <MaxLength>100</MaxLength>
      <Name>TimeToSolveCapcha</Name>
      <ParamType>String</ParamType>
      <Required>false</Required>
      <SelectedValue />
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>NumberOfAttempts</Label>
      <MaxLength>100</MaxLength>
      <Name>NumberOfAttempts</Name>
      <ParamType>String</ParamType>
      <Required>false</Required>
      <SelectedValue />
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Enabled</Label>
      <MaxLength>100</MaxLength>
      <Name>Enabled</Name>
      <ParamType>String</ParamType>
      <Required>false</Required>
      <SelectedValue />
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Necessary</Label>
      <MaxLength>100</MaxLength>
      <Name>Necessary</Name>
      <ParamType>String</ParamType>
      <Required>false</Required>
      <SelectedValue />
    </Parameters>
    <ShowAdd>true</ShowAdd>
    <ShowDelete>true</ShowDelete>
    <ShowEdit>true</ShowEdit>
  </Settings>
  <ShowSettings>true</ShowSettings>
  <Version>1.1</Version>
</ManagedExtension>');
GO
INSERT INTO [be_DataStoreSettings] ([DataStoreSettingRowId],[BlogId],[ExtensionType],[ExtensionId],[Settings]) VALUES (59,'27604f05-86ad-47ef-9e05-950bb762570c',N'0',N'BreakPost',N'<?xml version="1.0" encoding="utf-16"?>
<ManagedExtension xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" Name="BreakPost">
  <AdminPage />
  <Author>BlogEngine.NET</Author>
  <Description>Breaks a post where [more] is found in the body and adds a link to full post</Description>
  <Enabled>true</Enabled>
  <Priority>0</Priority>
  <ShowSettings>true</ShowSettings>
  <Version>1.4</Version>
</ManagedExtension>');
GO
INSERT INTO [be_DataStoreSettings] ([DataStoreSettingRowId],[BlogId],[ExtensionType],[ExtensionId],[Settings]) VALUES (60,'27604f05-86ad-47ef-9e05-950bb762570c',N'0',N'Smilies',N'<?xml version="1.0" encoding="utf-16"?>
<ManagedExtension xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" Name="Smilies">
  <AdminPage />
  <Author>BlogEngine.NET</Author>
  <Description>Converts ASCII smilies into real smilies in the comments</Description>
  <Enabled>true</Enabled>
  <Priority>0</Priority>
  <ShowSettings>true</ShowSettings>
  <Version>1.3</Version>
</ManagedExtension>');
GO
INSERT INTO [be_DataStoreSettings] ([DataStoreSettingRowId],[BlogId],[ExtensionType],[ExtensionId],[Settings]) VALUES (61,'27604f05-86ad-47ef-9e05-950bb762570c',N'0',N'TypePadFilter',N'<?xml version="1.0" encoding="utf-16"?>
<ManagedExtension xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" Name="TypePadFilter">
  <AdminPage />
  <Author>&lt;a href="http://lucsiferre.net"&gt;By Chris Nicola&lt;/a&gt;</Author>
  <Description>TypePad anti-spam comment filter (based on AkismetFilter)</Description>
  <Enabled>false</Enabled>
  <Priority>0</Priority>
  <Settings>
    <Delimiter>44</Delimiter>
    <Help />
    <Hidden>false</Hidden>
    <Index>0</Index>
    <IsScalar>true</IsScalar>
    <KeyField>SiteURL</KeyField>
    <Name>TypePadFilter</Name>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Site URL</Label>
      <MaxLength>100</MaxLength>
      <Name>SiteURL</Name>
      <ParamType>String</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>http://example.com/blog</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>API Key</Label>
      <MaxLength>100</MaxLength>
      <Name>ApiKey</Name>
      <ParamType>String</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>123456789</Values>
    </Parameters>
    <ShowAdd>true</ShowAdd>
    <ShowDelete>true</ShowDelete>
    <ShowEdit>true</ShowEdit>
  </Settings>
  <ShowSettings>true</ShowSettings>
  <Version>1.0</Version>
</ManagedExtension>');
GO
INSERT INTO [be_DataStoreSettings] ([DataStoreSettingRowId],[BlogId],[ExtensionType],[ExtensionId],[Settings]) VALUES (62,'27604f05-86ad-47ef-9e05-950bb762570c',N'0',N'SendPings',N'<?xml version="1.0" encoding="utf-16"?>
<ManagedExtension xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" Name="SendPings">
  <AdminPage />
  <Author>BlogEngine.NET</Author>
  <Description>Pings all the ping services specified on the PingServices admin page and send track- and pingbacks</Description>
  <Enabled>true</Enabled>
  <Priority>0</Priority>
  <ShowSettings>true</ShowSettings>
  <Version>1.3</Version>
</ManagedExtension>');
GO
INSERT INTO [be_DataStoreSettings] ([DataStoreSettingRowId],[BlogId],[ExtensionType],[ExtensionId],[Settings]) VALUES (63,'27604f05-86ad-47ef-9e05-950bb762570c',N'0',N'SyntaxHighlighter',N'<?xml version="1.0" encoding="utf-16"?>
<ManagedExtension xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" Name="SyntaxHighlighter">
  <AdminPage />
  <Author>&lt;a target="_new" href="http://dotnetblogengine.net/"&gt;BlogEngine.NET&lt;/a&gt;</Author>
  <Description>Adds &lt;a target="_new" href="http://alexgorbatchev.com/wiki/SyntaxHighlighter"&gt;Alex Gorbatchev''s&lt;/a&gt; source code formatter</Description>
  <Enabled>true</Enabled>
  <Priority>0</Priority>
  <Settings>
    <Delimiter>44</Delimiter>
    <Help>&lt;p&gt;This extension implements excellent Alex Gorbatchev''s syntax highlighter JS library for source code formatting. Please refer to &lt;a target="_new" href="http://alexgorbatchev.com/wiki/SyntaxHighlighter:Usage"&gt;this site&lt;/a&gt; for usage.&lt;/p&gt;
&lt;p&gt;&lt;b&gt;auto-links&lt;/b&gt;: Allows you to turn detection of links in the highlighted element on and off. If the option is turned off, URLs won''t be clickable.&lt;/p&gt;
&lt;p&gt;&lt;b&gt;collapse&lt;/b&gt;: Allows you to force highlighted elements on the page to be collapsed by default.&lt;/p&gt;
&lt;p&gt;&lt;b&gt;gutter&lt;/b&gt;:	Allows you to turn gutter with line numbers on and off.&lt;/p&gt;
&lt;p&gt;&lt;b&gt;light&lt;/b&gt;: Allows you to disable toolbar and gutter with a single property.&lt;/p&gt;
&lt;p&gt;&lt;b&gt;smart-tabs&lt;/b&gt;:	Allows you to turn smart tabs feature on and off.&lt;/p&gt;
&lt;p&gt;&lt;b&gt;tab-size&lt;/b&gt;: Allows you to adjust tab size.&lt;/p&gt;
&lt;p&gt;&lt;b&gt;toolbar&lt;/b&gt;: Toggles toolbar on/off.&lt;/p&gt;
&lt;p&gt;&lt;b&gt;wrap-lines&lt;/b&gt;: Allows you to turn line wrapping feature on and off.&lt;/p&gt;
&lt;p&gt;&lt;a target="_new" href="http://alexgorbatchev.com/wiki/SyntaxHighlighter:Configuration"&gt;more...&lt;/a&gt;&lt;/p&gt;
</Help>
    <Hidden>false</Hidden>
    <Index>0</Index>
    <IsScalar>true</IsScalar>
    <KeyField>gutter</KeyField>
    <Name>Options</Name>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Gutter</Label>
      <MaxLength>100</MaxLength>
      <Name>gutter</Name>
      <ParamType>Boolean</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>True</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Smart tabs</Label>
      <MaxLength>100</MaxLength>
      <Name>smart-tabs</Name>
      <ParamType>Boolean</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>True</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Auto links</Label>
      <MaxLength>100</MaxLength>
      <Name>auto-links</Name>
      <ParamType>Boolean</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>True</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Collapse</Label>
      <MaxLength>100</MaxLength>
      <Name>collapse</Name>
      <ParamType>Boolean</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>False</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Light</Label>
      <MaxLength>100</MaxLength>
      <Name>light</Name>
      <ParamType>Boolean</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>False</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Tab size</Label>
      <MaxLength>100</MaxLength>
      <Name>tab-size</Name>
      <ParamType>Integer</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>4</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Toolbar</Label>
      <MaxLength>100</MaxLength>
      <Name>toolbar</Name>
      <ParamType>Boolean</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>True</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Wrap lines</Label>
      <MaxLength>100</MaxLength>
      <Name>wrap-lines</Name>
      <ParamType>Boolean</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>True</Values>
    </Parameters>
    <ShowAdd>true</ShowAdd>
    <ShowDelete>true</ShowDelete>
    <ShowEdit>true</ShowEdit>
  </Settings>
  <Settings>
    <Delimiter>44</Delimiter>
    <Help />
    <Hidden>false</Hidden>
    <Index>1</Index>
    <IsScalar>true</IsScalar>
    <KeyField>shBrushBash</KeyField>
    <Name>Brushes</Name>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Bash (bash, shell)</Label>
      <MaxLength>100</MaxLength>
      <Name>shBrushBash</Name>
      <ParamType>Boolean</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>False</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>C++ (cpp, c)</Label>
      <MaxLength>100</MaxLength>
      <Name>shBrushCpp</Name>
      <ParamType>Boolean</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>False</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>C# (c-sharp, csharp)</Label>
      <MaxLength>100</MaxLength>
      <Name>shBrushCSharp</Name>
      <ParamType>Boolean</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>True</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Css (css)</Label>
      <MaxLength>100</MaxLength>
      <Name>shBrushCss</Name>
      <ParamType>Boolean</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>True</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Delphi (delphi, pas, pascal)</Label>
      <MaxLength>100</MaxLength>
      <Name>shBrushDelphi</Name>
      <ParamType>Boolean</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>False</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Diff (diff, patch)</Label>
      <MaxLength>100</MaxLength>
      <Name>shBrushDiff</Name>
      <ParamType>Boolean</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>False</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Groovy (groovy)</Label>
      <MaxLength>100</MaxLength>
      <Name>shBrushGroovy</Name>
      <ParamType>Boolean</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>False</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Java (java)</Label>
      <MaxLength>100</MaxLength>
      <Name>shBrushJava</Name>
      <ParamType>Boolean</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>False</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>JScript (js, jscript, javascript)</Label>
      <MaxLength>100</MaxLength>
      <Name>shBrushJScript</Name>
      <ParamType>Boolean</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>True</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>PHP (php)</Label>
      <MaxLength>100</MaxLength>
      <Name>shBrushPhp</Name>
      <ParamType>Boolean</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>False</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Plain (plain, text)</Label>
      <MaxLength>100</MaxLength>
      <Name>shBrushPlain</Name>
      <ParamType>Boolean</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>True</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Python (py, python)</Label>
      <MaxLength>100</MaxLength>
      <Name>shBrushPython</Name>
      <ParamType>Boolean</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>False</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Ruby (rails, ror, ruby)</Label>
      <MaxLength>100</MaxLength>
      <Name>shBrushRuby</Name>
      <ParamType>Boolean</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>False</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Scala (scala)</Label>
      <MaxLength>100</MaxLength>
      <Name>shBrushScala</Name>
      <ParamType>Boolean</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>False</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>SQL (sql)</Label>
      <MaxLength>100</MaxLength>
      <Name>shBrushSql</Name>
      <ParamType>Boolean</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>True</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>VB (vb, vbnet)</Label>
      <MaxLength>100</MaxLength>
      <Name>shBrushVb</Name>
      <ParamType>Boolean</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>True</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>XML (xml, xhtml, xslt, html, xhtml)</Label>
      <MaxLength>100</MaxLength>
      <Name>shBrushXml</Name>
      <ParamType>Boolean</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>True</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Cold Fusion (cf, coldfusion)</Label>
      <MaxLength>100</MaxLength>
      <Name>shBrushColdFusion</Name>
      <ParamType>Boolean</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>False</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Erlang (erlang, erl)</Label>
      <MaxLength>100</MaxLength>
      <Name>shBrushErlang</Name>
      <ParamType>Boolean</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>False</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>JavaFX (jfx, javafx)</Label>
      <MaxLength>100</MaxLength>
      <Name>shBrushJavaFX</Name>
      <ParamType>Boolean</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>False</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Perl (perl, pl)</Label>
      <MaxLength>100</MaxLength>
      <Name>shBrushPerl</Name>
      <ParamType>Boolean</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>False</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>PowerSell (ps, powershell)</Label>
      <MaxLength>100</MaxLength>
      <Name>shBrushPowerShell</Name>
      <ParamType>Boolean</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>False</Values>
    </Parameters>
    <ShowAdd>true</ShowAdd>
    <ShowDelete>true</ShowDelete>
    <ShowEdit>true</ShowEdit>
  </Settings>
  <Settings>
    <Delimiter>44</Delimiter>
    <Help />
    <Hidden>false</Hidden>
    <Index>2</Index>
    <IsScalar>true</IsScalar>
    <KeyField>SelectedTheme</KeyField>
    <Name>Themes</Name>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Themes</Label>
      <MaxLength>20</MaxLength>
      <Name>SelectedTheme</Name>
      <ParamType>ListBox</ParamType>
      <Required>false</Required>
      <SelectedValue>Default</SelectedValue>
      <Values>Default</Values>
      <Values>Django</Values>
      <Values>Eclipse</Values>
      <Values>Emacs</Values>
      <Values>FadeToGrey</Values>
      <Values>MDUltra</Values>
      <Values>Midnight</Values>
      <Values>Dark</Values>
    </Parameters>
    <ShowAdd>true</ShowAdd>
    <ShowDelete>true</ShowDelete>
    <ShowEdit>true</ShowEdit>
  </Settings>
  <ShowSettings>true</ShowSettings>
  <Version>2.5</Version>
</ManagedExtension>');
GO
INSERT INTO [be_DataStoreSettings] ([DataStoreSettingRowId],[BlogId],[ExtensionType],[ExtensionId],[Settings]) VALUES (64,'27604f05-86ad-47ef-9e05-950bb762570c',N'0',N'CodeFormatterExtension',N'<?xml version="1.0" encoding="utf-16"?>
<ManagedExtension xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" Name="CodeFormatterExtension">
  <AdminPage />
  <Author>www.manoli.net</Author>
  <Description>Converts text to formatted syntax highlighted code (beta).</Description>
  <Enabled>true</Enabled>
  <Priority>0</Priority>
  <ShowSettings>true</ShowSettings>
  <Version>0.1</Version>
</ManagedExtension>');
GO
INSERT INTO [be_DataStoreSettings] ([DataStoreSettingRowId],[BlogId],[ExtensionType],[ExtensionId],[Settings]) VALUES (50,'27604f05-86ad-47ef-9e05-950bb762570c',N'0',N'MetaExtension',N'<?xml version="1.0" encoding="utf-16"?>
<ManagedExtension xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" Name="MetaExtension">
  <AdminPage />
  <Author>BlogEngine.net</Author>
  <Description>Meta extension</Description>
  <Enabled>true</Enabled>
  <Priority>0</Priority>
  <Settings>
    <Delimiter>44</Delimiter>
    <Help />
    <Hidden>false</Hidden>
    <Index>0</Index>
    <IsScalar>false</IsScalar>
    <KeyField>ID</KeyField>
    <Name>BeCommentFilters</Name>
    <Parameters>
      <KeyField>true</KeyField>
      <Label>ID</Label>
      <MaxLength>20</MaxLength>
      <Name>ID</Name>
      <ParamType>Integer</ParamType>
      <Required>true</Required>
      <SelectedValue />
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Action</Label>
      <MaxLength>100</MaxLength>
      <Name>Action</Name>
      <ParamType>String</ParamType>
      <Required>false</Required>
      <SelectedValue />
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Subject</Label>
      <MaxLength>100</MaxLength>
      <Name>Subject</Name>
      <ParamType>String</ParamType>
      <Required>false</Required>
      <SelectedValue />
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Operator</Label>
      <MaxLength>100</MaxLength>
      <Name>Operator</Name>
      <ParamType>String</ParamType>
      <Required>false</Required>
      <SelectedValue />
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Filter</Label>
      <MaxLength>100</MaxLength>
      <Name>Filter</Name>
      <ParamType>String</ParamType>
      <Required>false</Required>
      <SelectedValue />
    </Parameters>
    <ShowAdd>true</ShowAdd>
    <ShowDelete>true</ShowDelete>
    <ShowEdit>true</ShowEdit>
  </Settings>
  <Settings>
    <Delimiter>44</Delimiter>
    <Help />
    <Hidden>false</Hidden>
    <Index>1</Index>
    <IsScalar>false</IsScalar>
    <KeyField>FullName</KeyField>
    <Name>BeCustomFilters</Name>
    <Parameters>
      <KeyField>true</KeyField>
      <Label>Name</Label>
      <MaxLength>100</MaxLength>
      <Name>FullName</Name>
      <ParamType>String</ParamType>
      <Required>true</Required>
      <SelectedValue />
      <Values>App_Code.Extensions.AkismetFilter</Values>
      <Values>App_Code.Extensions.StopForumSpam</Values>
      <Values>App_Code.Extensions.TypePadFilter</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Name</Label>
      <MaxLength>100</MaxLength>
      <Name>Name</Name>
      <ParamType>String</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>AkismetFilter</Values>
      <Values>StopForumSpam</Values>
      <Values>TypePadFilter</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Checked</Label>
      <MaxLength>100</MaxLength>
      <Name>Checked</Name>
      <ParamType>String</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>0</Values>
      <Values>0</Values>
      <Values>0</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Cought</Label>
      <MaxLength>100</MaxLength>
      <Name>Cought</Name>
      <ParamType>String</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>0</Values>
      <Values>0</Values>
      <Values>0</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Reported</Label>
      <MaxLength>100</MaxLength>
      <Name>Reported</Name>
      <ParamType>String</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>0</Values>
      <Values>0</Values>
      <Values>0</Values>
    </Parameters>
    <Parameters>
      <KeyField>false</KeyField>
      <Label>Priority</Label>
      <MaxLength>100</MaxLength>
      <Name>Priority</Name>
      <ParamType>String</ParamType>
      <Required>false</Required>
      <SelectedValue />
      <Values>0</Values>
      <Values>0</Values>
      <Values>0</Values>
    </Parameters>
    <ShowAdd>true</ShowAdd>
    <ShowDelete>true</ShowDelete>
    <ShowEdit>true</ShowEdit>
  </Settings>
  <ShowSettings>true</ShowSettings>
  <Version>1.0</Version>
</ManagedExtension>');
GO
SET IDENTITY_INSERT [be_DataStoreSettings] OFF;
GO
INSERT INTO [be_PackageFiles] ([PackageId],[FileOrder],[FilePath],[IsDirectory]) VALUES (N'JQ-Mobile',1,N'D:\Src\Hg\be\BlogEngine\BlogEngine.NET\App_Code\JQ-Mobile',1);
GO
INSERT INTO [be_PackageFiles] ([PackageId],[FileOrder],[FilePath],[IsDirectory]) VALUES (N'JQ-Mobile',2,N'D:\Src\Hg\be\BlogEngine\BlogEngine.NET\App_Code\JQ-Mobile\ThemeHelper.cs',0);
GO
INSERT INTO [be_PackageFiles] ([PackageId],[FileOrder],[FilePath],[IsDirectory]) VALUES (N'JQ-Mobile',3,N'D:\Src\Hg\be\BlogEngine\BlogEngine.NET\themes\JQ-Mobile',1);
GO
INSERT INTO [be_PackageFiles] ([PackageId],[FileOrder],[FilePath],[IsDirectory]) VALUES (N'JQ-Mobile',4,N'D:\Src\Hg\be\BlogEngine\BlogEngine.NET\themes\JQ-Mobile\controls',1);
GO
INSERT INTO [be_PackageFiles] ([PackageId],[FileOrder],[FilePath],[IsDirectory]) VALUES (N'JQ-Mobile',5,N'D:\Src\Hg\be\BlogEngine\BlogEngine.NET\themes\JQ-Mobile\controls\CommentView.ascx',0);
GO
INSERT INTO [be_PackageFiles] ([PackageId],[FileOrder],[FilePath],[IsDirectory]) VALUES (N'JQ-Mobile',6,N'D:\Src\Hg\be\BlogEngine\BlogEngine.NET\themes\JQ-Mobile\controls\CommentView.ascx.cs',0);
GO
INSERT INTO [be_PackageFiles] ([PackageId],[FileOrder],[FilePath],[IsDirectory]) VALUES (N'JQ-Mobile',7,N'D:\Src\Hg\be\BlogEngine\BlogEngine.NET\themes\JQ-Mobile\controls\Header.ascx',0);
GO
INSERT INTO [be_PackageFiles] ([PackageId],[FileOrder],[FilePath],[IsDirectory]) VALUES (N'JQ-Mobile',8,N'D:\Src\Hg\be\BlogEngine\BlogEngine.NET\themes\JQ-Mobile\controls\Header.ascx.cs',0);
GO
INSERT INTO [be_PackageFiles] ([PackageId],[FileOrder],[FilePath],[IsDirectory]) VALUES (N'JQ-Mobile',9,N'D:\Src\Hg\be\BlogEngine\BlogEngine.NET\themes\JQ-Mobile\controls\MainHeader.ascx',0);
GO
INSERT INTO [be_PackageFiles] ([PackageId],[FileOrder],[FilePath],[IsDirectory]) VALUES (N'JQ-Mobile',10,N'D:\Src\Hg\be\BlogEngine\BlogEngine.NET\themes\JQ-Mobile\controls\MainHeader.ascx.cs',0);
GO
INSERT INTO [be_PackageFiles] ([PackageId],[FileOrder],[FilePath],[IsDirectory]) VALUES (N'JQ-Mobile',11,N'D:\Src\Hg\be\BlogEngine\BlogEngine.NET\themes\JQ-Mobile\controls\Pager.ascx',0);
GO
INSERT INTO [be_PackageFiles] ([PackageId],[FileOrder],[FilePath],[IsDirectory]) VALUES (N'JQ-Mobile',12,N'D:\Src\Hg\be\BlogEngine\BlogEngine.NET\themes\JQ-Mobile\controls\Pager.ascx.cs',0);
GO
INSERT INTO [be_PackageFiles] ([PackageId],[FileOrder],[FilePath],[IsDirectory]) VALUES (N'JQ-Mobile',13,N'D:\Src\Hg\be\BlogEngine\BlogEngine.NET\themes\JQ-Mobile\controls\PostList.ascx',0);
GO
INSERT INTO [be_PackageFiles] ([PackageId],[FileOrder],[FilePath],[IsDirectory]) VALUES (N'JQ-Mobile',14,N'D:\Src\Hg\be\BlogEngine\BlogEngine.NET\themes\JQ-Mobile\controls\PostList.ascx.cs',0);
GO
INSERT INTO [be_PackageFiles] ([PackageId],[FileOrder],[FilePath],[IsDirectory]) VALUES (N'JQ-Mobile',15,N'D:\Src\Hg\be\BlogEngine\BlogEngine.NET\themes\JQ-Mobile\Archive.aspx',0);
GO
INSERT INTO [be_PackageFiles] ([PackageId],[FileOrder],[FilePath],[IsDirectory]) VALUES (N'JQ-Mobile',16,N'D:\Src\Hg\be\BlogEngine\BlogEngine.NET\themes\JQ-Mobile\Archive.aspx.cs',0);
GO
INSERT INTO [be_PackageFiles] ([PackageId],[FileOrder],[FilePath],[IsDirectory]) VALUES (N'JQ-Mobile',17,N'D:\Src\Hg\be\BlogEngine\BlogEngine.NET\themes\JQ-Mobile\CommentView.ascx',0);
GO
INSERT INTO [be_PackageFiles] ([PackageId],[FileOrder],[FilePath],[IsDirectory]) VALUES (N'JQ-Mobile',18,N'D:\Src\Hg\be\BlogEngine\BlogEngine.NET\themes\JQ-Mobile\Contact.aspx',0);
GO
INSERT INTO [be_PackageFiles] ([PackageId],[FileOrder],[FilePath],[IsDirectory]) VALUES (N'JQ-Mobile',19,N'D:\Src\Hg\be\BlogEngine\BlogEngine.NET\themes\JQ-Mobile\Contact.aspx.cs',0);
GO
INSERT INTO [be_PackageFiles] ([PackageId],[FileOrder],[FilePath],[IsDirectory]) VALUES (N'JQ-Mobile',20,N'D:\Src\Hg\be\BlogEngine\BlogEngine.NET\themes\JQ-Mobile\logo.png',0);
GO
INSERT INTO [be_PackageFiles] ([PackageId],[FileOrder],[FilePath],[IsDirectory]) VALUES (N'JQ-Mobile',21,N'D:\Src\Hg\be\BlogEngine\BlogEngine.NET\themes\JQ-Mobile\newsletter.html',0);
GO
INSERT INTO [be_PackageFiles] ([PackageId],[FileOrder],[FilePath],[IsDirectory]) VALUES (N'JQ-Mobile',22,N'D:\Src\Hg\be\BlogEngine\BlogEngine.NET\themes\JQ-Mobile\Post.aspx',0);
GO
INSERT INTO [be_PackageFiles] ([PackageId],[FileOrder],[FilePath],[IsDirectory]) VALUES (N'JQ-Mobile',23,N'D:\Src\Hg\be\BlogEngine\BlogEngine.NET\themes\JQ-Mobile\Post.aspx.cs',0);
GO
INSERT INTO [be_PackageFiles] ([PackageId],[FileOrder],[FilePath],[IsDirectory]) VALUES (N'JQ-Mobile',24,N'D:\Src\Hg\be\BlogEngine\BlogEngine.NET\themes\JQ-Mobile\PostView.ascx',0);
GO
INSERT INTO [be_PackageFiles] ([PackageId],[FileOrder],[FilePath],[IsDirectory]) VALUES (N'JQ-Mobile',25,N'D:\Src\Hg\be\BlogEngine\BlogEngine.NET\themes\JQ-Mobile\PostView.ascx.cs',0);
GO
INSERT INTO [be_PackageFiles] ([PackageId],[FileOrder],[FilePath],[IsDirectory]) VALUES (N'JQ-Mobile',26,N'D:\Src\Hg\be\BlogEngine\BlogEngine.NET\themes\JQ-Mobile\Readme.txt',0);
GO
INSERT INTO [be_PackageFiles] ([PackageId],[FileOrder],[FilePath],[IsDirectory]) VALUES (N'JQ-Mobile',27,N'D:\Src\Hg\be\BlogEngine\BlogEngine.NET\themes\JQ-Mobile\Search.aspx',0);
GO
INSERT INTO [be_PackageFiles] ([PackageId],[FileOrder],[FilePath],[IsDirectory]) VALUES (N'JQ-Mobile',28,N'D:\Src\Hg\be\BlogEngine\BlogEngine.NET\themes\JQ-Mobile\Search.aspx.cs',0);
GO
INSERT INTO [be_PackageFiles] ([PackageId],[FileOrder],[FilePath],[IsDirectory]) VALUES (N'JQ-Mobile',29,N'D:\Src\Hg\be\BlogEngine\BlogEngine.NET\themes\JQ-Mobile\site.master',0);
GO
INSERT INTO [be_PackageFiles] ([PackageId],[FileOrder],[FilePath],[IsDirectory]) VALUES (N'JQ-Mobile',30,N'D:\Src\Hg\be\BlogEngine\BlogEngine.NET\themes\JQ-Mobile\site.master.cs',0);
GO
INSERT INTO [be_PackageFiles] ([PackageId],[FileOrder],[FilePath],[IsDirectory]) VALUES (N'JQ-Mobile',31,N'D:\Src\Hg\be\BlogEngine\BlogEngine.NET\themes\JQ-Mobile\style.css',0);
GO
INSERT INTO [be_PackageFiles] ([PackageId],[FileOrder],[FilePath],[IsDirectory]) VALUES (N'JQ-Mobile',32,N'D:\Src\Hg\be\BlogEngine\BlogEngine.NET\themes\JQ-Mobile\theme.png',0);
GO
INSERT INTO [be_PackageFiles] ([PackageId],[FileOrder],[FilePath],[IsDirectory]) VALUES (N'JQ-Mobile',33,N'D:\Src\Hg\be\BlogEngine\BlogEngine.NET\themes\JQ-Mobile\theme.xml',0);
GO
INSERT INTO [be_Packages] ([PackageId],[Version]) VALUES (N'JQ-Mobile',N'1.2.3');
GO
SET IDENTITY_INSERT [be_PingService] ON;
GO
INSERT INTO [be_PingService] ([PingServiceID],[BlogID],[Link]) VALUES (1,'27604f05-86ad-47ef-9e05-950bb762570c',N'http://rpc.technorati.com/rpc/ping');
GO
INSERT INTO [be_PingService] ([PingServiceID],[BlogID],[Link]) VALUES (2,'27604f05-86ad-47ef-9e05-950bb762570c',N'http://rpc.pingomatic.com/rpc2');
GO
INSERT INTO [be_PingService] ([PingServiceID],[BlogID],[Link]) VALUES (3,'27604f05-86ad-47ef-9e05-950bb762570c',N'http://ping.feedburner.com');
GO
INSERT INTO [be_PingService] ([PingServiceID],[BlogID],[Link]) VALUES (4,'27604f05-86ad-47ef-9e05-950bb762570c',N'http://www.bloglines.com/ping');
GO
INSERT INTO [be_PingService] ([PingServiceID],[BlogID],[Link]) VALUES (5,'27604f05-86ad-47ef-9e05-950bb762570c',N'http://services.newsgator.com/ngws/xmlrpcping.aspx');
GO
INSERT INTO [be_PingService] ([PingServiceID],[BlogID],[Link]) VALUES (6,'27604f05-86ad-47ef-9e05-950bb762570c',N'http://api.my.yahoo.com/rpc2 ');
GO
INSERT INTO [be_PingService] ([PingServiceID],[BlogID],[Link]) VALUES (7,'27604f05-86ad-47ef-9e05-950bb762570c',N'http://blogsearch.google.com/ping/RPC2');
GO
INSERT INTO [be_PingService] ([PingServiceID],[BlogID],[Link]) VALUES (8,'27604f05-86ad-47ef-9e05-950bb762570c',N'http://rpc.pingthesemanticweb.com/');
GO
SET IDENTITY_INSERT [be_PingService] OFF;
GO
SET IDENTITY_INSERT [be_PostCategory] ON;
GO
INSERT INTO [be_PostCategory] ([PostCategoryID],[BlogID],[PostID],[CategoryID]) VALUES (2,'27604f05-86ad-47ef-9e05-950bb762570c','10ecdf6f-2cf7-447c-8ff7-223833550716','05366547-5c8e-4643-ad94-377d3f809ab8');
GO
SET IDENTITY_INSERT [be_PostCategory] OFF;
GO
SET IDENTITY_INSERT [be_Posts] ON;
GO
INSERT INTO [be_Posts] ([PostRowID],[BlogID],[PostID],[Title],[Description],[PostContent],[DateCreated],[DateModified],[Author],[IsPublished],[IsCommentEnabled],[Raters],[Rating],[Slug],[IsDeleted]) VALUES (1,'27604f05-86ad-47ef-9e05-950bb762570c','10ecdf6f-2cf7-447c-8ff7-223833550716',N'Welcome to BlogEngine.NET 2.6 using Microsoft SQL CE',N'The description is used as the meta description as well as shown in the related posts. It is recommended that you write a description, but not mandatory',N'<p>If you see this post it means that BlogEngine.NET 2.6 is running and the hard part of creating your own blog is done. There is only a few things left to do.</p>
<h2>Write Permissions</h2>
<p>To be able to log in to the blog and writing posts, you need to enable write permissions on the App_Data folder. If you&rsquo;re blog is hosted at a hosting provider, you can either log into your account&rsquo;s admin page or call the support. You need write permissions on the App_Data folder because all posts, comments, and blog attachments are saved as XML files and placed in the App_Data folder.&nbsp;</p>
<p>If you wish to use a database to to store your blog data, we still encourage you to enable this write access for an images you may wish to store for your blog posts.&nbsp; If you are interested in using Microsoft SQL Server, MySQL, SQL CE, or other databases, please see the <a href="http://blogengine.codeplex.com/documentation">BlogEngine wiki</a> to get started.</p>
<h2>Security</h2>
<p>When you''ve got write permissions to the App_Data folder, you need to change the username and password. Find the sign-in link located either at the bottom or top of the page depending on your current theme and click it. Now enter "admin" in both the username and password fields and click the button. You will now see an admin menu appear. It has a link to the "Users" admin page. From there you can change the username and password.&nbsp; Passwords are hashed by default so if you lose your password, please see the <a href="http://blogengine.codeplex.com/documentation">BlogEngine wiki</a> for information on recovery.</p>
<h2>Configuration and Profile</h2>
<p>Now that you have your blog secured, take a look through the settings and give your new blog a title.&nbsp; BlogEngine.NET 2.6 is set up to take full advantage of of many semantic formats and technologies such as FOAF, SIOC and APML. It means that the content stored in your BlogEngine.NET installation will be fully portable and auto-discoverable.&nbsp; Be sure to fill in your author profile to take better advantage of this.</p>
<h2>Themes, Widgets &amp; Extensions</h2>
<p>One last thing to consider is customizing the look of your blog.&nbsp; We have a few themes available right out of the box including two fully setup to use our new widget framework.&nbsp; The widget framework allows drop and drag placement on your side bar as well as editing and configuration right in the widget while you are logged in.&nbsp; Extensions allow you to extend and customize the behaivor of your blog.&nbsp; Be sure to check the <a href="http://dnbegallery.org/">BlogEngine.NET Gallery</a> at <a href="http://dnbegallery.org/">dnbegallery.org</a> as the go-to location for downloading widgets, themes and extensions.</p>
<h2>On the web</h2>
<p>You can find BlogEngine.NET on the <a href="http://www.dotnetblogengine.net">official website</a>. Here you''ll find tutorials, documentation, tips and tricks and much more. The ongoing development of BlogEngine.NET can be followed at <a href="http://blogengine.codeplex.com/">CodePlex</a> where the daily builds will be published for anyone to download.&nbsp; Again, new themes, widgets and extensions can be downloaded at the <a href="http://dnbegallery.org/">BlogEngine.NET gallery</a>.</p>
<p>Good luck and happy writing.</p>
<p>The BlogEngine.NET team</p>',{ts '2012-04-22 10:00:00.000'},{ts '2012-04-22 10:00:00.000'},N'Admin',1,1,0,0,N'Welcome-to-BlogEngineNET-25-using-Microsoft-SQL-CE',0);
GO
SET IDENTITY_INSERT [be_Posts] OFF;
GO
SET IDENTITY_INSERT [be_PostTag] ON;
GO
INSERT INTO [be_PostTag] ([PostTagID],[BlogID],[PostID],[Tag]) VALUES (3,'27604f05-86ad-47ef-9e05-950bb762570c','10ecdf6f-2cf7-447c-8ff7-223833550716',N'blog');
GO
INSERT INTO [be_PostTag] ([PostTagID],[BlogID],[PostID],[Tag]) VALUES (4,'27604f05-86ad-47ef-9e05-950bb762570c','10ecdf6f-2cf7-447c-8ff7-223833550716',N'welcome');
GO
SET IDENTITY_INSERT [be_PostTag] OFF;
GO
SET IDENTITY_INSERT [be_RightRoles] ON;
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (1,'27604f05-86ad-47ef-9e05-950bb762570c',N'ViewDetailedErrorMessages',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (2,'27604f05-86ad-47ef-9e05-950bb762570c',N'AccessAdminPages',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (3,'27604f05-86ad-47ef-9e05-950bb762570c',N'AccessAdminPages',N'Editors');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (4,'27604f05-86ad-47ef-9e05-950bb762570c',N'AccessAdminSettingsPages',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (5,'27604f05-86ad-47ef-9e05-950bb762570c',N'ManageWidgets',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (6,'27604f05-86ad-47ef-9e05-950bb762570c',N'ViewPublicComments',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (7,'27604f05-86ad-47ef-9e05-950bb762570c',N'ViewPublicComments',N'Anonymous');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (8,'27604f05-86ad-47ef-9e05-950bb762570c',N'ViewPublicComments',N'Editors');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (9,'27604f05-86ad-47ef-9e05-950bb762570c',N'ViewUnmoderatedComments',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (10,'27604f05-86ad-47ef-9e05-950bb762570c',N'ViewUnmoderatedComments',N'Editors');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (11,'27604f05-86ad-47ef-9e05-950bb762570c',N'CreateComments',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (12,'27604f05-86ad-47ef-9e05-950bb762570c',N'CreateComments',N'Anonymous');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (13,'27604f05-86ad-47ef-9e05-950bb762570c',N'CreateComments',N'Editors');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (14,'27604f05-86ad-47ef-9e05-950bb762570c',N'ModerateComments',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (15,'27604f05-86ad-47ef-9e05-950bb762570c',N'ModerateComments',N'Editors');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (16,'27604f05-86ad-47ef-9e05-950bb762570c',N'ViewPublicPosts',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (17,'27604f05-86ad-47ef-9e05-950bb762570c',N'ViewPublicPosts',N'Anonymous');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (18,'27604f05-86ad-47ef-9e05-950bb762570c',N'ViewPublicPosts',N'Editors');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (19,'27604f05-86ad-47ef-9e05-950bb762570c',N'ViewUnpublishedPosts',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (20,'27604f05-86ad-47ef-9e05-950bb762570c',N'ViewUnpublishedPosts',N'Editors');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (21,'27604f05-86ad-47ef-9e05-950bb762570c',N'CreateNewPosts',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (22,'27604f05-86ad-47ef-9e05-950bb762570c',N'CreateNewPosts',N'Editors');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (23,'27604f05-86ad-47ef-9e05-950bb762570c',N'EditOwnPosts',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (24,'27604f05-86ad-47ef-9e05-950bb762570c',N'EditOwnPosts',N'Editors');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (25,'27604f05-86ad-47ef-9e05-950bb762570c',N'EditOtherUsersPosts',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (26,'27604f05-86ad-47ef-9e05-950bb762570c',N'DeleteOwnPosts',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (27,'27604f05-86ad-47ef-9e05-950bb762570c',N'DeleteOwnPosts',N'Editors');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (28,'27604f05-86ad-47ef-9e05-950bb762570c',N'DeleteOtherUsersPosts',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (29,'27604f05-86ad-47ef-9e05-950bb762570c',N'PublishOwnPosts',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (30,'27604f05-86ad-47ef-9e05-950bb762570c',N'PublishOwnPosts',N'Editors');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (31,'27604f05-86ad-47ef-9e05-950bb762570c',N'PublishOtherUsersPosts',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (32,'27604f05-86ad-47ef-9e05-950bb762570c',N'ViewPublicPages',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (33,'27604f05-86ad-47ef-9e05-950bb762570c',N'ViewPublicPages',N'Anonymous');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (34,'27604f05-86ad-47ef-9e05-950bb762570c',N'ViewPublicPages',N'Editors');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (35,'27604f05-86ad-47ef-9e05-950bb762570c',N'ViewUnpublishedPages',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (36,'27604f05-86ad-47ef-9e05-950bb762570c',N'ViewUnpublishedPages',N'Editors');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (37,'27604f05-86ad-47ef-9e05-950bb762570c',N'CreateNewPages',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (38,'27604f05-86ad-47ef-9e05-950bb762570c',N'CreateNewPages',N'Editors');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (39,'27604f05-86ad-47ef-9e05-950bb762570c',N'EditOwnPages',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (40,'27604f05-86ad-47ef-9e05-950bb762570c',N'EditOwnPages',N'Editors');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (41,'27604f05-86ad-47ef-9e05-950bb762570c',N'EditOtherUsersPages',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (42,'27604f05-86ad-47ef-9e05-950bb762570c',N'DeleteOwnPages',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (43,'27604f05-86ad-47ef-9e05-950bb762570c',N'DeleteOwnPages',N'Editors');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (44,'27604f05-86ad-47ef-9e05-950bb762570c',N'DeleteOtherUsersPages',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (45,'27604f05-86ad-47ef-9e05-950bb762570c',N'PublishOwnPages',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (46,'27604f05-86ad-47ef-9e05-950bb762570c',N'PublishOwnPages',N'Editors');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (47,'27604f05-86ad-47ef-9e05-950bb762570c',N'PublishOtherUsersPages',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (48,'27604f05-86ad-47ef-9e05-950bb762570c',N'ViewRatingsOnPosts',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (49,'27604f05-86ad-47ef-9e05-950bb762570c',N'ViewRatingsOnPosts',N'Anonymous');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (50,'27604f05-86ad-47ef-9e05-950bb762570c',N'ViewRatingsOnPosts',N'Editors');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (51,'27604f05-86ad-47ef-9e05-950bb762570c',N'SubmitRatingsOnPosts',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (52,'27604f05-86ad-47ef-9e05-950bb762570c',N'SubmitRatingsOnPosts',N'Anonymous');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (53,'27604f05-86ad-47ef-9e05-950bb762570c',N'SubmitRatingsOnPosts',N'Editors');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (54,'27604f05-86ad-47ef-9e05-950bb762570c',N'ViewRoles',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (55,'27604f05-86ad-47ef-9e05-950bb762570c',N'CreateNewRoles',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (56,'27604f05-86ad-47ef-9e05-950bb762570c',N'EditRoles',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (57,'27604f05-86ad-47ef-9e05-950bb762570c',N'DeleteRoles',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (58,'27604f05-86ad-47ef-9e05-950bb762570c',N'EditOwnRoles',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (59,'27604f05-86ad-47ef-9e05-950bb762570c',N'EditOtherUsersRoles',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (60,'27604f05-86ad-47ef-9e05-950bb762570c',N'CreateNewUsers',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (61,'27604f05-86ad-47ef-9e05-950bb762570c',N'DeleteUserSelf',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (62,'27604f05-86ad-47ef-9e05-950bb762570c',N'DeleteUsersOtherThanSelf',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (63,'27604f05-86ad-47ef-9e05-950bb762570c',N'EditOwnUser',N'Administrators');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (64,'27604f05-86ad-47ef-9e05-950bb762570c',N'EditOwnUser',N'Editors');
GO
INSERT INTO [be_RightRoles] ([RightRoleRowId],[BlogId],[RightName],[Role]) VALUES (65,'27604f05-86ad-47ef-9e05-950bb762570c',N'EditOtherUsers',N'Administrators');
GO
SET IDENTITY_INSERT [be_RightRoles] OFF;
GO
SET IDENTITY_INSERT [be_Rights] ON;
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (1,'27604f05-86ad-47ef-9e05-950bb762570c',N'None');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (2,'27604f05-86ad-47ef-9e05-950bb762570c',N'ViewDetailedErrorMessages');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (3,'27604f05-86ad-47ef-9e05-950bb762570c',N'AccessAdminPages');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (4,'27604f05-86ad-47ef-9e05-950bb762570c',N'AccessAdminSettingsPages');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (5,'27604f05-86ad-47ef-9e05-950bb762570c',N'ManageWidgets');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (6,'27604f05-86ad-47ef-9e05-950bb762570c',N'ViewPublicComments');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (7,'27604f05-86ad-47ef-9e05-950bb762570c',N'ViewUnmoderatedComments');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (8,'27604f05-86ad-47ef-9e05-950bb762570c',N'CreateComments');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (9,'27604f05-86ad-47ef-9e05-950bb762570c',N'ModerateComments');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (10,'27604f05-86ad-47ef-9e05-950bb762570c',N'ViewPublicPosts');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (11,'27604f05-86ad-47ef-9e05-950bb762570c',N'ViewUnpublishedPosts');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (12,'27604f05-86ad-47ef-9e05-950bb762570c',N'CreateNewPosts');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (13,'27604f05-86ad-47ef-9e05-950bb762570c',N'EditOwnPosts');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (14,'27604f05-86ad-47ef-9e05-950bb762570c',N'EditOtherUsersPosts');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (15,'27604f05-86ad-47ef-9e05-950bb762570c',N'DeleteOwnPosts');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (16,'27604f05-86ad-47ef-9e05-950bb762570c',N'DeleteOtherUsersPosts');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (17,'27604f05-86ad-47ef-9e05-950bb762570c',N'PublishOwnPosts');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (18,'27604f05-86ad-47ef-9e05-950bb762570c',N'PublishOtherUsersPosts');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (19,'27604f05-86ad-47ef-9e05-950bb762570c',N'ViewPublicPages');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (20,'27604f05-86ad-47ef-9e05-950bb762570c',N'ViewUnpublishedPages');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (21,'27604f05-86ad-47ef-9e05-950bb762570c',N'CreateNewPages');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (22,'27604f05-86ad-47ef-9e05-950bb762570c',N'EditOwnPages');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (23,'27604f05-86ad-47ef-9e05-950bb762570c',N'EditOtherUsersPages');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (24,'27604f05-86ad-47ef-9e05-950bb762570c',N'DeleteOwnPages');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (25,'27604f05-86ad-47ef-9e05-950bb762570c',N'DeleteOtherUsersPages');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (26,'27604f05-86ad-47ef-9e05-950bb762570c',N'PublishOwnPages');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (27,'27604f05-86ad-47ef-9e05-950bb762570c',N'PublishOtherUsersPages');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (28,'27604f05-86ad-47ef-9e05-950bb762570c',N'ViewRatingsOnPosts');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (29,'27604f05-86ad-47ef-9e05-950bb762570c',N'SubmitRatingsOnPosts');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (30,'27604f05-86ad-47ef-9e05-950bb762570c',N'ViewRoles');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (31,'27604f05-86ad-47ef-9e05-950bb762570c',N'CreateNewRoles');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (32,'27604f05-86ad-47ef-9e05-950bb762570c',N'EditRoles');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (33,'27604f05-86ad-47ef-9e05-950bb762570c',N'DeleteRoles');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (34,'27604f05-86ad-47ef-9e05-950bb762570c',N'EditOwnRoles');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (35,'27604f05-86ad-47ef-9e05-950bb762570c',N'EditOtherUsersRoles');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (36,'27604f05-86ad-47ef-9e05-950bb762570c',N'CreateNewUsers');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (37,'27604f05-86ad-47ef-9e05-950bb762570c',N'DeleteUserSelf');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (38,'27604f05-86ad-47ef-9e05-950bb762570c',N'DeleteUsersOtherThanSelf');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (39,'27604f05-86ad-47ef-9e05-950bb762570c',N'EditOwnUser');
GO
INSERT INTO [be_Rights] ([RightRowId],[BlogId],[RightName]) VALUES (40,'27604f05-86ad-47ef-9e05-950bb762570c',N'EditOtherUsers');
GO
SET IDENTITY_INSERT [be_Rights] OFF;
GO
SET IDENTITY_INSERT [be_Roles] ON;
GO
INSERT INTO [be_Roles] ([RoleID],[BlogID],[Role]) VALUES (1,'27604f05-86ad-47ef-9e05-950bb762570c',N'Administrators');
GO
INSERT INTO [be_Roles] ([RoleID],[BlogID],[Role]) VALUES (2,'27604f05-86ad-47ef-9e05-950bb762570c',N'Editors');
GO
INSERT INTO [be_Roles] ([RoleID],[BlogID],[Role]) VALUES (3,'27604f05-86ad-47ef-9e05-950bb762570c',N'Anonymous');
GO
SET IDENTITY_INSERT [be_Roles] OFF;
GO
SET IDENTITY_INSERT [be_Settings] ON;
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (1,'27604f05-86ad-47ef-9e05-950bb762570c',N'administratorrole',N'Administrators');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (2,'27604f05-86ad-47ef-9e05-950bb762570c',N'alternatefeedurl',N'');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (3,'27604f05-86ad-47ef-9e05-950bb762570c',N'authorname',N'My name');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (4,'27604f05-86ad-47ef-9e05-950bb762570c',N'avatar',N'combine');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (5,'27604f05-86ad-47ef-9e05-950bb762570c',N'blogrollmaxlength',N'23');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (6,'27604f05-86ad-47ef-9e05-950bb762570c',N'blogrollupdateminutes',N'60');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (7,'27604f05-86ad-47ef-9e05-950bb762570c',N'blogrollvisibleposts',N'3');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (8,'27604f05-86ad-47ef-9e05-950bb762570c',N'contactformmessage',N'<p>I will answer the mail as soon as I can.</p>');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (9,'27604f05-86ad-47ef-9e05-950bb762570c',N'contactthankmessage',N'<h1>Thank you</h1><p>The message was sent.</p>');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (10,'27604f05-86ad-47ef-9e05-950bb762570c',N'culture',N'Auto');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (11,'27604f05-86ad-47ef-9e05-950bb762570c',N'dayscommentsareenabled',N'0');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (12,'27604f05-86ad-47ef-9e05-950bb762570c',N'description',N'Short description of the blog');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (13,'27604f05-86ad-47ef-9e05-950bb762570c',N'displaycommentsonrecentposts',N'True');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (14,'27604f05-86ad-47ef-9e05-950bb762570c',N'displayratingsonrecentposts',N'True');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (15,'27604f05-86ad-47ef-9e05-950bb762570c',N'email',N'user@example.com');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (16,'27604f05-86ad-47ef-9e05-950bb762570c',N'emailsubjectprefix',N'Weblog');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (17,'27604f05-86ad-47ef-9e05-950bb762570c',N'enablecommentsearch',N'True');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (18,'27604f05-86ad-47ef-9e05-950bb762570c',N'enablecommentsmoderation',N'False');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (19,'27604f05-86ad-47ef-9e05-950bb762570c',N'enablecontactattachments',N'True');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (20,'27604f05-86ad-47ef-9e05-950bb762570c',N'enablecountryincomments',N'True');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (21,'27604f05-86ad-47ef-9e05-950bb762570c',N'enablehttpcompression',N'True');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (22,'27604f05-86ad-47ef-9e05-950bb762570c',N'enableopensearch',N'True');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (23,'27604f05-86ad-47ef-9e05-950bb762570c',N'enablepingbackreceive',N'True');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (24,'27604f05-86ad-47ef-9e05-950bb762570c',N'enablepingbacksend',N'True');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (25,'27604f05-86ad-47ef-9e05-950bb762570c',N'enablerating',N'True');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (26,'27604f05-86ad-47ef-9e05-950bb762570c',N'enablereferrertracking',N'False');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (27,'27604f05-86ad-47ef-9e05-950bb762570c',N'enablerelatedposts',N'True');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (28,'27604f05-86ad-47ef-9e05-950bb762570c',N'enablessl',N'False');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (29,'27604f05-86ad-47ef-9e05-950bb762570c',N'enabletrackbackreceive',N'True');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (30,'27604f05-86ad-47ef-9e05-950bb762570c',N'enabletrackbacksend',N'True');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (31,'27604f05-86ad-47ef-9e05-950bb762570c',N'endorsement',N'http://www.dotnetblogengine.net/syndication.axd');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (32,'27604f05-86ad-47ef-9e05-950bb762570c',N'fileextension',N'.aspx');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (33,'27604f05-86ad-47ef-9e05-950bb762570c',N'geocodinglatitude',N'0');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (34,'27604f05-86ad-47ef-9e05-950bb762570c',N'geocodinglongitude',N'0');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (35,'27604f05-86ad-47ef-9e05-950bb762570c',N'handlewwwsubdomain',N'');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (36,'27604f05-86ad-47ef-9e05-950bb762570c',N'iscocommentenabled',N'False');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (37,'27604f05-86ad-47ef-9e05-950bb762570c',N'iscommentsenabled',N'True');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (38,'27604f05-86ad-47ef-9e05-950bb762570c',N'language',N'en-GB');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (39,'27604f05-86ad-47ef-9e05-950bb762570c',N'mobiletheme',N'JQ-Mobile');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (40,'27604f05-86ad-47ef-9e05-950bb762570c',N'name',N'Name of the blog');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (41,'27604f05-86ad-47ef-9e05-950bb762570c',N'numberofrecentcomments',N'10');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (42,'27604f05-86ad-47ef-9e05-950bb762570c',N'numberofrecentposts',N'10');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (43,'27604f05-86ad-47ef-9e05-950bb762570c',N'postsperfeed',N'10');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (44,'27604f05-86ad-47ef-9e05-950bb762570c',N'postsperpage',N'10');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (45,'27604f05-86ad-47ef-9e05-950bb762570c',N'removewhitespaceinstylesheets',N'True');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (46,'27604f05-86ad-47ef-9e05-950bb762570c',N'searchbuttontext',N'Search');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (47,'27604f05-86ad-47ef-9e05-950bb762570c',N'searchcommentlabeltext',N'Include comments in search');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (48,'27604f05-86ad-47ef-9e05-950bb762570c',N'searchdefaulttext',N'Enter search term');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (49,'27604f05-86ad-47ef-9e05-950bb762570c',N'sendmailoncomment',N'True');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (50,'27604f05-86ad-47ef-9e05-950bb762570c',N'showdescriptioninpostlist',N'False');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (51,'27604f05-86ad-47ef-9e05-950bb762570c',N'showlivepreview',N'True');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (52,'27604f05-86ad-47ef-9e05-950bb762570c',N'showpostnavigation',N'True');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (53,'27604f05-86ad-47ef-9e05-950bb762570c',N'smtppassword',N'password');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (54,'27604f05-86ad-47ef-9e05-950bb762570c',N'smtpserver',N'mail.example.dk');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (55,'27604f05-86ad-47ef-9e05-950bb762570c',N'smtpserverport',N'25');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (56,'27604f05-86ad-47ef-9e05-950bb762570c',N'smtpusername',N'user@example.com');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (57,'27604f05-86ad-47ef-9e05-950bb762570c',N'storagelocation',N'~/App_Data/');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (58,'27604f05-86ad-47ef-9e05-950bb762570c',N'syndicationformat',N'Rss');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (59,'27604f05-86ad-47ef-9e05-950bb762570c',N'theme',N'Standard');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (60,'27604f05-86ad-47ef-9e05-950bb762570c',N'timestamppostlinks',N'True');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (61,'27604f05-86ad-47ef-9e05-950bb762570c',N'timezone',N'0');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (62,'27604f05-86ad-47ef-9e05-950bb762570c',N'trackingscript',N'');
GO
INSERT INTO [be_Settings] ([SettingRowId],[BlogId],[SettingName],[SettingValue]) VALUES (63,'27604f05-86ad-47ef-9e05-950bb762570c',N'enablequicknotes',N'True');
GO
SET IDENTITY_INSERT [be_Settings] OFF;
GO
SET IDENTITY_INSERT [be_StopWords] ON;
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (1,'27604f05-86ad-47ef-9e05-950bb762570c',N'a');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (2,'27604f05-86ad-47ef-9e05-950bb762570c',N'about');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (3,'27604f05-86ad-47ef-9e05-950bb762570c',N'actually');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (4,'27604f05-86ad-47ef-9e05-950bb762570c',N'add');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (5,'27604f05-86ad-47ef-9e05-950bb762570c',N'after');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (6,'27604f05-86ad-47ef-9e05-950bb762570c',N'all');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (7,'27604f05-86ad-47ef-9e05-950bb762570c',N'almost');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (8,'27604f05-86ad-47ef-9e05-950bb762570c',N'along');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (9,'27604f05-86ad-47ef-9e05-950bb762570c',N'also');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (10,'27604f05-86ad-47ef-9e05-950bb762570c',N'an');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (11,'27604f05-86ad-47ef-9e05-950bb762570c',N'and');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (12,'27604f05-86ad-47ef-9e05-950bb762570c',N'any');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (13,'27604f05-86ad-47ef-9e05-950bb762570c',N'are');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (14,'27604f05-86ad-47ef-9e05-950bb762570c',N'as');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (15,'27604f05-86ad-47ef-9e05-950bb762570c',N'at');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (16,'27604f05-86ad-47ef-9e05-950bb762570c',N'be');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (17,'27604f05-86ad-47ef-9e05-950bb762570c',N'both');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (18,'27604f05-86ad-47ef-9e05-950bb762570c',N'but');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (19,'27604f05-86ad-47ef-9e05-950bb762570c',N'by');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (20,'27604f05-86ad-47ef-9e05-950bb762570c',N'can');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (21,'27604f05-86ad-47ef-9e05-950bb762570c',N'cannot');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (22,'27604f05-86ad-47ef-9e05-950bb762570c',N'com');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (23,'27604f05-86ad-47ef-9e05-950bb762570c',N'could');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (24,'27604f05-86ad-47ef-9e05-950bb762570c',N'de');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (25,'27604f05-86ad-47ef-9e05-950bb762570c',N'do');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (26,'27604f05-86ad-47ef-9e05-950bb762570c',N'down');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (27,'27604f05-86ad-47ef-9e05-950bb762570c',N'each');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (28,'27604f05-86ad-47ef-9e05-950bb762570c',N'either');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (29,'27604f05-86ad-47ef-9e05-950bb762570c',N'en');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (30,'27604f05-86ad-47ef-9e05-950bb762570c',N'for');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (31,'27604f05-86ad-47ef-9e05-950bb762570c',N'from');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (32,'27604f05-86ad-47ef-9e05-950bb762570c',N'good');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (33,'27604f05-86ad-47ef-9e05-950bb762570c',N'has');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (34,'27604f05-86ad-47ef-9e05-950bb762570c',N'have');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (35,'27604f05-86ad-47ef-9e05-950bb762570c',N'he');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (36,'27604f05-86ad-47ef-9e05-950bb762570c',N'her');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (37,'27604f05-86ad-47ef-9e05-950bb762570c',N'here');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (38,'27604f05-86ad-47ef-9e05-950bb762570c',N'hers');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (39,'27604f05-86ad-47ef-9e05-950bb762570c',N'his');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (40,'27604f05-86ad-47ef-9e05-950bb762570c',N'how');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (41,'27604f05-86ad-47ef-9e05-950bb762570c',N'i');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (42,'27604f05-86ad-47ef-9e05-950bb762570c',N'if');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (43,'27604f05-86ad-47ef-9e05-950bb762570c',N'in');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (44,'27604f05-86ad-47ef-9e05-950bb762570c',N'into');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (45,'27604f05-86ad-47ef-9e05-950bb762570c',N'is');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (46,'27604f05-86ad-47ef-9e05-950bb762570c',N'it');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (47,'27604f05-86ad-47ef-9e05-950bb762570c',N'its');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (48,'27604f05-86ad-47ef-9e05-950bb762570c',N'just');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (49,'27604f05-86ad-47ef-9e05-950bb762570c',N'la');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (50,'27604f05-86ad-47ef-9e05-950bb762570c',N'like');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (51,'27604f05-86ad-47ef-9e05-950bb762570c',N'long');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (52,'27604f05-86ad-47ef-9e05-950bb762570c',N'make');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (53,'27604f05-86ad-47ef-9e05-950bb762570c',N'me');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (54,'27604f05-86ad-47ef-9e05-950bb762570c',N'more');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (55,'27604f05-86ad-47ef-9e05-950bb762570c',N'much');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (56,'27604f05-86ad-47ef-9e05-950bb762570c',N'my');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (57,'27604f05-86ad-47ef-9e05-950bb762570c',N'need');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (58,'27604f05-86ad-47ef-9e05-950bb762570c',N'new');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (59,'27604f05-86ad-47ef-9e05-950bb762570c',N'now');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (60,'27604f05-86ad-47ef-9e05-950bb762570c',N'of');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (61,'27604f05-86ad-47ef-9e05-950bb762570c',N'off');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (62,'27604f05-86ad-47ef-9e05-950bb762570c',N'on');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (63,'27604f05-86ad-47ef-9e05-950bb762570c',N'once');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (64,'27604f05-86ad-47ef-9e05-950bb762570c',N'one');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (65,'27604f05-86ad-47ef-9e05-950bb762570c',N'ones');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (66,'27604f05-86ad-47ef-9e05-950bb762570c',N'only');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (67,'27604f05-86ad-47ef-9e05-950bb762570c',N'or');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (68,'27604f05-86ad-47ef-9e05-950bb762570c',N'our');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (69,'27604f05-86ad-47ef-9e05-950bb762570c',N'out');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (70,'27604f05-86ad-47ef-9e05-950bb762570c',N'over');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (71,'27604f05-86ad-47ef-9e05-950bb762570c',N'own');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (72,'27604f05-86ad-47ef-9e05-950bb762570c',N'really');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (73,'27604f05-86ad-47ef-9e05-950bb762570c',N'right');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (74,'27604f05-86ad-47ef-9e05-950bb762570c',N'same');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (75,'27604f05-86ad-47ef-9e05-950bb762570c',N'see');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (76,'27604f05-86ad-47ef-9e05-950bb762570c',N'she');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (77,'27604f05-86ad-47ef-9e05-950bb762570c',N'so');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (78,'27604f05-86ad-47ef-9e05-950bb762570c',N'some');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (79,'27604f05-86ad-47ef-9e05-950bb762570c',N'such');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (80,'27604f05-86ad-47ef-9e05-950bb762570c',N'take');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (81,'27604f05-86ad-47ef-9e05-950bb762570c',N'takes');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (82,'27604f05-86ad-47ef-9e05-950bb762570c',N'that');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (83,'27604f05-86ad-47ef-9e05-950bb762570c',N'the');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (84,'27604f05-86ad-47ef-9e05-950bb762570c',N'their');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (85,'27604f05-86ad-47ef-9e05-950bb762570c',N'these');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (86,'27604f05-86ad-47ef-9e05-950bb762570c',N'thing');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (87,'27604f05-86ad-47ef-9e05-950bb762570c',N'this');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (88,'27604f05-86ad-47ef-9e05-950bb762570c',N'to');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (89,'27604f05-86ad-47ef-9e05-950bb762570c',N'too');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (90,'27604f05-86ad-47ef-9e05-950bb762570c',N'took');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (91,'27604f05-86ad-47ef-9e05-950bb762570c',N'und');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (92,'27604f05-86ad-47ef-9e05-950bb762570c',N'up');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (93,'27604f05-86ad-47ef-9e05-950bb762570c',N'use');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (94,'27604f05-86ad-47ef-9e05-950bb762570c',N'used');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (95,'27604f05-86ad-47ef-9e05-950bb762570c',N'using');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (96,'27604f05-86ad-47ef-9e05-950bb762570c',N'very');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (97,'27604f05-86ad-47ef-9e05-950bb762570c',N'was');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (98,'27604f05-86ad-47ef-9e05-950bb762570c',N'we');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (99,'27604f05-86ad-47ef-9e05-950bb762570c',N'well');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (100,'27604f05-86ad-47ef-9e05-950bb762570c',N'what');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (101,'27604f05-86ad-47ef-9e05-950bb762570c',N'when');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (102,'27604f05-86ad-47ef-9e05-950bb762570c',N'where');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (103,'27604f05-86ad-47ef-9e05-950bb762570c',N'who');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (104,'27604f05-86ad-47ef-9e05-950bb762570c',N'will');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (105,'27604f05-86ad-47ef-9e05-950bb762570c',N'with');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (106,'27604f05-86ad-47ef-9e05-950bb762570c',N'www');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (107,'27604f05-86ad-47ef-9e05-950bb762570c',N'you');
GO
INSERT INTO [be_StopWords] ([StopWordRowId],[BlogId],[StopWord]) VALUES (108,'27604f05-86ad-47ef-9e05-950bb762570c',N'your');
GO
SET IDENTITY_INSERT [be_StopWords] OFF;
GO
SET IDENTITY_INSERT [be_UserRoles] ON;
GO
INSERT INTO [be_UserRoles] ([UserRoleID],[BlogID],[UserName],[Role]) VALUES (1,'27604f05-86ad-47ef-9e05-950bb762570c',N'Admin',N'Administrators');
GO
SET IDENTITY_INSERT [be_UserRoles] OFF;
GO
SET IDENTITY_INSERT [be_Users] ON;
GO
INSERT INTO [be_Users] ([UserID],[BlogID],[UserName],[Password],[LastLoginTime],[EmailAddress]) VALUES (1,'27604f05-86ad-47ef-9e05-950bb762570c',N'Admin',N'',{ts '2010-10-11 17:44:31.990'},N'email@example.com');
GO
SET IDENTITY_INSERT [be_Users] OFF;
GO
ALTER TABLE [be_BlogRollItems] ADD CONSTRAINT [PK_be_BlogRollItems_BlogRollRowId] PRIMARY KEY ([BlogRollRowId]);
GO
ALTER TABLE [be_Blogs] ADD CONSTRAINT [PK_be_Blogs_BlogRowId] PRIMARY KEY ([BlogRowId]);
GO
ALTER TABLE [be_Categories] ADD CONSTRAINT [PK_be_Categories_CategoryRowID] PRIMARY KEY ([CategoryRowID]);
GO
ALTER TABLE [be_DataStoreSettings] ADD CONSTRAINT [PK_be_DataStoreSettings_DataStoreSettingRowId] PRIMARY KEY ([DataStoreSettingRowId]);
GO
ALTER TABLE [be_FileStoreDirectory] ADD CONSTRAINT [PK_be_FileStoreDirectory] PRIMARY KEY ([Id]);
GO
ALTER TABLE [be_FileStoreFiles] ADD CONSTRAINT [PK_be_FileStoreFiles] PRIMARY KEY ([FileID]);
GO
ALTER TABLE [be_FileStoreFileThumbs] ADD CONSTRAINT [PK_be_FileStoreFileThumbs] PRIMARY KEY ([thumbnailId]);
GO
ALTER TABLE [be_PackageFiles] ADD CONSTRAINT [PK_be_PackageFiles] PRIMARY KEY ([PackageId],[FileOrder]);
GO
ALTER TABLE [be_Packages] ADD CONSTRAINT [PK_be_Packages] PRIMARY KEY ([PackageId]);
GO
ALTER TABLE [be_Pages] ADD CONSTRAINT [PK_be_Pages_PageRowID] PRIMARY KEY ([PageRowID]);
GO
ALTER TABLE [be_PingService] ADD CONSTRAINT [PK_be_PingService_PingServiceID] PRIMARY KEY ([PingServiceID]);
GO
ALTER TABLE [be_PostCategory] ADD CONSTRAINT [PK_be_PostCategory_PostCategoryID] PRIMARY KEY ([PostCategoryID]);
GO
ALTER TABLE [be_PostComment] ADD CONSTRAINT [PK_be_PostComment_PostCommentRowID] PRIMARY KEY ([PostCommentRowID]);
GO
ALTER TABLE [be_PostNotify] ADD CONSTRAINT [PK_be_PostNotify_PostNotifyID] PRIMARY KEY ([PostNotifyID]);
GO
ALTER TABLE [be_Posts] ADD CONSTRAINT [PK_be_Posts_PostRowID] PRIMARY KEY ([PostRowID]);
GO
ALTER TABLE [be_PostTag] ADD CONSTRAINT [PK_be_PostTag_PostTagID] PRIMARY KEY ([PostTagID]);
GO
ALTER TABLE [be_Profiles] ADD CONSTRAINT [PK_be_Profiles_ProfileID] PRIMARY KEY ([ProfileID]);
GO
ALTER TABLE [be_QuickNotes] ADD CONSTRAINT [PK_be_QuickNotes] PRIMARY KEY ([QuickNoteID]);
GO
ALTER TABLE [be_QuickSettings] ADD CONSTRAINT [PK_be_QuickSettings] PRIMARY KEY ([QuickSettingID]);
GO
ALTER TABLE [be_Referrers] ADD CONSTRAINT [PK_be_Referrers_ReferrerRowId] PRIMARY KEY ([ReferrerRowId]);
GO
ALTER TABLE [be_RightRoles] ADD CONSTRAINT [PK_be_RightRoles_RightRoleRowId] PRIMARY KEY ([RightRoleRowId]);
GO
ALTER TABLE [be_Rights] ADD CONSTRAINT [PK_be_Rights_RightRowId] PRIMARY KEY ([RightRowId]);
GO
ALTER TABLE [be_Roles] ADD CONSTRAINT [PK_be_Roles_RoleID] PRIMARY KEY ([RoleID]);
GO
ALTER TABLE [be_Settings] ADD CONSTRAINT [PK_be_Settings_SettingRowId] PRIMARY KEY ([SettingRowId]);
GO
ALTER TABLE [be_StopWords] ADD CONSTRAINT [PK_be_StopWords_StopWordRowId] PRIMARY KEY ([StopWordRowId]);
GO
ALTER TABLE [be_UserRoles] ADD CONSTRAINT [PK_be_UserRoles_UserRoleID] PRIMARY KEY ([UserRoleID]);
GO
ALTER TABLE [be_Users] ADD CONSTRAINT [PK_be_Users_UserID] PRIMARY KEY ([UserID]);
GO
CREATE INDEX [idx_be_BlogRollItems_BlogId] ON [be_BlogRollItems] ([BlogId] ASC);
GO
CREATE UNIQUE INDEX [idx_be_Categories_BlogID_CategoryID] ON [be_Categories] ([BlogID] ASC,[CategoryID] ASC);
GO
CREATE INDEX [idx_be_DataStoreSettings_BlogId_ExtensionType_TypeID] ON [be_DataStoreSettings] ([BlogId] ASC,[ExtensionType] ASC,[ExtensionId] ASC);
GO
CREATE INDEX [idx_Pages_BlogId_PageId] ON [be_Pages] ([BlogID] ASC,[PageID] ASC);
GO
CREATE INDEX [idx_be_PingService_BlogId] ON [be_PingService] ([BlogID] ASC);
GO
CREATE INDEX [idx_be_PostCategory_BlogId_CategoryId] ON [be_PostCategory] ([BlogID] ASC,[CategoryID] ASC);
GO
CREATE INDEX [idx_be_PostCategory_BlogId_PostId] ON [be_PostCategory] ([BlogID] ASC,[PostID] ASC);
GO
CREATE INDEX [idx_be_PostComment_BlogId_PostId] ON [be_PostComment] ([BlogID] ASC,[PostID] ASC);
GO
CREATE INDEX [FK_PostID] ON [be_PostNotify] ([BlogID] ASC,[PostID] ASC);
GO
CREATE UNIQUE INDEX [be_Posts_BlogID_PostID] ON [be_Posts] ([BlogID] ASC,[PostID] ASC);
GO
CREATE INDEX [idx_be_PostTag_BlogId_PostId] ON [be_PostTag] ([BlogID] ASC,[PostID] ASC);
GO
CREATE INDEX [idx_be_Profiles_BlogId_UserName] ON [be_Profiles] ([BlogID] ASC,[UserName] ASC);
GO
CREATE INDEX [idx_be_NoteId_BlogId_UserName] ON [be_QuickNotes] ([NoteID] ASC,[BlogID] ASC,[UserName] ASC);
GO
CREATE INDEX [idx_be_Referrers_BlogId] ON [be_Referrers] ([BlogId] ASC);
GO
CREATE INDEX [idx_be_RightRoles_BlogId] ON [be_RightRoles] ([BlogId] ASC);
GO
CREATE INDEX [idx_be_Rights_BlogId] ON [be_Rights] ([BlogId] ASC);
GO
CREATE UNIQUE INDEX [idx_be_Roles_BlogID_Role] ON [be_Roles] ([BlogID] ASC,[Role] ASC);
GO
CREATE INDEX [idx_be_Settings_BlogId] ON [be_Settings] ([BlogId] ASC);
GO
CREATE INDEX [idx_be_StopWords_BlogId] ON [be_StopWords] ([BlogId] ASC);
GO
CREATE INDEX [idx_be_UserRoles_BlogId] ON [be_UserRoles] ([BlogID] ASC);
GO
CREATE INDEX [idx_be_Users_BlogId_UserName] ON [be_Users] ([BlogID] ASC,[UserName] ASC);
GO


/****** Object:  Table [dbo].[polling_PollVotes]    Script Date: 08/27/2012 13:27:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sts_image]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[sts_image](
		[ImageId] [uniqueidentifier] NOT NULL,
		[UserId] [int] NOT NULL,
		[ImageGalleryId] [uniqueidentifier] NOT NULL,
		[FilePath] [nvarchar](255) NOT NULL,
		[Title] [nvarchar](255) NOT NULL,
		[Status] [nvarchar](1) NOT NULL,
		[CreatedDateUtc] [datetime] NOT NULL CONSTRAINT [DF_sts_image_CreatedDateUtc]  DEFAULT (getdate()),
		[LastUpdatedDateUtc] [datetime2](7) NOT NULL CONSTRAINT [DF_sts_image_LastUpdatedDateUtc]  DEFAULT (getdate()),
		[Degree] [int] NULL,
	 CONSTRAINT [PK_sts_image] PRIMARY KEY CLUSTERED 
	(
		[ImageId] ASC,
		[UserId] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[polling_Polls]    Script Date: 08/27/2012 13:27:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sts_image_gallery]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[sts_image_gallery](
		[Id] [uniqueidentifier] NOT NULL,
		[Name] [nvarchar](255) NOT NULL,
		[Description] [nvarchar](max) NULL,
		[AuthorUserId] [int] NOT NULL,
		[GroupId] [int] NOT NULL,
		[IsEnabled] [bit] NOT NULL CONSTRAINT [DF_sts_image_gallery_IsEnabled]  DEFAULT ((1)),
		[IsIndexed] [bit] NOT NULL CONSTRAINT [DF_sts_image_gallery_IsIndexed]  DEFAULT ((0)),
		[CreatedDateUtc] [datetime] NOT NULL CONSTRAINT [DF_sts_image_gallery_CreatedDateUtc]  DEFAULT (getdate()),
		[LastUpdatedDateUtc] [datetime] NOT NULL CONSTRAINT [DF_sts_image_gallery_LastUpdatedDateUtc]  DEFAULT (getdate()),
	 CONSTRAINT [PK_sts_image_gallery] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

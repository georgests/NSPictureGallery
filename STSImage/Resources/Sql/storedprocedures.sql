/****** Object:  StoredProcedure [dbo].[sts_image_gallery_AddUpdate]    Script Date: 08/27/2012 13:27:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS ( SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'dbo.[sts_image_gallery_AddUpdate]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1 )
	DROP PROCEDURE [dbo].[sts_image_gallery_AddUpdate]
GO
CREATE PROCEDURE [dbo].[sts_image_gallery_AddUpdate]
	@Id uniqueidentifier,
	@Name nvarchar(255),
	@Description nvarchar(max) = null,
	@AuthorUserId int,
	@GroupId int,
	@IsEnabled bit = 1,
	@DateUtc datetime
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @Err int
	DECLARE @RowCount int
	
	BEGIN TRAN
	
		UPDATE ig
		SET Name = @Name,
			Description = @Description,
			AuthorUserId = @AuthorUserId,
			GroupId = @GroupId,
			IsEnabled = @IsEnabled,
			LastUpdatedDateUtc = @DateUtc
		FROM sts_image_gallery ig
		WHERE ig.Id = @Id
		
		SELECT @Err = @@ERROR, @RowCount = @@ROWCOUNT
		
		IF @Err <> 0 BEGIN
			ROLLBACK TRAN
			RETURN
		END
		
		IF @RowCount = 0 BEGIN
		
			INSERT INTO sts_image_gallery (Id, Name, Description, AuthorUserId, GroupId, IsEnabled, CreatedDateUtc, LastUpdatedDateUtc)
			VALUES (@Id, @Name, @Description, @AuthorUserId, @GroupId, @IsEnabled, @DateUtc, @DateUtc)
			
			SET @Err = @@ERROR
			IF @Err <> 0 BEGIN
				ROLLBACK TRAN
				RETURN
			END
		
		END
		
	COMMIT TRAN

END
GO

/****** Object:  StoredProcedure [dbo].[sts_image_AddUpdate]    Script Date: 08/27/2012 13:27:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS ( SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'dbo.[sts_image_AddUpdate]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1 )
	DROP PROCEDURE [dbo].[sts_image_AddUpdate]
GO
CREATE PROCEDURE [dbo].[sts_image_AddUpdate]
	@ImageId uniqueidentifier,
	@UserId int,
	@FilePath nvarchar(255),
	@ImageTitle nvarchar(255),
	@Degree int,
	@ImageGalleryId uniqueidentifier,
	@DateUtc datetime
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @Err int
	DECLARE @RowCount int
	
	BEGIN TRAN
	
		UPDATE img
		SET ImageId = @ImageId,
			LastUpdatedDateUtc = @DateUtc,
			FilePath = @FilePath,
			Title = @ImageTitle,
			Degree = @Degree,
			Status = 'A'
		FROM sts_image img
		WHERE img.ImageId = @ImageId
			and img.ImageGalleryId = @ImageGalleryId
			and img.UserId = @UserId
		
		SELECT @Err = @@ERROR, @RowCount = @@ROWCOUNT
		
		IF @Err <> 0 BEGIN
			ROLLBACK TRAN
			RETURN
		END
		
		IF @RowCount = 0 BEGIN
		
			INSERT INTO sts_image (ImageId, UserId, ImageGalleryId, FilePath, Title, Degree, Status, CreatedDateUtc, LastUpdatedDateUtc)
			VALUES (@ImageId, @UserId, @ImageGalleryId, @FilePath, @ImageTitle, @Degree, 'A', @DateUtc, @DateUtc)
			
			SET @Err = @@ERROR
			IF @Err <> 0 BEGIN
				ROLLBACK TRAN
				RETURN
			END
		
		END
		
	COMMIT TRAN

END
GO


/****** Object:  StoredProcedure [dbo].[sts_image_delete]    Script Date: 08/27/2012 13:27:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS ( SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'dbo.[sts_image_delete]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1 )
	DROP PROCEDURE [dbo].[sts_image_delete]
GO
CREATE PROCEDURE [dbo].[sts_image_delete]
	@ImageId uniqueidentifier,
	@UserId int,
	@ImageGalleryId uniqueidentifier,
	@DateUtc datetime
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @Err int
	DECLARE @RowCount int
	
	BEGIN TRAN
	
		UPDATE img
		SET ImageId = @ImageId,
			LastUpdatedDateUtc = @DateUtc,
			Status = 'D'
		FROM sts_image img
		WHERE img.ImageId = @ImageId
		/*
			and img.ImageGalleryId = @ImageGalleryId
			and img.UserId = @UserId
		*/

		SELECT @Err = @@ERROR, @RowCount = @@ROWCOUNT
		
		IF @Err <> 0 BEGIN
			ROLLBACK TRAN
			RETURN
		END
		
			/*		
		IF @RowCount = 0 BEGIN
			INSERT INTO sts_image (ImageId, UserId, ImageGalleryId, Status, CreatedDateUtc, LastUpdatedDateUtc)
			VALUES (@ImageId, @UserId, @ImageGalleryId, 'A', @DateUtc, @DateUtc)
			
			SET @Err = @@ERROR
			IF @Err <> 0 BEGIN
				ROLLBACK TRAN
				RETURN
			END
		END
			*/
		
		
	COMMIT TRAN

END
GO


/****** Object:  StoredProcedure [dbo].[sts_image_update_degree]    Script Date: 08/27/2012 13:27:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS ( SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'dbo.[sts_image_update_degree]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1 )
	DROP PROCEDURE [dbo].[sts_image_update_degree]
GO
CREATE PROCEDURE [dbo].[sts_image_update_degree]
	@ImageId uniqueidentifier,
	@UserId int,
	@ImageGalleryId uniqueidentifier,
	@Degree int	
	
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @Err int
	DECLARE @RowCount int
	
	BEGIN TRAN
	
		UPDATE img
		SET ImageId = @ImageId,		 		
			Degree = @Degree
		FROM sts_image img
		WHERE img.ImageId = @ImageId
		
		SELECT @Err = @@ERROR, @RowCount = @@ROWCOUNT
		
		IF @Err <> 0 BEGIN
			ROLLBACK TRAN
			RETURN
		END
		
			/*		
		IF @RowCount = 0 BEGIN
			INSERT INTO sts_image (ImageId, UserId, ImageGalleryId, Status, CreatedDateUtc, LastUpdatedDateUtc)
			VALUES (@ImageId, @UserId, @ImageGalleryId, 'A', @DateUtc, @DateUtc)
			
			SET @Err = @@ERROR
			IF @Err <> 0 BEGIN
				ROLLBACK TRAN
				RETURN
			END
		END
			*/
		
		
	COMMIT TRAN

END
GO






/****** Object:  StoredProcedure [dbo].[sts_image_update_title]    Script Date: 08/27/2012 13:27:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS ( SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'dbo.[sts_image_update_title]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1 )
	DROP PROCEDURE [dbo].[sts_image_update_title]
GO
CREATE PROCEDURE [dbo].[sts_image_update_title]
	@ImageId uniqueidentifier,
	@UserId int,
	@ImageGalleryId uniqueidentifier,
	@ImageTitle nvarchar(255),
	@DateUtc datetime
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @Err int
	DECLARE @RowCount int
	
	BEGIN TRAN
	
		UPDATE img
		SET ImageId = @ImageId,
			LastUpdatedDateUtc = @DateUtc,
			Title = @ImageTitle
		FROM sts_image img
		WHERE img.ImageId = @ImageId
		
		SELECT @Err = @@ERROR, @RowCount = @@ROWCOUNT
		
		IF @Err <> 0 BEGIN
			ROLLBACK TRAN
			RETURN
		END
		
			/*		
		IF @RowCount = 0 BEGIN
			INSERT INTO sts_image (ImageId, UserId, ImageGalleryId, Status, CreatedDateUtc, LastUpdatedDateUtc)
			VALUES (@ImageId, @UserId, @ImageGalleryId, 'A', @DateUtc, @DateUtc)
			
			SET @Err = @@ERROR
			IF @Err <> 0 BEGIN
				ROLLBACK TRAN
				RETURN
			END
		END
			*/
		
		
	COMMIT TRAN

END
GO

/****** Object:  StoredProcedure [dbo].[sts_Images_GetAll]    Script Date: 08/27/2012 13:27:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS ( SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'dbo.[sts_Images_GetAll]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1 )
	DROP PROCEDURE [dbo].[sts_Images_GetAll]
GO
CREATE PROCEDURE [dbo].[sts_Images_GetAll]
	@ImageGalleryId uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM sts_image_gallery
	WHERE Id = @ImageGalleryId

    SELECT UserId, ImageId, ImageGalleryId, FilePath, Title, Degree, Status
    FROM sts_image
    WHERE ImageGalleryId = @ImageGalleryId
	ORDER BY LastUpdatedDateUtc DESC
END
GO

/****** Object:  StoredProcedure [dbo].[sts_Images_Get]    Script Date: 08/27/2012 13:27:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS ( SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'dbo.[sts_Images_Get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1 )
	DROP PROCEDURE [dbo].[sts_Images_Get]
GO
CREATE PROCEDURE [dbo].[sts_Images_Get]
	@ImageGalleryId uniqueidentifier,
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM sts_image_gallery
	WHERE Id = @ImageGalleryId

    SELECT UserId, ImageId, ImageGalleryId, FilePath, Title, Degree, Status
    FROM sts_image
    WHERE ImageGalleryId = @ImageGalleryId
		AND UserId = @UserId AND Status = 'A'
		ORDER BY LastUpdatedDateUtc DESC
END
GO

/****** Object:  StoredProcedure [dbo].[sts_Image_getById]    Script Date: 08/27/2012 13:27:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS ( SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'dbo.[sts_Image_getById]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1 )
	DROP PROCEDURE [dbo].[sts_Image_getById]
GO
CREATE PROCEDURE [dbo].[sts_Image_getById]
	@ImageId uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

    SELECT UserId, ImageId, ImageGalleryId, FilePath, Title, Degree, Status
    FROM sts_image
    WHERE ImageId = @ImageId
END
GO

/****** Object:  StoredProcedure [dbo].[sts_ImageGallerys_List]    Script Date: 08/27/2012 13:27:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS ( SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'dbo.[sts_ImageGallerys_List]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1 )
	DROP PROCEDURE [dbo].[sts_ImageGallerys_List]
GO
CREATE PROCEDURE [dbo].[sts_ImageGallerys_List]
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM sts_image_gallery
	WHERE AuthorUserId = @UserId
END
GO

/****** Object:  StoredProcedure [dbo].[sts_image_gallery_AddDefaultGallery]    Script Date: 08/27/2012 13:27:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS ( SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'dbo.[sts_image_gallery_AddDefaultGallery]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1 )
	DROP PROCEDURE [dbo].[sts_image_gallery_AddDefaultGallery]
GO
CREATE PROCEDURE [dbo].[sts_image_gallery_AddDefaultGallery]
	@Id uniqueidentifier,
	@Name nvarchar(255),
	@Description nvarchar(max) = null,
	@AuthorUserId int,
	@GroupId int,
	@IsEnabled bit = 1,
	@DateUtc datetime
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @Err int
	DECLARE @RowCount int

	BEGIN TRAN
		
		IF ((SELECT count(*) FROM sts_image_gallery WHERE AuthorUserId = @AuthorUserId) = 0) BEGIN
			INSERT INTO sts_image_gallery (Id, Name, Description, AuthorUserId, GroupId, IsEnabled, CreatedDateUtc, LastUpdatedDateUtc)
			VALUES (@Id, @Name, @Description, @AuthorUserId, @GroupId, @IsEnabled, @DateUtc, @DateUtc)
		END	
			
		SET @Err = @@ERROR
		IF @Err <> 0 BEGIN
			ROLLBACK TRAN
			RETURN
		END
		
	COMMIT TRAN

END
GO

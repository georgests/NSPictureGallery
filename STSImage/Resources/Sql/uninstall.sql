/****** Object:  StoredProcedure [dbo].[sts_image_gallery_AddUpdate]    Script Date: 08/27/2012 13:27:22 ******/
IF EXISTS ( SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'dbo.[sts_image_gallery_AddUpdate]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1 )
	DROP PROCEDURE [dbo].[sts_image_gallery_AddUpdate]
GO
/****** Object:  StoredProcedure [dbo].[sts_image_AddUpdate]    Script Date: 08/27/2012 13:27:22 ******/
IF EXISTS ( SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'dbo.[sts_image_AddUpdate]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1 )
	DROP PROCEDURE [dbo].[sts_image_AddUpdate]
GO
/****** Object:  StoredProcedure [dbo].[sts_image_delete]    Script Date: 08/27/2012 13:27:22 ******/
IF EXISTS ( SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'dbo.[sts_image_delete]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1 )
	DROP PROCEDURE [dbo].[sts_image_delete]
GO
/****** Object:  StoredProcedure [dbo].[sts_image_update_title]    Script Date: 08/27/2012 13:27:22 ******/
IF EXISTS ( SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'dbo.[sts_image_update_title]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1 )
	DROP PROCEDURE [dbo].[sts_image_update_title]
GO
/****** Object:  StoredProcedure [dbo].[polling_Images_GetAll]    Script Date: 08/27/2012 13:27:22 ******/
IF EXISTS ( SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'dbo.[polling_Images_GetAll]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1 )
	DROP PROCEDURE [dbo].[polling_Images_GetAll]
GO
/****** Object:  StoredProcedure [dbo].[polling_Images_Get]    Script Date: 08/27/2012 13:27:22 ******/
IF EXISTS ( SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'dbo.[polling_Images_Get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1 )
	DROP PROCEDURE [dbo].[polling_Images_Get]
GO

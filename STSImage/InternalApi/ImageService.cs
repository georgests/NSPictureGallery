using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telligent.Evolution.Extensibility.Caching.Version1;
using Telligent.Evolution.Extensibility.Api.Version1;
using Telligent.Evolution.Extensibility.Api.Entities.Version1;
using TEApi = Telligent.Evolution.Extensibility.Api.Version1.PublicApi;

namespace STSImage.InternalApi
{
	internal static class ImageService
	{
		internal static void AddUpdateImageGallery(ImageGallery ig)
		{
			bool isCreate = ig.Id == Guid.Empty;

			ValidateImageGallery(ig);

			if (isCreate)
                PublicApi.ImageGallerys.Events.OnBeforeCreate(ig);
			else
                PublicApi.ImageGallerys.Events.OnBeforeUpdate(ig);

			ImageDataService.AddUpdateImageGallery(ig);

			if (isCreate)
                PublicApi.ImageGallerys.Events.OnAfterCreate(ig);
			else
                PublicApi.ImageGallerys.Events.OnAfterUpdate(ig);
		}

        internal static ImageGallery GetImageGallery(Guid ImageGalleryId)
        {
            ImageGallery imageGallery = (ImageGallery)CacheService.Get(ImageGalleryCacheKey(ImageGalleryId), CacheScope.All);
            if (imageGallery == null)
            {
                imageGallery = ImageDataService.GetImageGallery(ImageGalleryId);
                if (imageGallery != null)
                    CacheService.Put(ImageGalleryCacheKey(ImageGalleryId), imageGallery, CacheScope.All, new string[] { ImageGalleryTag(imageGallery.GroupId) });
            }

            if (imageGallery != null && CanRead(imageGallery.GroupId))
                return imageGallery;

            return null;
        }
        internal static ImageGallery GetImageGalleryByCurrent(Guid ImageGalleryId,int userId)
        {
            ImageGallery imageGallery = (ImageGallery)CacheService.Get(ImageGalleryCacheKey(ImageGalleryId), CacheScope.All);
            if (imageGallery == null)
            {
                imageGallery = ImageDataService.GetImageGalleryByCurrent(ImageGalleryId, userId);
                if (imageGallery != null)
                    CacheService.Put(ImageGalleryCacheKey(ImageGalleryId), imageGallery, CacheScope.All, new string[] { ImageGalleryTag(imageGallery.GroupId) });
            }

            if (imageGallery != null && CanRead(imageGallery.GroupId))
                return imageGallery;

            return null;
        }

		internal static void DeleteImageGallery(ImageGallery ig)
		{
			ValidateImageGallery(ig);
            /*
            PublicApi.ImageGallerys.OnBeforeDelete(ig);
			ImageDataService.DeletePoll(ig.Id);
			ExpirePolls(ig.GroupId);
            PublicApi.Polls.Events.OnAfterDelete(ig);
            */
		}

        internal static PagedList<ImageGallery> ListImageGallerys(int userId, int pageSize, int pageIndex)
        {
            /*
            if (!CanRead(userId))
                return new PagedList<ImageGallery>();
            */

            PagedList<ImageGallery> imageGallerys = (PagedList<ImageGallery>)CacheService.Get(ImageGalleryCacheKey(userId, pageSize, pageIndex), CacheScope.Context | CacheScope.Process);
            if (imageGallerys == null)
            {
                imageGallerys = ImageDataService.GetImageGalleryList(userId);
                CacheService.Put(ImageGalleryCacheKey(userId, pageSize, pageIndex), imageGallerys, CacheScope.Context | CacheScope.Process, new string[] { ImageGalleryTag(userId) });
            }

            return imageGallerys;
        }

        /*
		internal static void ReassignPolls(int oldUserId, int newUserId)
		{
			if (oldUserId != newUserId)
			{
				ImageDataService.ReassigneUser(oldUserId, newUserId);
				// We don't expire the cache here because it would be potentially too large of an expiration
			}
		}

		internal static void DeletePolls(int groupId)
		{
			ImageDataService.DeletePollsByGroup(groupId);
			ExpirePolls(groupId);
		}



		internal static PagedList<ImageGallery> ListPollsToReindex(int pageSize, int pageIndex)
		{
			return ImageDataService.ListPollsToReindex(pageSize, pageIndex);
		}

		internal static void SetPollsAsIndexed(IEnumerable<Guid> pollIds)
		{
			ImageDataService.SetPollsAsIndexed(pollIds);
		}
        */

		internal static string AddUpdateImage(Image image)
		{
            string retVal = " Internal to AddUpdateImage ";
            try
            {

                bool isCreate = image.Id == Guid.Empty;

                ValidateImage(image);
                retVal = retVal + " After ValidateImage ";

                /*
                if (isCreate)
                    InternalApi.PollAnswers.Events.OnBeforeCreate(image);
                else
                    InternalApi.PollAnswers.Events.OnBeforeUpdate(image);
                */

                ImageDataService.AddUpdateImage(image);
                retVal = retVal + " After AddUpdateImage ";
                ExpireImageGallery(image.ImageGalleryId);
                retVal = retVal + " After ExpireImageGallery ";
            }
            catch (Exception e)
            {
                retVal = retVal + e.ToString();
            }

            return retVal;

            /*
			if (isCreate)
				InternalApi.PollAnswers.Events.OnAfterCreate(image);
			else
				InternalApi.PollAnswers.Events.OnAfterUpdate(image);
             */
		}

		internal static void DeleteImage(Image image)
		{
            ValidateImage(image);
            // InternalApi.PollAnswers.Events.OnBeforeDelete(image);
            ImageDataService.ImageDelete(image);
            ExpireImageGallery(image.ImageGalleryId);
            // InternalApi.PollAnswers.Events.OnAfterDelete(image);
		}

        internal static void UpdateImageTitle(Image image, string title)
        {
            image.Title = title;
            ValidateImage(image);
            // InternalApi.PollAnswers.Events.OnBeforeDelete(image);
            ImageDataService.ImageUpdateTitle(image);
            ExpireImageGallery(image.ImageGalleryId);
            // InternalApi.PollAnswers.Events.OnAfterDelete(image);
        }

        internal static void UpdateDegree(Image image, int degree)
        {
            image.Degree = degree;
            ValidateImage(image);
            // InternalApi.PollAnswers.Events.OnBeforeDelete(image);
            ImageDataService.ImageUpdateDegree(image);
            ExpireImageGallery(image.ImageGalleryId);
            // InternalApi.PollAnswers.Events.OnAfterDelete(image);
        }

        internal static Image GetImage(Guid imageId)
		{
            if (imageId != null) 
                return ImageDataService.GetImage(imageId);

            return null;
		}


		internal static bool CanVote(Guid imageGalleryId, int userId)
		{
			var ig = GetImageGallery(imageGalleryId);
			if (ig == null)
				return false;

			return CanCreate(ig.GroupId);
		}

		internal static bool CanRead(Guid igId, int userId)
		{
			var ig = GetImageGallery(igId);
			if (ig == null)
				return false;

			return CanRead(ig.GroupId);
		}

		internal static bool CanModerate(Guid igId, int userId)
		{
			var ig = GetImageGallery(igId);
			if (ig == null)
				return false;

			return TEApi.NodePermissions.Get("groups", ig.GroupId, "Group_ModifyGroup").IsAllowed;
		}

		public static bool CanRead(int groupId)
		{
			return TEApi.NodePermissions.Get("groups", groupId, "Group_ReadGroup").IsAllowed;
		}

		public static bool CanCreate(int groupId)
		{
			return !TEApi.Users.AccessingUser.IsSystemAccount.Value && CanRead(groupId);
		}

		internal static string RenderImageGalleryDescription(ImageGallery ig, string target)
		{
			if (string.IsNullOrEmpty(target))
				target = "web";
			else
				target = target.ToLowerInvariant();
			
			if (target == "raw")
				return ig.Description ?? string.Empty;
			else
				return PublicApi.ImageGallerys.Events.OnRender(ig, "Description", ig.Description ?? string.Empty, target);
		}

		internal static string ImageGalleryUrl(Guid igId)
		{
			var ig = GetImageGallery(igId);
			if (ig == null)
				return null;

			var group = TEApi.Groups.Get(new GroupsGetOptions { Id = ig.GroupId });
			if (group == null)
				return null;

			return TEApi.Url.Adjust(TEApi.GroupUrls.Custom(group.Id.Value, "imagegallery"), string.Concat("PollId=", ig.Id.ToString()));
		}

		internal static string CreateImageGalleryUrl(int groupId, bool checkPermissions)
		{
			var group = TEApi.Groups.Get(new GroupsGetOptions { Id = groupId });
			if (group == null)
				return null;

			if (checkPermissions && !CanRead(groupId))
				return null;

            return TEApi.GroupUrls.Custom(group.Id.Value, "createeditimagegallery");
		}

		internal static string EditImageGalleryUrl(Guid igId, bool checkPermissions)
		{
			var ig = GetImageGallery(igId);
			if (ig == null)
				return null;

			var group = TEApi.Groups.Get(new GroupsGetOptions { Id = ig.GroupId });
			if (group == null)
				return null;

			if (checkPermissions && !CanModerate(igId, TEApi.Users.AccessingUser.Id.Value))
				return null;

            return TEApi.Url.Adjust(TEApi.GroupUrls.Custom(group.Id.Value, "imagegallery"), string.Concat("PollId=", ig.Id.ToString()));
		}

		internal static string ImageGalleryListUrl(int groupId)
		{
			var group = TEApi.Groups.Get(new GroupsGetOptions { Id = groupId });
			if (group == null)
				return null;

            return TEApi.GroupUrls.Custom(group.Id.Value, "imagegallerys");
		}

		#region Validation

		private static void ValidateImageGallery(ImageGallery ig)
		{
			if (ig.Id == Guid.Empty)
			{
				ig.CreatedDateUtc = DateTime.UtcNow;
				ig.Id = Guid.NewGuid();
			}

			ig.LastUpdatedDateUtc = DateTime.UtcNow;
			ig.Name = TEApi.Html.Sanitize(TEApi.Html.EnsureEncoded(ig.Name));
			ig.Description = TEApi.Html.Sanitize(ig.Description ?? string.Empty);

			if (string.IsNullOrEmpty(ig.Name))
				throw new InvalidOperationException("The name of the Image Gallery must be defined.");

			var group = TEApi.Groups.Get(new GroupsGetOptions { Id = ig.GroupId });
			if (group == null || group.HasErrors())
				throw new InvalidOperationException("The group identified on the Image Gallery is invalid.");
		}

		private static void ValidateImage(Image img)
		{
			if (img.Id == Guid.Empty)
				img.Id = Guid.NewGuid();

			img.Title = TEApi.Html.Sanitize(TEApi.Html.EnsureEncoded(img.Title));
			if (string.IsNullOrEmpty(img.Title))
				throw new InvalidOperationException("The name of the Image Title must be defined.");

			ImageGallery ig = GetImageGallery(img.ImageGalleryId);
			if (ig == null)
				throw new InvalidOperationException("The Image Gallery associated to the Image does not exist.");

			var group = TEApi.Groups.Get(new GroupsGetOptions { Id = ig.GroupId });
			if (group == null || group.HasErrors())
				throw new InvalidOperationException("The group identified on the Image Gallery is invalid.");

			if (ig.AuthorUserId != TEApi.Users.AccessingUser.Id.Value /* && !TEApi.NodePermissions.Get("groups", group.Id.Value, "Group_ModifyGroup").IsAllowed */)
				throw new InvalidOperationException("The user does not have permission to create/edit this Image Gallery. The user must be the original creator or an admin in the group.");
        }

		#endregion

		#region Cache-related Methods

		private static void ExpireImageGallery(Guid igId)
		{
			CacheService.Remove(ImageGalleryCacheKey(igId), CacheScope.All);
		}

        private static void ExpireImageGallery(int groupId)
		{
			CacheService.RemoveByTags(new string[] { ImageGalleryTag(groupId) } , CacheScope.All);
		}

		private static string ImageGalleryCacheKey(Guid pollId)
		{
			return string.Concat("STS_PK_ImageGallery:", pollId.ToString("N"));
		}

		private static string ImageGalleryCacheKey(int groupId, int pageSize, int pageIndex)
		{
			return string.Concat("STS_PK_ImageGallery:", groupId, ":", pageSize, ":", pageIndex);
		}

		private static string ImageGalleryTag(int groupId)
		{
			return string.Concat("STS_ImageGallery_TAG_Group:", groupId);
		}

		#endregion
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telligent.Evolution.Extensibility.Api.Entities.Version1;
using Internal = STSImage.InternalApi;
using Telligent.Evolution.Extensibility.Api.Version1;
using TEApi = Telligent.Evolution.Extensibility.Api.Version1.PublicApi;

namespace STSImage.PublicApi
{
	public static class ImageGallerys
	{
        private static readonly Guid _contentTypeId = new Guid("afb835d976b44eb4ba2529cf3b4a4d71");
        private static readonly ImageGalleryEvents _events = new ImageGalleryEvents();

		public static Guid ContentTypeId { get { return _contentTypeId; } }

        public static ImageGalleryEvents Events
		{
			get { return _events; }
		}

		public static ImageGallery Get(Guid id)
		{
			try
			{
                Internal.ImageGallery imgGallery = Internal.ImageService.GetImageGallery(id);
				if (imgGallery == null)
					return null;

				return new ImageGallery(imgGallery);
			}
			catch (Exception ex)
			{
				return new ImageGallery(new AdditionalInfo(new Error(ex.GetType().FullName, ex.Message)));
			}
		}
        public static ImageGallery GetImageGalleryByCurrent(Guid id,int userId)
        {
            try
            {
                Internal.ImageGallery imgGallery = Internal.ImageService.GetImageGalleryByCurrent(id,userId);
                if (imgGallery == null)
                    return null;

                return new ImageGallery(imgGallery);
            }
            catch (Exception ex)
            {
                return new ImageGallery(new AdditionalInfo(new Error(ex.GetType().FullName, ex.Message)));
            }
        }


		public static AdditionalInfo Delete(Guid id)
		{
			try
			{
				var poll = Internal.ImageService.GetImageGallery(id);
				if (poll != null)
					Internal.ImageService.DeleteImageGallery(poll);

				return new AdditionalInfo();
			}
			catch (Exception ex)
			{
				return new AdditionalInfo(new Error(ex.GetType().FullName, ex.Message));
			}
		}

		public static ImageGallery Create(int groupId, string name, string description = null)
		{
			try
			{
                var ig = new Internal.ImageGallery();
				ig.GroupId = groupId;
				ig.AuthorUserId = TEApi.Users.AccessingUser.Id.Value;
				ig.Name = name;
				ig.Description = description;
				ig.IsEnabled = true;

				Internal.ImageService.AddUpdateImageGallery(ig);

				return Get(ig.Id);
			}
			catch (Exception ex)
			{
				return new ImageGallery(new AdditionalInfo(new Error(ex.GetType().FullName, ex.Message)));
			}
		}

        public static ImageGallery Update(Guid id, string name = null, string description = null)
		{
			try
			{
				var ig = Internal.ImageService.GetImageGallery(id);
				if (ig != null)
				{
					if (name != null)
						ig.Name = name;

					if (description != null)
						ig.Description = description;

					Internal.ImageService.AddUpdateImageGallery(ig);
				}

				return Get(id);
			}
			catch (Exception ex)
			{
				return new ImageGallery(new AdditionalInfo(new Error(ex.GetType().FullName, ex.Message)));
			}
		}

        public static PagedList<ImageGallery> List(int groupId, int pageIndex = 0, int pageSize = 20, string sortBy = "Date")
		{
			if (pageSize > 100)
				pageSize = 100;
			else if (pageSize < 1)
				pageSize = 1;

			if (pageIndex < 0)
				pageIndex = 0;

			try
			{
				if (string.Equals(sortBy, "TopImageGallerys", StringComparison.OrdinalIgnoreCase))
				{
					var group = TEApi.Groups.Get(new GroupsGetOptions { Id = groupId });
					if (group == null || group.HasErrors())
                        return new PagedList<ImageGallery>();

					var images = new List<Image>();
					foreach (var image in images)
					{
                        images.Add(image);
					}

                    IEnumerable<ImageGallery> ie = images as IEnumerable<ImageGallery>;
                    return new PagedList<ImageGallery>( ie, pageSize, pageIndex, images.Count);
				}
				else
				{
					var igs = InternalApi.ImageService.ListImageGallerys(groupId, pageSize, pageIndex);
					return new PagedList<ImageGallery>(igs.Select(x => new ImageGallery(x)), igs.PageSize, igs.PageIndex, igs.TotalCount);
				}				
			}
			catch (Exception ex)
			{
				return new PagedList<ImageGallery>(new AdditionalInfo(new Error(ex.GetType().FullName, ex.Message)));
			}
		}

        public static PagedList<ImageGallery> ListImageGallerys(int groupId, int pageSize = 20, int pageIndex = 0, string sortBy = "Date")
        {
            if (pageSize > 100)
                pageSize = 100;
            else if (pageSize < 1)
                pageSize = 1;

            if (pageIndex < 0)
                pageIndex = 0;

            try
            {
                var igs = InternalApi.ImageService.ListImageGallerys(groupId, pageSize, pageIndex);
                return new PagedList<ImageGallery>(igs.Select(x => new ImageGallery(x)), igs.PageSize, igs.PageIndex, igs.TotalCount);

                /*
                if (string.Equals(sortBy, "TopImageGallerys", StringComparison.OrdinalIgnoreCase))
                {
                    var group = TEApi.Groups.Get(new GroupsGetOptions { Id = groupId });
                    if (group == null || group.HasErrors())
                        return new PagedList<ImageGallery>();

                    var images = new List<Image>();
                    foreach (var image in images)
                    {
                        images.Add(image);
                    }

                    IEnumerable<ImageGallery> ie = images as IEnumerable<ImageGallery>;
                    return new PagedList<ImageGallery>(ie, pageSize, pageIndex, images.Count);
                }
                else
                {
                    var igs = InternalApi.ImageService.ListImageGallerys(groupId, pageSize, pageIndex);
                    return new PagedList<ImageGallery>(igs.Select(x => new ImageGallery(x)), igs.PageSize, igs.PageIndex, igs.TotalCount);
                }
                */
            }
            catch (Exception ex)
            {
                return new PagedList<ImageGallery>(new AdditionalInfo(new Error(ex.GetType().FullName, ex.Message)));
            }
        }

        public static bool CanCreate(int groupId)
		{
			return InternalApi.ImageService.CanCreate(groupId);
		}

		public static bool CanVote(Guid igId)
		{
            return InternalApi.ImageService.CanVote(igId, TEApi.Users.AccessingUser.Id.Value);
		}

		public static bool CanEdit(Guid igId)
		{
            return InternalApi.ImageService.CanModerate(igId, TEApi.Users.AccessingUser.Id.Value);
		}

		public static bool CanDelete(Guid igId)
		{
            return InternalApi.ImageService.CanModerate(igId, TEApi.Users.AccessingUser.Id.Value);
		}

//		public static string UI(Guid igId, bool readOnly = false, bool showNameAndDescription = true)
		public static string UI(Guid igId, string name = "")
		{
            /*
			return string.Concat(
				"<div class=\"ui-poll\" data-pollid=\"",
				igId.ToString(),
				"\" data-readonly=\"",
				(readOnly || !CanVote(igId)).ToString().ToLower(),
				"\" data-showname=\"",
				showNameAndDescription.ToString().ToLower(),
				"\"></div>");
             */
            return string.Concat(name, "<br/>");
		}
	}
}

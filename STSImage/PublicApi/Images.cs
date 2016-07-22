using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telligent.Evolution.Extensibility.Api.Entities.Version1;
using Internal = STSImage.InternalApi;

namespace STSImage.PublicApi
{
	public static class Images
	{
		private static readonly ImagesEvents _events = new ImagesEvents();

		public static ImagesEvents Events
		{
			get { return _events; }
		}

		public static Image Get(Guid id)
		{
			try
			{
				var image = Internal.ImageService.GetImage(id);
				if (image == null)
					return null;

				var ig = Internal.ImageService.GetImageGallery(image.ImageGalleryId);
				if (ig == null)
					return null;

				return new Image(image, ig);
			}
			catch (Exception ex)
			{
				return new Image(new AdditionalInfo(new Error(ex.GetType().FullName, ex.Message)));
			}
		}
        public static Image GetImageGalleryByCurrent(Guid id,int userId)
        {
            try
            {
                var image = Internal.ImageService.GetImage(id);
                if (image == null)
                    return null;

                var ig = Internal.ImageService.GetImageGalleryByCurrent(image.ImageGalleryId, userId);
                if (ig == null)
                    return null;

                return new Image(image, ig);
            }
            catch (Exception ex)
            {
                return new Image(new AdditionalInfo(new Error(ex.GetType().FullName, ex.Message)));
            }
        }
		public static AdditionalInfo Delete(Guid id)
		{
			try
			{
				var image = Internal.ImageService.GetImage(id);
				if (image != null)
					Internal.ImageService.DeleteImage(image);

				return new AdditionalInfo();
			}
			catch (Exception ex)
			{
				return new AdditionalInfo(new Error(ex.GetType().FullName, ex.Message));
			}
		}

		public static Image Create(Guid imgId, string title)
		{
			try
			{
				var image = new Internal.Image();
				image.Id = imgId;
				image.Title = title;

				Internal.ImageService.AddUpdateImage(image);

				return Get(image.Id);
			}
			catch (Exception ex)
			{
				return new Image(new AdditionalInfo(new Error(ex.GetType().FullName, ex.Message)));
			}
		}

		public static Image Update(Guid id, string title = null)
		{
			try
			{
				var img = Internal.ImageService.GetImage(id);
				if (img != null)
				{
					if (title != null)
						img.Title = title;

					Internal.ImageService.AddUpdateImage(img);
				}

				return Get(id);
			}
			catch (Exception ex)
			{
				return new Image(new AdditionalInfo(new Error(ex.GetType().FullName, ex.Message)));
			}
		}
	}
}

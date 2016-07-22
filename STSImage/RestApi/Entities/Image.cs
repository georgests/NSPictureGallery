using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace STSImage.RestApi
{
	public class Image
	{
		public Image()
		{
		}

        internal Image(InternalApi.Image image, InternalApi.ImageGallery ig)
		{
            ImageGalleryId = image.ImageGalleryId;
			Id = ig.Id;
			Name = image.Title;
		}

		public Guid ImageGalleryId { get; set; }
		public Guid Id { get; set; }
		public string Name { get; set; }
		public int VoteCount { get; set; }
	}
}

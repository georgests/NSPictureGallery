using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Internal = STSImage.InternalApi;
using Telligent.Evolution.Extensibility.Api.Entities.Version1;

namespace STSImage.PublicApi
{
	public class Image : ApiEntity
	{
		Internal.Image _image = null;

		public Image()
			: base()
		{
		}

		public Image(AdditionalInfo additionalInfo)
			: base(additionalInfo)
		{
		}

		public Image(IList<Warning> warnings, IList<Error> errors)
			: base(warnings, errors)
		{
		}

		internal Image(Internal.Image image, Internal.ImageGallery imageGallery)
			: base()
		{
			_image = image;
		}

		ImageGallery _imageGallery;
		public ImageGallery ImageGallery
		{
			get
			{
                if (_imageGallery == null && _image != null)
                    _imageGallery = ImageGallerys.Get(_image.ImageGalleryId);

                return _imageGallery;
			}
		}
    

		public Guid Id
		{
			get { return _image == null ? Guid.Empty : _image.Id; }
		}

		public string Title
		{
			get { return _image == null ? string.Empty : _image.Title; }
		}
        
        public string FilePath
        {
            get { return _image == null ? string.Empty : _image.FilePath; }
        }

        public int Degree
        {
            get { return _image == null ? 0 : _image.Degree; }
        }

        public Guid ImageGalleryId
        {
            get { return _image == null ? new Guid() : _image.ImageGalleryId; }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telligent.Evolution.Extensibility.Version1;
using Telligent.Evolution.Extensibility.UI.Version1;

namespace STSImage.Plugins
{
	public class ImageGalleryWidgetContextProvider : IPlugin, IScriptedContentFragmentContextProvider
	{
        private readonly Guid ImageGalleryContextId = new Guid("40306c023e6d4f1590f291119ff7b579");

		#region IPlugin Members

		public string Name
		{
			get { return "ImageGallery Widget Context Provider."; }
		}

		public string Description
		{
			get { return "Enables Studio Widgets to depend on imagegallry-related contexts."; }
		}

		public void Initialize()
		{
		}

		#endregion

		#region IScriptedContentFragmentContextProvider Members

		public IEnumerable<ContextItem> GetSupportedContextItems()
		{
			return new ContextItem[] {
				new ContextItem("ImageGallery", ImageGalleryContextId)
			};
		}

		public bool HasContextItem(System.Web.UI.Page page, Guid contextItemId)
		{
			if (ImageGalleryContextId != contextItemId)
				return false;

			Guid imageGalleryId;
			if (!Guid.TryParse(page.Request.QueryString["ImageGalleryId"], out imageGalleryId))
				return false;

			return InternalApi.ImageService.GetImageGallery(imageGalleryId) != null;
		}

		#endregion
	}
}

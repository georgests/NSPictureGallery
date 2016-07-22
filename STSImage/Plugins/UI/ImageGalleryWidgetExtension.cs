using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telligent.Evolution.Extensibility.Version1;
using Telligent.Evolution.Extensibility.UI.Version1;

namespace STSImage.Plugins
{
	public class ImageGalleryWidgetExtension : IPlugin, IScriptedContentFragmentExtension
	{
		#region IPlugin Members

		public string Name
		{
			get { return "NS Image Gallery Widget Extension (sts_v1_imagegallery)"; }
		}

		public string Description
		{
			get { return "Exposes Image Gallery to Studio Widgets."; }
		}

		public void Initialize()
		{
		}

		#endregion

		#region IExtension Members

		public object Extension
		{
			get { return new WidgetApi.ImageGallerys(); }
		}

		public string ExtensionName
		{
            get { return "sts_v1_imagegallery"; }
		}

		#endregion
	}
}

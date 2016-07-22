using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telligent.Evolution.Extensibility.Version1;
using Telligent.Evolution.Extensibility.UI.Version1;

namespace STSImage.Plugins
{
	public class ImageGalleryFactoryDefaultWidgetProvider : IPlugin, IScriptedContentFragmentFactoryDefaultProvider
	{
        public readonly Guid _id = new Guid("acfc246b33e14e2f9b1b9eecd828b04e");

		#region IPlugin Members

		public string Name
		{
			get { return "NS Image Gallery Factory Default Widget Provider"; }
		}

		public string Description
		{
			get { return "NS Defines the default widget set for Image Gallery."; }
		}

		public void Initialize()
		{
		}

		#endregion

		#region IScriptedContentFragmentFactoryDefaultProvider Members

		public Guid ScriptedContentFragmentFactoryDefaultIdentifier
		{
			get { return _id; }
		}

		#endregion
	}
}

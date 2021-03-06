﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telligent.Evolution.Extensibility.Version1;
using Telligent.Evolution.Extensibility.UI.Version1;
using Telligent.DynamicConfiguration.Components;

namespace STSImage.Plugins
{
	public class ImageGalleryGroupNavigation : IPlugin, ITranslatablePlugin, IGroupCustomNavigationPlugin, IGroupDefaultCustomNavigationPlugin
	{
        private readonly Guid _defaultId = new Guid("f378af53a64c4e9a9eb8bcdd778eefe3");

		ITranslatablePluginController _translation;

		#region IPlugin Members

		public string Name
		{
			get { return "Image Gallery Group Navigation"; }
		}

		public string Description
		{
			get { return "Adds Image Gallery custom navigation support within groups."; }
		}

		public void Initialize()
		{
		}

		#endregion

		#region ITranslatablePlugin Members

		public Translation[] DefaultTranslations
		{
			get
			{
				var translation = new Translation("en-US");
				translation.Set("configuration_options", "Options");
				translation.Set("configuration_label", "Label");
				translation.Set("configuration_label_description", "Enter an optional label for this link.");
				translation.Set("configuration_defaultLabel", "ImageGallery");
                translation.Set("navigationitem_name", "Group ImageGallery");

				return new Translation[] { translation };
			}
		}

		public void SetController(ITranslatablePluginController controller)
		{
			_translation = controller;
		}

		#endregion

		#region IGroupCustomNavigationPlugin Members

		public PropertyGroup[] GetConfigurationProperties(int groupId)
		{
			PropertyGroup group = new PropertyGroup("options", _translation.GetLanguageResourceValue("configuration_options"), 1);

			group.Properties.Add(new Property("groupid",  "", PropertyType.Int, 1, groupId.ToString()) { Visible = false, Editable = false });
			group.Properties.Add(new Property("label",  _translation.GetLanguageResourceValue("configuration_label"), PropertyType.String, 2,  "") { DescriptionText = _translation.GetLanguageResourceValue("configuration_label_description") });
			
			return new PropertyGroup[] { group };
		}

		public ICustomNavigationItem GetNavigationItem(Guid id, ICustomNavigationItemConfiguration configuration)
		{
			int groupId = configuration.GetIntValue("groupid", -1);
			if (groupId == -1)
				return null;

			string label = configuration.GetStringValue("label", "");

			return new ImageGalleryGroupNavigationItem(this, configuration, id, groupId, () => string.IsNullOrEmpty(label) ?  _translation.GetLanguageResourceValue("configuration_defaultLabel") : label);
		}

		public string NavigationTypeName
		{
			get { return _translation.GetLanguageResourceValue("navigationitem_name"); }
		}

		#endregion

		#region IGroupDefaultCustomNavigationPlugin Members

		public int DefaultOrderNumber
		{
			get { return 200; }
		}

		public ICustomNavigationItem GetDefaultNavigationItem(int groupId, ICustomNavigationItemConfiguration configuration)
		{
			return new ImageGalleryGroupNavigationItem(this, configuration, _defaultId, groupId, () => _translation.GetLanguageResourceValue("configuration_defaultLabel"));
		}

		#endregion

		public class ImageGalleryGroupNavigationItem : ICustomNavigationItem
		{
			int _groupId;
			Func<string> _getLabel;

			internal ImageGalleryGroupNavigationItem(ICustomNavigationPlugin plugin, ICustomNavigationItemConfiguration configuration, Guid id, int groupId, Func<string> getLabel)
			{
				Plugin = plugin;
				Configuration = configuration;
				UniqueID = id;
				_groupId = groupId;
				_getLabel = getLabel;
			}

			#region ICustomNavigationItem Members

			public ICustomNavigationItem[] Children { get; set; }
			public ICustomNavigationItemConfiguration Configuration { get; private set; }
			public string CssClass { get { return "list-polls"; } }
			public string Label { get { return _getLabel(); } }
			public ICustomNavigationPlugin Plugin { get; private set; }
			public Guid UniqueID { get; private set; }

			public bool IsSelected(string currentFullUrl)
			{
				return currentFullUrl.Contains("/p/imagegallery.aspx") || currentFullUrl.Contains("/p/imagegallerys.aspx") || currentFullUrl.Contains("/p/createeditimagegallery.aspx");
			}

			public bool IsVisible(int userID)
			{
				return !string.IsNullOrEmpty(Url);
			}

			public string Url
			{
				get
				{
					return InternalApi.ImageService.ImageGalleryListUrl(_groupId);
				}
			}

			#endregion
		}
	}
}

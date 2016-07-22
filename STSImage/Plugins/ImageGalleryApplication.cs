using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telligent.Evolution.Extensibility.Version1;
using Telligent.DynamicConfiguration.Components;
using UIApi = Telligent.Evolution.Extensibility.UI.Version1;
using Telligent.Evolution.Extensibility.Storage.Version1;
using System.Xml;
using TEApi = Telligent.Evolution.Extensibility.Api.Version1.PublicApi;
using Telligent.Evolution.Extensibility.Api.Version1;


namespace STSImage.Plugins
{
    public class ImageGalleryApplication : IPlugin, IConfigurablePlugin, IRequiredConfigurationPlugin, IInstallablePlugin, IPluginGroup, ICentralizedFileStore 
	{
		ImageGalleryFactoryDefaultWidgetProvider _widgetProvider;

		#region IPlugin Members

		public string Name
		{
			get { return "NS Image Gallery Application"; }
		}

		public string Description
		{
			get { return "Adds support for defining images within groups."; }
		}

		public void Initialize()
		{
			_widgetProvider = PluginManager.Get<ImageGalleryFactoryDefaultWidgetProvider>().FirstOrDefault();
		}

		#endregion

		#region IConfigurablePlugin Members

		public PropertyGroup[] ConfigurationOptions
		{
			get 
			{
				PropertyGroup group = new PropertyGroup("setup", "Setup", 1);
				group.Properties.Add(new Property("connectionString", "Database Connection String", PropertyType.String, 1, "") { DescriptionText = "The connection string used to access a SQL 2008 or newer database. The user identified should have db_owner permissions to the database." });
                return new PropertyGroup[] { group };
			}
		}

		public void Update(IPluginConfiguration configuration)
		{
			InternalApi.ImageDataService.ConnectionString = configuration.GetString("connectionString");
		}

		#endregion

		#region IRequiredConfigurationPlugin Members

		public bool IsConfigured
		{
			get 
			{
				return InternalApi.ImageDataService.IsConnectionStringValid();
			}
		}

		#endregion

		#region IInstallablePlugin Members

		public void Install(Version lastInstalledVersion)
		{
			if (lastInstalledVersion == null || lastInstalledVersion.Major == 0)
				InternalApi.ImageDataService.Install();

            /*
			if (lastInstalledVersion == null || lastInstalledVersion <= new Version(1, 1))
				InternalApi.ImageDataService.Install("update-1.1.sql");
            */

			InternalApi.ImageDataService.Install("storedprocedures.sql");

			#region Install Widgets

			_widgetProvider = PluginManager.Get<ImageGalleryFactoryDefaultWidgetProvider>().FirstOrDefault();
			UIApi.FactoryDefaultScriptedContentFragmentProviderFiles.DeleteAllFiles(_widgetProvider);

			var definitionFiles = new string[] { 
                "ImageGallery-Widget.xml"
                /*
				"ImageGalleryList-Widget.xml", 
                "ImageGalleryFileupload-Widget.xml"
				"PollingBreadcrumbs-Widget.xml",
				"PollingCreateEditPoll-Widget.xml",
				"PollingLinks-Widget.xml",
				"PollingPoll-Widget.xml",
				"PollingTopPollList-Widget.xml",
				"PollingTitle-Widget.xml",
				"PollingAddCommentForm-Widget.xml",
				"PollingCommentList-Widget.xml"
                */
			};

			foreach (string definitionFile in definitionFiles)
			{
				using (var stream = InternalApi.EmbeddedResources.GetStream("STSImage.Resources.Widgets." + definitionFile))
				{
					UIApi.FactoryDefaultScriptedContentFragmentProviderFiles.AddUpdateDefinitionFile(_widgetProvider, definitionFile, stream);
				}
			}

			var supplementaryFiles = new Dictionary<Guid,string[]>();
            supplementaryFiles[new Guid("ecea783dd78b476b97c84c85f440aba6")] = new string[] {
				"ImageGallery/ui.js",
                "ImageGallery/loadComments.vm",
				"ImageGallery/UploadImageData.vm",
				"ImageGallery/ImageGallery.vm",
				"ImageGallery/AddFile.vm",
				"ImageGallery/popup.js",
                "ImageGallery/Images/camera.jpg",
                "ImageGallery/Images/delete_img_bttn.jpg",
                "ImageGallery/Images/edit_done_bttn.png",
                "ImageGallery/Images/edit_text_icon.png",
                "ImageGallery/Images/lightbox-blank.gif",
                "ImageGallery/Images/lightbox-btn-close.gif",
                "ImageGallery/Images/lightbox-btn-next.gif",
                "ImageGallery/Images/lightbox-btn-prev.gif",
                "ImageGallery/Images/lightbox-ico-loading.gif",              
                "ImageGallery/Images/Rotate-icon-red.png",
                "ImageGallery/Images/rotate-icon.png",
               

/*              "ImageGallery/jquery-ui/jquery.js",
                "ImageGallery/jquery-ui/jquery-ui.min.css",
                "ImageGallery/jquery-ui/jquery-ui.min.js",
                "ImageGallery/jquery-ui/jquery-ui.structure.min.css",
                "ImageGallery/jquery-ui/jquery-ui.theme.min.css",
                "ImageGallery/jquery-ui/images/animated-overlay.gif",
                "ImageGallery/jquery-ui/images/ui-bg_diagonals-thick_18_b81900_40x40.png",
                "ImageGallery/jquery-ui/images/ui-bg_diagonals-thick_20_666666_40x40.png",
                "ImageGallery/jquery-ui/images/ui-bg_flat_10_000000_40x100.png",
                "ImageGallery/jquery-ui/images/ui-bg_glass_100_f6f6f6_1x400.png",
                "ImageGallery/jquery-ui/images/ui-bg_glass_100_fdf5ce_1x400.png",
                "ImageGallery/jquery-ui/images/ui-bg_glass_65_ffffff_1x400.png",
                "ImageGallery/jquery-ui/images/ui-bg_gloss-wave_35_f6a828_500x100.png",
                "ImageGallery/jquery-ui/images/ui-bg_highlight-soft_100_eeeeee_1x100.png",
                "ImageGallery/jquery-ui/images/ui-bg_highlight-soft_75_ffe45c_1x100.png",
                "ImageGallery/jquery-ui/images/ui-icons_222222_256x240.png",
                "ImageGallery/jquery-ui/images/ui-icons_228ef1_256x240.png",
                "ImageGallery/jquery-ui/images/ui-icons_ef8c08_256x240.png",
                "ImageGallery/jquery-ui/images/ui-icons_ffd27a_256x240.png",
                "ImageGallery/jquery-ui/images/ui-icons_ffffff_256x240.png"
 */
			};

            /*
            supplementaryFiles[new Guid("2117b4da3bbe4932b2e9904359a73224")] = new string[] {
				"STSImageGalleryList/ImageGalleryList.vm",
				"STSImageGalleryList/ui.js"
			};
            */

            /*
            supplementaryFiles[new Guid("df14363b01724be4b7978c67499f577f")] = new string[] {
				"ImageGallery/ui.js"
			};
            */

            foreach (Guid instanceId in supplementaryFiles.Keys)
			{
				foreach (string relativePath in supplementaryFiles[instanceId])
				{
					using (var stream = InternalApi.EmbeddedResources.GetStream("STSImage.Resources.Widgets." + relativePath.Replace("/", ".")))
					{
						UIApi.FactoryDefaultScriptedContentFragmentProviderFiles.AddUpdateSupplementaryFile(_widgetProvider, instanceId, relativePath.Substring(relativePath.LastIndexOf("/") + 1), stream);
					}
				}
			}

			#endregion

			#region Install latest version of the Image Gallert page into all group themes (and revert any configured defaults or contextul versions of these pages)
			
			XmlDocument xml;
			foreach (var theme in UIApi.Themes.List(UIApi.ThemeTypes.Group))
			{
				var themeName = "Fiji";
				if (theme.Name == "424eb7d9138d417b994b64bff44bf274") // use the Enterprise version of the page for the Enterprise theme
					themeName = "Enterprise";

				if (theme.IsConfigurationBased)
				{
					xml = new XmlDocument();
					xml.LoadXml(InternalApi.EmbeddedResources.GetString("STSImage.Resources.Pages.imagegallery-" + themeName + "-Groups-Page.xml"));
					UIApi.ThemePages.AddUpdateFactoryDefault(theme, xml.SelectSingleNode("theme/contentFragmentPages/contentFragmentPage"));

                    UIApi.ThemePages.DeleteDefault(theme, "imagegallery", true);

                    UIApi.ThemePages.Delete(theme, "createeditimagegallery", true);
                    UIApi.ThemePages.Delete(theme, "imagegallery", true);
                    UIApi.ThemePages.Delete(theme, "imagegallerys", true);
                }
				else
				{
					// non-configured-based themes don't support editing factory default pages.

					xml = new XmlDocument();
                    xml.LoadXml(InternalApi.EmbeddedResources.GetString("STSImage.Resources.Pages.imagegallery-" + themeName + "-Groups-Page.xml"));
					UIApi.ThemePages.AddUpdateDefault(theme, xml.SelectSingleNode("theme/contentFragmentPages/contentFragmentPage"));

				}

                UIApi.ThemePages.DeleteDefault(theme, "imagegallery", true);

				UIApi.ThemePages.Delete(theme, "createeditimagegallery", true);
				UIApi.ThemePages.Delete(theme, "imagegallery", true);
				UIApi.ThemePages.Delete(theme, "imagegallerys", true);
            }

			#endregion

			#region Install CSS Files

			foreach (var theme in UIApi.Themes.List(UIApi.ThemeTypes.Site))
			{
				if (theme.IsConfigurationBased)
				{
					var themeName = "Fiji";
					if (theme.Name == "424eb7d9138d417b994b64bff44bf274") // use the Enterprise version of the page for the Enterprise theme
						themeName = "Enterprise";

					using (var stream = InternalApi.EmbeddedResources.GetStream("STSImage.Resources.Css.polling-" + themeName + ".css"))
					{
						UIApi.ThemeFiles.AddUpdateFactoryDefault(theme, UIApi.ThemeProperties.CssFiles, "polling.css", stream, (int) stream.Length);
						stream.Seek(0, System.IO.SeekOrigin.Begin);
						UIApi.ThemeFiles.AddUpdate(theme, UIApi.ThemeContexts.Site, UIApi.ThemeProperties.CssFiles, "polling.css", stream, (int) stream.Length);
					}
				}
			}

			#endregion
		}

		public void Uninstall()
		{
			InternalApi.ImageDataService.UnInstall();

			#region Delete custom pages used to support polls (from factory defaults, configured defaults, and contextual pages)
			
			foreach (var theme in UIApi.Themes.List(UIApi.ThemeTypes.Group))
			{
				if (theme.IsConfigurationBased)
				{
					UIApi.ThemePages.DeleteFactoryDefault(theme, "createeditpoll", true);
					UIApi.ThemePages.DeleteFactoryDefault(theme, "poll", true);
					UIApi.ThemePages.DeleteFactoryDefault(theme, "polls", true);
				}

				UIApi.ThemePages.DeleteDefault(theme, "createeditpoll", true);
				UIApi.ThemePages.DeleteDefault(theme, "poll", true);
				UIApi.ThemePages.DeleteDefault(theme, "polls", true);

				UIApi.ThemePages.Delete(theme, "createeditpoll", true);
				UIApi.ThemePages.Delete(theme, "poll", true);
				UIApi.ThemePages.Delete(theme, "polls", true);
			}

			#endregion

			#region Remove Widget Files

			UIApi.FactoryDefaultScriptedContentFragmentProviderFiles.DeleteAllFiles(_widgetProvider);

			#endregion

			#region Uninstall CSS Files

			foreach (var theme in UIApi.Themes.List(UIApi.ThemeTypes.Site))
			{
				if (theme.IsConfigurationBased)
				{
					UIApi.ThemeFiles.Remove(theme, UIApi.ThemeContexts.Site, UIApi.ThemeProperties.CssFiles, "polling.css");
					UIApi.ThemeFiles.RemoveFactoryDefault(theme, UIApi.ThemeProperties.CssFiles, "polling.css");
				}
			}

			#endregion
		}

		public Version Version
		{
			get { return GetType().Assembly.GetName().Version; }
		}

		#endregion

		#region IPluginGroup Members

		public IEnumerable<Type> Plugins
		{
			get 
			{
				return new Type[] { 
					typeof(ImageGalleryContentType),
					typeof(ImageGallerySearchCategories),
					typeof(ImageGalleryWidgetExtension),
					typeof(ImageGalleryFactoryDefaultWidgetProvider),
					typeof(ImageGalleryRestEndpoints),
//					typeof(ImageGalleryNewPostLink),
//					typeof(ImageGalleryHeaderExtension),
					typeof(ImageGalleryWidgetContextProvider),
//					typeof(ImageGalleryUrlsWidgetExtension),
//					typeof(PollAnswerWidgetExtension),
//					typeof(PollVoteWidgetExtension),
//					typeof(PollGroupNavigation),
//					typeof(PollViewer),
//					typeof(PollVotesMetric),
//					typeof(TopPollsScore)
				};
			}
		}

		#endregion

        #region ICentralizedFileStore Members
        public const string FILE_STORAGE_KEY = "STSImages";

        public string FileStoreKey
        {
            get { return FILE_STORAGE_KEY; }
        }
        #endregion

	}
}

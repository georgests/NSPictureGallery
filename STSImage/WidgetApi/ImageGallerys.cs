using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telligent.Evolution.Extensibility.Api.Entities.Version1;
using Telligent.Evolution.Extensibility.Version1;
using System.Collections;
using Telligent.Evolution.Extensibility.Storage.Version1;
using TEApi = Telligent.Evolution.Extensibility.Api.Version1.PublicApi;
using System.IO;
using Telligent.Evolution.Extensibility.Api.Version1;
using System.Web;

namespace STSImage.WidgetApi
{
	[Documentation(Category="Image")]
	public class ImageGallerys
	{
        
		[Documentation("The content type identifier for images.")]
		public Guid ContentTypeId { get { return PublicApi.ImageGallerys.ContentTypeId; } }

		[Documentation("The current contextual poll.")]
		public PublicApi.ImageGallery Current
		{
			get
			{
				var context = System.Web.HttpContext.Current;
				if (context == null)
					return null;

				Guid imgGalleryId;
                if (!Guid.TryParse(context.Request.QueryString["ImageGalleryId"], out imgGalleryId))
					return null;

                return Get(imgGalleryId);
			}
		}

		[Documentation("List Image Gallerys within a group.")]
		public PagedList<PublicApi.ImageGallery> List(
			[Documentation("The group identifier.")]
			int groupId
			)
		{
			return List(groupId, null);
		}

		[Documentation("List Image Gallerys within a group.")]
		public PagedList<PublicApi.ImageGallery> List(
			[Documentation("The group identifier.")]
			int groupId, 
			[
			Documentation(Name="PageIndex", Type=typeof(int), Default=0, Description="The page index."),
			Documentation(Name="PageSize", Type=typeof(int), Default=20, Description="The number of Image Gallerys to return in a single page."),
			Documentation(Name="SortBy", Type=typeof(string), Default="Date", Description="The sorting mechanism.", Options=new string[] { "Date", "TopImageGallerys" })
			]
			IDictionary options
			)
		{
			int pageIndex = 0;
			int pageSize = 20;
			string sortBy = "Date";

			if (options != null)
			{
				if (options["PageIndex"] != null)
					pageIndex = Convert.ToInt32(options["PageIndex"]);

				if (options["PageSize"] != null)
					pageSize = Convert.ToInt32(options["PageSize"]);

				if (options["SortBy"] != null)
					sortBy = options["SortBy"].ToString();
			}

			return PublicApi.ImageGallerys.List(groupId, pageIndex, pageSize, sortBy);
		}

        
		[Documentation("Get a image gallery.")]
		public PublicApi.ImageGallery Get(
			[Documentation("The image gallerys's identifier.")]
			Guid id
			)
		{
			return PublicApi.ImageGallerys.Get(id);
		}

        [Documentation("Get a image gallery By current Id.")]
        public PublicApi.ImageGallery GetImageGalleryByCurrent(
            [Documentation("The image gallerys's identifier.")]
			Guid id,int userId
            )
        {
            return PublicApi.ImageGallerys.GetImageGalleryByCurrent(id,userId);
        }

         [Documentation("Get Web Browser Name.")]
        public string GetWebBrowserName()
        {
            string WebBrowserName = string.Empty;
            try
            {
                WebBrowserName = HttpContext.Current.Request.Browser.Browser;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return WebBrowserName;
        }
               
        		[Documentation("Delete a image gallery.")]
		public AdditionalInfo Delete(
			[Documentation("The image gallery's identifier.")]
			Guid id
			)
		{
			return PublicApi.ImageGallerys.Delete(id);
		}

		[Documentation("Create a new Image Gallery.")]
		public PublicApi.ImageGallery Create(
			[Documentation("The identifier of the group in which to create the image gallery.")]
			int groupId, 
			[Documentation("The name of the new image gallery.")]
			string name
			)
		{
			return Create(groupId, name, null);
		}

		[Documentation("Create a new image gallery.")]
		public PublicApi.ImageGallery Create(
			[Documentation("The identifier of the group in which to create the image gallery.")]
			int groupId, 
			[Documentation("The name of the new image gallery.")]
			string name, 
			[
				Documentation(Name="Description", Type=typeof(string), Description="The description of the image gallery."),
			]
			IDictionary options)
		{
			string description = null;

			if (options != null)
			{
				if (options["Description"] != null)
					description = options["Description"].ToString();

			}

			return PublicApi.ImageGallerys.Create(groupId, name, description);
		}

		[Documentation("Update an existing image gallery.")]
		public PublicApi.ImageGallery Update(
			[Documentation("The identifier of the Image Gallery to update.")]
			Guid id, 
			[
				Documentation(Name="Description", Type=typeof(string), Description="The description of the Image Gallery."),
				Documentation(Name = "Name", Type = typeof(string), Description = "The name of the Image Gallery."),
			]
			IDictionary options
			)
		{
			string name = null;
			string description = null;

			if (options != null)
			{
				if (options["Description"] != null)
					description = options["Description"].ToString();

				if (options["Name"] != null)
					name = options["Name"].ToString();

			}

			return PublicApi.ImageGallerys.Update(id, name, description);
		}

        /*
		[Documentation("Identifies if the accessing user can create a poll in the given group.")]
		public bool CanCreate(int groupId)
		{
			return PublicApi.ImageGallery.CanCreate(groupId);
		}

		[Documentation("Identifies if the accessing user can vote on the given poll.")]
		public bool CanVote(Guid pollId)
		{
			return PublicApi.ImageGallery.CanVote(pollId);
		}
        */

		[Documentation("Identifies if the accessing user can edit the given Image Gallery.")]
		public bool CanEdit(Guid igId)
		{
			return PublicApi.ImageGallerys.CanEdit(igId);
		}

		[Documentation("Identifies if the accessing user can delete the given Image Gallery.")]
		public bool CanDelete(Guid igId)
		{
			return PublicApi.ImageGallerys.CanDelete(igId);
		}

		[Documentation("Renders the Image Gallery user interface.")]
		public string UI(
			Guid pollId
			)
		{
			return UI(pollId, null);
		}

		[Documentation("Renders the Image Gallery user interface.")]
		public string UI(
			Guid pollId, 
			[
				Documentation(Name="ReadOnly", Type=typeof(bool), Description="When true, the UI does not support interactions.", Default=false),
				Documentation(Name="ShowNameAndDescription", Type=typeof(bool), Description="When true, the UI includes the name and description of the Image Gallery.", Default=true)
			]
			IDictionary options
			)
		{
			// bool readOnly = false;
			// bool showNameAndDescription = true;
            string name = null;

			if (options != null)
			{
                /*
				if (options["ReadOnly"] != null)
					readOnly = Convert.ToBoolean(options["ReadOnly"]);

				if (options["ShowNameAndDescription"] != null)
					showNameAndDescription = Convert.ToBoolean(options["ShowNameAndDescription"]);
                 */
                if (options["name"] != null)
                    name = Convert.ToString(options["name"]);
            }

			return PublicApi.ImageGallerys.UI(pollId, name);
		}



        /*
         * Upload Image Data Handling
         */


        private string imageFilePath = "sts.imagegellery.images";

        public string UploadImageData(string fileName, byte[] fileData)
        {
            string retVal = " ";
            try
            {
                Stream stream = new MemoryStream(fileData);

                ICentralizedFileStorageProvider storageProvider = CentralizedFileStorage.GetFileStore("STSImages");
                if (storageProvider == null)
                {
                    return "Failed to create File Storage Provider";
                }

                var file = storageProvider.AddFile(imageFilePath, fileName, stream, true);

                stream.Close();

                retVal =  file.GetDownloadUrl();
            }
            catch (Exception e)
            {
                return "EXCEPTION:====> " + retVal + " === " + e.ToString().Trim();
            }

             return retVal;
        }

        private void addDefaultImageGallery(int groupId, int userId)
        {
            User user = Telligent.Evolution.Extensibility.Api.Version1.PublicApi.Users.AccessingUser;
            var group = TEApi.Groups.Get(new GroupsGetOptions { Id = groupId });
            var ig = new InternalApi.ImageGallery();
            if (group.Id.HasValue)
            {
                ig.GroupId = group.Id.Value;
            }
            ig.Id = Guid.NewGuid();
            ig.Name = "Default Image Gallery";
            ig.IsEnabled = true;
            ig.AuthorUserId = userId; // user.Id ?? 0;

            InternalApi.ImageDataService.AddDefaultImageGallery(ig);

        }


        public Guid FirstImageGalleryId(int groupId, int userId)
        {
            addDefaultImageGallery(groupId, userId);
            var listIg = InternalApi.ImageService.ListImageGallerys(userId, 10, 0);
            var ig = listIg.FirstOrDefault<InternalApi.ImageGallery>();
            return ig.Id;
        }

        public PagedList<PublicApi.ImageGallery> ListImageGallerys(int userId, int pageSize = 20, int pageIndex = 0, string sortBy = "Date")
        {
            return PublicApi.ImageGallerys.ListImageGallerys(userId, pageSize, pageIndex);
        }


	}
}

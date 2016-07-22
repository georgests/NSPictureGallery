using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Internal = STSImage.InternalApi;
using Telligent.Evolution.Extensibility.Api.Entities.Version1;
using Telligent.Evolution.Extensibility.Api.Version1;
using TEApi = Telligent.Evolution.Extensibility.Api.Version1.PublicApi;
using Telligent.Evolution.Extensibility.Content.Version1;

namespace STSImage.PublicApi
{
	public class ImageGallery : ApiEntity, IContent
	{
		Internal.ImageGallery _imageGallery = null;

		public ImageGallery()
			: base()
		{
		}

		public ImageGallery(AdditionalInfo additionalInfo)
			: base(additionalInfo)
		{
		}

		public ImageGallery(IList<Warning> warnings, IList<Error> errors)
			: base(warnings, errors)
		{
		}

        internal ImageGallery(Internal.ImageGallery imageGallery)
			: base()
		{
            _imageGallery = imageGallery;
		}

		public Guid Id
		{
			get { return _imageGallery == null ? Guid.Empty : _imageGallery.Id; }
		}

		public Guid ContentId
		{
			get { return Id; }
		}

		public string Name
		{
			get { return _imageGallery == null ? string.Empty : _imageGallery.Name; }
		}

		public string Description()
		{
			return Description("web");
		}

		public string Description(string target)
		{
			if (_imageGallery != null)
				return InternalApi.ImageService.RenderImageGalleryDescription(_imageGallery, target);
			else
				return string.Empty;
		}

		User _author;
		public User Author
		{
			get
			{
				if (_author == null && _imageGallery != null)
					_author = TEApi.Users.Get(new UsersGetOptions() { Id = _imageGallery.AuthorUserId });

				return _author;
			}
		}

		Group _group;
		public Group Group
		{
			get
			{
				if (_group == null && _imageGallery != null)
					_group = TEApi.Groups.Get(new GroupsGetOptions() { Id = _imageGallery.GroupId });

				return _group;
			}
		}

		public bool IsEnabled
		{
			get { return _imageGallery == null ? false : _imageGallery.IsEnabled; }
		}

		public DateTime CreatedDate
		{
			get { return _imageGallery == null ? DateTime.Now : InternalApi.Formatting.FromUtcToUserTime(_imageGallery.CreatedDateUtc); }
		}

		public DateTime LastUpdatedDate
		{
            get { return _imageGallery == null ? DateTime.Now : InternalApi.Formatting.FromUtcToUserTime(_imageGallery.LastUpdatedDateUtc); }
		}

		public string Url
		{
			get { return _imageGallery == null ? null : InternalApi.ImageService.ImageGalleryUrl(_imageGallery.Id); }
		}
/*
		public bool HideResultsUntilVotingComplete
		{
			get { return _imageGallery == null ? false : _imageGallery.HideResultsUntilVotingComplete; }
		}

		public DateTime? VotingEndDate
		{
			get { return _imageGallery == null || !_imageGallery.VotingEndDateUtc.HasValue ? null : (DateTime?) InternalApi.Formatting.FromUtcToUserTime(_imageGallery.VotingEndDateUtc.Value); }
		}

		public int TotalVotes
		{
			get { return _imageGallery == null ? 0 : _imageGallery.Images.Sum(x => x.VoteCount); }
		}
*/

		List<Image> _images;
		public List<Image> Images
		{
			get
			{
				if (_images == null && _imageGallery != null && _imageGallery.Images != null)
					_images = new List<Image>(_imageGallery.Images.Select(x => new Image(x, _imageGallery)));

				return _images;
			}
		}

		#region IContent Members

		IApplication IContent.Application
		{
			get { return Group; }
		}

		string IContent.AvatarUrl
		{
			get { return null; }
		}

		Guid IContent.ContentTypeId
		{
            get { return ImageGallerys.ContentTypeId; }
		}

		int? IContent.CreatedByUserId
		{
			get { return _imageGallery == null ? (int?) null : _imageGallery.AuthorUserId; }
		}

		string IContent.HtmlDescription(string target)
		{
			return Description(target);
		}

		string IContent.HtmlName(string target)
		{
			return Name;
		}		

		#endregion
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TEApi = Telligent.Evolution.Extensibility.Api.Version1.PublicApi;

namespace STSImage.RestApi
{
	public class ImageGallery
	{
		public ImageGallery()
		{
		}

		internal ImageGallery(InternalApi.ImageGallery ig)
		{
			Id = ig.Id;
			ContentId = ig.Id;
			Name = ig.Name;
			Description = InternalApi.ImageService.RenderImageGalleryDescription(ig, "webservices");
			IsEnabled = ig.IsEnabled;
            CreatedDate = InternalApi.Formatting.FromUtcToUserTime(ig.CreatedDateUtc);
            LastUpdatedDate = InternalApi.Formatting.FromUtcToUserTime(ig.LastUpdatedDateUtc);
            Url = TEApi.Url.Absolute(InternalApi.ImageService.ImageGalleryUrl(ig.Id));
            Images = new List<Image>(ig.Images.Select(x => new Image(x, ig)));
			Group = new Group(ig.GroupId);
			AuthorUser = new User(ig.AuthorUserId);
		}

		public Guid Id { get; set; }
		public Guid ContentId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public User AuthorUser { get; set; }
		public Group Group { get; set; }
        public Guid GroupId { get; set; }
        public bool IsEnabled { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime LastUpdatedDate { get; set; }
		public string Url { get; set; }
		public List<Image> Images { get; set; }
		public bool HideResultsUntilVotingComplete { get; set; }
		public DateTime? VotingEndDate { get; set; }
		public int TotalVotes { get; set; }
	}
}

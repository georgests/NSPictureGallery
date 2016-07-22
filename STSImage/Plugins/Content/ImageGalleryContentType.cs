using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telligent.Evolution.Extensibility.Content.Version1;
using Telligent.Evolution.Extensibility.Version1;
using Telligent.Evolution.Extensibility.Api.Entities.Version1;
using Telligent.Evolution.Extensibility.Api.Version1;
using TEApi = Telligent.Evolution.Extensibility.Api.Version1.PublicApi;

namespace STSImage.Plugins
{
	public class ImageGalleryContentType : IPlugin, IContentType, ITranslatablePlugin, ICommentableContentType, IAbuseCheckingContentType, ITaggableContentType, IHashTaggableContentType, IMentionContainingContentType, ISearchableContentType
	{
		ITranslatablePluginController _translation;
		IContentStateChanges _contentState;
		IHashTagController _hashTagController;
		IMentionController _mentionController;

		#region IPlugin Members

		public string Name
		{
			get { return "Image Gallery Content Type"; }
		}

		public string Description
		{
			get { return "Exposes Image Gallery to the Evolution content model."; }
		}

		public void Initialize()
		{
            /*
			PublicApi.ImageGallery.Events.AfterCreate += new PublicApi.PollAfterCreateEventHandler(Events_AfterCreate);
			PublicApi.Polls.Events.AfterUpdate += new PublicApi.PollAfterUpdateEventHandler(Events_AfterUpdate);
			PublicApi.Polls.Events.Render += new PublicApi.PollRenderEventHandler(Events_Render);
			PublicApi.Polls.Events.AfterDelete += new PublicApi.PollAfterDeleteEventHandler(Events_AfterDelete);

			TEApi.Groups.Events.AfterDelete += new GroupAfterDeleteEventHandler(Events_AfterDelete);
			TEApi.Users.Events.AfterDelete += new UserAfterDeleteEventHandler(Events_AfterDelete);
            */
        }

		void Events_AfterDelete(UserAfterDeleteEventArgs e)
		{
            /*
			if (e.Id.HasValue && e.ReassignedUserId.HasValue)
				InternalApi.PollingService.ReassignPolls(e.Id.Value, e.ReassignedUserId.Value);
             */
		}

		void Events_AfterDelete(GroupAfterDeleteEventArgs e)
		{
            /*
			if (e.Id.HasValue)
				InternalApi.PollingService.DeletePolls(e.Id.Value);
             */
		}

		void Events_AfterCreate(PublicApi.ImageGalleryAfterCreateEventArgs e)
		{
			if (_hashTagController != null)
			{
				_hashTagController.AddUpdateHashTags(e.ContentId, "Description", e.Description("raw"));

				var tags = _hashTagController.GetHashTagsInUnRenderedHtml(e.Name);
				if (tags != null && tags.Length > 0)
				{
					TEApi.Tags.Add(e.ContentId, e.ContentTypeId, null, string.Join(",", tags));
					var name = _hashTagController.FormatHashTagsInUnRenderedHtml(e.Name);
					PublicApi.ImageGallerys.Update(e.Id, name);
				}
			}

			if (_mentionController != null)
				_mentionController.AddUpdateLoggedMentions(e.ContentId, "Description", e.Description("raw"));
		}

		void Events_AfterUpdate(PublicApi.ImageGalleryAfterUpdateEventArgs e)
		{
			if (_hashTagController != null)
			{
				_hashTagController.AddUpdateHashTags(e.ContentId, "Description", e.Description("raw"));

				var tags = _hashTagController.GetHashTagsInUnRenderedHtml(e.Name);
				if (tags != null && tags.Length > 0)
				{
					TEApi.Tags.Add(e.ContentId, e.ContentTypeId, null, string.Join(",", tags));
					var name = _hashTagController.FormatHashTagsInUnRenderedHtml(e.Name);
					PublicApi.ImageGallerys.Update(e.Id, name);
				}
			}

			if (_mentionController != null)
				_mentionController.AddUpdateLoggedMentions(e.ContentId, "Description", e.Description("raw"));
		}

		void Events_AfterDelete(PublicApi.ImageGalleryAfterDeleteEventArgs e)
		{
			if (_contentState != null)
				_contentState.Deleted(e.ContentId);
		}

		void Events_Render(PublicApi.ImageGalleryRenderEventArgs e)
		{
			if (e.RenderedProperty == "Description")
			{
				if (_hashTagController != null)
					e.RenderedHtml = _hashTagController.FormatHashTagsForRendering(Get(e.ContentId), e.RenderedHtml, e.RenderTarget);

				if (_mentionController != null)
					e.RenderedHtml = _mentionController.FormatMentionsForRendering(e.RenderedHtml, e.RenderTarget);
			}
		}
       		#endregion

		#region IContentType Members

		public Guid[] ApplicationTypes
		{
			get { return new Guid[] { TEApi.Groups.ApplicationTypeId }; }
		}

		public void AttachChangeEvents(IContentStateChanges stateChanges)
		{
			_contentState = stateChanges;
		}

		public Guid ContentTypeId
		{
			get { return PublicApi.ImageGallerys.ContentTypeId; }
		}

		public string ContentTypeName
		{
			get { return _translation.GetLanguageResourceValue("content_type_name"); }
		}

		public IContent Get(Guid contentId)
		{
			return PublicApi.ImageGallerys.Get(contentId);
		}

		#endregion

		#region ITranslatablePlugin Members

		public Translation[] DefaultTranslations
		{
			get 
			{
				var translation = new Translation("en-US");
				translation.Set("content_type_name", "Image Gallery");
				translation.Set("imagegallery_nopermission", "You don't have access to this image gallery or the image gallery has been deleted.");

				return new Translation[] { translation };
			}
		}

		public void SetController(ITranslatablePluginController controller)
		{
			_translation = controller;
		}

		#endregion

		#region ICommentableContentType Members

		public bool CanCreateComment(Guid contentId, int userId)
		{
            /*
            var image = InternalApi.ImageService.GetImage(contentId);
            if (image == null)
                return false;
            */

            return InternalApi.ImageService.CanVote(contentId, userId);
		}

		public bool CanDeleteComment(Guid commentId, int userId)
		{
			var comment = TEApi.Comments.Get(new CommentGetOptions() { CommentIds = new Guid[] { commentId } }).FirstOrDefault();
			if (comment == null)
				return false;

			if (comment.UserId == userId)
				return true;

            var ig = InternalApi.ImageService.GetImageGallery(comment.ContentId);
			if (ig == null)
				return false;

			return InternalApi.ImageService.CanModerate(ig.Id, userId);
		}

		public bool CanModifyComment(Guid commentId, int userId)
		{
			return CanDeleteComment(commentId, userId);
		}

		public bool CanReadComment(Guid commentId, int userId)
		{
			var comment = TEApi.Comments.Get(new CommentGetOptions() { CommentIds = new Guid[] { commentId } }).FirstOrDefault();
			if (comment == null)
				return false;

            var ig = InternalApi.ImageService.GetImageGallery(comment.ContentId);
			if (ig == null)
				return false;

            return InternalApi.ImageService.CanRead(ig.Id, userId);
		}

		#endregion

		#region IAbuseCheckingContentType Members

		public bool CanUserReviewAppeals(Guid contentId, int userId)
		{
            return InternalApi.ImageService.CanModerate(contentId, userId);
		}

		public void ContentConfirmedAbusive(Guid abuseId, Guid contentId)
		{
            var ig = InternalApi.ImageService.GetImageGallery(contentId);
			if (ig != null)
                InternalApi.ImageService.DeleteImageGallery(ig);
		}

		public void ContentFoundNotAbusive(Guid abuseId, Guid contentId)
		{
            var ig = InternalApi.ImageService.GetImageGallery(contentId);
			if (ig != null)
			{
				ig.IsEnabled = true;
                InternalApi.ImageService.AddUpdateImageGallery(ig);
			}
		}

		public void ContentSuspectedAbusive(Guid abuseId, Guid contentId)
		{
            var ig = InternalApi.ImageService.GetImageGallery(contentId);
			if (ig != null)
			{
				ig.IsEnabled = false;
                InternalApi.ImageService.AddUpdateImageGallery(ig);
			}
		}

		public IContent GetHiddenContent(Guid contentId)
		{
            var ig = InternalApi.ImageService.GetImageGallery(contentId);
			if (ig != null)
				return new PublicApi.ImageGallery(ig);
			else
				return null;
		}

		public List<int> GetReviewBoardUsers(Guid contentId)
		{
			List<int> userIds = new List<int>();

            var ig = InternalApi.ImageService.GetImageGallery(contentId);
			if (ig == null)
				return userIds;

			var roles = TEApi.Roles.List(new RolesListOptions { PermissionId = "Group_ReviewAbuseAppeals", Application = "groups", Id = ig.GroupId, Include = "granted" });
			foreach (var role in roles)
				foreach (var user in TEApi.Users.List(new UsersListOptions { RoleId = role.Id, PageSize = 100, PageIndex = 0, IncludeHidden = false }))
					userIds.Add(user.Id.Value);

			return userIds;
		}

		#endregion

		#region ITaggableContentType Members

		public bool CanAddTags(Guid contentId, int userId)
		{
            return InternalApi.ImageService.CanVote(contentId, userId);
		}

		public bool CanRemoveTags(Guid contentId, int userId)
		{
            return InternalApi.ImageService.CanVote(contentId, userId);
		}

		#endregion

		#region IHashTaggableContentType Members

		public void SetController(IHashTagController controller)
		{
			_hashTagController = controller;
		}

		#endregion

		#region IMentionContainingContentType Members

		public void SetController(IMentionController controller)
		{
			_mentionController = controller;
		}

		#endregion

		#region ISearchableContentType Members

		public IList<SearchIndexDocument> GetContentToIndex()
		{
            List<SearchIndexDocument> searchDocuments = new List<SearchIndexDocument>();

            /*
            var pollsToIndex = InternalApi.ImageService.ListPollsToReindex(500, 0);

			foreach (var poll in pollsToIndex)
			{
				var searchDocument = TEApi.SearchIndexing.NewDocument(poll.Id, PublicApi.Polls.ContentTypeId, "poll", RestApi.PollingService.PollUrl(poll.Id), poll.Name, RestApi.PollingService.RenderPollDescription(poll, "unknown"));

				searchDocument.AddField(TEApi.SearchIndexing.Constants.IsContent, true.ToString());
				searchDocument.AddField(TEApi.SearchIndexing.Constants.Date, TEApi.SearchIndexing.FormatDate(poll.CreatedDateUtc));
				searchDocument.AddField(TEApi.SearchIndexing.Constants.UserID, poll.AuthorUserId.ToString());
				searchDocument.AddField(TEApi.SearchIndexing.Constants.Category, "polls");

				var user = TEApi.Users.Get(new UsersGetOptions() { Id = poll.AuthorUserId });
				if (user != null && !user.HasErrors())
					searchDocument.AddField(TEApi.SearchIndexing.Constants.Username, user.Username);

				var group = TEApi.Groups.Get(new GroupsGetOptions() { Id = poll.GroupId });
				if (group != null && !group.HasErrors())
				{
					searchDocument.AddField(TEApi.SearchIndexing.Constants.ApplicationId, group.ApplicationId.ToString());
					searchDocument.AddField(TEApi.SearchIndexing.Constants.GroupID, group.Id.ToString());
					if (group.ParentGroupId.HasValue)
						searchDocument.AddField(TEApi.SearchIndexing.Constants.ParentGroupID, group.ParentGroupId.Value.ToString());
					searchDocument.AddField(TEApi.SearchIndexing.Constants.ContainerId, group.ContainerId.ToString());
				}

				var tags = TEApi.Tags.Get(poll.Id, PublicApi.Polls.ContentTypeId, null);
				if (tags != null)
				{
					foreach (var tag in tags)
					{
						searchDocument.AddField(TEApi.SearchIndexing.Constants.TagKeyword, tag.TagName.ToLower());
						searchDocument.AddField(TEApi.SearchIndexing.Constants.Tag, tag.TagName);
					}
				}

				searchDocuments.Add(searchDocument);
			}
            */

			return searchDocuments;
		}

		public string GetViewHtml(IContent content, Target target)
		{
            var ig = InternalApi.ImageService.GetImageGallery(content.ContentId);
			if (ig == null)
                return _translation.GetLanguageResourceValue("imagegallery_nopermission");

			StringBuilder html = new StringBuilder();

			html.Append(@"<div class=""abbreviated-post-header""></div>
<div class=""abbreviated-post"">
	<div class=""post-metadata"">
		<ul class=""property-list"">
			<li class=""property-item date"">");
			html.Append(TEApi.Language.FormatDate(InternalApi.Formatting.FromUtcToUserTime(ig.CreatedDateUtc)));
			html.Append(@"</li>");

			var user = TEApi.Users.Get(new UsersGetOptions { Id = ig.AuthorUserId });
			if (user != null && !user.HasErrors())
			{
				html.Append(@"<li class=""property-item author"">
					<span class=""user-name"">");

				if (!string.IsNullOrEmpty(user.ProfileUrl))
				{
					html.Append(@"<a href=""");
					html.Append(TEApi.Html.EncodeAttribute(user.ProfileUrl));
					html.Append(@""" class=""internal-link view-user-profile""><span></span>");
					html.Append(user.DisplayName);
					html.Append("</a>");
				}
				else
				{
					html.Append(user.DisplayName);
				}

				html.Append(@"</span></li>");
			}

			html.Append(@"<li>
				<ul class=""details"">
					<li class=""property-item type"">");

			html.Append(_translation.GetLanguageResourceValue("content_type_name"));
			html.Append(@"
					</li>
				</ul>
		</ul>
	</div>
	<h4 class=""post-name"">
		<a class=""internal-link view-poll"" title=""");
			html.Append(ig.Name);
			html.Append(@""" href=""");
            html.Append(TEApi.Html.EncodeAttribute(InternalApi.ImageService.ImageGalleryUrl(ig.Id)));
			html.Append(@""">");
			html.Append(ig.Name);
			html.Append(@"</a>
	</h4>");

			var group = TEApi.Groups.Get(new GroupsGetOptions { Id = ig.GroupId });
			if (group != null)
			{
				html.Append(@"
	<div class=""post-application"">
		<a href=""");

				html.Append(TEApi.Html.EncodeAttribute(group.Url));
				html.Append(@""">");
				html.Append(group.Name);
				html.Append(@"</a>
	</div>");
			}

			html.Append(@"
	<div class=""post-summary"" style=""margin-right: 150px;"">");
//			html.Append(PublicApi.ImageGallerys.UI(ig.Id, true, false));
			html.Append(@"
	</div>
</div>
<div class=""abbreviated-post-footer""></div>");

			return html.ToString();
		}

		public int[] GetViewSecurityRoles(Guid contentId)
		{
            var ig = InternalApi.ImageService.GetImageGallery(contentId);
			if (ig != null)
			{
				var group = TEApi.Groups.Get(new GroupsGetOptions() { Id = ig.GroupId });
				if (!group.HasErrors())
				{
					var groupSearchType = PluginManager.Get<ISearchableContentType>().FirstOrDefault(x => x.ContentTypeId == TEApi.Groups.ContentTypeId);
					if (groupSearchType != null)
						return groupSearchType.GetViewSecurityRoles(group.ContainerId);
				}
			}

			return new int[0];
		}

		public bool IsCacheable
		{
			get { return true; }
		}

		public void SetIndexStatus(Guid[] contentIds, bool isIndexed)
		{
            /*
			if (isIndexed)
                InternalApi.ImageService.SetPollsAsIndexed(contentIds);
             */
		}

		public bool VaryCacheByUser
		{
			get { return true; }
		}

		#endregion
	}
}

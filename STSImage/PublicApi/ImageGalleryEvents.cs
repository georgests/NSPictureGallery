using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InternalEntity = STSImage.InternalApi.ImageGallery;
using Telligent.Evolution.Extensibility.Api.Version1;
using TEApi = Telligent.Evolution.Extensibility.Api.Version1.PublicApi;
using Telligent.Evolution.Extensibility.Content.Version1;
using STSImage.PublicApi;

namespace STSImage.PublicApi
{
	public delegate void ImageGalleryBeforeCreateEventHandler(ImageGalleryBeforeCreateEventArgs e);
    public delegate void ImageGalleryAfterCreateEventHandler(ImageGalleryAfterCreateEventArgs e);
    public delegate void ImageGalleryBeforeUpdateEventHandler(ImageGalleryBeforeUpdateEventArgs e);
    public delegate void ImageGalleryAfterUpdateEventHandler(ImageGalleryAfterUpdateEventArgs e);
    public delegate void ImageGalleryBeforeDeleteEventHandler(ImageGalleryBeforeDeleteEventArgs e);
    public delegate void ImageGalleryAfterDeleteEventHandler(ImageGalleryAfterDeleteEventArgs e);
    public delegate void ImageGalleryRenderEventHandler(ImageGalleryRenderEventArgs e);

	public class ImageGalleryEvents : Telligent.Evolution.Extensibility.Events.Version1.EventsBase
	{
		#region Create

		private readonly object BeforeCreateEvent = new object();

		public event ImageGalleryBeforeCreateEventHandler BeforeCreate
		{
			add { Add(BeforeCreateEvent, value); }
			remove { Remove(BeforeCreateEvent, value); }
		}

		internal void OnBeforeCreate(InternalEntity poll)
		{
			var handlers = Get<ImageGalleryBeforeCreateEventHandler>(BeforeCreateEvent);
			if (handlers != null)
			{
				var args = new ImageGalleryBeforeCreateEventArgs(poll);
				handlers(args);
			}

			TEApi.Html.Events.OnBeforeCreate(GetHtmlProperties(poll));
			TEApi.Content.Events.OnBeforeCreate(GetContent(poll));
		}

		private readonly object AfterCreateEvent = new object();

        public event ImageGalleryAfterCreateEventHandler AfterCreate
		{
			add { Add(AfterCreateEvent, value); }
			remove { Remove(AfterCreateEvent, value); }
		}

		internal void OnAfterCreate(InternalEntity poll)
		{
            var handlers = Get<ImageGalleryAfterCreateEventHandler>(AfterCreateEvent);
			if (handlers != null)
				handlers(new ImageGalleryAfterCreateEventArgs(poll));

			TEApi.Content.Events.OnAfterCreate(GetContent(poll));
		}

		#endregion

		#region Update

		private readonly object BeforeUpdateEvent = new object();

        public event ImageGalleryBeforeUpdateEventHandler BeforeUpdate
		{
			add { Add(BeforeUpdateEvent, value); }
			remove { Remove(BeforeUpdateEvent, value); }
		}

		internal void OnBeforeUpdate(InternalEntity poll)
		{
            var handlers = Get<ImageGalleryBeforeUpdateEventHandler>(BeforeUpdateEvent);
			if (handlers != null)
			{
				var args = new ImageGalleryBeforeUpdateEventArgs(poll);
				handlers(args);
			}

			TEApi.Html.Events.OnBeforeUpdate(GetHtmlProperties(poll));
			TEApi.Content.Events.OnBeforeUpdate(GetContent(poll));
		}

		private readonly object AfterUpdateEvent = new object();

        public event ImageGalleryAfterUpdateEventHandler AfterUpdate
		{
			add { Add(AfterUpdateEvent, value); }
			remove { Remove(AfterUpdateEvent, value); }
		}

		internal void OnAfterUpdate(InternalEntity poll)
		{
            var handlers = Get<ImageGalleryAfterUpdateEventHandler>(AfterUpdateEvent);
			if (handlers != null)
				handlers(new ImageGalleryAfterUpdateEventArgs(poll));

			TEApi.Content.Events.OnAfterUpdate(GetContent(poll));
		}

		#endregion

		#region Delete

		private readonly object BeforeDeleteEvent = new object();

        public event ImageGalleryBeforeDeleteEventHandler BeforeDelete
		{
			add { Add(BeforeDeleteEvent, value); }
			remove { Remove(BeforeDeleteEvent, value); }
		}

		internal void OnBeforeDelete(InternalEntity poll)
		{
            var handlers = Get<ImageGalleryBeforeDeleteEventHandler>(BeforeDeleteEvent);
			if (handlers != null)
				handlers(new ImageGalleryBeforeDeleteEventArgs(poll));

			TEApi.Content.Events.OnBeforeDelete(GetContent(poll));
		}

		private readonly object AfterDeleteEvent = new object();

        public event ImageGalleryAfterDeleteEventHandler AfterDelete
		{
			add { Add(AfterDeleteEvent, value); }
			remove { Remove(AfterDeleteEvent, value); }
		}

		internal void OnAfterDelete(InternalEntity poll)
		{
            var handlers = Get<ImageGalleryAfterDeleteEventHandler>(AfterDeleteEvent);
			if (handlers != null)
				handlers(new ImageGalleryAfterDeleteEventArgs(poll));

			TEApi.Content.Events.OnAfterDelete(GetContent(poll));
		}

		#endregion

		#region Render

		private readonly object RenderEvent = new object();

        public event ImageGalleryRenderEventHandler Render
		{
			add { Add(RenderEvent, value); }
			remove { Remove(RenderEvent, value); }
		}

		internal string OnRender(InternalEntity poll, string propertyName, string propertyHtml, string target)
		{
            var handlers = Get<ImageGalleryRenderEventHandler>(RenderEvent);
			if (handlers != null)
			{
				var args = new ImageGalleryRenderEventArgs(poll, propertyName, propertyHtml, target);
				handlers(args);
				propertyHtml = args.RenderedHtml;
			}

			return TEApi.Html.Events.OnRender(propertyName, propertyHtml, target);
		}

		#endregion

		private HtmlProperties GetHtmlProperties(InternalEntity internalEntity)
		{
			return new HtmlProperties()
				.Add("Description", () => internalEntity.Description, (string html) => internalEntity.Description = html, true)
				.Add("Name", () => internalEntity.Name, (string html) => internalEntity.Name = html, false);
		}

		private IContent GetContent(InternalEntity internalEntity)
		{
			return new ImageGallery(internalEntity);
		}
	}

	public abstract class ReadOnlyUnrenderableImageGalleryEventArgsBase
	{
		internal ReadOnlyUnrenderableImageGalleryEventArgsBase(InternalEntity poll)
		{
			InternalEntity = poll;
		}

		internal InternalEntity InternalEntity { get; private set; }

		public Guid ContentId { get { return InternalEntity.Id; } }
		public Guid ContentTypeId { get { return PublicApi.ImageGallerys.ContentTypeId; } }
		public Guid Id { get { return InternalEntity.Id; } }
		public string Name { get { return InternalEntity.Name; } }
		public int AuthorUserId { get { return InternalEntity.AuthorUserId; } }
		public int GroupId { get { return InternalEntity.GroupId; } }
		public bool IsEnabled { get { return InternalEntity.IsEnabled; } }
        public DateTime CreatedDate { get { return InternalApi.Formatting.FromUtcToUserTime(InternalEntity.CreatedDateUtc); } }
        public DateTime LastUpdatedDate { get { return InternalApi.Formatting.FromUtcToUserTime(InternalEntity.LastUpdatedDateUtc); } }

		List<Image> _images = null;
		public IList<Image> Images
		{
			get
			{
                if (_images == null)
				{
					if (InternalEntity.Images != null)
                        _images = new List<Image>(InternalEntity.Images.Select(x => new Image(x, InternalEntity)));
					else
                        _images = new List<Image>();
				}

                return _images;
			}
		}
	}

	public abstract class ReadOnlyPollEventArgsBase : ReadOnlyUnrenderableImageGalleryEventArgsBase
	{
		internal ReadOnlyPollEventArgsBase(InternalEntity poll)
			: base(poll)
		{
		}

		public string Description()
		{
			return Description("web");
		}

		public string Description(string target)
		{
            return InternalApi.ImageService.RenderImageGalleryDescription(InternalEntity, target);
		}
	}

	public abstract class EditablePollEventArgsBase
	{
		internal EditablePollEventArgsBase(InternalEntity poll)
		{
			InternalEntity = poll;
		}

		internal InternalEntity InternalEntity { get; private set; }

		public Guid ContentId { get { return InternalEntity.Id; } }
		public Guid ContentTypeId { get { return PublicApi.ImageGallerys.ContentTypeId; } }
		public Guid Id { get { return InternalEntity.Id; } }
		public string Name { get { return InternalEntity.Name; } set { InternalEntity.Name = value; } }
		public string Description { get { return InternalEntity.Description; } set { InternalEntity.Description = value; } }
		public int AuthorUserId { get { return InternalEntity.AuthorUserId; } }
		public int GroupId { get { return InternalEntity.GroupId; } }
		public bool IsEnabled { get { return InternalEntity.IsEnabled; } set { InternalEntity.IsEnabled = value; } }
        public DateTime CreatedDate { get { return InternalApi.Formatting.FromUtcToUserTime(InternalEntity.CreatedDateUtc); } }
        public DateTime LastUpdatedDate { get { return InternalApi.Formatting.FromUtcToUserTime(InternalEntity.LastUpdatedDateUtc); } }

		List<Image> _answers = null;
		public IList<Image> Images
		{
			get
			{
				if (_answers == null)
				{
					if (InternalEntity.Images != null)
						_answers = new List<Image>(InternalEntity.Images.Select(x => new Image(x, InternalEntity)));
					else
						_answers = new List<Image>();
				}

				return _answers;
			}
		}
	}

	public class ImageGalleryBeforeCreateEventArgs : EditablePollEventArgsBase
    {
        internal ImageGalleryBeforeCreateEventArgs(InternalEntity poll)
            : base(poll)
        {
        }
    }

    public class ImageGalleryAfterCreateEventArgs : ReadOnlyPollEventArgsBase
    {
        internal ImageGalleryAfterCreateEventArgs(InternalEntity poll)
            : base(poll)
        {
        }
    }

    public class ImageGalleryBeforeUpdateEventArgs : EditablePollEventArgsBase
    {
        internal ImageGalleryBeforeUpdateEventArgs(InternalEntity poll)
            : base(poll)
        {
        }
    }

    public class ImageGalleryAfterUpdateEventArgs : ReadOnlyPollEventArgsBase
    {
        internal ImageGalleryAfterUpdateEventArgs(InternalEntity poll)
            : base(poll)
        {
        }
    }

    public class ImageGalleryBeforeDeleteEventArgs : ReadOnlyPollEventArgsBase
    {
        internal ImageGalleryBeforeDeleteEventArgs(InternalEntity poll)
            : base(poll)
        {
        }
    }

    public class ImageGalleryAfterDeleteEventArgs : ReadOnlyPollEventArgsBase
    {
        internal ImageGalleryAfterDeleteEventArgs(InternalEntity poll)
            : base(poll)
        {
        }
    }

    public class ImageGalleryRenderEventArgs : ReadOnlyUnrenderableImageGalleryEventArgsBase
    {
        internal ImageGalleryRenderEventArgs(InternalEntity poll, string renderedProperty, string renderedHtml, string target)
            : base(poll)
        {
            RenderedHtml = renderedHtml;
            RenderedProperty = renderedProperty;
            RenderTarget = target;
        }

        public string RenderedProperty { get; private set; }
        public string RenderedHtml { get; set; }
        public string RenderTarget { get; private set; }
    }
}

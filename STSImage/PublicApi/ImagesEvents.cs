using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InternalEntity = STSImage.InternalApi.Image;
using Telligent.Evolution.Extensibility.Api.Version1;
using TEApi = Telligent.Evolution.Extensibility.Api.Version1.PublicApi;
using Telligent.Evolution.Extensibility.Content.Version1;

namespace STSImage.PublicApi
{
	public delegate void ImageBeforeCreateEventHandler(ImageBeforeCreateEventArgs e);
    public delegate void ImageAfterCreateEventHandler(ImageAfterCreateEventArgs e);
    public delegate void ImageBeforeUpdateEventHandler(ImageBeforeUpdateEventArgs e);
    public delegate void ImageAfterUpdateEventHandler(ImageAfterUpdateEventArgs e);
    public delegate void ImageBeforeDeleteEventHandler(ImageBeforeDeleteEventArgs e);
    public delegate void ImageAfterDeleteEventHandler(ImageAfterDeleteEventArgs e);

	public class ImagesEvents : Telligent.Evolution.Extensibility.Events.Version1.EventsBase
	{
		#region Create

		private readonly object BeforeCreateEvent = new object();

		public event ImageBeforeCreateEventHandler BeforeCreate
		{
			add { Add(BeforeCreateEvent, value); }
			remove { Remove(BeforeCreateEvent, value); }
		}

		internal void OnBeforeCreate(InternalEntity image)
		{
			var handlers = Get<ImageBeforeCreateEventHandler>(BeforeCreateEvent);
			if (handlers != null)
			{
				var args = new ImageBeforeCreateEventArgs(image);
				handlers(args);
			}

			TEApi.Html.Events.OnBeforeCreate(GetHtmlProperties(image));
		}

		private readonly object AfterCreateEvent = new object();

		public event ImageAfterCreateEventHandler AfterCreate
		{
			add { Add(AfterCreateEvent, value); }
			remove { Remove(AfterCreateEvent, value); }
		}

		internal void OnAfterCreate(InternalEntity image)
		{
			var handlers = Get<ImageAfterCreateEventHandler>(AfterCreateEvent);
			if (handlers != null)
				handlers(new ImageAfterCreateEventArgs(image));
		}

		#endregion

		#region Update

		private readonly object BeforeUpdateEvent = new object();

		public event ImageBeforeUpdateEventHandler BeforeUpdate
		{
			add { Add(BeforeUpdateEvent, value); }
			remove { Remove(BeforeUpdateEvent, value); }
		}

		internal void OnBeforeUpdate(InternalEntity Image)
		{
			var handlers = Get<ImageBeforeUpdateEventHandler>(BeforeUpdateEvent);
			if (handlers != null)
			{
                var args = new ImageBeforeUpdateEventArgs(Image);
				handlers(args);
			}

            TEApi.Html.Events.OnBeforeUpdate(GetHtmlProperties(Image));
		}

		private readonly object AfterUpdateEvent = new object();

		public event ImageAfterUpdateEventHandler AfterUpdate
		{
			add { Add(AfterUpdateEvent, value); }
			remove { Remove(AfterUpdateEvent, value); }
		}

		internal void OnAfterUpdate(InternalEntity image)
		{
			var handlers = Get<ImageAfterUpdateEventHandler>(AfterUpdateEvent);
			if (handlers != null)
				handlers(new ImageAfterUpdateEventArgs(image));
		}

		#endregion

		#region Delete

		private readonly object BeforeDeleteEvent = new object();

		public event ImageBeforeDeleteEventHandler BeforeDelete
		{
			add { Add(BeforeDeleteEvent, value); }
			remove { Remove(BeforeDeleteEvent, value); }
		}

		internal void OnBeforeDelete(InternalEntity image)
		{
			var handlers = Get<ImageBeforeDeleteEventHandler>(BeforeDeleteEvent);
			if (handlers != null)
				handlers(new ImageBeforeDeleteEventArgs(image));
		}

		private readonly object AfterDeleteEvent = new object();

		public event ImageAfterDeleteEventHandler AfterDelete
		{
			add { Add(AfterDeleteEvent, value); }
			remove { Remove(AfterDeleteEvent, value); }
		}

		internal void OnAfterDelete(InternalEntity image)
		{
			var handlers = Get<ImageAfterDeleteEventHandler>(AfterDeleteEvent);
			if (handlers != null)
                handlers(new ImageAfterDeleteEventArgs(image));
		}

		#endregion

		private HtmlProperties GetHtmlProperties(InternalEntity internalEntity)
		{
			return new HtmlProperties()
				.Add("Name", () => internalEntity.Title, (string html) => internalEntity.Title = html, false);
		}
	}


	public abstract class ReadOnlyImageEventArgsBase
	{
		internal ReadOnlyImageEventArgsBase(InternalEntity image)
		{
            InternalEntity = image;
		}

		internal InternalEntity InternalEntity { get; private set; }

		public Guid ImageGalleryId { get { return InternalEntity.ImageGalleryId; } }
		public Guid Id { get { return InternalEntity.Id; } }
		public string Name { get { return InternalEntity.Title; } }
	}

	public abstract class EditableImageEventArgsBase
	{
        internal EditableImageEventArgsBase(InternalEntity image)
		{
            InternalEntity = image;
		}

		internal InternalEntity InternalEntity { get; private set; }

        public Guid ImageGalleryId { get { return InternalEntity.ImageGalleryId; } }
        public Guid Id { get { return InternalEntity.Id; } }
        public string Name { get { return InternalEntity.Title; } }
    }

	public class ImageBeforeCreateEventArgs : EditableImageEventArgsBase
    {
        internal ImageBeforeCreateEventArgs(InternalEntity image)
            : base(image)
        {
        }
    }

    public class ImageAfterCreateEventArgs : ReadOnlyImageEventArgsBase
    {
        internal ImageAfterCreateEventArgs(InternalEntity image)
            : base(image)
        {
        }
    }

    public class ImageBeforeUpdateEventArgs : EditableImageEventArgsBase
    {
        internal ImageBeforeUpdateEventArgs(InternalEntity image)
            : base(image)
        {
        }
    }

    public class ImageAfterUpdateEventArgs : ReadOnlyImageEventArgsBase
    {
        internal ImageAfterUpdateEventArgs(InternalEntity image)
            : base(image)
        {
        }
    }

    public class ImageBeforeDeleteEventArgs : ReadOnlyImageEventArgsBase
    {
        internal ImageBeforeDeleteEventArgs(InternalEntity image)
            : base(image)
        {
        }
    }

    public class ImageAfterDeleteEventArgs : ReadOnlyImageEventArgsBase
    {
        internal ImageAfterDeleteEventArgs(InternalEntity image)
            : base(image)
        {
        }
    }
}

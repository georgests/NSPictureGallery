using System;
using System.Collections.Generic;

namespace STSImage.InternalApi
{
    [Serializable]
    internal class ImageGallery
    {

        internal ImageGallery() {}
        internal ImageGallery(ImageGallery ig)
        {
            Id = ig.Id;
            Name = ig.Name;
            Description = ig.Description;
            AuthorUserId = ig.AuthorUserId;
            GroupId = ig.GroupId;
            IsEnabled = ig.IsEnabled;
            CreatedDateUtc = ig.CreatedDateUtc;
            LastUpdatedDateUtc = ig.LastUpdatedDateUtc;
        }


        internal Guid Id { get; set; }
        internal string Name { get; set; }
        internal string Description { get; set; }
        internal int AuthorUserId { get; set; }
        internal int GroupId { get; set; }
        internal bool IsEnabled { get; set; }
        internal DateTime CreatedDateUtc { get; set; }
        internal DateTime LastUpdatedDateUtc { get; set; }

        internal List<Image> Images { get; set; }
    }
}

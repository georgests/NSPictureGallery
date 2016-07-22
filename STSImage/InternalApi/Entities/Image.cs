using System;

namespace STSImage.InternalApi
{
    [Serializable]
    internal class Image
    {
        internal Guid Id { get; set; }
        internal int UserId { get; set; }
        internal Guid ImageGalleryId { get; set; }
        internal string FilePath { get; set; }
        internal string Title { get; set; }
        internal string Status { get; set; }
        internal int Degree { get; set; }

    }
}

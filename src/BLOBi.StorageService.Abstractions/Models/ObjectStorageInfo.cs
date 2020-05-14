using System;

namespace BLOBi.StorageService.Abstractions.Models
{
    public sealed class ObjectStorageInfo
    {
        public DateTimeOffset? DateCreated { get; set; }

        public DateTimeOffset? DateUpdated { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public long Size { get; set; }

        public string Category { get; set; }

        public string ContentType { get; set; }
    }
}

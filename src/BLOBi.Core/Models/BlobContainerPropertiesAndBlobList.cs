using Azure.Storage.Blobs.Models;

namespace BLOBi.Core.Models
{
    public sealed class BlobContainerPropertiesAndBlobList
    {
        public BlobContainerProperties BlobContainerProperties { get; set; }

        public BlobItem[] BlobItems { get; set; }
    }
}

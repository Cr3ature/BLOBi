using System.Threading;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace BLOBi.Core.Extensions
{
    internal static class BlobServiceClientExtensions
    {
        internal static BlobClient GetBlobClient(this BlobServiceClient blobServiceClient, string blobName, string containerName, PublicAccessType publicAccessType, CancellationToken cancellationToken = default)
            => blobServiceClient.GetOrCreateBlobContainerClient(containerName, publicAccessType, cancellationToken).GetBlobClient(blobName);

        internal static BlobContainerClient GetOrCreateBlobContainerClient(this BlobServiceClient blobServiceClient, string containerName, PublicAccessType publicAccessType, CancellationToken cancellationToken = default)
        {
            BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName.ToLower());

            blobContainerClient.CreateIfNotExists(publicAccessType: publicAccessType, cancellationToken: cancellationToken);

            return blobContainerClient;
        }
    }
}

using System;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BLOBi.Core.Models;

namespace BLOBi.Core.Internals
{
    internal static class BlobStorageManager
    {
        internal static BlobClient GetBlobClient(AzureStorageManagement azureStorageOptions, string blobContainerName, string blobName)
            => GetBlobContainerClient(azureStorageOptions, blobContainerName).GetBlobClient(blobName);

        internal static BlobClient GetBlobClient(AzureStorageManagement azureStorageOptions, string containerName, string blobName, PublicAccessType publicAccessType = PublicAccessType.None)
            => GetBlobContainerClient(azureStorageOptions, containerName, publicAccessType).GetBlobClient(blobName);

        internal static BlobContainerClient GetBlobContainerClient(AzureStorageManagement azureStorageOptions, string blobContainerName, PublicAccessType publicAccessType = PublicAccessType.None)
            => azureStorageOptions.UseManagedIdentity
                ? new BlobContainerClient(new Uri($"https://{azureStorageOptions.AccountName}.blob.core.windows.net/{blobContainerName}"), new Azure.Identity.DefaultAzureCredential())
                : GetBlobContainerClientWithConnection(azureStorageOptions, blobContainerName, publicAccessType);

        private static BlobContainerClient GetBlobContainerClientWithConnection(AzureStorageManagement azureStorageOptions, string blobContainerName, PublicAccessType publicAccessType)
        {
            var blobContainerClient = string.IsNullOrWhiteSpace(azureStorageOptions.ConnectionString)
                ? new BlobContainerClient(new Uri($"DefaultEndpointsProtocol=https;AccountName={azureStorageOptions.AccountName};AccountKey={azureStorageOptions.AccountKey};EndpointSuffix=core.windows.net"))
                : new BlobContainerClient(azureStorageOptions.ConnectionString, blobContainerName);

            blobContainerClient.CreateIfNotExists(publicAccessType);

            return blobContainerClient;
        }
    }
}

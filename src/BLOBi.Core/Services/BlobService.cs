using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BLOBi.Core.Exceptions;
using BLOBi.Core.Internals;
using BLOBi.Core.Models;
using Microsoft.Extensions.Options;

namespace BLOBi.Core.Services
{
    internal sealed class BlobService : IBlobService
    {
        private readonly AzureStorageManagement _azureStorageOptions;

        public BlobService(IOptions<AzureStorageManagement> azureStorageOptions)
        {
            _azureStorageOptions = azureStorageOptions.Value;
        }

        public async Task<bool> AbortCopyBlobFromUri(string copyOperationId, string blobName, string containerName, CancellationToken cancellationToken)
        {
            BlobClient client =
               BlobStorageManager.GetBlobClient(
                   azureStorageOptions: _azureStorageOptions,
                   containerName: containerName,
                   blobName: blobName);

            try
            {
                Azure.Response response = await client.AbortCopyFromUriAsync(copyId: copyOperationId, cancellationToken: cancellationToken);
                return response.Status == (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                throw new BlobServiceException(
                  message: ex.Message,
                  blobName: blobName,
                  containerName: containerName,
                  methodName: nameof(AbortCopyBlobFromUri),
                  innerException: ex);
            }
        }

        public async Task<bool> BlobExists(string blobName, string containerName, CancellationToken cancellationToken)
        {
            BlobContainerClient containerClient = BlobStorageManager.GetBlobContainerClient(
                azureStorageOptions: _azureStorageOptions,
                blobContainerName: containerName);

            try
            {
                BlobClient blob = containerClient.GetBlobClient(blobName);
                return await blob.ExistsAsync(cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                throw new BlobServiceException(
                   message: ex.Message,
                   blobName: blobName,
                   containerName: containerName,
                   methodName: nameof(BlobExists),
                   innerException: ex);
            }
        }

        public async Task<CopyFromUriOperation> CopyBlobFromUri(Uri source, string blobName, string containerName, CancellationToken cancellationToken = default)
        {
            BlobClient client =
                BlobStorageManager.GetBlobClient(
                    azureStorageOptions: _azureStorageOptions,
                    containerName: containerName,
                    blobName: blobName);

            try
            {
                IDictionary<string, string> metaData = new Dictionary<string, string>();
                metaData.Add(new KeyValuePair<string, string>("sourceFullUrl", source.AbsoluteUri));
                metaData.Add(new KeyValuePair<string, string>("sourceFullPath", source.AbsolutePath));

                return await client.StartCopyFromUriAsync(source: source, metadata: metaData, cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                throw new BlobServiceException(
                   message: ex.Message,
                   blobName: blobName,
                   containerName: containerName,
                   methodName: nameof(CopyBlobFromUri),
                   innerException: ex);
            }
        }

        public async Task<BlobSnapshotInfo> CreateBlobSnapshot(string blobName, string containerName, CancellationToken cancellationToken = default)
        {
            BlobClient client =
                BlobStorageManager.GetBlobClient(
                    azureStorageOptions: _azureStorageOptions,
                    containerName: containerName,
                    blobName: blobName);

            try
            {
                return await client.CreateSnapshotAsync(cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                throw new BlobServiceException(
                   message: ex.Message,
                   blobName: blobName,
                   containerName: containerName,
                   methodName: nameof(CreateBlobSnapshot),
                   innerException: ex);
            }
        }

        public async Task<bool> DeleteBlobIfExists(string blobName, string containerName, CancellationToken cancellationToken = default)
        {
            BlobClient client =
                BlobStorageManager.GetBlobClient(
                    azureStorageOptions: _azureStorageOptions,
                    containerName: containerName,
                    blobName: blobName);

            try
            {
                return await client.DeleteIfExistsAsync(cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                throw new BlobServiceException(
                   message: ex.Message,
                   blobName: blobName,
                   containerName: containerName,
                   methodName: nameof(DeleteBlobIfExists),
                   innerException: ex);
            }
        }

        public async Task<BlobDownloadInfo> DownloadBlob(string blobName, string containerName, CancellationToken cancellationToken = default)
        {
            BlobClient client =
                BlobStorageManager.GetBlobClient(
                    azureStorageOptions: _azureStorageOptions,
                    containerName: containerName,
                    blobName: blobName);

            try
            {
                return await client.DownloadAsync(cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                throw new BlobServiceException(
                   message: ex.Message,
                   blobName: blobName,
                   containerName: containerName,
                   methodName: nameof(DownloadBlob),
                   innerException: ex);
            }
        }

        public async Task<bool> DownloadBlobTo(Stream destination, string blobName, string containerName, CancellationToken cancellationToken = default)
        {
            BlobClient client =
                 BlobStorageManager.GetBlobClient(
                     azureStorageOptions: _azureStorageOptions,
                     containerName: containerName,
                     blobName: blobName);

            try
            {
                Azure.Response response = await client.DownloadToAsync(destination);
                return response.Status == (int)HttpStatusCode.Created;
            }
            catch (Exception ex)
            {
                throw new BlobServiceException(
                   message: ex.Message,
                   blobName: blobName,
                   containerName: containerName,
                   methodName: nameof(DownloadBlobTo),
                   innerException: ex);
            }
        }

        public async Task<BlobProperties> GetBlobProperties(string blobName, string containerName, CancellationToken cancellationToken = default)
        {
            BlobClient client =
                BlobStorageManager.GetBlobClient(
                    azureStorageOptions: _azureStorageOptions,
                    containerName: containerName,
                    blobName: blobName);

            try
            {
                return await client.GetPropertiesAsync(cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                throw new BlobServiceException(
                   message: ex.Message,
                   blobName: blobName,
                   containerName: containerName,
                   methodName: nameof(GetBlobProperties),
                   innerException: ex);
            }
        }

        public async Task<bool> UndeleteBlob(string blobName, string containerName, CancellationToken cancellationToken = default)
        {
            BlobClient client =
                 BlobStorageManager.GetBlobClient(
                     azureStorageOptions: _azureStorageOptions,
                     containerName: containerName,
                     blobName: blobName);

            try
            {
                Azure.Response response = await client.UndeleteAsync(cancellationToken: cancellationToken);
                return response.Status == (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                throw new BlobServiceException(
                   message: ex.Message,
                   blobName: blobName,
                   containerName: containerName,
                   methodName: nameof(UndeleteBlob),
                   innerException: ex);
            }
        }

        public async Task<BlobContentInfo> UploadBlob(Stream objectStream, string blobName, string containerName, bool allowAnonymousRead = false, CancellationToken cancellationToken = default)
        {
            PublicAccessType publicAccessType = allowAnonymousRead ? PublicAccessType.Blob : PublicAccessType.None;
            BlobClient client =
                  BlobStorageManager.GetBlobClient(
                      azureStorageOptions: _azureStorageOptions,
                      containerName: containerName,
                      blobName: blobName,
                      publicAccessType: publicAccessType);

            try
            {
                return await client.UploadAsync(objectStream, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new BlobServiceException(
                   message: ex.Message,
                   blobName: blobName,
                   containerName: containerName,
                   methodName: nameof(UploadBlob),
                   innerException: ex);
            }
        }

        public async Task<BlobContentInfo> UploadBlob(Stream objectStream, string blobName, string containerName, IDictionary<string, string> metaData, bool allowAnonymousRead = false, CancellationToken cancellationToken = default)
        {
            PublicAccessType publicAccessType = allowAnonymousRead ? PublicAccessType.Blob : PublicAccessType.None;
            BlobClient client =
                  BlobStorageManager.GetBlobClient(
                      azureStorageOptions: _azureStorageOptions,
                      containerName: containerName,
                      blobName: blobName,
                      publicAccessType: publicAccessType);

            try
            {
                return await client.UploadAsync(
                    content: objectStream,
                    metadata: metaData,
                    cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                throw new BlobServiceException(
                   message: ex.Message,
                   blobName: blobName,
                   containerName: containerName,
                   methodName: nameof(UploadBlob),
                   innerException: ex);
            }
        }
    }
}

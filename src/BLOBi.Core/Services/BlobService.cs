using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BLOBi.Core.Exceptions;
using BLOBi.Core.Extensions;

namespace BLOBi.Core.Services
{
    internal sealed class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<bool> AbortCopyBlobFromUri(string copyOperationId, string blobName, string containerName, CancellationToken cancellationToken)
        {
            try
            {
                BlobClient blobClient = _blobServiceClient.GetBlobContainerClient(containerName).GetBlobClient(blobName);
                Azure.Response response = await blobClient.AbortCopyFromUriAsync(copyId: copyOperationId, cancellationToken: cancellationToken);
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
            try
            {
                BlobClient blobClient = _blobServiceClient.GetBlobContainerClient(containerName).GetBlobClient(blobName);
                return await blobClient.ExistsAsync(cancellationToken: cancellationToken);
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

        public async Task<CopyFromUriOperation> CopyBlobFromUri(Uri source, string blobName, string containerName, PublicAccessType publicAccessType = PublicAccessType.None, CancellationToken cancellationToken = default)
        {
            try
            {
                BlobClient client = _blobServiceClient.GetBlobClient(
                    blobName: blobName,
                    containerName: containerName,
                    publicAccessType: publicAccessType,
                    cancellationToken: cancellationToken);

                IDictionary<string, string> metaData = new Dictionary<string, string>
                {
                    { "sourceFullUrl", source.AbsoluteUri },
                    { "sourceFullPath", source.AbsolutePath }
                };

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
            try
            {
                BlobClient client = _blobServiceClient.GetBlobContainerClient(containerName).GetBlobClient(blobName: blobName);
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
            try
            {
                BlobClient client = _blobServiceClient.GetBlobContainerClient(containerName).GetBlobClient(blobName);

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
            try
            {
                BlobClient client = _blobServiceClient.GetBlobContainerClient(containerName).GetBlobClient(blobName);
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
            try
            {
                BlobClient client = _blobServiceClient.GetBlobContainerClient(containerName).GetBlobClient(blobName);
                Azure.Response response = await client.DownloadToAsync(destination, cancellationToken: cancellationToken);
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
            try
            {
                BlobClient client = _blobServiceClient.GetBlobContainerClient(containerName).GetBlobClient(blobName);
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
            try
            {
                BlobClient client = _blobServiceClient.GetBlobContainerClient(containerName).GetBlobClient(blobName);
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

        public async Task<BlobContentInfo> UploadBlob(Stream objectStream, string blobName, string containerName, PublicAccessType publicAccessType = PublicAccessType.None, CancellationToken cancellationToken = default)
        {
            try
            {
                BlobClient client = _blobServiceClient.GetBlobClient(containerName: containerName, blobName: blobName, publicAccessType: publicAccessType, cancellationToken: cancellationToken);
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

        public async Task<BlobContentInfo> UploadBlob(Stream objectStream, string blobName, string containerName, IDictionary<string, string> metaData, PublicAccessType publicAccessType = PublicAccessType.None, CancellationToken cancellationToken = default)
        {
            try
            {
                BlobClient client = _blobServiceClient.GetBlobClient(containerName: containerName, blobName: blobName, publicAccessType: publicAccessType, cancellationToken: cancellationToken);
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

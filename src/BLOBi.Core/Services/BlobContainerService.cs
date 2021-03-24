using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BLOBi.Core.Exceptions;
using BLOBi.Core.Extensions;
using BLOBi.Core.Models;

namespace BLOBi.Core.Services
{
    internal sealed class BlobContainerService : IBlobContainerService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobContainerService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<bool> AppendContainerMetaData(
            string containerName,
            IDictionary<string, string> metaData,
            PublicAccessType publicAccessType = PublicAccessType.None,
            CancellationToken cancellationToken = default)
        {
            try
            {
                BlobContainerClient containerClient = _blobServiceClient.GetOrCreateBlobContainerClient(
                containerName: containerName,
                publicAccessType: publicAccessType,
                cancellationToken: cancellationToken);

                BlobContainerProperties blobContainerProperties = await containerClient.GetPropertiesAsync(cancellationToken: cancellationToken);
                IDictionary<string, string> existingMetaData = blobContainerProperties.Metadata;

                foreach (KeyValuePair<string, string> metaItem in metaData)
                {
                    existingMetaData.Add(metaItem);
                }

                Response<BlobContainerInfo> result = await containerClient.SetMetadataAsync(metadata: metaData, cancellationToken: cancellationToken);

                return result.GetRawResponse().Status == (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                throw new BlobServiceException(
                  message: ex.Message,
                  blobName: string.Empty,
                  containerName: containerName,
                  methodName: nameof(AppendContainerMetaData),
                  innerException: ex);
            }
        }

        public async Task<bool> CreateContainerAsync(
            string containerName,
            PublicAccessType publicAccessType = PublicAccessType.None,
            CancellationToken cancellationToken = default)
            => await CreateContainer(containerName, publicAccessType, cancellationToken);

        public async Task<bool> DeleteContainerAsync(string containerName, CancellationToken cancellationToken)
            => await _blobServiceClient.GetBlobContainerClient(blobContainerName: containerName).DeleteIfExistsAsync(cancellationToken: cancellationToken);

        public async Task<BlobContainerProperties> GetContainerProperties(string containerName, CancellationToken cancellationToken)
        {
            try
            {
                return await _blobServiceClient.GetBlobContainerClient(containerName).GetPropertiesAsync(cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                throw new BlobServiceException(
                  message: ex.Message,
                  blobName: string.Empty,
                  containerName: containerName,
                  methodName: nameof(GetContainerProperties),
                  innerException: ex);
            }
        }

        public async Task<BlobContainerPropertiesAndBlobList> GetContainerPropertiesAndBlobDetails(string containerName, CancellationToken cancellationToken = default)
        {
            try
            {
                BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

                BlobContainerProperties blobContainerProperties = await containerClient.GetPropertiesAsync(cancellationToken: cancellationToken);
                IEnumerable<BlobItem> blobContainerContent = containerClient.GetBlobs(cancellationToken: cancellationToken);

                return new BlobContainerPropertiesAndBlobList
                {
                    BlobContainerProperties = blobContainerProperties,
                    BlobItems = blobContainerContent.ToArray(),
                };
            }
            catch (Exception ex)
            {
                throw new BlobServiceException(
                  message: ex.Message,
                  blobName: string.Empty,
                  containerName: containerName,
                  methodName: nameof(GetContainerPropertiesAndBlobDetails),
                  innerException: ex);
            }
        }

        public async Task<BlobItem[]> ListContainerContentAsync(string containerName, CancellationToken cancellationToken = default)
        {
            try
            {
                IEnumerable<BlobItem> blobs = _blobServiceClient.GetBlobContainerClient(containerName).GetBlobs(cancellationToken: cancellationToken);
                return await Task.FromResult(blobs.ToArray());
            }
            catch (Exception ex)
            {
                throw new BlobServiceException(
                  message: ex.Message,
                  blobName: string.Empty,
                  containerName: containerName,
                  methodName: nameof(ListContainerContentAsync),
                  innerException: ex);
            }
        }

        public async Task<bool> SetContainerAccessType(string containerName, PublicAccessType accessType, CancellationToken cancellationToken = default)
        {
            try
            {
                BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

                _ = await containerClient.SetAccessPolicyAsync(accessType: accessType, cancellationToken: cancellationToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SetContainerMetaData(string containerName, IDictionary<string, string> metaData, CancellationToken cancellationToken = default)
        {
            try
            {
                BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

                Response<BlobContainerInfo> result = await containerClient.SetMetadataAsync(metadata: metaData, cancellationToken: cancellationToken);

                return result.GetRawResponse().Status == (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                throw new BlobServiceException(
                  message: ex.Message,
                  blobName: string.Empty,
                  containerName: containerName,
                  methodName: nameof(SetContainerMetaData),
                  innerException: ex);
            }
        }

        private async Task<bool> CreateContainer(string containerName, PublicAccessType publicAccessType = PublicAccessType.None, CancellationToken cancellationToken = default)
        {
            try
            {
                BlobContainerClient containerClient = _blobServiceClient.GetOrCreateBlobContainerClient(containerName: containerName, publicAccessType: publicAccessType, cancellationToken: cancellationToken);

                return await Task.FromResult(containerClient != null);
            }
            catch (Exception ex)
            {
                throw new BlobServiceException(
                  message: ex.Message,
                  blobName: string.Empty,
                  containerName: containerName,
                  methodName: nameof(CreateContainer),
                  innerException: ex);
            }
        }
    }
}

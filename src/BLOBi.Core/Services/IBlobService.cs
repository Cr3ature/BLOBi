using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;

namespace BLOBi.Core
{
    public interface IBlobService
    {
        Task<bool> AbortCopyBlobFromUri(string copyOperationId, string blobName, string containerName, CancellationToken cancellationToken = default);

        Task<bool> AppendMetaData(string blobName, string containerName, IDictionary<string, string> metaData, CancellationToken cancellationToken = default);

        Task<bool> BlobExists(string blobName, string containerName, CancellationToken cancellationToken = default);

        Task<CopyFromUriOperation> CopyBlobFromUri(Uri source, string blobName, string containerName, CancellationToken cancellationToken = default);

        Task<BlobSnapshotInfo> CreateBlobSnapshot(string blobName, string containerName, CancellationToken cancellationToken = default);

        Task<bool> DeleteBlobIfExists(string blobName, string containerName, CancellationToken cancellationToken = default);

        Task<BlobDownloadInfo> DownloadBlob(string blobName, string containerName, CancellationToken cancellationToken = default);

        Task<bool> DownloadBlobTo(Stream destination, string blobName, string containerName, CancellationToken cancellationToken = default);

        Task<BlobProperties> GetBlobProperties(string blobName, string containerName, CancellationToken cancellationToken = default);

        Task<bool> SetBlobMetaData(string blobName, string containerName, IDictionary<string, string> metaData, CancellationToken cancellationToken = default);

        Task<bool> UndeleteBlob(string blobName, string containerName, CancellationToken cancellationToken = default);

        Task<BlobContentInfo> UploadBlob(Stream objectStream, string blobName, string containerName, CancellationToken cancellationToken = default);
    }
}

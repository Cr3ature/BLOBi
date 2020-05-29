using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;

namespace BLOBi.Core.Internals
{
    public static class BlobDownloadInfoExtensions
    {
        public static byte[] ToByteArray(this BlobDownloadInfo blobDownloadInfo)
        {
            using var stream = blobDownloadInfo.ToMemoryStream();

            return stream.ToArray();
        }

        public static async Task<byte[]> ToByteArrayAsync(this BlobDownloadInfo blobDownloadInfo)
        {
            using MemoryStream stream = await blobDownloadInfo.ToMemoryStreamAsync();

            return stream.ToArray();
        }

        public static MemoryStream ToMemoryStream(this BlobDownloadInfo blobDownloadInfo)
        {
            var memoryStream = new MemoryStream();
            blobDownloadInfo.Content.CopyTo(memoryStream);

            return memoryStream;
        }

        public static async Task<MemoryStream> ToMemoryStreamAsync(this BlobDownloadInfo blobDownloadInfo)
        {
            var memoryStream = new MemoryStream();
            await blobDownloadInfo.Content.CopyToAsync(memoryStream);

            return memoryStream;
        }

        public static string ToString(this BlobDownloadInfo blobDownloadInfo)
        {
            using var stream = blobDownloadInfo.ToMemoryStream();
            using var streamReader = new StreamReader(stream);

            return streamReader.ReadToEnd();
        }

        public static async Task<string> ToStringAsync(this BlobDownloadInfo blobDownloadInfo)
        {
            using MemoryStream stream = await blobDownloadInfo.ToMemoryStreamAsync();
            using var streamReader = new StreamReader(stream);

            return await streamReader.ReadToEndAsync();
        }
    }
}

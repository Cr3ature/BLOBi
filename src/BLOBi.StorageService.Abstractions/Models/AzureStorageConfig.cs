namespace BLOBi.StorageService.Abstractions.Models
{
    internal sealed class AzureStorageConfig
    {
        public string AccountKey { get; internal set; }

        public string AccountName { get; internal set; }

        public string ConnectionString { get; internal set; }

        public string DocumentContainer { get; internal set; }

        public string PictureContainer { get; internal set; }

        public string ThumbnailContainer { get; internal set; }
    }
}

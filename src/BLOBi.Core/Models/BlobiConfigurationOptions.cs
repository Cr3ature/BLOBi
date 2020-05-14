namespace BLOBi.Core.Models
{
    public sealed class BlobiConfigurationOptions
    {
        public BlobiConfigurationOptions(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public BlobiConfigurationOptions(string accountKey, string accountName)
        {
            AccountKey = accountKey;
            AccountName = accountName;
        }

        public BlobiConfigurationOptions(string accountKey, string accountName, string connectionString)
        {
            AccountKey = accountKey;
            AccountName = accountName;
            ConnectionString = connectionString;
        }

        public string AccountKey { get; }

        public string AccountName { get; }

        public string ConnectionString { get; }
    }
}

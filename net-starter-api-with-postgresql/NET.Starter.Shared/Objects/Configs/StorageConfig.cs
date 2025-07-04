namespace NET.Starter.Shared.Objects.Configs
{
    /// <summary>
    /// Configuration settings related to storage services.
    /// </summary>
    public class StorageConfig
    {
        /// <summary>
        /// Configuration for Google Cloud Storage integration.
        /// </summary>
        public GoogleCloudStorage GoogleCloudStorage { get; set; } = null!;
    }

    /// <summary>
    /// Settings required to connect and authenticate with Google Cloud Storage.
    /// </summary>
    public class GoogleCloudStorage
    {
        /// <summary>
        /// The Google Cloud project identifier.
        /// </summary>
        public string ProjectId { get; set; } = string.Empty;

        /// <summary>
        /// The name of the Google Cloud Storage bucket.
        /// </summary>
        public string BucketName { get; set; } = string.Empty;

        /// <summary>
        /// Service account credentials used to authenticate with Google Cloud.
        /// </summary>
        public GoogleServiceAccount ServiceAccount { get; set; } = null!;
    }
}
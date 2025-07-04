using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using NET.Starter.Shared.Objects.Configs;
using NET.Starter.Shared.Objects.Dtos;
using Serilog;

namespace NET.Starter.Shared.Helpers
{
    /// <summary>
    /// Provides helper methods for uploading, downloading, and retrieving files in Google Cloud Storage.
    /// <para>Must be initialized first by calling <see cref="Initialize"/>.</para>
    /// </summary>
    public static class GoogleCloudStorageHelper
    {
        public static GoogleCredential? GoogleCredential { get; internal set; }
        public static string? BucketName { get; internal set; }

        /// <summary>
        /// Initializes the Google Cloud Storage client with the specified configuration.
        /// Must be called before performing any storage operations.
        /// </summary>
        /// <param name="config">The configuration object containing Google Cloud credentials and bucket information.</param>
        internal static void Initialize(GoogleCloudStorage config)
        {
            GoogleCredential = GoogleCredential.FromJsonParameters(new()
            {
                Type = config.ServiceAccount.Type,
                ProjectId = config.ProjectId,
                PrivateKey = config.ServiceAccount.PrivateKey,
                ClientEmail = config.ServiceAccount.ClientEmail
            });

            BucketName = config.BucketName;

            Log.Logger.Information("Google Cloud Storage enabled for bucket {BucketName}.", BucketName);
        }

        /// <summary>
        /// Ensures the helper is initialized with valid Google Cloud credentials and bucket name.
        /// Throws an exception if initialization is missing.
        /// </summary>
        /// <exception cref="Exception">Thrown when Google Cloud Storage is not properly configured.</exception>
        private static void CheckInitialization()
        {
            if (GoogleCredential == null || string.IsNullOrWhiteSpace(BucketName))
            {
                Log.Logger.Error("Google Cloud Storage operation failed because configuration is missing.");
                throw new Exception("Google Cloud Storage configuration is missing.");
            }
        }

        /// <summary>
        /// Uploads a file to the configured Google Cloud Storage bucket at the specified path.
        /// The uploaded file will be publicly readable.
        /// </summary>
        /// <param name="file">The file to upload, typically received from an HTTP request.</param>
        /// <param name="path">The folder path inside the bucket where the file should be stored.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the full object name assigned in the bucket.</returns>
        public static async Task<string> UploadFile(IFormFile file, string path)
        {
            CheckInitialization();

            Log.Logger.Information("Uploading file to bucket {BucketName} at path {Path}.", BucketName, path);

            var cloudStorageResourceId = Guid.NewGuid();
            var objectName = $"{path}/{cloudStorageResourceId}";

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);

                using var storageClient = await StorageClient.CreateAsync(GoogleCredential);
                await storageClient.UploadObjectAsync(BucketName, objectName, file.ContentType, memoryStream, new UploadObjectOptions()
                {
                    PredefinedAcl = PredefinedObjectAcl.PublicRead
                });
            }

            Log.Logger.Information("File uploaded to bucket {BucketName} at path {Path}.", BucketName, path);
            return objectName;
        }

        /// <summary>
        /// Retrieves a signed, time-limited URL for accessing a file stored in Google Cloud Storage.
        /// </summary>
        /// <param name="objectName">The full name of the object in the bucket.</param>
        /// <param name="durationInSeconds">The duration in seconds for which the signed URL should be valid. Default is 60 seconds.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the signed URL string.</returns>
        public static async Task<string> RetrieveSignedUrlFile(string objectName, int durationInSeconds = 60)
        {
            CheckInitialization();

            Log.Logger.Information("Generating signed URL for object {ObjectName} in bucket {BucketName} with expiration {Duration} seconds.", objectName, BucketName, durationInSeconds);

            var urlSigner = UrlSigner.FromCredential(GoogleCredential);
            var urlFile = await urlSigner.SignAsync(BucketName, objectName, TimeSpan.FromSeconds(durationInSeconds));

            Log.Logger.Information("Signed URL generated for object {ObjectName} in bucket {BucketName}.", objectName, BucketName);
            return urlFile;
        }

        /// <summary>
        /// Downloads a file from Google Cloud Storage.
        /// </summary>
        /// <param name="objectName">The full name of the object in the bucket to download.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="FileDto"/> with the file stream, name, and content type.</returns>
        public static async Task<FileDto> DownloadFile(string objectName)
        {
            CheckInitialization();

            Log.Logger.Information("Downloading file from bucket {BucketName} at path {Path}.", BucketName, objectName);

            using var storageClient = await StorageClient.CreateAsync(GoogleCredential);
            var stream = new MemoryStream();
            var downloadedFile = await storageClient.DownloadObjectAsync(BucketName, objectName, stream);

            stream.Position = 0;

            Log.Logger.Information("File downloaded from bucket {BucketName} at path {Path}.", BucketName, objectName);
            return new FileDto
            {
                FileStream = stream,
                FileName = downloadedFile.Name,
                ContentType = downloadedFile.ContentType
            };
        }
    }
}
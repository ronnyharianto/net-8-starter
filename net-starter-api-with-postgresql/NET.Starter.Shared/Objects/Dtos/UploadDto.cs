namespace NET.Starter.Shared.Objects.Dtos
{
    /// <summary>
    /// Represents the response details after a file upload operation.
    /// </summary>
    public class UploadDto
    {
        /// <summary>
        /// The name of the uploaded file object.
        /// </summary>
        public string ObjectName { get; set; } = string.Empty;

        /// <summary>
        /// The accessible URL of the uploaded file object.
        /// </summary>
        public string ObjectUrl { get; set; } = string.Empty;
    }
}

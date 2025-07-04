namespace NET.Starter.Shared.Objects.Dtos
{
    /// <summary>
    /// Data Transfer Object representing a file, including its content stream, MIME type, and filename.
    /// </summary>
    public class FileDto
    {
        /// <summary>
        /// The file content as a memory stream.
        /// </summary>
        public MemoryStream? FileStream { get; set; }

        /// <summary>
        /// The MIME content type of the file (e.g., "image/png", "application/pdf").
        /// </summary>
        public string ContentType { get; set; } = string.Empty;

        /// <summary>
        /// The name of the file, including extension.
        /// </summary>
        public string FileName { get; set; } = string.Empty;
    }
}

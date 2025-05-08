namespace NET.Starter.Shared.Objects.Dtos
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for enumerations, 
    /// including value, optional mapping, and description.
    /// </summary>
    public class EnumDto
    {
        /// <summary>
        /// Enum value this entry is mapped from.
        /// </summary>
        public int? EnumValueMapFrom { get; set; }

        /// <summary>
        /// Actual integer value of the enum.
        /// </summary>
        public int EnumValue { get; set; }

        /// <summary>
        /// The human-readable description of the enum value.
        /// </summary>
        public string EnumDescription { get; set; } = string.Empty;
    }
}

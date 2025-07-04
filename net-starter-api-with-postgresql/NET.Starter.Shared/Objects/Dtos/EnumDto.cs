namespace NET.Starter.Shared.Objects.Dtos
{
    /// <summary>
    /// Data Transfer Object (DTO) representing an enumeration entry, 
    /// including its value, an optional mapped value, and a description.
    /// </summary>
    public class EnumDto
    {
        /// <summary>
        /// The enum value this entry is mapped from, if any.
        /// </summary>
        public int? EnumValueMapFrom { get; set; }

        /// <summary>
        /// The integer value of the enum entry.
        /// </summary>
        public int EnumValue { get; set; }

        /// <summary>
        /// The human-readable description of the enum entry.
        /// </summary>
        public string EnumDescription { get; set; } = string.Empty;
    }
}

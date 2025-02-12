using NET.Starter.Shared.Attributes;
using System.ComponentModel;
using WADIG_CIST.BackEnd.Shared.Objects.Dtos;

namespace NET.Starter.Shared.Helpers
{
    public static class EnumHelper
    {
        /// <summary>
        /// Retrieves the description attribute of an enum value. If no description is found, returns the enum name.
        /// </summary>
        /// <param name="value">The enum value.</param>
        /// <returns>The description string or the enum name if no description is found.</returns>
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            if (field != null)
            {
                var attribute = (DescriptionAttribute?)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

                if (attribute != null) return attribute.Description;
            }

            return value.ToString();
        }

        /// <summary>
        /// Filters an enum list based on a case-insensitive search keyword within the description.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="filterKey">The filter keyword to search within the descriptions.</param>
        /// <returns>A filtered list of enum values that contain the keyword in their descriptions.</returns>
        public static List<TEnum> FilterEnumList<TEnum>(string? filterKey)
            where TEnum : Enum
        {
            var result = new List<TEnum>();

            foreach (TEnum item in Enum.GetValues(typeof(TEnum)))
            {
                if (item.GetDescription().Contains(filterKey ?? string.Empty, StringComparison.CurrentCultureIgnoreCase))
                {
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieves a list of enum values and their descriptions.
        /// </summary>
        /// <typeparam name="TEnum">The enum type to retrieve values from.</typeparam>
        /// <returns>A list of <see cref="EnumDto"/> containing the enum values and descriptions.</returns>
        public static List<EnumDto> RetrieveEnumList<TEnum>()
            where TEnum : Enum
        {
            var result = new List<EnumDto>();

            foreach (TEnum enumData in Enum.GetValues(typeof(TEnum)))
            {
                result.Add(new()
                {
                    EnumValue = (int)(object)enumData,
                    EnumDescription = enumData.GetDescription()
                });
            }

            return result;
        }

        /// <summary>
        /// Retrieves a list of enum values and their descriptions, with an optional mapping from another enum type.
        /// </summary>
        /// <typeparam name="TEnum">The enum type to retrieve values from.</typeparam>
        /// <typeparam name="TMapFromEnum">The enum type to map values from using the MapFrom attribute.</typeparam>
        /// <returns>A list of <see cref="EnumDto"/> containing the enum values, descriptions, and mapped from values.</returns>
        public static List<EnumDto> RetrieveEnumList<TEnum, TMapFromEnum>()
            where TEnum : Enum
            where TMapFromEnum : Enum
        {
            var result = new List<EnumDto>();

            foreach (var enumData in Enum.GetValues(typeof(TEnum)).Cast<TEnum>())
            {
                var fieldInfo = typeof(TEnum).GetField(enumData.ToString());

                result.Add(new()
                {
                    EnumValueMapFrom = fieldInfo?
                                        .GetCustomAttributes(typeof(MapFromAttribute<TMapFromEnum>), false)
                                        .FirstOrDefault() is MapFromAttribute<TMapFromEnum> mapFromAttribute ? (int)(object)mapFromAttribute.Target : null,
                    EnumValue = (int)(object)enumData,
                    EnumDescription = enumData.GetDescription()
                });
            }

            return result;
        }
    }
}

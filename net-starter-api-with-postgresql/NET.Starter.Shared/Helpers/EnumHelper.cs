using NET.Starter.Shared.Attributes;
using NET.Starter.Shared.Objects.Dtos;
using System.ComponentModel;

namespace NET.Starter.Shared.Helpers
{
    public static class EnumHelper
    {
        /// <summary>
        /// Retrieves the <see cref="DescriptionAttribute"/> text of an enum value.
        /// If no description attribute is found, returns the enum's name as a fallback.
        /// </summary>
        /// <param name="value">The enum value to get the description for.</param>
        /// <returns>The description text or the enum name if no description is found.</returns>
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
        /// Filters an enum's values based on a case-insensitive substring search on their descriptions.
        /// </summary>
        /// <typeparam name="TEnum">The enum type to filter.</typeparam>
        /// <param name="filterKey">The substring keyword to filter descriptions by.</param>
        /// <returns>A list of enum values whose descriptions contain the filter keyword.</returns>
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
        /// Retrieves all values of an enum along with their descriptions.
        /// </summary>
        /// <typeparam name="TEnum">The enum type to retrieve values from.</typeparam>
        /// <returns>A list of <see cref="EnumDto"/> objects representing each enum value and description.</returns>
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
        /// Retrieves all values of an enum with their descriptions, and optionally includes mapped values from another enum.
        /// </summary>
        /// <typeparam name="TEnum">The enum type to retrieve values from.</typeparam>
        /// <typeparam name="TMapFromEnum">The enum type to map from, using the <see cref="MapFromAttribute{T}"/> attribute.</typeparam>
        /// <returns>A list of <see cref="EnumDto"/> including the enum value, description, and mapped value if available.</returns>
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

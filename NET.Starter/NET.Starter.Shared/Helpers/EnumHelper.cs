using System.ComponentModel;

namespace NET.Starter.Shared.Helpers
{
    public static class EnumHelper
    {
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

        public static List<TEnum> FilterEnumList<TEnum>(string? filterKey)
            where TEnum : Enum
        {
            List<TEnum> result = [];

            foreach (TEnum item in Enum.GetValues(typeof(TEnum)))
            {
                if (item.GetDescription().Contains(filterKey ?? string.Empty, StringComparison.CurrentCultureIgnoreCase))
                {
                    result.Add(item);
                }
            }

            return result;
        }
    }
}

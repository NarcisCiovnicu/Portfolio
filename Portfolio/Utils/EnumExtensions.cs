using System.ComponentModel;
using System.Reflection;

namespace Portfolio.Utils
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            Type? type = value.GetType();
            string stringValue = value.ToString();
            if (type != null)
            {
                FieldInfo? field = type.GetField(stringValue);
                if (field != null)
                {
                    if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                    {
                        return attribute.Description;
                    }
                }
            }
            return stringValue;
        }
    }
}

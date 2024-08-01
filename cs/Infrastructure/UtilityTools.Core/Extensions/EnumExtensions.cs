using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UtilityTools.Core.Models.UX;

namespace UniversalFramework.Core.Extensions
{
    public static class EnumExtensions
    {
        public static TAttribute GetAttribute<TAttribute>(this Enum value)
            where TAttribute : Attribute
        {
            //var type = value.GetType();
            //var name = Enum.GetName(type, value);
            //return type.GetField(name.ToString()) // I prefer to get attributes this way
            //    .GetCustomAttributes(false)
            //    .OfType<TAttribute>()
            //    .SingleOrDefault();
            var field = value.GetType().GetField(value.ToString());

            if (field == null) return null;

            var attributes = field.GetCustomAttributes(false).OfType<TAttribute>().ToList();

            if (attributes.Count == 0) return null;

            return attributes[0];
        }


        public static string GetDescription(this Enum value)
        {
            var type = value.GetType();
            if (!Enum.IsDefined(type, value))
                return $"未定义【{value}】";

            object[] attributes = type
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), true);

            if (attributes.Length > 0)
            {
                var attribute = attributes[0] as DescriptionAttribute;
                if (attribute != null)
                    return attribute.Description;
            }

            return Enum.GetName(type, value);
        }

        public static bool IsInEnum<TEnum>(this int value)
        {
            return Enum.IsDefined(typeof(TEnum), value);
        }


        public static TEnum Convert2Enum<TEnum>(this string value) where TEnum : struct
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (Enum.TryParse<TEnum>(value, out TEnum triggerType))
                {
                    return triggerType;
                }
            }
            return default;
        }

        public static IList<SearchDropdownItem> GetDropdownItems<T>() where T:struct
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException($"{typeof(T)} is not an enum");


            var days = Enum.GetValues(typeof(T))
                      .Cast<T>()
                      .Select(d => new SearchDropdownItem { Name = d.ToString(),Value=d.ToString()  })
                      .ToList();

            return days;
        }


    }
}

using System;

namespace Post.Service.Base.HelperClasses
{
    public static class EnumExtension
    {
        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}

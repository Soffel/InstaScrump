using System;
using System.Text;

namespace InstaScrump.Common.Extension
{
    public static class StringExtension
    {
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static byte[] GetBytes<T>(this string value)
        {
            if(typeof(T) == typeof(ASCIIEncoding))
                return Encoding.ASCII.GetBytes(value);

            if (typeof(T) == typeof(UTF8Encoding))
                return Encoding.UTF8.GetBytes(value);

            if (typeof(T) == typeof(UTF32Encoding))
                return Encoding.UTF32.GetBytes(value);

            if (typeof(T) == typeof(UTF7Encoding))
                return Encoding.UTF7.GetBytes(value);

            if (typeof(T) == typeof(UnicodeEncoding))
                return Encoding.Unicode.GetBytes(value);

            throw new NotImplementedException($"{typeof(T)} is not implemented");
        }

        public static string GetString<T>(this byte[] value)
        {
            if (typeof(T) == typeof(ASCIIEncoding))
                return Encoding.ASCII.GetString(value);

            if (typeof(T) == typeof(UTF8Encoding))
                return Encoding.UTF8.GetString(value);

            if (typeof(T) == typeof(UTF32Encoding))
                return Encoding.UTF32.GetString(value);

            if (typeof(T) == typeof(UTF7Encoding))
                return Encoding.UTF7.GetString(value);

            if (typeof(T) == typeof(UnicodeEncoding))
                return Encoding.Unicode.GetString(value);

            throw new NotImplementedException($"{typeof(T)} is not implemented");
        }
    }
}

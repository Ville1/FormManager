﻿namespace FormManager.Utils
{
    public static class ExtensionMethods
    {
        public static string Capitalize(this string s)
        {
            return s.Length == 0 ? string.Empty : (s.Length == 1 ? s.ToUpper() : s.Substring(0, 1).ToUpper() + s.Substring(1));
        }

        public static string Uncapitalize(this string s)
        {
            return s.Length == 0 ? string.Empty : (s.Length == 1 ? s.ToLower() : s.Substring(0, 1).ToLower() + s.Substring(1));
        }
    }
}

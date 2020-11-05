using System;
using System.Collections.Generic;
using System.Text;

namespace DayzTraderManager.Helpers
{
    public static class StringExtensions
    {
        public static string RemoveTag(this string value, string tag)
        {
            var retLine = value.Substring(tag.Length, value.Length - tag.Length).Trim();

            return string.IsNullOrEmpty(retLine) ? "":retLine;

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.AppCode.Extensions
{
    public static partial class Extension
    {
        public static string RemoveTags(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            Regex pattern = new Regex("<[^>]*>");

            return pattern.Replace(value, "");
        }
    }
}

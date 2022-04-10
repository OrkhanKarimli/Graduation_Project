using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.AppCode.Extensions
{
    public static partial class Extension
    {
        public static string EllipseText(this string value, int max = 75)
        {
            if (value.Length < max)
            {
                return value;
            }
            else if (value.Length.Equals(max))
            {
                return value;
            }

            return $"{value.Substring(0, max)}...";
        }
    }
}

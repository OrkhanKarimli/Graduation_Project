using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.AppCode.Extensions
{
    public static partial class SeoFriendlyUrlExtension
    {
        static public string FriendlyUrlContext(this IUrlHelper url, string context)
        {
            if (string.IsNullOrWhiteSpace(context))
                return "";


            context = context.Replace("Ü", "u").Replace("ü", "u")
                .Replace("İ", "i").Replace("I", "i").Replace("ı", "i")
                .Replace("Ş", "s").Replace("ş", "s")
                .Replace("Ö", "o").Replace("ö", "o")
                .Replace("Ç", "c").Replace("ç", "c")
                .Replace("Ğ", "g").Replace("ğ", "g")
                .Replace("Ə", "e").Replace("ə", "e")
                .Replace(" ", "-").Replace("?", "").Replace("/", "")
                .Replace("\\", "").Replace(".", "").Replace("'", "").Replace("#", "").Replace("%", "")
                .Replace("&", "").Replace("*", "").Replace("!", "").Replace("@", "").Replace("+", "")
                .ToLower().Trim();

            context = Regex.Replace(context, @"\&+", "and");
            context = Regex.Replace(context, @"[^a-z0-9]", "-");
            context = Regex.Replace(context, @"-+", "-");
            context = context.Trim('-');

            return context;
        }
    }
}

using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.AppCode.Extensions
{
    public static partial class Extension
    {
        public static bool IsModelStateValid(this IActionContextAccessor ctx)
        {
            return ctx.ActionContext.ModelState.IsValid;
        }
    }
}
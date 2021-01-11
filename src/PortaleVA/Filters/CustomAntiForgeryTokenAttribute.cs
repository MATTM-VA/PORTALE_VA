using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using VAPortale.Code;
using VALib.Helpers;

namespace VAPortale.Filters
{
    public class CustomAntiForgeryTokenAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            bool shouldValidate = !filterContext
                .ActionDescriptor
                .GetCustomAttributes(typeof(ExcludeFromAntiForgeryValidationAttribute), true)
                .Any();
            if (shouldValidate)
            {
                System.Web.Helpers.AntiForgery.Validate();
            }
        }
    }
    [AttributeUsage(AttributeTargets.Method)]
    public class ExcludeFromAntiForgeryValidationAttribute : Attribute
    {
    }
}
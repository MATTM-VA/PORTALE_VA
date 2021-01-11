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
    public class LanguageAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            string cultureName = (filterContext.RouteData.Values["lang"] as string) ?? string.Empty;    

            if (CultureHelper.IsValidCulture(cultureName))
                cultureName = CultureHelper.GetImplementedCulture(cultureName);
            else
                cultureName = CultureHelper.GetImplementedCulture("it-IT"); 

            filterContext.RouteData.Values["lang"] = cultureName;
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

        }
    }
}
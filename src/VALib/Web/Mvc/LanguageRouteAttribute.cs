using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace VALib.Web.Mvc
{
    public class LanguageRouteAttribute : AuthorizeAttribute
    {
        public LanguageRouteAttribute()
            : base()
        {

        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //string cultureName = (filterContext.RouteData.Values["lang"] as string) ?? string.Empty;

            //if (CultureHelper.IsValidCulture(cultureName))
            //    cultureName = CultureHelper.GetImplementedCulture(cultureName);
            //else
            //    cultureName = CultureHelper.GetImplementedCulture("it-IT"); // la default è italiano

            //filterContext.RouteData.Values["lang"] = cultureName;
            //Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            //Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;


            base.OnAuthorization(filterContext);
        }

        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            return true;
        }
    }
}

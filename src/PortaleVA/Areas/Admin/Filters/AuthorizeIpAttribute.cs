using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using VAPortale.Code;

namespace VAPortale.Areas.Admin.Filters
{
    public class AuthorizeIpAttribute : ActionFilterAttribute
    {

        IPList allowedIPListToCheck = new IPList();


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string ipAddress = HttpContext.Current.Request.UserHostAddress;

            if (!IsIpAddressAllowed(ipAddress.Trim()))
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { area = "" , controller = "Home", action = "index" }));
                //filterContext.Result = new HttpStatusCodeResult(403);
            }

            base.OnActionExecuting(filterContext);
        }

        private bool IsIpAddressAllowed(string IpAddress)
        {
            if (!string.IsNullOrWhiteSpace(IpAddress))
            {
                String AllowedSingleIPs = ConfigurationManager.AppSettings["AllowedAdminSingleIPs"];
                String AllowedMaskedIPs = ConfigurationManager.AppSettings["AllowedAdminMaskedIPs"];

                if (!string.IsNullOrEmpty(AllowedSingleIPs))
                    SplitAndAddSingleIPs(AllowedSingleIPs, allowedIPListToCheck);

                if (!string.IsNullOrEmpty(AllowedMaskedIPs))
                    SplitAndAddMaskedIPs(AllowedMaskedIPs, allowedIPListToCheck);




                //string[] AllowedIPs = Convert.ToString(ConfigurationManager.AppSettings["AllowedAdminSingleIPs"]).Split(',');
                //string[] AllowedMaskedIPs = Convert.ToString(ConfigurationManager.AppSettings["AllowedAdminMaskedIPs"]).Split(',');

                //return AllowedIPs.Where(a => a.Trim().Equals(IpAddress, StringComparison.InvariantCultureIgnoreCase)).Any();
            }
            return allowedIPListToCheck.CheckNumber(IpAddress);
        }

        private void SplitAndAddSingleIPs(string ips, IPList list)
        {
            var splitSingleIPs = ips.Split(',');
            foreach (string ip in splitSingleIPs)
                list.Add(ip);
        }

        private void SplitAndAddMaskedIPs(string ips, IPList list)
        {
            var splitMaskedIPs = ips.Split(',');
            foreach (string maskedIp in splitMaskedIPs)
            {
                var ipAndMask = maskedIp.Split(';');
                list.Add(ipAndMask[0], ipAndMask[1]); // IP;MASK
            }
        }
    }
}
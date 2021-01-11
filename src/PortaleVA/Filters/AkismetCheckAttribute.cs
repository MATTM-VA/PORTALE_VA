using Joel.Net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VALib.Web;

namespace VAPortale.Filters
{
    public class AkismetCheckAttribute : ActionFilterAttribute
    {
        public AkismetCheckAttribute(
            string authorField,
            string emailField,
            string websiteField,
            string commentField)
        {
            this.AuthorField = authorField;
            this.EmailField = emailField;
            this.WebsiteField = websiteField;
            this.CommentField = commentField;
        }

        public string AuthorField { get; set; }
        public string EmailField { get; set; }
        public string WebsiteField { get; set; }
        public string CommentField { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Create a new instance of the Akismet API and verify your key is valid.
            Akismet api = new Akismet(ConfigurationManager.AppSettings["AkismetApiKey"], UrlUtility.VADomain, filterContext.HttpContext.Request.UserAgent);
            if (!api.VerifyKey()) throw new Exception("Akismet API key invalid.");

            //Now create an instance of AkismetComment, populating it with values
            //from the POSTed form collection.
            AkismetComment akismetComment = new AkismetComment
            {
                Blog = UrlUtility.VADomain,
                UserIp = filterContext.HttpContext.Request.UserHostAddress,
                UserAgent = filterContext.HttpContext.Request.UserAgent,
                CommentContent = filterContext.HttpContext.Request[this.CommentField],
                CommentType = "comment",
                CommentAuthor = filterContext.HttpContext.Request[this.AuthorField],
                CommentAuthorEmail = filterContext.HttpContext.Request[this.EmailField],
                CommentAuthorUrl = filterContext.HttpContext.Request[this.WebsiteField]
            };

            //Check if Akismet thinks this comment is spam. Returns TRUE if spam.
            if (api.CommentCheck(akismetComment))
                //Comment is spam, add error to model state.
                filterContext.Controller.ViewData.ModelState.AddModelError("spam", "Comment identified as spam.");
            base.OnActionExecuting(filterContext);
        }
    }
}
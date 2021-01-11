using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
 
using VALib.Domain.Entities.Membership;
using VALib.Domain.Entities.Contenuti;
using VALib.Domain.Repositories.Membership;
using VALib.Domain.Services;
using VALib.Configuration;
using VAPortale.Areas.Admin.Models;
using VAPortale.Code;
using VAPortale.Areas.Admin.Filters;
using ElogToolkit.Net.Mail;
using System.Text;
using System.Transactions;
using System.Web.Security;
using VALib.Web;
using VALib.Domain.Repositories.Contenuti;

namespace VAPortale.Areas.Admin.Controllers
{
    [UtenteAuthorize(Roles = RuoloUtenteCodici.GestoreUtenti)]
    public class EventsController : VAController
    {

        public ActionResult Index()
        {
            

            WebEventsIndexModel model = new WebEventsIndexModel();
            MembershipService service = new MembershipService();
            List<WebEvent> ElencoWebEvents = WebEventsRepository.Instance.RecuperaEventi(null);
            model.ElencoWebEvents = ElencoWebEvents;
            return View(model);

        }



    }
}

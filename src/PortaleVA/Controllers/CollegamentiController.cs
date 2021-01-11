using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VAPortale.Filters;
using VAPortale.Models;
using VALib.Domain.Entities.UI;
using VALib.Domain.Repositories.UI;
using VALib.Domain.Entities.Contenuti;
using VALib.Domain.Repositories.Contenuti;
using VALib.Helpers;
using VAPortale.Code;
using ElogToolkit;
using VALib.Domain.Services;
using VAPortale.Models.Support;
using Newtonsoft.Json;
using VALib.Domain.Report;

namespace VAPortale.Controllers
{
    [LanguageAttribute]
    public class CollegamentiController : Controller
    {
        //
        // GET: /Collegamenti/

        public ActionResult Regioni()
        {
            CollegamentiVasViaRegioniModel model = new CollegamentiVasViaRegioniModel();
            VoceMenu voce = VoceMenuRepository.Instance.RecuperaVoceMenu("Regioni");
          
            model.VoceMenu = voce;

            return View(model);
        }

        
        public ActionResult AIAregionali()
        {
            CollegamentiVasViaRegioniModel model = new CollegamentiVasViaRegioniModel();
            VoceMenu voce = VoceMenuRepository.Instance.RecuperaVoceMenu("AIAregionali");
          
            model.VoceMenu = voce;

            return View(model);
        }

       
    }

}

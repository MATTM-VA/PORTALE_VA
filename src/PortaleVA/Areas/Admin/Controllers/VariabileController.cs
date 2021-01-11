using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VALib.Domain.Entities.Membership;
using VALib.Domain.Entities.UI;
using VALib.Domain.Repositories.UI;
using VALib.Domain.Services;
using VAPortale.Areas.Admin.Filters;
using VAPortale.Areas.Admin.Models;

namespace VAPortale.Areas.Admin.Controllers
{
    [UtenteAuthorize(Roles = RuoloUtenteCodici.GestoreVariabili)]
    public class VariabileController : Controller
    {
        //
        // GET: /Admin/Variabile/

        public ActionResult Index(string stato = "")
        {
            VariabileIndexModel model = new VariabileIndexModel();
            List<Variabile> variabili = VariabileRepository.Instance.RecuperaVariabili();

            model.Variabili = variabili;
            model.TotaleRisultati = variabili.Count;
            
            return View(model);
        }

        [HttpPost]
        public ActionResult Edita(string chiave, string valore)
        {
            ActionResult result = null;
            Variabile variabile = VariabileRepository.Instance.RecuperaVariabile(chiave);
            string stato = "";

            if (variabile != null)
            {
                ContenutoService cs = new ContenutoService();

                cs.SalvaVariabile(chiave, valore);
            }
            else
            {
                stato = "La variabile non è stata trovata";
            }

            result = RedirectToAction("Index", new { stato = stato });

            return result;
        }
    }
}

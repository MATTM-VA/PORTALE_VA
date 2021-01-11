using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VALib.Domain.Entities.Membership;
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

namespace VAPortale.Areas.Admin.Controllers
{
    [UtenteAuthorize(Roles = RuoloUtenteCodici.GestoreUtenti)]
    public class UtenteController : VAController
    {
        //
        // GET: /Admin/Utente/

       
        public ActionResult Index()
        {
            UtenteIndexModel model = new UtenteIndexModel();
            MembershipService service = new MembershipService();
            List<Utente> utenti = service.RecuperaListaUtenti();
            model.Utenti = utenti;
            return View(model);
        }

        [HttpGet]
        public ActionResult GestioneRuoli(int id)
        {
            GestioneRuoliModel model = new GestioneRuoliModel();
            model.UtenteID = id;
            MembershipService service = new MembershipService();
            Utente utente = service.RecuperaUtente(id);
            Utente utenteCorrente = MembershipUtils.RecuperaUtenteCorrente();

            List<RuoloUtente> ruoli = new List<RuoloUtente>();
            ruoli = service.RecuperaListaRuoliUtente();
            if (utente.ID == utenteCorrente.ID)
            {
                model.UtenteCorrente = true;
            }
            else
            {
                model.UtenteCorrente = false;
            }

            model.Utente = utente;
            model.RuoliUtente.AddRange(ruoli);
            return View(model);
        }

        [HttpPost]
        public JsonResult AggiornaPermesso(int ruoloUtenteID, int utenteID, bool aggiungi)
        {
            object result = null;
            MembershipService service = new MembershipService();
            Utente utente = service.RecuperaUtente(utenteID);
            //Utente utente = UtenteRepository.Instance.RecuperaUtente(utenteID);           
            if (utente != null)
            {
                try
                {
                    if (aggiungi)
                        service.AssegnaRuoloUtente(utenteID, ruoloUtenteID);
                    else
                        service.EliminaRuoloUtente(utenteID, ruoloUtenteID);

                    result = new { status = "Ok", message = "Operazione effettuata con successo" };
                }

                catch (Exception ex)
                {
                    result = new { status = "Error", message = "Si è verificato un errore sul server" };
                }
            }
            else
            {
                result = new { status = "Error", message = "L'utente non esiste" };
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult ValidaEmail(string email, int id = 0)
        {
            try
            {
                MembershipService service = new MembershipService();
                Utente utente = service.RecuperaUtente(email);

                if (utente == null || utente.ID == id)
                    return Json(true);
                else
                    return Json(false);
            }
            catch (Exception ex)
            {
                return Json("Errore server");
            }
        }

        [HttpGet]
        public ActionResult ModificaUtente(int id)
        {
            ActionResult result = null;
            ModificaUtenteModel model = new ModificaUtenteModel();
            MembershipService service = new MembershipService();
            Utente utente = service.RecuperaUtente(id);
            Utente utenteCorrente = MembershipUtils.RecuperaUtenteCorrente();

            if (utenteCorrente.ID == utente.ID)
            {
                model.UtenteCorrente = true;
            }
            else
            {
                model.UtenteCorrente = false;
            }

            //UtenteRepository.Instance.RecuperaUtenteDaEmail(utente.Email);
            if (utente != null)
            {
                model.Id = id;
                model.Nome = utente.Nome;
                model.NomeUtente = utente.NomeUtente;
                model.DataUltimoLogin = utente.DataUltimoLogin;
                model.Cognome = utente.Cognome;
                model.Email = utente.Email;
                model.Abilitato = utente.Abilitato;
                model.DataUltimoCambioPassword = utente.DataUltimoCambioPassword;
                result = View(model);
            }
            else
            {
                result = HttpNotFound();
            }
            return result;
        }

        [HttpPost]
        public ActionResult ModificaUtente(ModificaUtenteModel model)
        {
            ActionResult result = null;
            if (ModelState.IsValid)
            {
                MembershipService service = new MembershipService();

                Utente utente = service.RecuperaUtente(model.Id);

                utente.Cognome = model.Cognome;
                utente.Nome = model.Nome;
                utente.Email = model.Email;
                utente.Abilitato = model.Abilitato;

                model.NomeUtente = utente.NomeUtente;
                model.DataUltimoLogin = utente.DataUltimoLogin;
                model.DataUltimoCambioPassword = utente.DataUltimoCambioPassword;

                Utente utenteCorrente = User.Utente;

                if (utenteCorrente.ID == utente.ID)
                {
                    model.UtenteCorrente = true;
                }
                else
                {
                    model.UtenteCorrente = false;
                }

                service.ModificaUtente(utente);

                result = RedirectToAction("ModificaUtente", new { id = model.Id });
            }
            else
            {
                result = View(model);
            }

            return result;
        }

        [HttpGet]
        public ActionResult CreaUtente()
        {
            CreaUtenteModel model = new CreaUtenteModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreaUtente(CreaUtenteModel model)
        {
            ActionResult result = null;
            Utente nuovoUtente = null;
            string nomeUtente = "";
            string token = "";
            string urlCreazioneToken = "";
            //string oggettoMail = "";
            //StringBuilder bodyMail = new StringBuilder();

            if (ModelState.IsValid)
            {
                MembershipService service = new MembershipService();
                nomeUtente = service.GeneraNomeUtente(model.Nome, model.Cognome);

                using (TransactionScope tScope = new TransactionScope())
                {
                    try
                    {

                        nuovoUtente = service.CreaNuovoUtenteESalva(nomeUtente, model.Nome, model.Cognome, true, model.Email);
                        // result = RedirectToAction("GestioneRuoli", new { id = nuovoUtente.ID });

                        token = MembershipService.GeneraTokenPrimoLogin(nuovoUtente);

                        urlCreazioneToken = Url.Action("ResetPassword", "Home", new { t = token }, this.Request.Url.Scheme);

                        EmailService.InvioToken(urlCreazioneToken, nuovoUtente, TipoEmail.Registrazione);

                        model.UtenteRegistrato = true;

                        result = View(model);

                        tScope.Complete();
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Si è verificato un errore nella creazione dell'utente");
                        new VAWebRequestErrorEvent("Errore in creazione utente", this, ex).Raise();

                        result = View(model);
                    }
                    finally
                    {
                        tScope.Dispose();
                    }
                }

            }
            else
            {
                result = View(model);
            }

            return result;
        }

        [HttpPost]
        public ActionResult ResetPasswordUtente(int Id)
        {
            ActionResult result = null;
            string token = null;
            string urlCreazioneToken = null;
            Utente utente = null;
            MembershipService service = null;

            service = new MembershipService();

            utente = service.RecuperaUtente(Id);

            if (utente != null)
            {
                using (TransactionScope tScope = new TransactionScope())
                {
                    try
                    {
                        service.ResetPasswordUtente(utente);

                        token = MembershipService.GeneraTokenPrimoLogin(utente);

                        urlCreazioneToken = Url.Action("ResetPassword", "Home", new { t = token }, this.Request.Url.Scheme);

                        EmailService.InvioToken(urlCreazioneToken, utente, TipoEmail.ResetPassword);

                        tScope.Complete();
                        tScope.Dispose();
                    }
                    catch (Exception ex)
                    {
                        new VAWebRequestErrorEvent("Errore in reset password utente", this, ex).Raise();
                    }
                    finally
                    {
                        tScope.Dispose();
                    }

                    result = RedirectToAction("ModificaUtente", new { id = utente.ID });
                }
            }
            else
            {
                result = HttpNotFound();
            }


            return result;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VAPortale.Areas.Admin.Models;
using VALib.Domain.Entities.Membership;
using VAPortale.Code;
using VALib.Domain.Services;
using VALib.Domain.Repositories.Membership;
using System.Web.Security;
using VAPortale.Areas.Admin.Filters;

namespace VAPortale.Areas.Admin.Controllers
{
    
    [Authorize]
    public class HomeController : VAController
    {
        // GET: /Home/
        public ActionResult Index()
        {
            HomeIndexModel model = new HomeIndexModel();
            Utente utente = MembershipUtils.RecuperaUtenteCorrente();

            model.Utente = utente;

            return View(model);
        }

        [AuthorizeIp]
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            HomeLoginModel model = new HomeLoginModel();
            return View(model);
        }

        [AuthorizeIp]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(HomeLoginModel model)
        {
            ActionResult result = null;
            bool loginEseguito = false;
            Utente utente = null;

            if (ModelState.IsValid)
            {
                MembershipService membershipSrv = new MembershipService();
                loginEseguito = membershipSrv.EseguiLoginUtente(model.NomeUtente, model.Pswd);
            }

            if (loginEseguito)
            {
                utente = UtenteRepository.Instance.RecuperaUtente(model.NomeUtente);

                if (utente != null)
                {
                    FormsAuthentication.SetAuthCookie(model.NomeUtente, false);
                    result = RedirectToAction("Index", "Home");
                }
                else
                    result = View(model);
            }
            else 
            {
                ModelState.AddModelError("", "Username o password errati");
                result = View(model);
            }
            return result;
        }

        public ActionResult LogOut()
        {
            if (MembershipUtils.UtenteAutenticato)
                FormsAuthentication.SignOut();

            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult CambioPassword()
        {
            HomeCambioPasswordModel model = new HomeCambioPasswordModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult CambioPassword(HomeCambioPasswordModel model)
        {
            ActionResult result = null;
            
            if (ModelState.IsValid)
            {
                
                string oldHashedPassword = UtenteRepository.Instance.RecuperaPassword(User.Utente.ID);
                MembershipService membershipSrv = new MembershipService();                
                if (membershipSrv.EseguiValidazionePassword(oldHashedPassword,model.VecchiaPassword))
                {                 
                    string hashedPassword = membershipSrv.EseguiHashPassword(model.NuovaPassword);
                    UtenteRepository.Instance.AggiornaPassword(User.Utente.ID, hashedPassword);
                    result = RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("VecchiaPassword","Vecchia password non valida");
                }
            }
            if (result==null)
            {
                result = View(model);
            }   
       
            return result;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult ResetPassword(string t)
        {
            string nomeUtente = null;
            ActionResult result = null;
            ResetPasswordModel model = new ResetPasswordModel();
            Utente utente = null;
            MembershipService mService = new MembershipService();
            bool utenteAutorizzato = false;


            utenteAutorizzato = mService.EseguiLoginUtenteToken(t, out nomeUtente);

            if (utenteAutorizzato)
            {
                utente = UtenteRepository.Instance.RecuperaUtente(nomeUtente);

                if (utente != null)
                {
                    FormsAuthentication.SetAuthCookie(nomeUtente, false);
                    model.TokenValido = true;
                    model.UtenteId = utente.ID;
                }
                else
                {
                    model.TokenValido = false;
                }
            }
            else
            {
                model.TokenValido = false;
            }


            result = View(model);
            return result;
        }


        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            string hashedPassword = null;
            MembershipService service = new MembershipService();
            ActionResult result = null;

            if (ModelState.IsValid)
            {
                hashedPassword = service.EseguiHashPassword(model.Password);
                UtenteRepository.Instance.AggiornaPassword(model.UtenteId, hashedPassword);
                result = RedirectToAction("Index", "Home");
            }
            else
            {
                model.TokenValido = true;
                result = View(model);
            }

           
            return result;
        }

       
    }
}

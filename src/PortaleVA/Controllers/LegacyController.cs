using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElogToolkit;
using VALib.Domain.Services;
using VALib.Domain.Repositories.UI;

namespace VAPortale.Controllers
{
    public class LegacyController : Controller
    {
        //
        // GET: /Legacy/

        public ActionResult Index()
        {
            ActionResult result = null;

            result = RedirectToActionPermanent("Index", "Home");

            return result;
        }

        public ActionResult InfoVia()
        {
            ActionResult result = null;

            int id = Parse.ToInt32OrDefault(Request.QueryString["ID_Progetto"], 0);

            if (id > 0)
                result = RedirectToActionPermanent("Info", "Oggetti", new { id = id });
            else
                result = HttpNotFound();

            return result;
        }

        public ActionResult InfoVas()
        {
            ActionResult result = null;

            int id = Parse.ToInt32OrDefault(Request.QueryString["ID_PianoProgramma"], 0);

            if (id > 0)
            {
                id = LegacyService.GetOggettoVasID(id);
                result = RedirectToActionPermanent("Info", "Oggetti", new { id = id });
            }
            else
                result = HttpNotFound();

            return result;
        }

        public ActionResult DocumentazioneVia()
        {
            ActionResult result = null;

            int id = Parse.ToInt32OrDefault(Request.QueryString["ID_Progetto"], 0);
            int oggettoProceduraID = LegacyService.GetUltimoOggettoProceduraID(id);

            if (id > 0 && oggettoProceduraID > 0)
                result = RedirectToActionPermanent("Documentazione", "Oggetti", new { id = id, oggettoProceduraID = oggettoProceduraID });
            else
                result = HttpNotFound();

            return result;
        }

        public ActionResult DocumentazioneVas()
        {
            ActionResult result = null;

            int id = Parse.ToInt32OrDefault(Request.QueryString["ID_PianoProgramma"], 0);
            
            if (id > 0)
                id = LegacyService.GetOggettoVasID(id);
            
            int oggettoProceduraID = LegacyService.GetUltimoOggettoProceduraID(id);

            if (id > 0 && oggettoProceduraID > 0)
                result = RedirectToActionPermanent("Documentazione", "Oggetti", new { id = id, oggettoProceduraID = oggettoProceduraID });
            else
                result = HttpNotFound();

            return result;
        }

        public ActionResult MetadatoDocumentoVia()
        {
            ActionResult result = null;

            int id = Parse.ToInt32OrDefault(Request.QueryString["ID_Documento"], 0);

            if (id > 0)
                result = RedirectToActionPermanent("MetadatoDocumento", "Oggetti", new { id = id });
            else
                result = HttpNotFound();

            return result;
        }

        public ActionResult MetadatoDocumentoVas()
        {
            ActionResult result = null;

            int id = Parse.ToInt32OrDefault(Request.QueryString["ID_Documento"], 0);

            if (id > 0)
            {
                id = LegacyService.GetDocumentoVasID(id);
                result = RedirectToActionPermanent("MetadatoDocumento", "Oggetti", new { id = id });
            }
            else
                result = HttpNotFound();

            return result;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VALib.Domain.Entities.Contenuti;
using VALib.Domain.Repositories.UI;
using VAPortale.Filters;
using VAPortale.Models;
using VALib.Domain.Services;
using VALib.Web;
using VALib.Domain.Entities.Contenuti.Base;
using VALib.Domain.Repositories.Contenuti;
using VALib.Helpers;
using VAPortale.Models.Common;
using VAPortale.Code;

namespace VAPortale.Controllers
{
    [LanguageAttribute]
    public class EventiController : Controller
    {
        public ActionResult Oggetto(int id = 0)
        {
            OggettiController OG = new OggettiController();

            ViewResult result = OG.Info(id) as ViewResult;
            dynamic model = result.Model;
            dynamic oggetto = model.Oggetto;
            model.EventiModel = new OggettoEventiModel()
            {
                Eventi = (EventoRepository.Instance.RecuperaEventi(oggetto.ID, TipoEventoEnum.GruppoDiLavoro) as List<GM_Evento>)
                    .Join(TipoEventoRepository.Instance.RecuperaTipiEventi(), e => (int)e.TipoEvento, te => te.ID, (e, te) => new EventoModel() { Evento = e, TipoEvento = te })
                    .ToList()
            };
            model.DatiAmministrativiModel = new OggettiDatiAmministrativiModel();
            model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu(120);

            return result;
        }


        [ChildActionOnly]
        public ActionResult ElencoEventi(OggettoEventiModel model)
        {
            if (model != null)
                return PartialView("GruppiDiLavoro", model);
            else
                return new EmptyResult();
        }


        public ActionResult Documentazione(EventiDocumentazioneModel model)
        {
            ActionResult result = null;

            GM_Evento evento = EventoRepository.Instance.RecuperaEvento(model.ID);

            if (evento != null)
            {
                model.Evento = new EventoModel()
                {
                    Evento = evento,
                    TipoEvento = TipoEventoRepository.Instance.RecuperaTipoEvento((int)evento.TipoEvento)
                };

                model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("documentazione");

                int totale = 0;

                if (!string.IsNullOrWhiteSpace(model.Mode) && model.Mode.Equals("export", StringComparison.CurrentCultureIgnoreCase))
                {
               
                }
                else
                {
                    IEnumerable<DocumentoElenco> documenti = EventoDocumentiElencoRepository.Instance.RecuperaDocumentiElenco(
                                                    model.ID,
                                                    model.RaggruppamentoID,
                                                    CultureHelper.GetCurrentCultureShortName(),
                                                    model.Testo ?? "",
                                                    "",
                                                    "",
                                                    model.IndiceInizio,
                                                    model.IndiceInizio + model.DimensionePagina,
                                                    out totale);
                    model.Documenti = documenti;

                    model.TotaleRisultati = totale;

                    result = View("Documentazione", model);
                }

            }
            else
                result = HttpNotFound();

            return result;
        }


        public ActionResult Documento(int id = 0)
        {
            ActionResult result = null;
            DocumentoDownload documento = EventoDocumentiElencoRepository.Instance.RecuperaDocumentoDownload(id);

            if (documento != null)
            {
                string filepath = null;
                string ext = documento.Estensione;
                string nomeFile = documento.PercorsoFile.Substring(documento.PercorsoFile.LastIndexOf("/") + 1);

                filepath = FileUtility.VADocumentoAiaEventi(documento.PercorsoFile);

                if (System.IO.File.Exists(filepath))
                {
                    result = File(filepath, "application/octet-stream", nomeFile);
                    if (!Request.Browser.Crawler)
                        new VAWebRequestDocumentoDownloadEvent("VA Download documento", this, id, VAWebEventTypeEnum.DownloadAltroDocumento).Raise();
                }
                else
                    result = HttpNotFound();
            }
            else
            {
                result = HttpNotFound();
            }

            return result;
        }


        [ChildActionOnly]
        public ActionResult RaggruppamentiRootNodes(int EventoID, int? raggruppamentoID)
        {
            EventiRaggruppamentiTreeViewModel model = new EventiRaggruppamentiTreeViewModel();
            List<Raggruppamento> raggruppamenti = RaggruppamentoEventiRepository.Instance.RecuperaRaggruppamentiPerEventoID(EventoID);

            if (raggruppamentoID.HasValue)
                model.RaggruppamentoID = raggruppamentoID.Value;

            model.EventoID = EventoID;
            model.Raggruppamenti = raggruppamenti.Where(x => x.GenitoreID == 0).ToList();

            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult RaggruppamentiChildNodes(int EventoID, int genitoreID, int? raggruppamentoID)
        {
            EventiRaggruppamentiTreeViewModel model = new EventiRaggruppamentiTreeViewModel();
            List<Raggruppamento> raggruppamenti = RaggruppamentoEventiRepository.Instance.RecuperaRaggruppamentiPerEventoID(EventoID);

            if (raggruppamentoID.HasValue)
                model.RaggruppamentoID = raggruppamentoID.Value;

            model.EventoID = EventoID;
            model.Raggruppamenti = raggruppamenti.Where(x => x.GenitoreID == genitoreID).ToList();

            return PartialView(model);
        }


    }
}

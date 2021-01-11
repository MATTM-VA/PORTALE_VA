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
    public class OggettiController : Controller
    {
        //
        // GET: /Oggetti/

        public ActionResult Info(int id = 0)
        {
            ActionResult result = null;
            object oggetto = OggettoRepository.Instance.RecuperaOggettoInfo(id);

            if (oggetto != null)
            {
                // in comune tra via e vas
                OggettiTerritoriModel territoriModel = null;
                OggettiDatiAmministrativiModel datiAmministrativiModel = new OggettiDatiAmministrativiModel();
                bool immagineLocalizzazione = FileUtility.EsisteImmagine(FormatoImmagineEnum.Localizzazione, id);

                datiAmministrativiModel.OggettoID = id;

                if (oggetto is OggettoInfoVia)
                {
                    territoriModel = CreaTerritoriModel(id, MacroTipoOggettoEnum.Via, ((OggettoInfoVia)oggetto).Territori, immagineLocalizzazione, ((OggettoInfoVia)oggetto).LinkLocalizzazione);
                    datiAmministrativiModel.ProcedureCollegate = ((OggettoInfoVia)oggetto).ProcedureCollegate;
                    datiAmministrativiModel.DatiAmministrativi = ((OggettoInfoVia)oggetto).DatiAmministrativi;

                    result = InfoVia((OggettoInfoVia)oggetto, territoriModel, datiAmministrativiModel);
                }
                else if (oggetto is OggettoInfoVas)
                {
                    territoriModel = CreaTerritoriModel(id, MacroTipoOggettoEnum.Vas, ((OggettoInfoVas)oggetto).Territori, immagineLocalizzazione, ((OggettoInfoVas)oggetto).LinkLocalizzazione);
                    datiAmministrativiModel.ProcedureCollegate = ((OggettoInfoVas)oggetto).ProcedureCollegate;
                    datiAmministrativiModel.DatiAmministrativi = ((OggettoInfoVas)oggetto).DatiAmministrativi;

                    result = InfoVas((OggettoInfoVas)oggetto, territoriModel, datiAmministrativiModel);
                }
                else if (oggetto is OggettoInfoAIA)
                {
                    territoriModel = CreaTerritoriModel(id, MacroTipoOggettoEnum.Aia, ((OggettoInfoAIA)oggetto).Territori, immagineLocalizzazione, ((OggettoInfoAIA)oggetto).LinkLocalizzazione);
                    datiAmministrativiModel.ProcedureCollegate = ((OggettoInfoAIA)oggetto).ProcedureCollegate;
                    datiAmministrativiModel.DatiAmministrativi = ((OggettoInfoAIA)oggetto).DatiAmministrativi;
                    if (datiAmministrativiModel.ProcedureCollegate.FirstOrDefault(x => x.ViperaAiaID != null) == null)
                        // Id della URL corrisponde ad un AIA Regionale, utente viene reindirizzato alla view corrispondente
                        result = RedirectToAction("InfoAiaRegionale", new { id = id });
                    else
                        result = InfoAIA((OggettoInfoAIA)oggetto, territoriModel, datiAmministrativiModel);
                }

            }

            if (oggetto == null)
                result = HttpNotFound();

            return result;
        }

        public ActionResult InfoAiaRegionale(int id = 0)
        {
            ActionResult result = null;
            object oggetto = OggettoRepository.Instance.RecuperaOggettoInfoAiaRegionale(id);

            if (oggetto != null)
            {
                bool immagineLocalizzazione = FileUtility.EsisteImmagine(FormatoImmagineEnum.Localizzazione, id);
                OggettiTerritoriModel territoriModel = null;
                territoriModel = CreaTerritoriModel(id, MacroTipoOggettoEnum.Aia, ((OggettoInfoAIA)oggetto).Territori, immagineLocalizzazione, ((OggettoInfoAIA)oggetto).LinkLocalizzazione);

                OggettiInfoAIAModel model = new OggettiInfoAIAModel();
                model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("info");
                model.Oggetto = (OggettoInfoAIA)oggetto;
                model.TerritoriModel = territoriModel;
             
                return View("InfoAiaRegionale", model);
            }

            if (oggetto == null)
                result = HttpNotFound();

            return result;
        }

        public ActionResult InfoVia(OggettoInfoVia oggetto, OggettiTerritoriModel territoriModel, OggettiDatiAmministrativiModel datiAministrativiModel)
        {
            OggettiInfoViaModel model = new OggettiInfoViaModel();

            model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("info");
            model.Oggetto = oggetto;
            model.TerritoriModel = territoriModel;
            model.DatiAmministrativiModel = datiAministrativiModel;

            return View("InfoVia", model);
        }

        public ActionResult InfoVas(OggettoInfoVas oggetto, OggettiTerritoriModel territoriModel, OggettiDatiAmministrativiModel datiAministrativiModel)
        {
            OggettiInfoVasModel model = new OggettiInfoVasModel();

            model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("info");
            model.Oggetto = oggetto;
            model.TerritoriModel = territoriModel;
            model.DatiAmministrativiModel = datiAministrativiModel;

            model.AutoritaProcedente = oggetto.EntitaCollegate.FirstOrDefault(x => x.Ruolo.Enum == RuoloEntitaEnum.AutoritaProcedente);
            model.AutoritaCompetente = oggetto.EntitaCollegate.FirstOrDefault(x => x.Ruolo.Enum == RuoloEntitaEnum.AutoritaCompetente);
            model.Proponente = oggetto.EntitaCollegate.FirstOrDefault(x => x.Ruolo.Enum == RuoloEntitaEnum.Proponente);

            return View("InfoVas", model);
        }

        public ActionResult InfoAIA(OggettoInfoAIA oggetto, OggettiTerritoriModel territoriModel, OggettiDatiAmministrativiModel datiAministrativiModel)
        {
            OggettiInfoAIAModel model = new OggettiInfoAIAModel();

            model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("info");
            model.Oggetto = oggetto;
            model.TerritoriModel = territoriModel;
            model.DatiAmministrativiModel = datiAministrativiModel;
         
            return View("InfoAIA", model);
        }

        [ChildActionOnly]
        public ActionResult Territori(OggettiTerritoriModel model)
        {
            return PartialView(model);
        }

   

        [ChildActionOnly]
        public ActionResult DatiAmministrativiRestable(OggettiDatiAmministrativiModel model, string f = null, string nf = null, int da = 0)
        {
           
            if ((f != null || nf != null) && da != 0)
            {
                List<int> vdaFilter = model.DatiAmministrativi
                    .Where(x => x.DatoAmministrativo.ID == da && ((f != null && x.GetValore().IndexOf(f) == -1) || (nf != null && x.GetValore().IndexOf(nf) != -1)))
                    .GroupBy(g => g.OggettoProceduraID)
                    .Select(x => x.Key)
                    .ToList();

                model.ProcedureCollegate = model.ProcedureCollegate.Where(x => !vdaFilter.Contains(x.OggettoProceduraID)).ToList();
            }

            return PartialView(model);
        }

        private OggettiTerritoriModel CreaTerritoriModel(int oggettoID, MacroTipoOggettoEnum macroTipoOggetto, List<Territorio> territori, bool immagineLocalizzazione, string linkLocalizzazione)
        {
            OggettiTerritoriModel model = new OggettiTerritoriModel();

            string regioni = "";
            string province = "";
            string comuni = "";
            string areeMarine = "";

            foreach (Territorio t in territori)
            {
                switch (t.Tipologia.Enum)
                {
                    case TipologiaTerritorioEnum.Nessuna:
                    case TipologiaTerritorioEnum.Nazione:
                    case TipologiaTerritorioEnum.AreeExtraterritoriali:
                        break;
                    case TipologiaTerritorioEnum.Regione:
                        regioni += string.Format("{0}, ", t.Nome);
                        break;
                    case TipologiaTerritorioEnum.Provincia:
                        province += string.Format("{0}, ", t.Nome);
                        break;
                    case TipologiaTerritorioEnum.Comune:
                        comuni += string.Format("{0}, ", t.Nome);
                        break;
                    case TipologiaTerritorioEnum.Mare:
                        areeMarine += string.Format("{0}, ", t.Nome);
                        break;
                    default:
                        break;
                }
            }

            if (!string.IsNullOrWhiteSpace(regioni))
                regioni = regioni.Substring(0, regioni.Length - 2);
            else
                switch (macroTipoOggetto)
                {
                    case MacroTipoOggettoEnum.Via:
                    case MacroTipoOggettoEnum.Aia:
                        regioni = DizionarioService.OGGETTO_TestoNoRegioni;
                        break;
                    case MacroTipoOggettoEnum.Vas:
                        regioni = DizionarioService.OGGETTO_TestoTutteLeRegioni;
                        break;
                    default:
                        break;
                }

            if (!string.IsNullOrWhiteSpace(province))
                province = province.Substring(0, province.Length - 2);
            else
                switch (macroTipoOggetto)
                {
                    case MacroTipoOggettoEnum.Via:
                    case MacroTipoOggettoEnum.Aia:
                        province = DizionarioService.OGGETTO_TestoNoProvince;
                        break;
                    case MacroTipoOggettoEnum.Vas:
                        province = string.Format(DizionarioService.OGGETTO_TestoTutteLeProvince, regioni);
                        break;
                    default:
                        break;
                }

            if (!string.IsNullOrWhiteSpace(comuni))
                comuni = comuni.Substring(0, comuni.Length - 2);
            else
                switch (macroTipoOggetto)
                {
                    case MacroTipoOggettoEnum.Via:
                    case MacroTipoOggettoEnum.Aia:
                        comuni = DizionarioService.OGGETTO_TestoNoComuni;
                        break;
                    case MacroTipoOggettoEnum.Vas:
                        comuni = string.Format(DizionarioService.OGGETTO_TestoTuttiIComuni, regioni);
                        break;
                    default:
                        break;
                }

            if (!string.IsNullOrWhiteSpace(areeMarine))
                areeMarine = areeMarine.Substring(0, areeMarine.Length - 2);
            else
                areeMarine = DizionarioService.OGGETTO_TestoNoMari;

            model.Regioni = regioni;
            model.Province = province;
            model.Comuni = comuni;
            model.AreeMarine = areeMarine;
            model.LinkLocalizzazione = linkLocalizzazione;
            model.OggettoID = oggettoID;
            model.ImmagineLocalizzazione = immagineLocalizzazione;

            return model;
        }


        [RemoveScript]
        [ExcludeFromAntiForgeryValidation]
        public ActionResult Documentazione(OggettiDocumentazioneBaseModel model)
        {
            ActionResult result = null;
            OggettoDocumentazioneBase oggetto = OggettoRepository.Instance.RecuperaOggettoDocumentazione(model.ID, model.OggettoProceduraID);

            ViewBag.ClasseDocumentazione = "documentazione";

            if (oggetto != null)
            {
        
                model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("documentazione");
                model.Oggetto = oggetto;

                int totale = 0;

                if (!string.IsNullOrWhiteSpace(model.Mode) && model.Mode.Equals("export", StringComparison.CurrentCultureIgnoreCase))
                {
                    byte[] data = null;

                    IEnumerable<DocumentoElenco> esportazione = DocumentoElencoRepository.Instance.RecuperaDocumentiElenco(oggetto.TipoOggetto.MacroTipoOggetto.ID,
                                                    model.OggettoProceduraID,
                                                    model.RaggruppamentoID,
                                                    CultureHelper.GetCurrentCultureShortName(),
                                                    model.Testo ?? "",
                                                    "",
                                                    "",
                                                    0,
                                                    int.MaxValue,
                                                    out totale);

                    data = EsportazioneUtils.GeneraXlsxDocumentiDocumentazione(esportazione, oggetto.TipoOggetto.MacroTipoOggetto.Enum);

                    if (data != null)
                        result = File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Export.xlsx");
                    else
                        result = HttpNotFound();
                }
                else
                {
                    IEnumerable<DocumentoElenco> documenti = DocumentoElencoRepository.Instance.RecuperaDocumentiElenco(oggetto.TipoOggetto.MacroTipoOggetto.ID,
                                                    model.OggettoProceduraID,
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

      

        [ChildActionOnly]
        public ActionResult RaggruppamentiRootNodes(int oggettoProceduraID, int? raggruppamentoID)
        {
            OggettiRaggruppamentiTreeViewModel model = new OggettiRaggruppamentiTreeViewModel();
            List<Raggruppamento> raggruppamenti = RaggruppamentoRepository.Instance.RecuperaRaggruppamentiPerOggettoProceduraID(oggettoProceduraID);

            if (raggruppamentoID.HasValue)
                model.RaggruppamentoID = raggruppamentoID.Value;

            model.OggettoProceduraID = oggettoProceduraID;
            model.Raggruppamenti = raggruppamenti.Where(x => x.GenitoreID == 0).ToList();

            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult RaggruppamentiChildNodes(int oggettoProceduraID, int genitoreID, int? raggruppamentoID)
        {
            OggettiRaggruppamentiTreeViewModel model = new OggettiRaggruppamentiTreeViewModel();
            List<Raggruppamento> raggruppamenti = RaggruppamentoRepository.Instance.RecuperaRaggruppamentiPerOggettoProceduraID(oggettoProceduraID);

            if (raggruppamentoID.HasValue)
                model.RaggruppamentoID = raggruppamentoID.Value;

            model.OggettoProceduraID = oggettoProceduraID;
            model.Raggruppamenti = raggruppamenti.Where(x => x.GenitoreID == genitoreID).ToList();

            return PartialView(model);
        }

        public ActionResult MetadatoDocumento(int id = 0)
        {
            ActionResult result = null;
            OggettiMetadatoDocumentoModel model = new OggettiMetadatoDocumentoModel();

            Documento documento = DocumentoRepository.Instance.RecuperaDocumento(id);
            model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("MetadatoDocumento");

            if (documento != null && model.VoceMenu != null)
            {
                model.Documento = documento;

                // Informazioni Generali

                if (!string.IsNullOrWhiteSpace(documento.CodiceElaborato))
                    ModelUtils.AggiungiRiga(model.InformazioniGenerali, DizionarioService.METADATO_LabelCodiceElaborato, documento.CodiceElaborato);

                foreach (EntitaCollegata e in documento.Entita)
                {
                    ModelUtils.AggiungiRiga(model.InformazioniGenerali, e.Ruolo.GetNome(), e.Entita.Nome);
                }

                if (!string.IsNullOrWhiteSpace(documento.Descrizione))
                    ModelUtils.AggiungiRiga(model.InformazioniGenerali, DizionarioService.METADATO_LabelAbstract, documento.Descrizione);

                if (!string.IsNullOrWhiteSpace(documento.Tipologia))
                    ModelUtils.AggiungiRiga(model.InformazioniGenerali, DizionarioService.METADATO_LabelTipoDocumento, documento.Tipologia.Equals("R", StringComparison.CurrentCultureIgnoreCase) ? DizionarioService.METADATO_ValoreTipoDocumentoTestuale : DizionarioService.METADATO_ValoreTipoDocumentoGrafico);

                if (!string.IsNullOrWhiteSpace(documento.Scala))
                    ModelUtils.AggiungiRiga(model.InformazioniGenerali, DizionarioService.METADATO_LabelScala, documento.Scala);

                if (!string.IsNullOrWhiteSpace(documento.Diritti))
                    ModelUtils.AggiungiRiga(model.InformazioniGenerali, DizionarioService.METADATO_LabelDiritti, documento.Diritti);

                if (!string.IsNullOrWhiteSpace(documento.LinguaDocumento))
                    ModelUtils.AggiungiRiga(model.InformazioniGenerali, DizionarioService.METADATO_LabelLingua, documento.LinguaDocumento);

                ModelUtils.AggiungiRiga(model.InformazioniGenerali, DizionarioService.METADATO_LabelDimensione, string.Format("{0} kB", documento.Dimensione));


                // Informazioni Contenuto
                ModelUtils.AggiungiRiga(model.InformazioniContenuto, documento.TipoOggetto.GetNome(), documento.GetNomeOggetto());

                ModelUtils.AggiungiRiga(model.InformazioniContenuto, DizionarioService.METADATO_LabelProcedura, documento.Procedura.GetNome());

                if (!string.IsNullOrWhiteSpace(documento.GetRaggruppamenti()))
                    ModelUtils.AggiungiRiga(model.InformazioniContenuto, DizionarioService.METADATO_LabelSezione, documento.GetRaggruppamenti());

                if (!string.IsNullOrWhiteSpace(documento.GetArgomenti()))
                    ModelUtils.AggiungiRiga(model.InformazioniContenuto, DizionarioService.METADATO_LabelArgomenti, documento.GetArgomenti());

                if (!string.IsNullOrWhiteSpace(documento.Riferimenti))
                    ModelUtils.AggiungiRiga(model.InformazioniContenuto, DizionarioService.METADATO_LabelRiferimenti, documento.Riferimenti);

                if (!string.IsNullOrWhiteSpace(documento.Origine))
                    ModelUtils.AggiungiRiga(model.InformazioniContenuto, DizionarioService.METADATO_LabelOrigine, documento.Origine);

                if (!string.IsNullOrWhiteSpace(documento.Copertura))
                    ModelUtils.AggiungiRiga(model.InformazioniContenuto, DizionarioService.METADATO_LabelCopertura, documento.Copertura);

                // Date
                ModelUtils.AggiungiRiga(model.Date, DizionarioService.METADATO_LabelDataPubblicazione, documento.DataPubblicazione.ToString(CultureHelper.GetDateFormat()));

                ModelUtils.AggiungiRiga(model.Date, DizionarioService.METADATO_LabelDataStesura, documento.DataStesura.ToString(CultureHelper.GetDateFormat()));

                model.Widget = WidgetCorrelatoRepository.Instance.RecuperaWidgetCorrelati(model.VoceMenu.ID);

                result = View(model);
            }
            else
                result = HttpNotFound();

            return result;
        }

        [ValidateAntiForgeryToken]
        public JsonResult CercaOggettoPerViperaAiaID(string viperaAiaID, int? macroTipoOggettoID)
        {
            JsonResult result = null;
            List<int> oggettoID = new List<int>();
            oggettoID.Add(0);

            string messaggio = "";

            //if (int.TryParse(viperaAiaID, out iViperaID))
            if (viperaAiaID.Trim().Length > 0)
            {
                oggettoID = OggettoRepository.Instance.RecuperaOggettoIDPerViperaAiaID(viperaAiaID.Trim(), macroTipoOggettoID);
            }

            if (oggettoID[0] == 0)
                messaggio = DizionarioService.RICERCA_NessunRisultato;

            result = Json(new object[] { oggettoID, messaggio });

            return result;

        }


        [ExcludeFromAntiForgeryValidation]
        public JsonResult CercaOggettoPerViperaID(string viperaID, int? macroTipoOggettoID)
        {
            JsonResult result = null;
            int iViperaID = 0;
            int oggettoID = 0;
            string messaggio = "";

            if (int.TryParse(viperaID, out iViperaID))
            {
                oggettoID = OggettoRepository.Instance.RecuperaOggettoIDPerViperaID(iViperaID, macroTipoOggettoID);
            }

            if (oggettoID == 0)
                messaggio = DizionarioService.RICERCA_NessunRisultato;

            result = Json(new object[] { oggettoID, messaggio });

            return result;
        }

        public JsonResult CercaOggettoPerAiaID(string AiaID, int? macroTipoOggettoID)
        {
            JsonResult result = null;
            int oggettoID = 0;
            string messaggio = "";

            if (AiaID.Trim().Length > 0)
            {
                oggettoID = OggettoRepository.Instance.RecuperaOggettoIDPerAiaID(AiaID.Trim(), macroTipoOggettoID);
            }

            if (oggettoID == 0)
                messaggio = DizionarioService.RICERCA_NessunRisultato;

            result = Json(new object[] { oggettoID, messaggio });

            return result;
        }

    }
}

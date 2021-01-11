using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VAPortale.Filters;
using VAPortale.Models;
using VAPortale.Code;
using VALib.Domain.Entities.Contenuti;
using VALib.Domain.Repositories.UI;
using VALib.Helpers;
using VALib.Domain.Repositories.Contenuti;
using System.Text.RegularExpressions;


namespace VAPortale.Controllers
{
    [LanguageAttribute]
    public class RicercaController : Controller
    {
        //
        // GET: /Ricerca/
        public ActionResult Via()
        {
            RicercaViaModel model = new RicercaViaModel();
            model.ProcedureSelectList = ModelUtils.CreaProceduraSelectList(MacroTipoOggettoEnum.Via, true);
            model.TipologieSelectList = ModelUtils.CreaTipologiaSelectList(true);
            model.TipologieTerritorioSelectList = ModelUtils.CreaTipologiaTerritorioSelectList(true);

            model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("Via");
  
            return View(model);
        }


        [RemoveScript]
        [ExcludeFromAntiForgeryValidation]
        public ActionResult ViaVasAia(RicercaViaVasModel model, string ids)
        {
            ActionResult result = null;

            model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("ViaVas");
            
            int totale = 0;
            
            if (ModelState.IsValid || ids.Length > 0)
            {
                if (!string.IsNullOrWhiteSpace(model.Mode) && model.Mode.Equals("export", StringComparison.CurrentCultureIgnoreCase))
                {
                    byte[] data = null;
                    List<OggettoElenco> esportazione = OggettoElencoRepository.Instance.RecuperaOggettiElenco(
                                                CultureHelper.GetCurrentCultureShortName(),
                                                model.Testo ?? "",
                                                "", "", // ordinamento
                                                0,
                                                int.MaxValue,
                                                out totale,
                                                ids);

                    data = EsportazioneUtils.GeneraXlsxOggettiRicerca(esportazione, null, false);

                    if (data != null)
                        result = File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Export.xlsx");
                    else
                        result = HttpNotFound();
                }
                else
                {
                    List<OggettoElenco> oggetti = OggettoElencoRepository.Instance.RecuperaOggettiElenco(
                                                    CultureHelper.GetCurrentCultureShortName(),
                                                    model.Testo ?? "",
                                                    "", "", // ordinamento
                                                    model.IndiceInizio,
                                                    model.IndiceInizio + model.DimensionePagina,
                                                    out totale,
                                                    ids);
                    model.Oggetti = oggetti;

                    model.TotaleRisultati = totale;
                    result = View(model);
                }
            }
            else
            {
                model.TotaleRisultati = totale;
                result = View(model);
            }
            

            return result;
        }

        
        [RemoveScript]
        [ExcludeFromAntiForgeryValidation]
        public ActionResult RicercaCodice(RicercaViaVasModel model, string ids)
        {
            ActionResult result = null;

            if (ids == null)
            {
                 ids = string.Join(", ", OggettoRepository.Instance.RecuperaOggettoIDPerViperaAiaID(model.Testo, (int)MacroTipoOggettoEnum.Aia));

            }
            else {
                var regex = new Regex(@"^[0-9]+(,[0-9]+)*,?$");
                if (!regex.IsMatch (ids))
                    throw new InvalidCastException();

                
            }

            model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("ViaVas");

            int totale = 0;

            if (ModelState.IsValid || ids.Length > 0)
            {
                if (!string.IsNullOrWhiteSpace(model.Mode) && model.Mode.Equals("export", StringComparison.CurrentCultureIgnoreCase))
                {
                    byte[] data = null;
                    List<OggettoElenco> esportazione = OggettoElencoRepository.Instance.RecuperaOggettiElenco(
                                                CultureHelper.GetCurrentCultureShortName(),
                                                "",
                                                "", "", // ordinamento
                                                0,
                                                int.MaxValue,
                                                out totale,
                                                ids);

                    data = EsportazioneUtils.GeneraXlsxOggettiRicerca(esportazione, null, false);

                    if (data != null)
                        result = File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Export.xlsx");
                    else
                        result = HttpNotFound();
                }
                else
                {
                    List<OggettoElenco> oggetti = OggettoElencoRepository.Instance.RecuperaOggettiElenco(
                                                    CultureHelper.GetCurrentCultureShortName(),
                                                    "",
                                                    "", "", // ordinamento
                                                    model.IndiceInizio,
                                                    model.IndiceInizio + model.DimensionePagina,
                                                    out totale,
                                                    ids);
                    model.Oggetti = oggetti;

                    model.TotaleRisultati = totale;
                    result = View(model);
                }
            }
            else
            {
                model.TotaleRisultati = totale;
                result = View(model);
            }
            
            return result;
        }


        [RemoveScript]
        [ExcludeFromAntiForgeryValidation]
        public ActionResult ViaLibera(RicercaLiberaModel model)
        {
            ActionResult result = null;
            
            model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("ViaLibera");
        
            int totale = 0;

            if (!string.IsNullOrWhiteSpace(model.Mode) && model.Mode.Equals("export", StringComparison.CurrentCultureIgnoreCase))
            {
               
               byte[] data = null;
                if (!string.IsNullOrEmpty(model.T) && model.T.Equals("o", StringComparison.InvariantCultureIgnoreCase))
                {
                    IEnumerable<OggettoElenco> esportazione = OggettoElencoRepository.Instance.RecuperaOggettiElencoVia(
                                                null, null,
                                                CultureHelper.GetCurrentCultureShortName(),
                                                model.Testo ?? "",
                                                "", "", // ordinamento
                                                0,
                                                int.MaxValue,
                                                out totale);

                    data = EsportazioneUtils.GeneraXlsxOggettiRicerca(esportazione, MacroTipoOggettoEnum.Via, false);
                }

                if (!string.IsNullOrEmpty(model.T) && model.T.Equals("d", StringComparison.InvariantCultureIgnoreCase))
                {
                    IEnumerable<DocumentoElenco> esportazione = DocumentoElencoRepository.Instance.RecuperaDocumentiElenco((int)MacroTipoOggettoEnum.Via,
                                                    null, null,
                                                    CultureHelper.GetCurrentCultureShortName(),
                                                    model.Testo ?? "",
                                                    "", "", // ordinamento
                                                    0,
                                                    20000,
                                                    out totale);

                    data = EsportazioneUtils.GeneraXlsxDocumentiRicerca(esportazione, MacroTipoOggettoEnum.Via);
                }

                if (data != null)
                    result = File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Export.xlsx");
                else
                    result = HttpNotFound();
            }
            else
            {
                if (!string.IsNullOrEmpty(model.T) && model.T.Equals("o", StringComparison.InvariantCultureIgnoreCase))
                {
                    List<OggettoElenco> oggetti = OggettoElencoRepository.Instance.RecuperaOggettiElencoVia(
                                                    null, null,
                                                    CultureHelper.GetCurrentCultureShortName(),
                                                    model.Testo ?? "",
                                                    "", "", // ordinamento
                                                    model.IndiceInizio,
                                                    model.IndiceInizio + model.DimensionePagina,
                                                    out totale);
                    model.Oggetti = oggetti;
                }

                if (!string.IsNullOrEmpty(model.T) && model.T.Equals("d", StringComparison.InvariantCultureIgnoreCase))
                {
                    IEnumerable<DocumentoElenco> documenti = DocumentoElencoRepository.Instance.RecuperaDocumentiElenco((int)MacroTipoOggettoEnum.Via,
                                                    null, null,
                                                    CultureHelper.GetCurrentCultureShortName(),
                                                    model.Testo ?? "",
                                                    "", "", // ordinamento
                                                    model.IndiceInizio,
                                                    model.IndiceInizio + model.DimensionePagina,
                                                    out totale);
                    model.Documenti = documenti;
                }

                model.TotaleRisultati = totale;
                result = View(model);
            }

            return result;
        }

        [ExcludeFromAntiForgeryValidation]
        public ActionResult ViaProcedura(RicercaViaProceduraModel model)
        {
            ActionResult result = null;
            int totale = 0;

            if (!string.IsNullOrWhiteSpace(model.Mode) && model.Mode.Equals("export", StringComparison.CurrentCultureIgnoreCase))
            {
                byte[] data = null;

                List<OggettoElenco> esportazione = OggettoElencoRepository.Instance.RecuperaOggettiElencoVia(
                                            model.ProceduraID, null,
                                            CultureHelper.GetCurrentCultureShortName(),
                                            model.Testo ?? "",
                                            "", "", // ordinamento
                                            0,
                                            int.MaxValue,
                                            out totale);

                data = EsportazioneUtils.GeneraXlsxOggettiRicerca(esportazione, MacroTipoOggettoEnum.Via, true);

                if (data != null)
                    result = File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Export.xlsx");
                else
                    result = HttpNotFound();
            }
            else
            {
                model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("ViaProcedura");
  
                model.ProcedureSelectList = ModelUtils.CreaProceduraSelectList(MacroTipoOggettoEnum.Via, true);

                List<OggettoElenco> oggetti = OggettoElencoRepository.Instance.RecuperaOggettiElencoVia(
                                                model.ProceduraID, null,
                                                CultureHelper.GetCurrentCultureShortName(),
                                                model.Testo ?? "",
                                                "", "", // ordinamento
                                                model.IndiceInizio,
                                                model.IndiceInizio + model.DimensionePagina,
                                                out totale);
                model.Oggetti = oggetti;

                model.TotaleRisultati = totale;

                result = View(model);
            }

            return result;
        }


        [RemoveScript]
        [ExcludeFromAntiForgeryValidation]
        public ActionResult ViaTerritorio(RicercaTerritorioModel model)
        {
            ActionResult result = null;
            int totale = 0;

            if (!string.IsNullOrWhiteSpace(model.Mode) && model.Mode.Equals("export", StringComparison.CurrentCultureIgnoreCase))
            {
                byte[] data = null;

                List<OggettoElenco> esportazione = OggettoElencoRepository.Instance.RecuperaOggettiElencoTerritorio(
                                            model.MacroTipoOggettoID, model.TipologiaTerritorioID,
                                            model.Testo ?? "",
                                            "", "", // ordinamento
                                            0,
                                            int.MaxValue,
                                            out totale);

                data = EsportazioneUtils.GeneraXlsxOggettiRicerca(esportazione, MacroTipoOggettoEnum.Via, false);

                if (data != null)
                    result = File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Export.xlsx");
                else
                    result = HttpNotFound();
            }
            else
            {
                model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("ViaTerritorio");
 
                model.TipologieTerritorioSelectList = ModelUtils.CreaTipologiaTerritorioSelectList(true);

                if (model.MacroTipoOggettoID == 0) { model.MacroTipoOggettoID = 1; }

                List<OggettoElenco> oggetti = OggettoElencoRepository.Instance.RecuperaOggettiElencoTerritorio(
                                                model.MacroTipoOggettoID, model.TipologiaTerritorioID,
                                                model.Testo ?? "",
                                                "", "", // ordinamento
                                                model.IndiceInizio,
                                                model.IndiceInizio + model.DimensionePagina,
                                                out totale);
                model.Oggetti = oggetti;

                model.TotaleRisultati = totale;

                result = View(model);
            }

            return result;
        }


        [RemoveScript]
        [ExcludeFromAntiForgeryValidation]
        public ActionResult ViaTipologia(RicercaViaTipologiaModel model)
        {
            ActionResult result = null;
            int totale = 0;

            if (!string.IsNullOrWhiteSpace(model.Mode) && model.Mode.Equals("export", StringComparison.CurrentCultureIgnoreCase))
            {
                byte[] data = null;

                List<OggettoElenco> esportazione = OggettoElencoRepository.Instance.RecuperaOggettiElencoVia(
                                            null, model.TipologiaID,
                                            CultureHelper.GetCurrentCultureShortName(),
                                            model.Testo ?? "",
                                            "", "", // ordinamento
                                            0,
                                            int.MaxValue,
                                            out totale);

                data = EsportazioneUtils.GeneraXlsxOggettiRicerca(esportazione, MacroTipoOggettoEnum.Via, false);

                if (data != null)
                    result = File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Export.xlsx");
                else
                    result = HttpNotFound();
            }
            else
            {
                model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("ViaTipologia");
     
                model.TipologieSelectList = ModelUtils.CreaTipologiaSelectList(true);

                List<OggettoElenco> oggetti = OggettoElencoRepository.Instance.RecuperaOggettiElencoVia(
                                                null, model.TipologiaID,
                                                CultureHelper.GetCurrentCultureShortName(),
                                                model.Testo ?? "",
                                                "", "", // ordinamento
                                                model.IndiceInizio,
                                                model.IndiceInizio + model.DimensionePagina,
                                                out totale);
                model.Oggetti = oggetti;

                model.TotaleRisultati = totale;

                result = View(model);
            }

            return result;
        }


        [ExcludeFromAntiForgeryValidation]
        public ActionResult ViaSpaziale(RicercaViaSpazialeModel model)
        {
           

            ActionResult result = null;
            int totale = 0;

            if (!string.IsNullOrWhiteSpace(model.Mode) && model.Mode.Equals("export", StringComparison.CurrentCultureIgnoreCase))
            {
                byte[] data = null;

                List<OggettoElenco> esportazione = OggettoElencoRepository.Instance.RecuperaOggettiSpaziali(MacroTipoOggettoEnum.Via,
                                            Convert.ToDouble(model.XMax, new System.Globalization.CultureInfo("en")),
                                            Convert.ToDouble(model.YMax, new System.Globalization.CultureInfo("en")),
                                            Convert.ToDouble(model.XMin, new System.Globalization.CultureInfo("en")),
                                            Convert.ToDouble(model.YMin, new System.Globalization.CultureInfo("en")),
                                            "", "", // ordinamento
                                            0,
                                            int.MaxValue,
                                            out totale);

                data = EsportazioneUtils.GeneraXlsxOggettiRicerca(esportazione, MacroTipoOggettoEnum.Via, false);

                if (data != null)
                    result = File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Export.xlsx");
                else
                    result = HttpNotFound();
            }
            else
            {
                model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("ViaSpaziale");
      
                List<OggettoElenco> oggetti = OggettoElencoRepository.Instance.RecuperaOggettiSpaziali(MacroTipoOggettoEnum.Via,
                                            Convert.ToDouble(model.XMax, new System.Globalization.CultureInfo("en")),
                                            Convert.ToDouble(model.YMax, new System.Globalization.CultureInfo("en")),
                                            Convert.ToDouble(model.XMin, new System.Globalization.CultureInfo("en")),
                                            Convert.ToDouble(model.YMin, new System.Globalization.CultureInfo("en")),
                                            "", "", // ordinamento
                                            model.IndiceInizio,
                                            model.IndiceInizio + model.DimensionePagina,
                                            out totale);
                model.Oggetti = oggetti;

                model.TotaleRisultati = totale;

                result = View(model);
            }

            return result;
        }

        public ActionResult Vas()
        {
            RicercaVasModel model = new RicercaVasModel();
            model.ProcedureSelectList = ModelUtils.CreaProceduraSelectList(MacroTipoOggettoEnum.Vas, true);
            model.SettoriSelectList = ModelUtils.CreaSettoreSelectList(true);

            model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("Vas");
    
            return View(model);
        }


        [RemoveScript]
        [ExcludeFromAntiForgeryValidation]
        public ActionResult VasLibera(RicercaLiberaModel model)
        {
            ActionResult result = null;

            model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("VasLibera");
   
            int totale = 0;

            if (!string.IsNullOrWhiteSpace(model.Mode) && model.Mode.Equals("export", StringComparison.CurrentCultureIgnoreCase))
            {
                byte[] data = null;
                if (!string.IsNullOrEmpty(model.T) && model.T.Equals("o", StringComparison.InvariantCultureIgnoreCase))
                {
                    List<OggettoElenco> esportazione = OggettoElencoRepository.Instance.RecuperaOggettiElencoVas(
                                                null, null,
                                                CultureHelper.GetCurrentCultureShortName(),
                                                model.Testo ?? "",
                                                "", "", // ordinamento
                                                0,
                                                int.MaxValue,
                                                out totale);

                    data = EsportazioneUtils.GeneraXlsxOggettiRicerca(esportazione, MacroTipoOggettoEnum.Vas, false);
                }

                if (!string.IsNullOrEmpty(model.T) && model.T.Equals("d", StringComparison.InvariantCultureIgnoreCase))
                {
                    IEnumerable<DocumentoElenco> esportazione = DocumentoElencoRepository.Instance.RecuperaDocumentiElenco((int)MacroTipoOggettoEnum.Vas,
                                                    null, null,
                                                    CultureHelper.GetCurrentCultureShortName(),
                                                    model.Testo ?? "",
                                                    "", "", // ordinamento
                                                    0,
                                                    20000,
                                                    out totale);

                    data = EsportazioneUtils.GeneraXlsxDocumentiRicerca(esportazione, MacroTipoOggettoEnum.Vas);
                }

                if (data != null)
                    result = File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Export.xlsx");
                else
                    result = HttpNotFound();
            }
            else
            {
                if (!string.IsNullOrEmpty(model.T) && model.T.Equals("o", StringComparison.InvariantCultureIgnoreCase))
                {
                    List<OggettoElenco> oggetti = OggettoElencoRepository.Instance.RecuperaOggettiElencoVas(
                                                    null, null,
                                                    CultureHelper.GetCurrentCultureShortName(),
                                                    model.Testo ?? "",
                                                    "", "", // ordinamento
                                                    model.IndiceInizio,
                                                    model.IndiceInizio + model.DimensionePagina,
                                                    out totale);
                    model.Oggetti = oggetti;
                }

                if (!string.IsNullOrEmpty(model.T) && model.T.Equals("d", StringComparison.InvariantCultureIgnoreCase))
                {
                    IEnumerable<DocumentoElenco> documenti = DocumentoElencoRepository.Instance.RecuperaDocumentiElenco((int)MacroTipoOggettoEnum.Vas,
                                                    null, null,
                                                    CultureHelper.GetCurrentCultureShortName(),
                                                    model.Testo ?? "",
                                                    "", "", // ordinamento
                                                    model.IndiceInizio,
                                                    model.IndiceInizio + model.DimensionePagina,
                                                    out totale);
                    model.Documenti = documenti;
                }

                model.TotaleRisultati = totale;
                result = View(model);
            }

            return result;
        }


        [RemoveScript]
        [ExcludeFromAntiForgeryValidation]
        public ActionResult VasProcedura(RicercaVasProceduraModel model)
        {
            ActionResult result = null;
            int totale = 0;

            if (!string.IsNullOrWhiteSpace(model.Mode) && model.Mode.Equals("export", StringComparison.CurrentCultureIgnoreCase))
            {
                byte[] data = null;

                List<OggettoElenco> esportazione = OggettoElencoRepository.Instance.RecuperaOggettiElencoVas(
                                            model.ProceduraID, null,
                                            CultureHelper.GetCurrentCultureShortName(),
                                            model.Testo ?? "",
                                            "", "", // ordinamento
                                            0,
                                            int.MaxValue,
                                            out totale);

                data = EsportazioneUtils.GeneraXlsxOggettiRicerca(esportazione, MacroTipoOggettoEnum.Vas, true);

                if (data != null)
                    result = File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Export.xlsx");
                else
                    result = HttpNotFound();
            }
            else
            {
                model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("VasProcedura");
    
                model.ProcedureSelectList = ModelUtils.CreaProceduraSelectList(MacroTipoOggettoEnum.Vas, true);

                List<OggettoElenco> oggetti = OggettoElencoRepository.Instance.RecuperaOggettiElencoVas(
                                                model.ProceduraID, null,
                                                CultureHelper.GetCurrentCultureShortName(),
                                                model.Testo ?? "",
                                                "", "", // ordinamento
                                                model.IndiceInizio,
                                                model.IndiceInizio + model.DimensionePagina,
                                                out totale);
                model.Oggetti = oggetti;

                model.TotaleRisultati = totale;

                result = View(model);
            }

            return result;
        }


        [RemoveScript]
        [ExcludeFromAntiForgeryValidation]
        public ActionResult VasSettore(RicercaVasSettoreModel model)
        {
            ActionResult result = null;
            int totale = 0;

            if (!string.IsNullOrWhiteSpace(model.Mode) && model.Mode.Equals("export", StringComparison.CurrentCultureIgnoreCase))
            {
                byte[] data = null;

                List<OggettoElenco> esportazione = OggettoElencoRepository.Instance.RecuperaOggettiElencoVas(
                                            null, model.SettoreID,
                                            CultureHelper.GetCurrentCultureShortName(),
                                            model.Testo ?? "",
                                            "", "", // ordinamento
                                            0,
                                            int.MaxValue,
                                            out totale);

                data = EsportazioneUtils.GeneraXlsxOggettiRicerca(esportazione, MacroTipoOggettoEnum.Vas, false);

                if (data != null)
                    result = File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Export.xlsx");
                else
                    result = HttpNotFound();
            }
            else
            {
                model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("VasSettore");
    
                model.SettoriSelectList = ModelUtils.CreaSettoreSelectList(true);

                List<OggettoElenco> oggetti = OggettoElencoRepository.Instance.RecuperaOggettiElencoVas(
                                                null, model.SettoreID,
                                                CultureHelper.GetCurrentCultureShortName(),
                                                model.Testo ?? "",
                                                "", "", // ordinamento
                                                model.IndiceInizio,
                                                model.IndiceInizio + model.DimensionePagina,
                                                out totale);
                model.Oggetti = oggetti;

                model.TotaleRisultati = totale;

                result = View(model);
            }

            return result;
        }

        [ChildActionOnly]
        public ActionResult TerritoriRootNodes(List<Territorio> territori, MacroTipoOggettoEnum macroTipoOggetto)
        {
            RicercaTerritoriTreeViewModel model = new RicercaTerritoriTreeViewModel();
            model.Territori = territori;
            model.MacroTipoOggetto = macroTipoOggetto;
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult TerritoriChildNodes(List<Territorio> territori, MacroTipoOggettoEnum macroTipoOggetto, Guid? territorioID)
        {
            RicercaTerritoriTreeViewModel model = new RicercaTerritoriTreeViewModel();
            model.Territori = territori;
            model.MacroTipoOggetto = macroTipoOggetto;
            model.TerritorioID = territorioID;
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult Mappa(RicercaViaSpazialeModel m)
        {

            m.MapCenter = m.MapCenter ?? "41.69258836703085|12.6314697265625";
            m.MapZoom = m.MapZoom ?? "5";
            return PartialView(m);
        }

        public ActionResult Aia()
        {
            RicercaAiaModel model = new RicercaAiaModel();
            model.ProcedureSelectList = ModelUtils.CreaProceduraSelectList(MacroTipoOggettoEnum.Aia, true);
            model.CategorieInstallazioneSelectList = ModelUtils.CreaCategoriaSelectList(true);
            model.TipologieTerritorioSelectList = ModelUtils.CreaTipologiaTerritorioSelectList(true);

            model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("Aia");
            
            return View(model);
        }


        [RemoveScript]
        [ExcludeFromAntiForgeryValidation]
        public ActionResult AiaLibera(RicercaLiberaModel model)
        {
            ActionResult result = null;

            model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("Aia");
            
            int totale = 0;

            if (!string.IsNullOrWhiteSpace(model.Mode) && model.Mode.Equals("export", StringComparison.CurrentCultureIgnoreCase))
            {
                byte[] data = null;
                if (!string.IsNullOrEmpty(model.T) && model.T.Equals("o", StringComparison.InvariantCultureIgnoreCase))
                {
                    IEnumerable<OggettoElenco> esportazione = OggettoElencoRepository.Instance.RecuperaOggettiElencoAia(
                                                null, null,
                                                CultureHelper.GetCurrentCultureShortName(),
                                                model.Testo ?? "",
                                                "", "", // ordinamento
                                                0,
                                                int.MaxValue,
                                                out totale);

                    data = EsportazioneUtils.GeneraXlsxOggettiRicerca(esportazione, MacroTipoOggettoEnum.Aia, true);
                }

                if (!string.IsNullOrEmpty(model.T) && model.T.Equals("d", StringComparison.InvariantCultureIgnoreCase))
                {
                    IEnumerable<DocumentoElenco> esportazione = DocumentoElencoRepository.Instance.RecuperaDocumentiElenco((int)MacroTipoOggettoEnum.Aia,
                                                    null, null,
                                                    CultureHelper.GetCurrentCultureShortName(),
                                                    model.Testo ?? "",
                                                    "", "", // ordinamento
                                                    0,
                                                    20000,
                                                    out totale);

                    data = EsportazioneUtils.GeneraXlsxDocumentiRicerca(esportazione, MacroTipoOggettoEnum.Aia);
                }

                if (data != null)
                    result = File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Export.xlsx");
                else
                    result = HttpNotFound();
            }
            else
            {
                if (!string.IsNullOrEmpty(model.T) && model.T.Equals("o", StringComparison.InvariantCultureIgnoreCase))
                {
                    List<OggettoElenco> oggetti = OggettoElencoRepository.Instance.RecuperaOggettiElencoAia(
                                                    null, null,
                                                    CultureHelper.GetCurrentCultureShortName(),
                                                    model.Testo ?? "",
                                                    "", "", // ordinamento
                                                    model.IndiceInizio,
                                                    model.IndiceInizio + model.DimensionePagina,
                                                    out totale);
                    model.Oggetti = oggetti;
                }

                if (!string.IsNullOrEmpty(model.T) && model.T.Equals("d", StringComparison.InvariantCultureIgnoreCase))
                {
                    IEnumerable<DocumentoElenco> documenti = DocumentoElencoRepository.Instance.RecuperaDocumentiElenco((int)MacroTipoOggettoEnum.Aia,
                                                    null, null,
                                                    CultureHelper.GetCurrentCultureShortName(),
                                                    model.Testo ?? "",
                                                    "", "", // ordinamento
                                                    model.IndiceInizio,
                                                    model.IndiceInizio + model.DimensionePagina,
                                                    out totale);
                    model.Documenti = documenti;
                }

                model.TotaleRisultati = totale;
                result = View(model);
            }

            return result;
        }


        [RemoveScript]
        [ExcludeFromAntiForgeryValidation]
        public ActionResult AiaProcedura(RicercaViaProceduraModel model)
        {
            ActionResult result = null;
            int totale = 0;

            if (!string.IsNullOrWhiteSpace(model.Mode) && model.Mode.Equals("export", StringComparison.CurrentCultureIgnoreCase))
            {
                byte[] data = null;

                List<OggettoElenco> esportazione = OggettoElencoRepository.Instance.RecuperaOggettiElencoAia(
                                            model.ProceduraID, null,
                                            CultureHelper.GetCurrentCultureShortName(),
                                            model.Testo ?? "",
                                            "", "", // ordinamento
                                            0,
                                            int.MaxValue,
                                            out totale);

                data = EsportazioneUtils.GeneraXlsxOggettiRicerca(esportazione, MacroTipoOggettoEnum.Aia, true);

                if (data != null)
                    result = File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Export.xlsx");
                else
                    result = HttpNotFound();
            }
            else
            {

                model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("AiaProcedura");
                
                model.ProcedureSelectList = ModelUtils.CreaProceduraSelectList(MacroTipoOggettoEnum.Aia, true);

                List<OggettoElenco> oggetti = OggettoElencoRepository.Instance.RecuperaOggettiElencoAia(
                                                model.ProceduraID, null,
                                                CultureHelper.GetCurrentCultureShortName(),
                                                model.Testo ?? "",
                                                "", "", // ordinamento
                                                model.IndiceInizio,
                                                model.IndiceInizio + model.DimensionePagina,
                                                out totale);
                model.Oggetti = oggetti;

                model.TotaleRisultati = totale;

                result = View(model);
            }
            return result;
        }


        [RemoveScript]
        [ExcludeFromAntiForgeryValidation]
        public ActionResult AiaTerritorio(RicercaTerritorioModel model)
        {
            ActionResult result = null;
            int totale = 0;

            if (!string.IsNullOrWhiteSpace(model.Mode) && model.Mode.Equals("export", StringComparison.CurrentCultureIgnoreCase))
            {
                byte[] data = null;

                List<OggettoElenco> esportazione = OggettoElencoRepository.Instance.RecuperaOggettiElencoTerritorio(
                                            model.MacroTipoOggettoID, model.TipologiaTerritorioID,
                                            model.Testo ?? "",
                                            "", "", // ordinamento
                                            0,
                                            int.MaxValue,
                                            out totale);
                
                data = EsportazioneUtils.GeneraXlsxOggettiRicerca(esportazione, MacroTipoOggettoEnum.Aia, true);

                if (data != null)
                    result = File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Export.xlsx");
                else
                    result = HttpNotFound();
            }
            else
            {
                model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("AIATerritorio");
                model.TipologieTerritorioSelectList = ModelUtils.CreaTipologiaTerritorioSelectList(true);

                if (model.MacroTipoOggettoID == 0 ) { model.MacroTipoOggettoID = 3;  }

                List<OggettoElenco> oggetti = OggettoElencoRepository.Instance.RecuperaOggettiElencoTerritorio(
                                                model.MacroTipoOggettoID, model.TipologiaTerritorioID,
                                                model.Testo ?? "",
                                                "", "", // ordinamento
                                                model.IndiceInizio,
                                                model.IndiceInizio + model.DimensionePagina,
                                                out totale);
                model.Oggetti = oggetti;
                model.TotaleRisultati = totale;

                result = View(model);
            }
            return result;
        }


        [RemoveScript]
        [ExcludeFromAntiForgeryValidation]
        public ActionResult AiaInstallazione(RicercaAiaInstallazioneModel model)
        {
            ActionResult result = null;
            int totale = 0;
            if (!string.IsNullOrWhiteSpace(model.Mode) && model.Mode.Equals("export", StringComparison.CurrentCultureIgnoreCase))
            {
                byte[] data = null;

                List<OggettoElenco> esportazione = OggettoElencoRepository.Instance.RecuperaOggettiElencoAia(
                                                null, model.TipologiaID,
                                                CultureHelper.GetCurrentCultureShortName(),
                                                model.Testo ?? "",
                                                "", "", // ordinamento
                                                0,
                                                int.MaxValue,
                                                out totale);

                data = EsportazioneUtils.GeneraXlsxOggettiRicerca(esportazione, MacroTipoOggettoEnum.Aia, true);
                
                if (data != null)
                    result = File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Export.xlsx");
                else
                    result = HttpNotFound();
            }
            else
            {
                model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("AIAInstallazione");
                model.TipologieSelectList = ModelUtils.CreaCategoriaSelectList(true);

                List<OggettoElenco> oggetti = OggettoElencoRepository.Instance.RecuperaOggettiElencoAia(
                                                null, model.TipologiaID,
                                                CultureHelper.GetCurrentCultureShortName(),
                                                model.Testo ?? "",
                                                "", "", // ordinamento
                                                model.IndiceInizio,
                                                model.IndiceInizio + model.DimensionePagina,
                                                out totale);
                model.Oggetti = oggetti;

                model.TotaleRisultati = totale;

                result = View(model);
            }

            return result;
        }
    }
}

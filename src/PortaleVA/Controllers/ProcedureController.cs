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
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace VAPortale.Controllers
{
    [LanguageAttribute]
    public class ProcedureController : Controller
    {
        //
        // GET: /Procedure/

        public ActionResult ProcedureInCorso()
        {
            ProcedureViaVasInCorsoModel model = new ProcedureViaVasInCorsoModel();

            model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("ProcedureInCorso");
           
            IEnumerable<ConteggioProcedura> righe = ConteggioProceduraRepository.Instance.RecuperaConteggiProcedure(MacroTipoOggettoEnum.Via, false);
            int totale = 0;

            foreach (ConteggioProcedura cp in righe)
            {
                totale += cp.Conteggio;
            }

            model.RigheVia = righe;
            model.TotaleVia = totale;

            /**************************************************************************************************************/
            /**************************************************************************************************************/
            // Recupero proceure VAS
            IEnumerable<ConteggioProcedura> righeVas = ConteggioProceduraRepository.Instance.RecuperaConteggiProcedure(MacroTipoOggettoEnum.Vas, false);
            totale = 0;

            foreach (ConteggioProcedura cp in righeVas)
            {
                totale += cp.Conteggio;
            }

            model.RigheVas = righeVas;
            model.TotaleVas = totale;

            /**************************************************************************************************************/
            /**************************************************************************************************************/
            // Recupero proceure AIA
            IEnumerable<ConteggioProcedura> righeAIA = ConteggioProceduraRepository.Instance.RecuperaConteggiProcedure(MacroTipoOggettoEnum.Aia, false);
            totale = 0;

            foreach (ConteggioProcedura cp in righeAIA)
            {
                totale += cp.Conteggio;
            }

            model.RigheAia = righeAIA;
            model.TotaleAia = totale;

            return View(model);
        }

        [ExcludeFromAntiForgeryValidation]
        public ActionResult VasInCorso()
        {
            ProcedureInCorsoModel model = new ProcedureInCorsoModel();

            model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("VasInCorso");
       
            IEnumerable<ConteggioProcedura> righe = ConteggioProceduraRepository.Instance.RecuperaConteggiProcedure(MacroTipoOggettoEnum.Vas, false);
            int totale = 0;

            foreach (ConteggioProcedura cp in righe)
            {
                totale += cp.Conteggio;
            }

            model.Righe = righe;
            model.Totale = totale;

            return View(model);
        }

        [ExcludeFromAntiForgeryValidation]
        public ActionResult ViaInCorso()
        {
            ProcedureInCorsoModel model = new ProcedureInCorsoModel();

            model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("ViaInCorso");
  
            IEnumerable<ConteggioProcedura> righe = ConteggioProceduraRepository.Instance.RecuperaConteggiProcedure(MacroTipoOggettoEnum.Via, false);
            int totale = 0;

            foreach (ConteggioProcedura cp in righe)
            {
                totale += cp.Conteggio;
            }

            model.Righe = righe;
            model.Totale = totale;

            return View(model);
        }

        [ExcludeFromAntiForgeryValidation]
        public ActionResult AiaInCorso()
        {
            ProcedureInCorsoModel model = new ProcedureInCorsoModel();

            model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("AiaInCorso");

            IEnumerable<ConteggioProcedura> righe = ConteggioProceduraRepository.Instance.RecuperaConteggiProcedure(MacroTipoOggettoEnum.Aia, false);
            int totale = 0;

            foreach (ConteggioProcedura cp in righe)
            {
                totale += cp.Conteggio;
            }

            model.Righe = righe;
            model.Totale = totale;

            return View(model);
        }


        [RemoveScript]
        [ExcludeFromAntiForgeryValidation]
        public ActionResult ViaElenco(ProcedureElencoModel model)
        {
            ActionResult result = null;
            int totale = 0;

            if (!string.IsNullOrWhiteSpace(model.Mode) && model.Mode.Equals("export", StringComparison.CurrentCultureIgnoreCase))
            {
                byte[] data = null;

                List<OggettoElencoProcedura> esportazione = OggettoElencoRepository.Instance.RecuperaOggettiElencoProcedura(MacroTipoOggettoEnum.Via,
                                                                                                                model.Parametro,
                                                                                                                CultureHelper.GetCurrentCultureShortName(),
                                                                                                                model.Testo ?? "",
                                                                                                                "", "", //Ordinamento
                                                                                                                0,
                                                                                                                int.MaxValue,
                                                                                                                out totale);

                data = EsportazioneUtils.GeneraXlsxProcedureInCorso(esportazione, MacroTipoOggettoEnum.Via);

                if (data != null)
                    result = File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Export.xlsx");
                else
                    result = HttpNotFound();
            }
            else
            {
                model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("ViaElenco");
               
                List<OggettoElencoProcedura> oggetti = OggettoElencoRepository.Instance.RecuperaOggettiElencoProcedura(MacroTipoOggettoEnum.Via,
                                                                                                                model.Parametro,
                                                                                                                CultureHelper.GetCurrentCultureShortName(),
                                                                                                                model.Testo ?? "",
                                                                                                                "", "", //Ordinamento
                                                                                                                model.IndiceInizio,
                                                                                                                model.IndiceInizio + model.DimensionePagina,
                                                                                                                out totale);
                model.Procedura = ProceduraRepository.Instance.RecuperaProcedura(model.Id);
                model.TotaleRisultati = totale;
                model.Oggetti = oggetti;

                result = View(model);
            }

            return result;
        }

        
        [RemoveScript]
        [ExcludeFromAntiForgeryValidation]
        public ActionResult VasElenco(ProcedureElencoModel model)
        {
            ActionResult result = null;
            int totale = 0;

            if (!string.IsNullOrWhiteSpace(model.Mode) && model.Mode.Equals("export", StringComparison.CurrentCultureIgnoreCase))
            {
                byte[] data = null;

                List<OggettoElencoProcedura> esportazione = OggettoElencoRepository.Instance.RecuperaOggettiElencoProcedura(MacroTipoOggettoEnum.Vas,
                                                                                                                model.Parametro,
                                                                                                                CultureHelper.GetCurrentCultureShortName(),
                                                                                                                model.Testo ?? "",
                                                                                                                "", "", //Ordinamento
                                                                                                                0,
                                                                                                                int.MaxValue,
                                                                                                                out totale);

                data = EsportazioneUtils.GeneraXlsxProcedureInCorso(esportazione, MacroTipoOggettoEnum.Vas);

                if (data != null)
                    result = File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Export.xlsx");
                else
                    result = HttpNotFound();
            }
            else
            {
                model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("VasElenco");
          
                List<OggettoElencoProcedura> oggetti = OggettoElencoRepository.Instance.RecuperaOggettiElencoProcedura(MacroTipoOggettoEnum.Vas,
                                                                                                                model.Parametro,
                                                                                                                CultureHelper.GetCurrentCultureShortName(),
                                                                                                                model.Testo ?? "",
                                                                                                                "", "", //Ordinamento
                                                                                                                model.IndiceInizio,
                                                                                                                model.IndiceInizio + model.DimensionePagina,
                                                                                                                out totale);
                model.Procedura = ProceduraRepository.Instance.RecuperaProcedura(model.Id);
                model.TotaleRisultati = totale;
                model.Oggetti = oggetti;
                result = View(model);
            }

            return result;
        }



        [RemoveScript]
        [ExcludeFromAntiForgeryValidation]
        public ActionResult AiaElenco(ProcedureElencoModel model)
        {
            ActionResult result = null;
            int totale = 0;

            if (!string.IsNullOrWhiteSpace(model.Mode) && model.Mode.Equals("export", StringComparison.CurrentCultureIgnoreCase))
            {
                byte[] data = null;

                List<OggettoElencoProcedura> esportazione = OggettoElencoRepository.Instance.RecuperaOggettiElencoProcedura(MacroTipoOggettoEnum.Aia,
                                                                                                                model.Parametro,
                                                                                                                CultureHelper.GetCurrentCultureShortName(),
                                                                                                                model.Testo ?? "",
                                                                                                                "", "", //Ordinamento
                                                                                                                0,
                                                                                                                int.MaxValue,
                                                                                                                out totale);

                data = EsportazioneUtils.GeneraXlsxProcedureInCorso(esportazione, MacroTipoOggettoEnum.Aia);

                if (data != null)
                    result = File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Export.xlsx");
                else
                    result = HttpNotFound();
            }
            else
            {
                model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("AiaElenco");

                List<OggettoElencoProcedura> oggetti = OggettoElencoRepository.Instance.RecuperaOggettiElencoProcedura(MacroTipoOggettoEnum.Aia,
                                                                                                                model.Parametro,
                                                                                                                CultureHelper.GetCurrentCultureShortName(),
                                                                                                                model.Testo ?? "",
                                                                                                                "", "", //Ordinamento
                                                                                                                model.IndiceInizio,
                                                                                                                model.IndiceInizio + model.DimensionePagina,
                                                                                                                out totale);
                model.Procedura = ProceduraRepository.Instance.RecuperaProcedura(model.Id);
                model.TotaleRisultati = totale;
                model.Oggetti = oggetti;
                result = View(model);
            }

            return result;
        }


        [ChildActionOnly]
        public ActionResult PartialTab(int attributoID, string voce)
        {
            ProcedureAttributiPartialTabModel model = new ProcedureAttributiPartialTabModel();

            Attributo attributo = AttributoRepository.Instance.RecuperaAttributo(attributoID);

            model.Attributo = attributo;
            model.Voce = voce;
            model.Attributi = AttributoRepository.Instance.RecuperaAttributi(attributo.TipoAttributo.ID);

            return PartialView(model);
        }

       
        [RemoveScript]
        [ExcludeFromAntiForgeryValidation]
        public ActionResult ConsultazioniTransfrontaliere(ProcedureAttributiModel model)
        {
            ActionResult result = null;

            model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("ConsultazioniTransfrontaliere");

            if (model.VoceMenu != null)
            {
                IEnumerable<Attributo> attributi = AttributoRepository.Instance.RecuperaAttributi();
                List<OggettoElenco> oggetti = new List<OggettoElenco>();

                foreach (Attributo attributo in attributi)
                {
                    if (attributo.GetNome(CultureHelper._it).Equals(model.NomeAttributo, StringComparison.InvariantCultureIgnoreCase) && attributo.TipoAttributo.ID == 1)
                    {
                        model.Attributo = attributo;
                        break;
                    }
                }

                if (model.Attributo != null)
                {
                    int totale = 0;

                    if (!string.IsNullOrWhiteSpace(model.Mode) && model.Mode.Equals("export", StringComparison.CurrentCultureIgnoreCase))
                    {
                        byte[] data = null;

                        if (model.Attributo.MacroTipoOggetto.Enum == MacroTipoOggettoEnum.Via)
                        {
                            oggetti = OggettoElencoRepository.Instance.RecuperaOggettiElencoVia(
                                                        null, null, model.Attributo.ID,
                                                        CultureHelper.GetCurrentCultureShortName(),
                                                        model.Testo ?? "",
                                                        "", "", // ordinamento
                                                        0,
                                                        int.MaxValue,
                                                        out totale);
                        }
                        else if (model.Attributo.MacroTipoOggetto.Enum == MacroTipoOggettoEnum.Vas)
                        {
                            oggetti = OggettoElencoRepository.Instance.RecuperaOggettiElencoVas(
                                                        null, null, model.Attributo.ID,
                                                        CultureHelper.GetCurrentCultureShortName(),
                                                        model.Testo ?? "",
                                                        "", "", // ordinamento
                                                        0,
                                                        int.MaxValue,
                                                        out totale);
                        }
                        else if (model.Attributo.MacroTipoOggetto.Enum == MacroTipoOggettoEnum.Aia)
                        {
                            oggetti = OggettoElencoRepository.Instance.RecuperaOggettiElencoAia(
                                                        null, null, model.Attributo.ID,
                                                        CultureHelper.GetCurrentCultureShortName(),
                                                        model.Testo ?? "",
                                                        "", "", // ordinamento
                                                        0,
                                                        int.MaxValue,
                                                        out totale);
                        }
                        data = EsportazioneUtils.GeneraXlsxOggettiRicerca(oggetti, model.Attributo.MacroTipoOggetto.Enum, true);

                        if (data != null)
                            result = File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Export.xlsx");
                        else
                            result = HttpNotFound();
                    }
                    else
                    {
                      
                        if (model.Attributo.MacroTipoOggetto.Enum == MacroTipoOggettoEnum.Via)
                        {
                            model.TitoloGriglia = DizionarioService.RICERCA_TitoloRisultatiOggettiVia;
                            model.NomeColonnaOggetto = DizionarioService.GRIGLIA_ColonnaOggettoVia;

                            oggetti = OggettoElencoRepository.Instance.RecuperaOggettiElencoVia(
                                                            null, null, model.Attributo.ID,
                                                            CultureHelper.GetCurrentCultureShortName(),
                                                            model.Testo ?? "",
                                                            "", "", // ordinamento
                                                            model.IndiceInizio,
                                                            model.IndiceInizio + model.DimensionePagina,
                                                            out totale);
                        }
                        else if (model.Attributo.MacroTipoOggetto.Enum == MacroTipoOggettoEnum.Vas)
                        {
                            model.TitoloGriglia = DizionarioService.RICERCA_TitoloRisultatiOggettiVas;
                            model.NomeColonnaOggetto = DizionarioService.GRIGLIA_ColonnaOggettoVas;

                            oggetti = OggettoElencoRepository.Instance.RecuperaOggettiElencoVas(
                                                            null, null, model.Attributo.ID,
                                                            CultureHelper.GetCurrentCultureShortName(),
                                                            model.Testo ?? "",
                                                            "", "", // ordinamento
                                                            model.IndiceInizio,
                                                            model.IndiceInizio + model.DimensionePagina,
                                                            out totale);
                        }

                        else if (model.Attributo.MacroTipoOggetto.Enum == MacroTipoOggettoEnum.Aia)
                        {
                            model.TitoloGriglia = DizionarioService.RICERCA_LabelSceltaOggettiAia;
                            model.NomeColonnaOggetto = DizionarioService.GRIGLIA_ColonnaOggettoAia;

                            oggetti = OggettoElencoRepository.Instance.RecuperaOggettiElencoAia(
                                                            null, null, model.Attributo.ID,
                                                            CultureHelper.GetCurrentCultureShortName(),
                                                            model.Testo ?? "",
                                                            "", "", // ordinamento
                                                            model.IndiceInizio,
                                                            model.IndiceInizio + model.DimensionePagina,
                                                            out totale);
                        }


                        model.Oggetti = oggetti;

                        model.TotaleRisultati = totale;

                        result = View(model);
                    }

                }
                else
                    result = HttpNotFound();
            }
            else
                result = HttpNotFound();

            return result;
        }


        [RemoveScript]
        [ExcludeFromAntiForgeryValidation]
        public ActionResult ProcedureIntegrateECoordinate(ProcedureAttributiModel model)
        {
            ActionResult result = null;

            model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("ProcedureIntegrateECoordinate");

            if (model.VoceMenu != null)
            {
                IEnumerable<Attributo> attributi = AttributoRepository.Instance.RecuperaAttributi();
                List<OggettoElenco> oggetti = new List<OggettoElenco>();

                foreach (Attributo attributo in attributi)
                {
                    if (attributo.GetNome(CultureHelper._it).Equals(model.NomeAttributo, StringComparison.InvariantCultureIgnoreCase) && attributo.TipoAttributo.ID == 2)
                    {
                        model.Attributo = attributo;
                        break;
                    }
                }

                if (model.Attributo != null)
                {
                    int totale = 0;

                    if (!string.IsNullOrWhiteSpace(model.Mode) && model.Mode.Equals("export", StringComparison.CurrentCultureIgnoreCase))
                    {
                        byte[] data = null;

                        if (model.Attributo.MacroTipoOggetto.Enum == MacroTipoOggettoEnum.Via)
                        {
                            oggetti = OggettoElencoRepository.Instance.RecuperaOggettiElencoVia(
                                                        null, null, model.Attributo.ID,
                                                        CultureHelper.GetCurrentCultureShortName(),
                                                        model.Testo ?? "",
                                                        "", "", // ordinamento
                                                        0,
                                                        int.MaxValue,
                                                        out totale);
                        }
                        else if (model.Attributo.MacroTipoOggetto.Enum == MacroTipoOggettoEnum.Vas)
                        {
                            oggetti = OggettoElencoRepository.Instance.RecuperaOggettiElencoVas(
                                                        null, null, model.Attributo.ID,
                                                        CultureHelper.GetCurrentCultureShortName(),
                                                        model.Testo ?? "",
                                                        "", "", // ordinamento
                                                        0,
                                                        int.MaxValue,
                                                        out totale);
                        }

                        data = EsportazioneUtils.GeneraXlsxOggettiRicerca(oggetti, model.Attributo.MacroTipoOggetto.Enum, true);

                        if (data != null)
                            result = File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Export.xlsx");
                        else
                            result = HttpNotFound();
                    }
                    else
                    {
                       
                        if (model.Attributo.MacroTipoOggetto.Enum == MacroTipoOggettoEnum.Via)
                        {
                            model.TitoloGriglia = DizionarioService.RICERCA_TitoloRisultatiOggettiVia;
                            model.NomeColonnaOggetto = DizionarioService.GRIGLIA_ColonnaOggettoVia;

                            oggetti = OggettoElencoRepository.Instance.RecuperaOggettiElencoVia(
                                                            null, null, model.Attributo.ID,
                                                            CultureHelper.GetCurrentCultureShortName(),
                                                            model.Testo ?? "",
                                                            "", "", // ordinamento
                                                            model.IndiceInizio,
                                                            model.IndiceInizio + model.DimensionePagina,
                                                            out totale);
                        }
                        else if (model.Attributo.MacroTipoOggetto.Enum == MacroTipoOggettoEnum.Vas)
                        {
                            model.TitoloGriglia = DizionarioService.RICERCA_TitoloRisultatiOggettiVas;
                            model.NomeColonnaOggetto = DizionarioService.GRIGLIA_ColonnaOggettoVas;

                            oggetti = OggettoElencoRepository.Instance.RecuperaOggettiElencoVas(
                                                            null, null, model.Attributo.ID,
                                                            CultureHelper.GetCurrentCultureShortName(),
                                                            model.Testo ?? "",
                                                            "", "", // ordinamento
                                                            model.IndiceInizio,
                                                            model.IndiceInizio + model.DimensionePagina,
                                                            out totale);
                        }


                        model.Oggetti = oggetti;

                        model.TotaleRisultati = totale;

                        result = View(model);
                    }

                }
                else
                    result = HttpNotFound();
            }
            else
                result = HttpNotFound();

            return result;
        }

        public ActionResult Provvedimenti()
        {
            ProcedureProvvedimentiModel model = new ProcedureProvvedimentiModel();
            model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("Provvedimenti");
           
            IEnumerable<ConteggioTipoProvvedimento> righe = ConteggioTipoProvvedimentoRepository.Instance.RecuperaConteggiTipiProvvedimenti();

            foreach (AreaTipoProvvedimento area in righe.Select(x => x.TipoProvvedimento.Area).Distinct().OrderBy(x => x.Ordine))
            {
                model.Tabelle.Add(new TabellaConteggioProvvedimenti() { Area = area, Righe = righe.Where(x => x.TipoProvvedimento.Area.ID == area.ID).ToList() });
            }

            return View(model);
        }

        

        public ActionResult ProvvedimentiElenco(ProcedureProvvedimentiElencoModel model)
        {
            ActionResult result = null;
            model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu(model.VoceMenuID);
            model.TipoProvvedimento = TipoProvvedimentoRepository.Instance.RecuperaTipoProvvedimentoPerVoceMenu(model.VoceMenuID);
         
            if (model.TipoProvvedimento != null)
            {
                DateTime? dataDa = null;
                DateTime? dataA = null;

                dataDa = Parse.ToDateTimeOrNull(model.DataDa, "dd-MM-yyyy");
                dataA = Parse.ToDateTimeOrNull(model.DataA, "dd-MM-yyyy");

                model.Testo = model.Testo != null ? Regex.Replace(model.Testo, @"[^0-9a-zèòàùì ]+", "") : "";

                int totale = 0;
         
                model.Risorse = ProvvedimentoRepository.Instance.RecuperaProvvedimenti(CultureHelper.GetCurrentCultureShortName(),
                                model.Testo,
                                dataDa, dataA, model.TipoProvvedimento.ID,
                                model.IndiceInizio,
                                model.IndiceInizio + model.DimensionePagina,
                                out totale);

                model.TotaleRisultati = totale;

                result = View("ProvvedimentiElenco", model);
            }
            else
            {
                result = HttpNotFound();
            }

            return result;
        }


        
        [ExcludeFromAntiForgeryValidation]
        public ActionResult Provvedimenti2(ProcedureProvvedimentiElencoModel model)
        {
            model.VoceMenuID = 9;
            model.Azione = "Provvedimenti2";
            return ProvvedimentiElenco(model);
        }

        
         
        [ExcludeFromAntiForgeryValidation]
        public ActionResult Provvedimenti3(ProcedureProvvedimentiElencoModel model)
        {
            model.VoceMenuID = 10;
            model.Azione = "Provvedimenti3";
            return ProvvedimentiElenco(model);
        }



        
        [ExcludeFromAntiForgeryValidation]
        public ActionResult Provvedimenti4(ProcedureProvvedimentiElencoModel model)
        {
            model.VoceMenuID = 11;
            model.Azione = "Provvedimenti4";
            return ProvvedimentiElenco(model);
        }



         
        [ExcludeFromAntiForgeryValidation]
        public ActionResult Provvedimenti5(ProcedureProvvedimentiElencoModel model)
        {
            model.VoceMenuID = 12;
            model.Azione = "Provvedimenti5";
            return ProvvedimentiElenco(model);
        }

                 
        [ExcludeFromAntiForgeryValidation]
        public ActionResult Provvedimenti6(ProcedureProvvedimentiElencoModel model)
        {
            model.VoceMenuID = 13;
            model.Azione = "Provvedimenti6";
            return ProvvedimentiElenco(model);
        }


        [ExcludeFromAntiForgeryValidation]
        public ActionResult Provvedimenti7(ProcedureProvvedimentiElencoModel model)
        {
            model.VoceMenuID = 14;
            model.Azione = "Provvedimenti7";
            return ProvvedimentiElenco(model);
        }

        [ExcludeFromAntiForgeryValidation]
        public ActionResult Provvedimenti8(ProcedureProvvedimentiElencoModel model)
        {
            model.VoceMenuID = 15;
            model.Azione = "Provvedimenti8";
            return ProvvedimentiElenco(model);
        }

        [ExcludeFromAntiForgeryValidation]
        public ActionResult Provvedimenti9(ProcedureProvvedimentiElencoModel model)
        {
            model.VoceMenuID = 16;
            model.Azione = "Provvedimenti9";
            return ProvvedimentiElenco(model);
        }

        [ExcludeFromAntiForgeryValidation]
        public ActionResult Provvedimenti10(ProcedureProvvedimentiElencoModel model)
        {
            model.VoceMenuID = 17;
            model.Azione = "Provvedimenti10";
            return ProvvedimentiElenco(model);
        }

        [ExcludeFromAntiForgeryValidation]
        public ActionResult Provvedimenti11(ProcedureProvvedimentiElencoModel model)
        {
            model.VoceMenuID = 88;
            model.Azione = "Provvedimenti11";
            return ProvvedimentiElenco(model);
        }

        [ExcludeFromAntiForgeryValidation]
        // Provvedimenti AIA
        public ActionResult Provvedimenti12(ProcedureProvvedimentiElencoModel model)
        {
            model.VoceMenuID = 102;
            model.Azione = "Provvedimenti12";
            return ProvvedimentiElenco(model);
        }

        [ExcludeFromAntiForgeryValidation]
        public ActionResult Provvedimenti13(ProcedureProvvedimentiElencoModel model)
        {
            model.VoceMenuID = 103;
            model.Azione = "Provvedimenti13";
            return ProvvedimentiElenco(model);
        }

        [ExcludeFromAntiForgeryValidation]
        public ActionResult Provvedimenti14(ProcedureProvvedimentiElencoModel model)
        {
            model.VoceMenuID = 104;
            model.Azione = "Provvedimenti14";
            return ProvvedimentiElenco(model);
        }

        [ExcludeFromAntiForgeryValidation]
        public ActionResult Provvedimenti15(ProcedureProvvedimentiElencoModel model)
        {
            model.VoceMenuID = 105;
            model.Azione = "Provvedimenti15";
            return ProvvedimentiElenco(model);
        }

        [ExcludeFromAntiForgeryValidation]
        public ActionResult Provvedimenti16(ProcedureProvvedimentiElencoModel model)
        {
            model.VoceMenuID = 106;
            model.Azione = "Provvedimenti16";
            return ProvvedimentiElenco(model);
        }

        [ExcludeFromAntiForgeryValidation]
        public ActionResult Provvedimenti17(ProcedureProvvedimentiElencoModel model)
        {
            model.VoceMenuID = 107;
            model.Azione = "Provvedimenti17";
            return ProvvedimentiElenco(model);
        }

        [ExcludeFromAntiForgeryValidation]
        public ActionResult Provvedimenti18(ProcedureProvvedimentiElencoModel model)
        {
            model.VoceMenuID = 108;
            model.Azione = "Provvedimenti18";
            return ProvvedimentiElenco(model);
        }


        public ActionResult Osservatori(string mode)
        {
            ActionResult result = null;
            int totale = 0;

            if (!string.IsNullOrWhiteSpace(mode) && mode.Equals("export", StringComparison.CurrentCultureIgnoreCase))
            {
                byte[] data = null;

                List<OggettoElenco> esportazione = OggettoElencoRepository.Instance.RecuperaOggettiElencoVia(
                                            11, null,
                                            CultureHelper.GetCurrentCultureShortName(),
                                            "",
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
                ProcedureOsservatoriModel model = new ProcedureOsservatoriModel();

                VoceMenu voce = VoceMenuRepository.Instance.RecuperaVoceMenu("Osservatori");

                List<OggettoElenco> oggetti = new List<OggettoElenco>();
                List<Tuple<OggettoElenco, LinkCollegato>> elenco = new List<Tuple<OggettoElenco, LinkCollegato>>();

                oggetti = OggettoElencoRepository.Instance.RecuperaOggettiElencoVia(
                                                11, null,
                                                CultureHelper.GetCurrentCultureShortName(),
                                                "",
                                                "", "", // ordinamento
                                                0,
                                                50,
                                                out totale);

                foreach (OggettoElenco o in oggetti)
                {
                    LinkCollegato lc = LinkCollegatoRepository.Instance.RecuperaLinkCollegatiPerOggetto(o.ID, TipoLinkEnum.SitoWebInteresse);
                    elenco.Add(new Tuple<OggettoElenco, LinkCollegato>(o, lc));
                }

                model.Oggetti = elenco;
                model.VoceMenu = voce;

                result = View(model);
            }

            return result;
        }



        [ExcludeFromAntiForgeryValidation]
        public ActionResult AvvisiAlPubblico(ProcedureAvvisiAlPubblicoModel model)
        {

            model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("AvvisiAlPubblico");

            if (model.VoceMenu == null) return HttpNotFound();

            model.Attributo = AttributoRepository.Instance.RecuperaAttributi(3)
                .FirstOrDefault(x => x.GetNome(CultureHelper._it).Equals(model.NomeAttributo, StringComparison.InvariantCultureIgnoreCase));

            if (model.Attributo == null) return HttpNotFound();

            int totale = 0;
            Dictionary<int, int> Raggruppamenti = new Dictionary<int, int>() { { 8, 141 }, { 9, 248 } };
            Dictionary<int, bool> FiltroData = new Dictionary<int, bool>() { { 8, true }, { 9, false } };
            Dictionary<int, int> VociMenuID = new Dictionary<int, int>() { { 8, 90 }, { 9, 80 } };

            bool XLS = !string.IsNullOrWhiteSpace(model.Mode) && model.Mode.Equals("export", StringComparison.CurrentCultureIgnoreCase);

            IEnumerable<DocumentoElenco> documenti = DocumentoElencoRepository.Instance.RecuperaDocumentiElenco((int)MacroTipoOggettoEnum.Via,
                        null, Raggruppamenti[model.Attributo.ID],
                        CultureHelper.GetCurrentCultureShortName(),
                        model.Testo ?? "",
                        XLS ? "D.DataPubblicazione" : "D.DataStesura", "DESC", 
                        XLS ? 0 : model.IndiceInizio,
                        XLS ? int.MaxValue : model.IndiceInizio + model.DimensionePagina,
                        out totale,
                        FiltroData[model.Attributo.ID]);

            if (XLS)
            {
                byte[] data = EsportazioneUtils.GeneraXlsxDocumentiRicerca(documenti, MacroTipoOggettoEnum.Via);
                if (data == null) return HttpNotFound();

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Export.xlsx");
            }
            else
            {
                model.Documenti = documenti;
                model.TotaleRisultati = totale;
                model.PaginaStatica = PaginaStaticaRepository.Instance.RecuperaPaginaStatica(VociMenuID[model.Attributo.ID]);

                return View(model);
            }

        }

      
        public ActionResult Statistiche(ProcedureStatisticheModel model)
        {
            ActionResult result = null;

            if (model.Anno == null)
                result = RedirectToActionPermanent("Statistiche", "Procedure", new { anno = DateTime.Now.Year });
            else
            {
                model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("Statistiche");
                model.AnnoCorrente = (model.Anno.Value == DateTime.Now.Year);

                model.AnniSelectList = ModelUtils.CreaAnnoSelectList(2015, false);

                IEnumerable<StatisticheProcedura> righe = StatisticheProceduraRepository.Instance.RecuperaStatisticheProcedure(model.Anno.Value);

                foreach (AmbitoProcedura ambitoProcedura in righe.Select(x => x.Procedura.AmbitoProcedura).Distinct())
                {
                    model.Tabelle.Add(new TabellaStatisticheProcedure() { AmbitoProcedura = ambitoProcedura, Righe = righe.Where(x => x.Procedura.AmbitoProcedura.ID == ambitoProcedura.ID).ToList() });
                }

                result = View(model);
            }

            return result;
        }

        public ActionResult Report(ProcedureReportModel model)
        {
            ActionResult result = null;
            bool valid = false;
            MacroTipoOggettoEnum macroTipoOggetto = (MacroTipoOggettoEnum)model.Mto;
            Procedura procedura = null;
            byte[] data = null;

            if (model.Anno > 0 && model.Mto > 0 && model.ProceduraID > 0 && model.Tipo > 0)
            {
                if (Enum.IsDefined(typeof(MacroTipoOggettoEnum), model.Mto))
                    valid = true;

                if (valid && model.Anno >= 2015)
                    valid = true;
                else
                    valid = false;

                if (valid)
                    procedura = ProceduraRepository.Instance.RecuperaProcedura(model.ProceduraID);

                if (procedura != null)
                    valid = true;
                else
                    valid = false;

                if (valid && model.Tipo > 0 && model.Tipo < 4)
                    valid = true;
                else
                    valid = false;
            }

            if (valid)
            {
                IEnumerable<ReportProcedura> righe = ReportProceduraRepository.Instance.RecuperaReportProcedure(model.Anno, model.ProceduraID, model.Tipo == 1, model.Tipo == 2, model.Tipo == 3);

                if (righe != null)
                    data = CsvUtils.GeneraCsvFileBytesperReportProcedura(righe, macroTipoOggetto, procedura.AmbitoProcedura, model.Tipo);

                if (data != null)
                    result = File(data, "text/csv", "Report.csv");
            }

            return result;
        }

        public ActionResult Grafici()
        {
            ProcedureGraficiModel model = new ProcedureGraficiModel();
            model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("Grafici");
     
            ActionResult result = HttpNotFound();
            bool actionAbilitata = false;
            string variabile = null;
            try
            {
                variabile = VariabileService.GetValore("PaginaGraficiAbilitata");
            }
            catch
            {

            }
            if (!string.IsNullOrWhiteSpace(variabile))
            {
                if (bool.TryParse(variabile, out actionAbilitata))
                {
                    if (actionAbilitata)
                    {
                        result = View(model);
                    }
                }
            }

            return result;
        }

        public ContentResult GetDataBarChart()
        {
            ReportTipologia reportTipologiaTotali = ReportRepository.Instance.RecuperaReportTipologia();

            GoogleChartTable table = new GoogleChartTable();

            table.Cols.Add(new GoogleChartColumn() { Label = "", Type = "string", Pattern = "" });
            table.Cols.Add(new GoogleChartColumn() { Label = reportTipologiaTotali.TipoProvvedimento[1].GetNome(), Type = "number", Pattern = "" });
            table.Cols.Add(new GoogleChartColumn() { Label = reportTipologiaTotali.TipoProvvedimento[2].GetNome(), Type = "number", Pattern = "" });
            table.Cols.Add(new GoogleChartColumn() { Label = reportTipologiaTotali.TipoProvvedimento[9].GetNome(), Type = "number", Pattern = "" });

            foreach (ReportTipologiaItem rtp in reportTipologiaTotali.RtpItem)
            {
                GoogleChartRow row = new GoogleChartRow();
                row.Cells.Add(new GoogleChartCell() { Value = rtp.Tipologia.GetNome() });

                int count = rtp.TotaliTipi.Count;
                for (int index = 0; index < count; index++)
                {
                    var item = rtp.TotaliTipi.ElementAt(index);
                    row.Cells.Add(new GoogleChartCell() { Value = item.Value });
                }
                table.Rows.Add(row);
            }
            string json = JsonConvert.SerializeObject(table);
            return Content(json, "application/json");
        }

        public ContentResult GetDataColumnChart()
        {

            GoogleChartTable table = new GoogleChartTable();
            ReportTipoProvvedimento reportTipoProvvedimentoTotali = ReportRepository.Instance.RecuperaReportTipoProvvedimento();

            table.Cols.Add(new GoogleChartColumn() { Label = "Anni", Type = "string", Pattern = "" });
            table.Cols.Add(new GoogleChartColumn() { Label = reportTipoProvvedimentoTotali.TipoProvvedimento[1].GetNome(), Type = "number", Pattern = "" });
            table.Cols.Add(new GoogleChartColumn() { Label = reportTipoProvvedimentoTotali.TipoProvvedimento[2].GetNome(), Type = "number", Pattern = "" });
            table.Cols.Add(new GoogleChartColumn() { Label = reportTipoProvvedimentoTotali.TipoProvvedimento[9].GetNome(), Type = "number", Pattern = "" });
            //"Provvedimenti VIA" "Provvedimenti di Verifica di Assoggettabilità alla VIA" "Pareri VIA (Legge Obiettivo 443/2001) trasmessi al Ministero delle Infrastrutture e dei Trasporti"
            List<GoogleChartCell> cells = new List<GoogleChartCell>();

             foreach (ReportTipoProvvedimentoItem rtp in reportTipoProvvedimentoTotali.RtpItem)
            {
                GoogleChartRow row = new GoogleChartRow();
                row.Cells.Add(new GoogleChartCell() { Value = rtp.Anno.ToString() });

                int count = rtp.TotaliTipi.Count;
                for (int index = 0; index < count; index++)
                {
                    var item = rtp.TotaliTipi.ElementAt(index);
                    row.Cells.Add(new GoogleChartCell() { Value = item.Value });

                }
         
                table.Rows.Add(row);
            }

        
            string json = JsonConvert.SerializeObject(table);
            return Content(json, "application/json");
        }

        public ContentResult GetDataPartialTortaVIA()
        {
            GoogleChartTable table = new GoogleChartTable();
            List<GoogleChartCell> cells = new List<GoogleChartCell>();
            Dictionary<int, int> contaTipologie = new Dictionary<int, int>();

            ReportTipologia reportTipologiaTotali = ReportRepository.Instance.RecuperaReportTipologia();


            table.Cols.Add(new GoogleChartColumn() { Label = "Tipo provvedimento", Type = "string", Pattern = "" });
            table.Cols.Add(new GoogleChartColumn() { Label = "Totale", Type = "number", Pattern = "" });

            GoogleChartRow row = new GoogleChartRow();
            row.Cells.Add(new GoogleChartCell() { Value = reportTipologiaTotali.TipoProvvedimento[1].GetNome() });
            row.Cells.Add(new GoogleChartCell() { Value = reportTipologiaTotali.ReportMT.MT1Via });
            table.Rows.Add(row);

            row = new GoogleChartRow();
            row.Cells.Add(new GoogleChartCell() { Value = reportTipologiaTotali.TipoProvvedimento[2].GetNome() });
            row.Cells.Add(new GoogleChartCell() { Value = reportTipologiaTotali.ReportMT.MT2Via });
            table.Rows.Add(row);

            string json = JsonConvert.SerializeObject(table);
            return Content(json, "application/json");
        }

        public ContentResult GetDataPartialTortaVIALO()
        {
            GoogleChartTable table = new GoogleChartTable();
            List<GoogleChartCell> cells = new List<GoogleChartCell>();
            Dictionary<int, int> contaTipologie = new Dictionary<int, int>();

            ReportTipologia reportTipologiaTotali = ReportRepository.Instance.RecuperaReportTipologia();

            table.Cols.Add(new GoogleChartColumn() { Label = "Tipo provvedimento", Type = "string", Pattern = "" });
            table.Cols.Add(new GoogleChartColumn() { Label = "Totale", Type = "number", Pattern = "" });

            GoogleChartRow row = new GoogleChartRow();
            row = new GoogleChartRow();
            row.Cells.Add(new GoogleChartCell() { Value = reportTipologiaTotali.TipoProvvedimento[9].GetNome() });
            row.Cells.Add(new GoogleChartCell() { Value = reportTipologiaTotali.ReportMT.MT1ViaLO });
            table.Rows.Add(row);

            row = new GoogleChartRow();
            row.Cells.Add(new GoogleChartCell() { Value = reportTipologiaTotali.TipoProvvedimento[9].GetNome() });
            row.Cells.Add(new GoogleChartCell() { Value = reportTipologiaTotali.ReportMT.MT2ViaLO });
            table.Rows.Add(row);

            string json = JsonConvert.SerializeObject(table);
            return Content(json, "application/json");
        }

      
        public ContentResult GetDataTortaEsitiVIA()
        {
            GoogleChartTable table = new GoogleChartTable();

            ReportEsito re = ReportRepository.Instance.RecuperaReportEsitoVIA();

            re.Title = "Provvedimenti VIA (1989 - I trimestre 2016) per esito";

            table.Cols.Add(new GoogleChartColumn() { Label = "Esito", Type = "string", Pattern = "" });
            table.Cols.Add(new GoogleChartColumn() { Label = "Percentuale", Type = "number", Pattern = "" });

            List<GoogleChartCell> cells = new List<GoogleChartCell>();

         

            foreach (ReportEsitoItem rtp in re.ListaEsiti)
            {
                GoogleChartRow row = new GoogleChartRow();
                row.Cells.Add(new GoogleChartCell() { Value = rtp.Esito });
                row.Cells.Add(new GoogleChartCell() { Value = rtp.Percentuale });
                table.Rows.Add(row);
            }
            string json = JsonConvert.SerializeObject(table);
            return Content(json, "application/json");
        }

        public ContentResult GetDataTortaEsitiVIALO()
        {
            GoogleChartTable table = new GoogleChartTable();

            ReportEsito re = ReportRepository.Instance.RecuperaReportEsitoVIALO();

            re.Title = "Pareri VIA Legge Obiettivo (2003 - I trimestre 2016) per esito";

            table.Cols.Add(new GoogleChartColumn() { Label = "Esito", Type = "string", Pattern = "" });
            table.Cols.Add(new GoogleChartColumn() { Label = "Percentuale", Type = "number", Pattern = "" });

            List<GoogleChartCell> cells = new List<GoogleChartCell>();


            foreach (ReportEsitoItem rtp in re.ListaEsiti)
            {
                GoogleChartRow row = new GoogleChartRow();
                row.Cells.Add(new GoogleChartCell() { Value = rtp.Esito });
                row.Cells.Add(new GoogleChartCell() { Value = rtp.Percentuale });
                table.Rows.Add(row);
            }
            string json = JsonConvert.SerializeObject(table);
            return Content(json, "application/json");
        }

        public ActionResult TestJson()
        {
            return View();
        }
    }
}



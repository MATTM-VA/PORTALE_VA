using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VAPortale.Filters;
using VAPortale.Models;
using VALib.Domain.Entities.DatiAmbientali;
using VALib.Domain.Repositories.DatiAmbientali;
using VALib.Domain.Repositories.UI;
using VALib.Domain.Entities.UI;
using System.Data;
using VALib.Helpers;
using VAPortale.Code;
using VALib.Domain.Services;

namespace VAPortale.Controllers
{
    [LanguageAttribute]
    public class DatiEStrumentiController : Controller
    {
        //
        // GET: /Condivisione/

        public ActionResult Index()
        {
            return View();
        }

               
        public ActionResult DatiAmbientali(DatiEStrumentiDatiAmbientaliModel model)
        {
            ActionResult result = null;
            int totale = 0;

            if (!string.IsNullOrWhiteSpace(model.Mode) && model.Mode.Equals("export", StringComparison.CurrentCultureIgnoreCase))
            {
                byte[] data = null;

                List<RisorsaElenco> esportazione = RisorsaElencoRepository.Instance.RecuperaRisorseElenco(model.TemaID,
                                                model.Testo ?? "",
                                                "",
                                                0,
                                                int.MaxValue,
                                                out totale);

                data = EsportazioneUtils.GeneraXlsxDatiAmbientaliRicerca(esportazione);

                if (data != null)
                    result = File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Export.xlsx");
                else
                    result = HttpNotFound();
            }
            else
            {
                model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("datiambientali");
                model.Elenchi = ElencoRepository.Instance.RecuperaElenchiDatiEStrumenti("datiambientali");

                model.TemiSelectList = ModelUtils.CreaTemaSelectList(true);

                List<RisorsaElenco> risorse = RisorsaElencoRepository.Instance.RecuperaRisorseElenco(model.TemaID,
                                                model.Testo ?? "",
                                                "",
                                                model.IndiceInizio,
                                                model.IndiceInizio + model.DimensionePagina,
                                                out totale);
                model.Risorse = risorse;
                model.TotaleRisultati = totale;

                result = View(model);
            }
            return result;
        }

       


        [RemoveScript]
        [ExcludeFromAntiForgeryValidation]
        public ActionResult Normativa(DatiEstrumentiTabModel model)
        {
            ActionResult result = null;
            if (string.IsNullOrWhiteSpace(model.NomeElenco))
            {
                model.NomeElenco = "Normativa nazionale";
                RouteData.Values["nomeElenco"] = "Normativa nazionale";
            }

            int totale = 0;

            Elenco elenco = ElencoRepository.Instance.RecuperaElenco(model.NomeElenco);
            model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("normativa");

            if (elenco != null && model.VoceMenu != null)
            {
                if (!string.IsNullOrWhiteSpace(model.Mode) && model.Mode.Equals("export", StringComparison.CurrentCultureIgnoreCase))
                {
                    byte[] data = null;

                    List<DocumentoCondivisione> esportazione = DocumentoCondivisioneRepository.Instance.RecuperaDocumentiCondivisione(model.Testo,
                                                    elenco.ID.ToString(),
                                                    0,
                                                    int.MaxValue,
                                                    out totale);

                    data = EsportazioneUtils.GeneraXlsxDatiAmbientaliCondivisione(esportazione);

                    if (data != null)
                        result = File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Export.xlsx");
                    else
                        result = HttpNotFound();
                }
                else
                {
                    List<DocumentoCondivisione> risorse = DocumentoCondivisioneRepository.Instance.RecuperaDocumentiCondivisione(model.Testo,
                                                    elenco.ID.ToString(),
                                                    model.IndiceInizio,
                                                    model.IndiceInizio + model.DimensionePagina,
                                                    out totale);
                    model.Risorse = risorse;
                    model.Elenco = elenco;
                    model.TotaleRisultati = totale;
                    
                    result = View(model);
                }
            }
            else
                result = HttpNotFound();

            return result;
        }

        [RemoveScript]
        [ExcludeFromAntiForgeryValidation] 
        public ActionResult StudiEIndaginiDiSettore(DatiEstrumentiTabModel model)
        {
            ActionResult result = null;
          
            int totale = 0;
            
            var NomeElenco = new string[] { "VIA", "VAS" };
           
            string elenco = ElencoRepository.Instance.RecuperaElencoStudiIndagini(string.Join(",", NomeElenco));

            model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("StudiEIndaginiDiSettore");

            if (elenco != null && model.VoceMenu != null)
            {
                if (!string.IsNullOrWhiteSpace(model.Mode) && model.Mode.Equals("export", StringComparison.CurrentCultureIgnoreCase))
                {
                    byte[] data = null;

                    List<DocumentoCondivisione> esportazione = DocumentoCondivisioneRepository.Instance.RecuperaDocumentiCondivisione(model.Testo,
                                                    elenco,
                                                    0,
                                                    int.MaxValue,
                                                    out totale);

                    data = EsportazioneUtils.GeneraXlsxDatiAmbientaliCondivisione(esportazione);

                    if (data != null)
                        result = File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Export.xlsx");
                    else
                        result = HttpNotFound();
                }
                else
                {
                    
                    List<DocumentoCondivisione> risorse = DocumentoCondivisioneRepository.Instance.RecuperaDocumentiCondivisione(model.Testo,
                                                    elenco,
                                                    model.IndiceInizio,
                                                    model.IndiceInizio + model.DimensionePagina,
                                                    out totale);
                    model.Risorse = risorse;
                    model.TotaleRisultati = totale;
                
                    result = View(model);
                }
            }
            else
                result = HttpNotFound();

            return result;
        }

        
        
        [ChildActionOnly]
        public ActionResult PartialTab(int elencoID, string voce)
        {
            DatiEStrumentiPartialTabModel model = new DatiEStrumentiPartialTabModel();

            model.Voce = voce;
            model.Voci = ElencoRepository.Instance.RecuperaElenchiDatiEStrumenti(voce);
            model.ElencoSelezionatoID = elencoID;

            return PartialView(model);
        }

        public ActionResult MetadatoRisorsaCondivisione(Guid id)
        {
            ActionResult result = null;
            DatiEStrumentiMetadatoRisorsaCondivisioneModel model = new DatiEStrumentiMetadatoRisorsaCondivisioneModel();

            model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("MetadatoRisorsaCondivisione");

            DocumentoCondivisione risorsa = DocumentoCondivisioneRepository.Instance.RecuperaDocumentoCondivisione(id);

            if (risorsa != null && model.VoceMenu != null)
            {
                model.Risorsa = risorsa;

                
                if (!string.IsNullOrWhiteSpace(risorsa.Soggetto))
                    ModelUtils.AggiungiRiga(model.InformazioniGenerali, DizionarioService.METADATO_LabelOggetto, risorsa.Soggetto);

                if (!string.IsNullOrWhiteSpace(risorsa.Scopo))
                    ModelUtils.AggiungiRiga(model.InformazioniGenerali, DizionarioService.METADATO_LabelScopo, risorsa.Scopo);

                if (!string.IsNullOrWhiteSpace(risorsa.Abstract))
                    ModelUtils.AggiungiRiga(model.InformazioniGenerali, DizionarioService.METADATO_LabelAbstract, risorsa.Abstract);

                if (!string.IsNullOrWhiteSpace(risorsa.Lingua))
                    ModelUtils.AggiungiRiga(model.InformazioniGenerali, DizionarioService.METADATO_LabelLingua, risorsa.Lingua);

                // Informazioni Ricerca
                if (!string.IsNullOrWhiteSpace(risorsa.Commenti))
                    ModelUtils.AggiungiRiga(model.InformazioniRicerca, DizionarioService.METADATO_LabelCommenti, risorsa.Commenti);

                if (!string.IsNullOrWhiteSpace(risorsa.Origine))
                    ModelUtils.AggiungiRiga(model.InformazioniRicerca, DizionarioService.METADATO_LabelOrigine, risorsa.Origine);

                if (!string.IsNullOrWhiteSpace(risorsa.Riferimenti))
                    ModelUtils.AggiungiRiga(model.InformazioniRicerca, DizionarioService.METADATO_LabelRiferimenti, risorsa.Riferimenti);

                if (!string.IsNullOrWhiteSpace(risorsa.ParoleChiave))
                    ModelUtils.AggiungiRiga(model.InformazioniRicerca, DizionarioService.METADATO_LabelParoleChiave, risorsa.ParoleChiave);

                if (!string.IsNullOrWhiteSpace(risorsa.Territori))
                    ModelUtils.AggiungiRiga(model.InformazioniRicerca, DizionarioService.METADATO_LabelTerritori, risorsa.Territori);

                // Date
                if (risorsa.DataScadenza.HasValue)
                    ModelUtils.AggiungiRiga(model.Date, DizionarioService.METADATO_LabelDataScadenza, risorsa.DataScadenza.Value.ToString(CultureHelper.GetDateFormat()));

                if (risorsa.DataPubblicazione.HasValue)
                    ModelUtils.AggiungiRiga(model.Date, DizionarioService.METADATO_LabelDataPubblicazione, risorsa.DataPubblicazione.Value.ToString(CultureHelper.GetDateFormat()));

                if (risorsa.DataCreazione.HasValue)
                    ModelUtils.AggiungiRiga(model.Date, DizionarioService.METADATO_LabelDataStesura, risorsa.DataCreazione.Value.ToString(CultureHelper.GetDateFormat()));
                
                
                if (!string.IsNullOrWhiteSpace(risorsa.Autore))
                    ModelUtils.AggiungiRiga(model.Entita, DizionarioService.METADATO_LabelAutore, risorsa.Autore);

                if (!string.IsNullOrWhiteSpace(risorsa.ResponsabileMetadato))
                    ModelUtils.AggiungiRiga(model.Entita, DizionarioService.METADATO_LabelResponsabileMetadato, risorsa.ResponsabileMetadato);

                if (!string.IsNullOrWhiteSpace(risorsa.ResponsabilePubblicazione))
                    ModelUtils.AggiungiRiga(model.Entita, DizionarioService.METADATO_LabelResponsabilePubblicazione, risorsa.ResponsabilePubblicazione);


               // model.Widget = widget;
                result = View(model);
            }
            else
                result = HttpNotFound();

            return result;
        }

        public ActionResult MetadatoRisorsa(Guid id)
        {
            ActionResult result = null;
            DatiEStrumentiMetadatoRisorsaModel model = new DatiEStrumentiMetadatoRisorsaModel();

            model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("MetadatoRisorsa");

            Risorsa risorsa = RisorsaRepository.Instance.RecuperaRisorsa(id);

            if (risorsa != null && model.VoceMenu != null)
            {
                model.Risorsa = risorsa;

                //List<WidgetCorrelato> widget = new List<WidgetCorrelato>();
                //widget = WidgetCorrelatoRepository.Instance.RecuperaWidgetCorrelati(model.VoceMenu.ID);

                // Informazioni Generali
                //if (!string.IsNullOrWhiteSpace(risorsa.Titolo))
                //    AggiungiRiga(model.InformazioniGenerali, "Titolo", risorsa.Titolo);

                if (!string.IsNullOrWhiteSpace(risorsa.Soggetto))
                    ModelUtils.AggiungiRiga(model.InformazioniGenerali, DizionarioService.METADATO_LabelOggetto, risorsa.Soggetto);

                if (!string.IsNullOrWhiteSpace(risorsa.Scopo))
                    ModelUtils.AggiungiRiga(model.InformazioniGenerali, DizionarioService.METADATO_LabelScopo, risorsa.Scopo);

                if (!string.IsNullOrWhiteSpace(risorsa.Abstract))
                    ModelUtils.AggiungiRiga(model.InformazioniGenerali, DizionarioService.METADATO_LabelAbstract, risorsa.Abstract);


                // Informazioni Ricerca
                if (!string.IsNullOrWhiteSpace(risorsa.Argomenti))
                    ModelUtils.AggiungiRiga(model.InformazioniRicerca, DizionarioService.METADATO_LabelArgomenti, risorsa.Argomenti);

                if (!string.IsNullOrWhiteSpace(risorsa.Commenti))
                    ModelUtils.AggiungiRiga(model.InformazioniRicerca, DizionarioService.METADATO_LabelCommenti, risorsa.Commenti);

                if (!string.IsNullOrWhiteSpace(risorsa.Origine))
                    ModelUtils.AggiungiRiga(model.InformazioniRicerca, DizionarioService.METADATO_LabelOrigine, risorsa.Origine);

                if (!string.IsNullOrWhiteSpace(risorsa.Riferimenti))
                    ModelUtils.AggiungiRiga(model.InformazioniRicerca, DizionarioService.METADATO_LabelRiferimenti, risorsa.Riferimenti);

                if (!string.IsNullOrWhiteSpace(risorsa.ParoleChiave))
                    ModelUtils.AggiungiRiga(model.InformazioniRicerca, DizionarioService.METADATO_LabelParoleChiave, risorsa.ParoleChiave);

                // Date
                if (risorsa.DataScadenza.HasValue)
                    ModelUtils.AggiungiRiga(model.Date, DizionarioService.METADATO_LabelDataScadenza, risorsa.DataScadenza.Value.ToString(CultureHelper.GetDateFormat()));

                if (risorsa.DataPubblicazione.HasValue)
                    ModelUtils.AggiungiRiga(model.Date, DizionarioService.METADATO_LabelDataPubblicazione, risorsa.DataPubblicazione.Value.ToString(CultureHelper.GetDateFormat()));

                if (risorsa.DataCreazione.HasValue)
                    ModelUtils.AggiungiRiga(model.Date, DizionarioService.METADATO_LabelDataStesura, risorsa.DataCreazione.Value.ToString(CultureHelper.GetDateFormat()));

                // Entita
                foreach (Tuple<string, string> t in risorsa.Entita)
                {
                    if (!string.IsNullOrWhiteSpace(t.Item2))
                        ModelUtils.AggiungiRiga(model.Entita, t.Item1, t.Item2);
                }

                //model.Widget = widget;
                result = View(model);
            }
            else
                result = HttpNotFound();

            return result;
        }

        public ActionResult MetadatoStrato(Guid id)
        {
            ActionResult result = null;
            DatiEStrumentiMetadatoStratoModel model = new DatiEStrumentiMetadatoStratoModel();

            model.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu("MetadatoStrato");

            Strato strato = StratoRepository.Instance.RecuperaStrato(id);

            if (strato != null && model.VoceMenu != null)
            {
                model.Strato = strato;

                //List<WidgetCorrelato> widget = new List<WidgetCorrelato>();
                //widget = WidgetCorrelatoRepository.Instance.RecuperaWidgetCorrelati(model.VoceMenu.ID);

                if (strato.Identificazione.Count > 0)
                    model.Tabs.Add(new Tuple<string, string>("Identificazione", DizionarioService.METADATO_SezioneIdentificazione));

                if (strato.Classificazione.Count > 0)
                    model.Tabs.Add(new Tuple<string, string>("Classificazione", DizionarioService.METADATO_SezioneClassificazione));

                if (strato.ParoleChiave.Count > 0)
                    model.Tabs.Add(new Tuple<string, string>("ParoleChiave", DizionarioService.METADATO_SezioneParoleChiave));

                if (strato.Localizzazione.Count > 0)
                    model.Tabs.Add(new Tuple<string, string>("Localizzazione", DizionarioService.METADATO_SezioneLocalizzazione));

                if (strato.RiferimentoTemporale.Count > 0)
                    model.Tabs.Add(new Tuple<string, string>("RiferimentoTemporale", DizionarioService.METADATO_SezioneRiferimentoTemporale));

                if (strato.Qualita.Count > 0)
                    model.Tabs.Add(new Tuple<string, string>("Qualita", DizionarioService.METADATO_SezioneQualita));

                if (strato.SistemaRiferimento.Count > 0)
                    model.Tabs.Add(new Tuple<string, string>("SistemaRiferimento", DizionarioService.METADATO_SezioneSistemaRiferimento));

                if (strato.Conformita.Count > 0)
                    model.Tabs.Add(new Tuple<string, string>("Conformita", DizionarioService.METADATO_SezioneConformita));

                if (strato.Vincoli.Count > 0)
                    model.Tabs.Add(new Tuple<string, string>("Vincoli", DizionarioService.METADATO_SezioneVincoli));

                if (strato.Responsabili.Count > 0)
                    model.Tabs.Add(new Tuple<string, string>("Responsabili", DizionarioService.METADATO_SezioneResponsabili));

                if (strato.Distribuzione.Count > 0)
                    model.Tabs.Add(new Tuple<string, string>("Distribuzione", DizionarioService.METADATO_SezioneDistribusione));

                if (strato.Metadati.Count > 0)
                    model.Tabs.Add(new Tuple<string, string>("Metadati", DizionarioService.METADATO_SezioneMetadati));


                //model.Widget = widget;
                result = View(model);
            }
            else
                result = HttpNotFound();

            return result;
        }


        [ChildActionOnly]
        public ActionResult StudiIndaginiPS(string nomeSezione, string nomeVoce)
        {
            ActionResult result = null;
            PagineVoceModel model = new PagineVoceModel();
            VoceMenu voceMenu = null;
            PaginaStatica paginaStatica = null;            
          
            voceMenu = VoceMenuRepository.Instance.RecuperaVociMenu().SingleOrDefault(x => x.Sezione.Equals(nomeSezione, StringComparison.InvariantCultureIgnoreCase) && x.Voce.Equals(nomeVoce, StringComparison.InvariantCultureIgnoreCase));

            if (voceMenu != null)
            {
                paginaStatica = PaginaStaticaRepository.Instance.RecuperaPaginaStatica(voceMenu.ID);
          
                model.PaginaStatica = paginaStatica;
                model.VoceMenu = voceMenu;
                result = PartialView(model);
            }
            else
                result = HttpNotFound();

            return result;

        }
    }
}

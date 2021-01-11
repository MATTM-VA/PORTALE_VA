using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VALib.Domain.Repositories.UI;
using VALib.Domain.Entities.UI;

namespace VALib.Domain.Services
{
    public static class DizionarioService
    {
        private static string GetValore(string nome)
        {
            string result = nome;

            VoceDizionario voce = null;

            voce = VoceDizionarioRepository.Instance.RecuperaVoceDizionario(nome);

            if (voce != null)
                result = voce.GetValore();
            else
                throw new KeyNotFoundException(nome);

            return result;
        }

        public static string DATIAMBIENTALI_LabelDocumento { get { return GetValore("DATIAMBIENTALI_LabelDocumento"); } }
        public static string DATIAMBIENTALI_LabelMappaInterattiva { get { return GetValore("DATIAMBIENTALI_LabelMappaInterattiva"); } }
        public static string DATIAMBIENTALI_LabelMetadato { get { return GetValore("DATIAMBIENTALI_LabelMetadato"); } }
        public static string DATIAMBIENTALI_LabelRisorsa { get { return GetValore("DATIAMBIENTALI_LabelRisorsa"); } }
        public static string DATIAMBIENTALI_LabelRisorsaOnline { get { return GetValore("DATIAMBIENTALI_LabelRisorsaOnline"); } }
        public static string DATIAMBIENTALI_LabelVisualizza { get { return GetValore("DATIAMBIENTALI_LabelVisualizza"); } }
        public static string DATIAMBIENTALI_LabelCreaMappa { get { return GetValore("DATIAMBIENTALI_LabelCreaMappa"); } }
        public static string FORM_BottoneInvia { get { return GetValore("FORM_BottoneInvia"); } }
        public static string FORM_LabelMail { get { return GetValore("FORM_LabelMail"); } }
        public static string GRIGLIA_ColonnaArgomento { get { return GetValore("GRIGLIA_ColonnaArgomento"); } }
        public static string GRIGLIA_ColonnaCodiceElaborato { get { return GetValore("GRIGLIA_ColonnaCodiceElaborato"); } }
        public static string GRIGLIA_ColonnaData { get { return GetValore("GRIGLIA_ColonnaData"); } }
        public static string GRIGLIA_ColonnaDataAvvio { get { return GetValore("GRIGLIA_ColonnaDataAvvio"); } }
        public static string GRIGLIA_ColonnaDataENumero { get { return GetValore("GRIGLIA_ColonnaDataENumero"); } }
        public static string GRIGLIA_ColonnaDimensione { get { return GetValore("GRIGLIA_ColonnaDimensione"); } }
        public static string GRIGLIA_ColonnaEsito { get { return GetValore("GRIGLIA_ColonnaEsito"); } }
        public static string GRIGLIA_ColonnaOggetto { get { return GetValore("GRIGLIA_ColonnaOggetto"); } }
        public static string GRIGLIA_ColonnaOggettoOrizzontale { get { return GetValore("GRIGLIA_ColonnaOggettoOrizzontale"); } }
        public static string GRIGLIA_ColonnaOggettoVas { get { return GetValore("GRIGLIA_ColonnaOggettoVas"); } }
        public static string GRIGLIA_ColonnaOggettoVia { get { return GetValore("GRIGLIA_ColonnaOggettoVia"); } }
        public static string GRIGLIA_ColonnaProcedura { get { return GetValore("GRIGLIA_ColonnaProcedura"); } }
        public static string GRIGLIA_ColonnaProponente { get { return GetValore("GRIGLIA_ColonnaProponente"); } }
        public static string GRIGLIA_ColonnaScala { get { return GetValore("GRIGLIA_ColonnaScala"); } }
        public static string GRIGLIA_ColonnaServizi { get { return GetValore("GRIGLIA_ColonnaServizi"); } }
        public static string GRIGLIA_ColonnaSezione { get { return GetValore("GRIGLIA_ColonnaSezione"); } }
        public static string GRIGLIA_ColonnaStatoProcedura { get { return GetValore("GRIGLIA_ColonnaStatoProcedura"); } }
        public static string GRIGLIA_ColonnaTema { get { return GetValore("GRIGLIA_ColonnaTema"); } }
        public static string GRIGLIA_ColonnaTitolo { get { return GetValore("GRIGLIA_ColonnaTitolo"); } }
        public static string GRIGLIA_ColonnaUltimaProcedura { get { return GetValore("GRIGLIA_ColonnaUltimaProcedura"); } }
        public static string GRIGLIA_OrdinaAsc { get { return GetValore("GRIGLIA_OrdinaAsc"); } }
        public static string GRIGLIA_OrdinaDesc { get { return GetValore("GRIGLIA_OrdinaDesc"); } }
        public static string GRIGLIA_PaginatorePagine { get { return GetValore("GRIGLIA_PaginatorePagine"); } }
        public static string GRIGLIA_PaginatorePrimaPagina { get { return GetValore("GRIGLIA_PaginatorePrimaPagina"); } }
        public static string GRIGLIA_PaginatoreUltimaPagina { get { return GetValore("GRIGLIA_PaginatoreUltimaPagina"); } }
        public static string GRIGLIA_ResetOrdinamento { get { return GetValore("GRIGLIA_ResetOrdinamento"); } }
        public static string HOME_DataEmissioneProvvedimento { get { return GetValore("HOME_DataEmissioneProvvedimento"); } }
        public static string HOME_LabelRicercaOrizzontale { get { return GetValore("HOME_LabelRicercaOrizzontale"); } }
        public static string HOME_LinkOggetto { get { return GetValore("HOME_LinkOggetto"); } }
        public static string HOME_OggettiConsultazioneNoRisultatiVAS { get { return GetValore("HOME_OggettiConsultazioneNoRisultatiVAS"); } }
        public static string HOME_OggettiConsultazioneNoRisultatiVIA { get { return GetValore("HOME_OggettiConsultazioneNoRisultatiVIA"); } }
        public static string HOME_ProvvedimentiConsultazioneNoRisultati { get { return GetValore("HOME_ProvvedimentiConsultazioneNoRisultati"); } }
        public static string HOME_TitoloOggettiConsultazione { get { return GetValore("HOME_TitoloOggettiConsultazione"); } }
        public static string HOME_TitoloProvvedimenti { get { return GetValore("HOME_TitoloProvvedimenti"); } }
        public static string METADATO_LabelAbstract { get { return GetValore("METADATO_LabelAbstract"); } }
        public static string METADATO_LabelAltriDettagli { get { return GetValore("METADATO_LabelAltriDettagli"); } }
        public static string METADATO_LabelAltriVincoli { get { return GetValore("METADATO_LabelAltriVincoli"); } }
        public static string METADATO_LabelArgomenti { get { return GetValore("METADATO_LabelArgomenti"); } }
        public static string METADATO_LabelAutore { get { return GetValore("METADATO_LabelAutore"); } }
        public static string METADATO_LabelCodiceElaborato { get { return GetValore("METADATO_LabelCodiceElaborato"); } }
        public static string METADATO_LabelCommenti { get { return GetValore("METADATO_LabelCommenti"); } }
        public static string METADATO_LabelComuni { get { return GetValore("METADATO_LabelComuni"); } }
        public static string METADATO_LabelCondizioniApplicabili { get { return GetValore("METADATO_LabelCondizioniApplicabili"); } }
        public static string METADATO_LabelConformitaSpecifica { get { return GetValore("METADATO_LabelConformitaSpecifica"); } }
        public static string METADATO_LabelCopertura { get { return GetValore("METADATO_LabelCopertura"); } }
        public static string METADATO_LabelData { get { return GetValore("METADATO_LabelData"); } }
        public static string METADATO_LabelDataCreazione { get { return GetValore("METADATO_LabelDataCreazione"); } }
        public static string METADATO_LabelDataMetadati { get { return GetValore("METADATO_LabelDataMetadati"); } }
        public static string METADATO_LabelDataPubblicazione { get { return GetValore("METADATO_LabelDataPubblicazione"); } }
        public static string METADATO_LabelDataRevisione { get { return GetValore("METADATO_LabelDataRevisione"); } }
        public static string METADATO_LabelDataScadenza { get { return GetValore("METADATO_LabelDataScadenza"); } }
        public static string METADATO_LabelDataStesura { get { return GetValore("METADATO_LabelDataStesura"); } }
        public static string METADATO_LabelDescrizione { get { return GetValore("METADATO_LabelDescrizione"); } }
        public static string METADATO_LabelDimensione { get { return GetValore("METADATO_LabelDimensione"); } }
        public static string METADATO_LabelDiritti { get { return GetValore("METADATO_LabelDiritti"); } }
        public static string METADATO_LabelDistanzaX { get { return GetValore("METADATO_LabelDistanzaX"); } }
        public static string METADATO_LabelDistanzaY { get { return GetValore("METADATO_LabelDistanzaY"); } }
        public static string METADATO_LabelDocumento { get { return GetValore("METADATO_LabelDocumento"); } }
        public static string METADATO_LabelEnte { get { return GetValore("METADATO_LabelEnte"); } }
        public static string METADATO_LabelEstensioneTemporale { get { return GetValore("METADATO_LabelEstensioneTemporale"); } }
        public static string METADATO_LabelFormatoDati { get { return GetValore("METADATO_LabelFormatoDati"); } }
        public static string METADATO_LabelFormatoPresentazione { get { return GetValore("METADATO_LabelFormatoPresentazione"); } }
        public static string METADATO_LabelGenealogiaProcessoProduzione { get { return GetValore("METADATO_LabelGenealogiaProcessoProduzione"); } }
        public static string METADATO_LabelIdentificatoreLivelloSuperiore { get { return GetValore("METADATO_LabelIdentificatoreLivelloSuperiore"); } }
        public static string METADATO_LabelIdentificatoreUnico { get { return GetValore("METADATO_LabelIdentificatoreUnico"); } }
        public static string METADATO_LabelIdentificatoreUnicoMetadati { get { return GetValore("METADATO_LabelIdentificatoreUnicoMetadati"); } }
        public static string METADATO_LabelIdentificatoreUnicoMetadatiSuperiore { get { return GetValore("METADATO_LabelIdentificatoreUnicoMetadatiSuperiore"); } }
        public static string METADATO_LabelIndirizzoEmail { get { return GetValore("METADATO_LabelIndirizzoEmail"); } }
        public static string METADATO_LabelIndirizzoWeb { get { return GetValore("METADATO_LabelIndirizzoWeb"); } }
        public static string METADATO_LabelInformazioneSupplementari { get { return GetValore("METADATO_LabelInformazioneSupplementari"); } }
        public static string METADATO_LabelLatitudineNord { get { return GetValore("METADATO_LabelLatitudineNord"); } }
        public static string METADATO_LabelLatitudineSud { get { return GetValore("METADATO_LabelLatitudineSud"); } }
        public static string METADATO_LabelLingua { get { return GetValore("METADATO_LabelLingua"); } }
        public static string METADATO_LabelLinguaDellaRisorsa { get { return GetValore("METADATO_LabelLinguaDellaRisorsa"); } }
        public static string METADATO_LabelLinguaMetadati { get { return GetValore("METADATO_LabelLinguaMetadati"); } }
        public static string METADATO_LabelLongitudineEst { get { return GetValore("METADATO_LabelLongitudineEst"); } }
        public static string METADATO_LabelLongitudineOvest { get { return GetValore("METADATO_LabelLongitudineOvest"); } }
        public static string METADATO_LabelNazioni { get { return GetValore("METADATO_LabelNazioni"); } }
        public static string METADATO_LabelNumeroTelefono { get { return GetValore("METADATO_LabelNumeroTelefono"); } }
        public static string METADATO_LabelOggetto { get { return GetValore("METADATO_LabelOggetto"); } }
        public static string METADATO_LabelOrigine { get { return GetValore("METADATO_LabelOrigine"); } }
        public static string METADATO_LabelParoleChiave { get { return GetValore("METADATO_LabelParoleChiave"); } }
        public static string METADATO_LabelProcedura { get { return GetValore("METADATO_LabelProcedura"); } }
        public static string METADATO_LabelProvince { get { return GetValore("METADATO_LabelProvince"); } }
        public static string METADATO_LabelRegioni { get { return GetValore("METADATO_LabelRegioni"); } }
        public static string METADATO_LabelResponsabileMetadato { get { return GetValore("METADATO_LabelResponsabileMetadato"); } }
        public static string METADATO_LabelResponsabilePubblicazione { get { return GetValore("METADATO_LabelResponsabilePubblicazione"); } }
        public static string METADATO_LabelRiferimenti { get { return GetValore("METADATO_LabelRiferimenti"); } }
        public static string METADATO_LabelRisorsa { get { return GetValore("METADATO_LabelRisorsa"); } }
        public static string METADATO_LabelRuolo { get { return GetValore("METADATO_LabelRuolo"); } }
        public static string METADATO_LabelScala { get { return GetValore("METADATO_LabelScala"); } }
        public static string METADATO_LabelScopo { get { return GetValore("METADATO_LabelScopo"); } }
        public static string METADATO_LabelSezione { get { return GetValore("METADATO_LabelSezione"); } }
        public static string METADATO_LabelSistemaRiferimento { get { return GetValore("METADATO_LabelSistemaRiferimento"); } }
        public static string METADATO_LabelTerritori { get { return GetValore("METADATO_LabelTerritori"); } }
        public static string METADATO_LabelThesaurus { get { return GetValore("METADATO_LabelThesaurus"); } }
        public static string METADATO_LabelTipoData { get { return GetValore("METADATO_LabelTipoData"); } }
        public static string METADATO_LabelTipoDocumento { get { return GetValore("METADATO_LabelTipoDocumento"); } }
        public static string METADATO_LabelTipoRappresentazioneSpaziale { get { return GetValore("METADATO_LabelTipoRappresentazioneSpaziale"); } }
        public static string METADATO_LabelTipoRisorsa { get { return GetValore("METADATO_LabelTipoRisorsa"); } }
        public static string METADATO_LabelTitolo { get { return GetValore("METADATO_LabelTitolo"); } }
        public static string METADATO_LabelTitoloSpecifiche { get { return GetValore("METADATO_LabelTitoloSpecifiche"); } }
        public static string METADATO_LabelVersioneFormato { get { return GetValore("METADATO_LabelVersioneFormato"); } }
        public static string METADATO_LabelVincoliAccesso { get { return GetValore("METADATO_LabelVincoliAccesso"); } }
        public static string METADATO_LabelVincoliFruibilita { get { return GetValore("METADATO_LabelVincoliFruibilita"); } }
        public static string METADATO_LabelVincoliSicurezza { get { return GetValore("METADATO_LabelVincoliSicurezza"); } }
        public static string METADATO_SezioneClassificazione { get { return GetValore("METADATO_SezioneClassificazione"); } }
        public static string METADATO_SezioneConformita { get { return GetValore("METADATO_SezioneConformita"); } }
        public static string METADATO_SezioneDistribusione { get { return GetValore("METADATO_SezioneDistribusione"); } }
        public static string METADATO_SezioneIdentificazione { get { return GetValore("METADATO_SezioneIdentificazione"); } }
        public static string METADATO_SezioneLocalizzazione { get { return GetValore("METADATO_SezioneLocalizzazione"); } }
        public static string METADATO_SezioneMetadati { get { return GetValore("METADATO_SezioneMetadati"); } }
        public static string METADATO_SezioneParoleChiave { get { return GetValore("METADATO_SezioneParoleChiave"); } }
        public static string METADATO_SezioneQualita { get { return GetValore("METADATO_SezioneQualita"); } }
        public static string METADATO_SezioneResponsabili { get { return GetValore("METADATO_SezioneResponsabili"); } }
        public static string METADATO_SezioneRiferimentoTemporale { get { return GetValore("METADATO_SezioneRiferimentoTemporale"); } }
        public static string METADATO_SezioneSistemaRiferimento { get { return GetValore("METADATO_SezioneSistemaRiferimento"); } }
        public static string METADATO_SezioneVincoli { get { return GetValore("METADATO_SezioneVincoli"); } }
        public static string METADATO_TitoloDate { get { return GetValore("METADATO_TitoloDate"); } }
        public static string METADATO_TitoloEntitaCoinvolte { get { return GetValore("METADATO_TitoloEntitaCoinvolte"); } }
        public static string METADATO_TitoloInformazioniContenuto { get { return GetValore("METADATO_TitoloInformazioniContenuto"); } }
        public static string METADATO_TitoloInformazioniGenerali { get { return GetValore("METADATO_TitoloInformazioniGenerali"); } }
        public static string METADATO_TitoloInformazioniRicerca { get { return GetValore("METADATO_TitoloInformazioniRicerca"); } }
        public static string METADATO_ValoreTipoDocumentoGrafico { get { return GetValore("METADATO_ValoreTipoDocumentoGrafico"); } }
        public static string METADATO_ValoreTipoDocumentoTestuale { get { return GetValore("METADATO_ValoreTipoDocumentoTestuale"); } }
        public static string OGGETTO_LabelAltriProgetti { get { return GetValore("OGGETTO_LabelAltriProgetti"); } }
        public static string OGGETTO_LabelComuni { get { return GetValore("OGGETTO_LabelComuni"); } }
        public static string OGGETTO_LabelCup { get { return GetValore("OGGETTO_LabelCup"); } }
        public static string OGGETTO_LabelDescrizione { get { return GetValore("OGGETTO_LabelDescrizione"); } }
        public static string OGGETTO_LabelEspandiComprimi { get { return GetValore("OGGETTO_LabelEspandiComprimi"); } }
        public static string OGGETTO_LabelMari { get { return GetValore("OGGETTO_LabelMari"); } }
        public static string OGGETTO_LabelOpera { get { return GetValore("OGGETTO_LabelOpera"); } }
        public static string OGGETTO_LabelProvince { get { return GetValore("OGGETTO_LabelProvince"); } }
        public static string OGGETTO_LabelRegioni { get { return GetValore("OGGETTO_LabelRegioni"); } }
        public static string OGGETTO_LabelScadenzaOsservazioni { get { return GetValore("OGGETTO_LabelScadenzaOsservazioni"); } }
        public static string OGGETTO_LabelSettorePiano { get { return GetValore("OGGETTO_LabelSettorePiano"); } }
        public static string OGGETTO_LabelSettoreProgramma { get { return GetValore("OGGETTO_LabelSettoreProgramma"); } }
        public static string OGGETTO_LabelTabDocumentazione { get { return GetValore("OGGETTO_LabelTabDocumentazione"); } }
        public static string OGGETTO_LabelTabInfo { get { return GetValore("OGGETTO_LabelTabInfo"); } }
        public static string OGGETTO_LabelTipologiaOpera { get { return GetValore("OGGETTO_LabelTipologiaOpera"); } }
        public static string OGGETTO_LinkLocalizzazione { get { return GetValore("OGGETTO_LinkLocalizzazione"); } }
        public static string OGGETTO_TestoDocumentazioneNonDisponibile { get { return GetValore("OGGETTO_TestoDocumentazioneNonDisponibile"); } }
        public static string OGGETTO_TestoNoComuni { get { return GetValore("OGGETTO_TestoNoComuni"); } }
        public static string OGGETTO_TestoNoMari { get { return GetValore("OGGETTO_TestoNoMari"); } }
        public static string OGGETTO_TestoNoProvince { get { return GetValore("OGGETTO_TestoNoProvince"); } }
        public static string OGGETTO_TestoNoRegioni { get { return GetValore("OGGETTO_TestoNoRegioni"); } }
        public static string OGGETTO_TestoTutteLeProvince { get { return GetValore("OGGETTO_TestoTutteLeProvince"); } }
        public static string OGGETTO_TestoTutteLeRegioni { get { return GetValore("OGGETTO_TestoTutteLeRegioni"); } }
        public static string OGGETTO_TestoTuttiIComuni { get { return GetValore("OGGETTO_TestoTuttiIComuni"); } }
        public static string OGGETTO_TitoloDocumentazione { get { return GetValore("OGGETTO_TitoloDocumentazione"); } }
        public static string OGGETTO_TitoloInformazioniGenerali { get { return GetValore("OGGETTO_TitoloInformazioniGenerali"); } }
        public static string OGGETTO_TitoloIterAmministrativi { get { return GetValore("OGGETTO_TitoloIterAmministrativi"); } }
        public static string OGGETTO_TitoloScegliProcedura { get { return GetValore("OGGETTO_TitoloScegliProcedura"); } }
        public static string OGGETTO_TitoloTerritori { get { return GetValore("OGGETTO_TitoloTerritori"); } }
        public static string PROCEDURE_TestoIntestazioneTabellaInCorso { get { return GetValore("PROCEDURE_TestoIntestazioneTabellaInCorso"); } }
        public static string PROCEDURE_TestoLinkProgettoCartografico { get { return GetValore("PROCEDURE_TestoLinkProgettoCartografico"); } }
        public static string RICERCA_BottoneEsegui { get { return GetValore("RICERCA_BottoneEsegui"); } }
        public static string RICERCA_Esporta { get { return GetValore("RICERCA_Esporta"); } }
        public static string RICERCA_LabelCodiceProcedura { get { return GetValore("RICERCA_LabelCodiceProcedura"); } }
        public static string RICERCA_LabelSceltaDataA { get { return GetValore("RICERCA_LabelSceltaDataA"); } }
        public static string RICERCA_LabelSceltaDataDa { get { return GetValore("RICERCA_LabelSceltaDataDa"); } }
        public static string RICERCA_LabelSceltaDocumenti { get { return GetValore("RICERCA_LabelSceltaDocumenti"); } }
        public static string RICERCA_LabelSceltaOggettiVas { get { return GetValore("RICERCA_LabelSceltaOggettiVas"); } }
        public static string RICERCA_LabelSceltaOggettiVia { get { return GetValore("RICERCA_LabelSceltaOggettiVia"); } }
        public static string RICERCA_LabelSceltaProcedura { get { return GetValore("RICERCA_LabelSceltaProcedura"); } }
        public static string RICERCA_LabelSceltaSettore { get { return GetValore("RICERCA_LabelSceltaSettore"); } }
        public static string RICERCA_LabelSceltaTema { get { return GetValore("RICERCA_LabelSceltaTema"); } }
        public static string RICERCA_LabelSceltaTipologia { get { return GetValore("RICERCA_LabelSceltaTipologia"); } }        
        public static string RICERCA_LabelSceltaTipologiaTerritorio { get { return GetValore("RICERCA_LabelSceltaTipologiaTerritorio"); } }
        public static string RICERCA_LabelTesto { get { return GetValore("RICERCA_LabelTesto"); } }
        public static string RICERCA_NessunRisultato { get { return GetValore("RICERCA_NessunRisultato"); } }
        public static string RICERCA_TitoloLibera { get { return GetValore("RICERCA_TitoloLibera"); } }
        public static string RICERCA_TitoloProcedura { get { return GetValore("RICERCA_TitoloProcedura"); } }
        public static string RICERCA_TitoloRicerca { get { return GetValore("RICERCA_TitoloRicerca"); } }
        public static string RICERCA_TitoloRisultatiDatiAmbientali { get { return GetValore("RICERCA_TitoloRisultatiDatiAmbientali"); } }
        public static string RICERCA_TitoloRisultatiDocumentazione { get { return GetValore("RICERCA_TitoloRisultatiDocumentazione"); } }
        public static string RICERCA_TitoloRisultatiDocumenti { get { return GetValore("RICERCA_TitoloRisultatiDocumenti"); } }
        public static string RICERCA_TitoloRisultatiOggettiVas { get { return GetValore("RICERCA_TitoloRisultatiOggettiVas"); } }
        public static string RICERCA_TitoloRisultatiOggettiVia { get { return GetValore("RICERCA_TitoloRisultatiOggettiVia"); } }
        public static string RICERCA_TitoloRisultatiProvvedimenti { get { return GetValore("RICERCA_TitoloRisultatiProvvedimenti"); } }
        public static string RICERCA_TitoloSettore { get { return GetValore("RICERCA_TitoloSettore"); } }
        public static string RICERCA_TitoloTerritorio { get { return GetValore("RICERCA_TitoloTerritorio"); } }
        public static string RICERCA_TitoloTipologia { get { return GetValore("RICERCA_TitoloTipologia"); } }
        public static string SITO_LabelBreadcrumbs { get { return GetValore("SITO_LabelBreadcrumbs"); } }
        public static string SITO_TestoMenuPrincipale { get { return GetValore("SITO_TestoMenuPrincipale"); } }
        public static string SITO_TestoMenuServizio { get { return GetValore("SITO_TestoMenuServizio"); } }
        public static string SITO_TitoloParte1 { get { return GetValore("SITO_TitoloParte1"); } }
        public static string SITO_TitoloParte2 { get { return GetValore("SITO_TitoloParte2"); } }
        public static string TOOLTIP_Documentazione { get { return GetValore("TOOLTIP_Documentazione"); } }
        public static string TOOLTIP_InfoOggetto { get { return GetValore("TOOLTIP_InfoOggetto"); } }
        public static string TOOLTIP_Metadato { get { return GetValore("TOOLTIP_Metadato"); } }
        public static string TOOLTIP_ProgettoCartografico { get { return GetValore("TOOLTIP_ProgettoCartografico"); } }
        public static string TOOLTIP_ScaricaDocumento { get { return GetValore("TOOLTIP_ScaricaDocumento"); } }
        public static string TOOLTIP_TooltipInvioOsservazioni { get { return GetValore("TOOLTIP_TooltipInvioOsservazioni"); } }
        public static string TOOLTIP_TooltipLocalizzazione { get { return GetValore("TOOLTIP_TooltipLocalizzazione"); } }
        public static string TOOLTIP_TooltipProvvedimento { get { return GetValore("TOOLTIP_TooltipProvvedimento"); } }
        public static string TOOLTIP_TooltipSintesiNonTecnica { get { return GetValore("TOOLTIP_TooltipSintesiNonTecnica"); } }
        public static string RICERCA_BottoneSeleziona { get { return GetValore("RICERCA_BottoneSeleziona"); } }
        public static string RICERCA_LabelLatNord { get { return GetValore("RICERCA_LabelLatNord"); } }
        public static string RICERCA_LabelLatSud { get { return GetValore("RICERCA_LabelLatSud"); } }
        public static string RICERCA_LabelLongEst { get { return GetValore("RICERCA_LabelLongEst"); } }
        public static string RICERCA_LabelLongOvest { get { return GetValore("RICERCA_LabelLongOvest"); } }
        public static string RICERCA_NotaRicercaSpaziale { get { return GetValore("RICERCA_NotaRicercaSpaziale"); } }
        public static string SITO_LabelInvioOsservazioni { get { return GetValore("SITO_LabelInvioOsservazioni"); } }
        public static string RICERCA_TitoloRisultatiAvvisi { get { return GetValore("RICERCA_TitoloRisultatiAvvisi"); } }
        public static string GRIGLIA_ColonnaDataPubblicazione { get { return GetValore("GRIGLIA_ColonnaDataPubblicazione"); } }
        public static string OGGETTO_LabelDettagliProcedura { get { return GetValore("OGGETTO_LabelDettagliProcedura"); } }
        public static string SITO_PrivacyPolicyAlertText { get { return GetValore("SITO_PrivacyPolicyAlertText"); } }
        public static string SITO_PrivacyPolicyAlertButtonText { get { return GetValore("SITO_PrivacyPolicyAlertButtonText"); } }
        public static string SITO_PrivacyPolicyLinkText { get { return GetValore("SITO_PrivacyPolicyLinkText"); } }
        public static string COMUNICAZIONE_ArchivioProvvedimenti { get { return GetValore("COMUNICAZIONE_ArchivioProvvedimenti"); } }
        public static string COMUNICAZIONE_ProvvedimentiAnnoInCorso { get { return GetValore("COMUNICAZIONE_ProvvedimentiAnnoInCorso"); } }
        public static string SITO_Anno { get { return GetValore("SITO_Anno"); } }
        public static string GRIGLIA_ColonnaDataENumeroProvvedimento { get { return GetValore("GRIGLIA_ColonnaDataENumeroProvvedimento"); } }
        public static string SITO_InCorso { get { return GetValore("SITO_InCorso"); } }
        public static string SITO_Avviate { get { return GetValore("SITO_Avviate"); } }
        public static string SITO_Concluse { get { return GetValore("SITO_Concluse"); } }
        public static string RICERCA_FiltraPerAnno { get { return GetValore("RICERCA_FiltraPerAnno"); } }
        public static string SITO_CategoriaNotiziaNessuna { get { return GetValore("SITO_CategoriaNotiziaNessuna"); } }
        public static string SITO_CategoriaNotiziaEventiENotizie { get { return GetValore("SITO_CategoriaNotiziaEventiENotizie"); } }
        public static string SITO_CategoriaNotiziaLaDirezioneInforma { get { return GetValore("SITO_CategoriaNotiziaLaDirezioneInforma"); } }
        public static string SITO_CategoriaNotiziaAreaGiuridica { get { return GetValore("SITO_CategoriaNotiziaAreaGiuridica"); } }
        public static string SITO_CategoriaNotiziaUltimiProvvedimenti { get { return GetValore("SITO_CategoriaNotiziaUltimiProvvedimenti"); } }

        //public static string MENU_Procedure { get { return GetValore("MENU_Procedure"); } }
        //public static string MENU_Provvedimenti { get { return GetValore("MENU_Provvedimenti"); } }

        //public static string MENU_SpazioCittadino { get { return GetValore("MENU_SpazioCittadino"); } }

        //public static string MENU_SpazioProponente { get { return GetValore("MENU_SpazioProponente"); } }

        //public static string MENU_Modulistica { get { return GetValore("MENU_Modulistica"); } }

        public static string MENU_Avvisi { get { return GetValore("MENU_Avvisi"); } }
        public static string PROCEDURE_ViaInCorso { get { return GetValore("PROCEDURE_ViaInCorso"); } }
        public static string PROCEDURE_VasInCorso { get { return GetValore("PROCEDURE_VasInCorso"); } }
        public static string HOME_inEvidenza { get { return GetValore("HOME_inEvidenza"); } }
        public static string HOME_consultazione { get { return GetValore("HOME_consultazione"); } }
        public static string HOME_LeggiTutto { get { return GetValore("HOME_LeggiTutto"); } }
        public static string HOME_InConsultazionePubblica { get { return GetValore("HOME_InConsultazionePubblica"); } }
        public static string HOME_PianiProgrammi { get { return GetValore("HOME_PianiProgrammi"); } }
        public static string HOME_PianiProgrammiLink { get { return GetValore("HOME_PianiProgrammiLink"); } }
        public static string HOME_LeSezioni { get { return GetValore("HOME_LeSezioni"); } }
        public static string HOME_OpenConsultazione { get { return GetValore("HOME_OpenConsultazione"); } }
        public static string HOME_CloseConsultazione { get { return GetValore("HOME_CloseConsultazione"); } }
        public static string TOOLTIP_TooltipSelezionaData { get { return GetValore("TOOLTIP_TooltipSelezionaData"); } }
        public static string PROCEDURE_ViaInCorsoLabel { get { return GetValore("PROCEDURE_ViaInCorsoLabel"); } }
        public static string PROCEDURE_VasInCorsoLabel { get { return GetValore("PROCEDURE_VasInCorsoLabel"); } }
        public static string GRIGLIA_ColonnaDocumento { get { return GetValore("GRIGLIA_ColonnaDocumento"); } }
        public static string RICERCA_LabelSceltaOggettiAia { get { return GetValore("RICERCA_LabelSceltaOggettiAia"); } }
        public static string RICERCA_TitoloInstallazione { get { return GetValore("RICERCA_TitoloInstallazione"); } }
        public static string PROCEDURE_AiaInCorso { get { return GetValore("PROCEDURE_AiaInCorso"); } }
        
        public static string GRIGLIA_ColonnaGestore { get { return GetValore("GRIGLIA_ColonnaGestore"); } }
        public static string RICERCA_TitoloRisultatiOggettiAia { get { return GetValore("RICERCA_TitoloRisultatiOggettiAia"); } }
        public static string GRIGLIA_ColonnaOggettoAia { get { return GetValore("GRIGLIA_ColonnaOggettoAia"); } }
        public static string HOME_OggettiConsultazioneNoRisultatiAIA { get { return GetValore("HOME_OggettiConsultazioneNoRisultatiAIA"); } }
         public static string OGGETTO_LabelRagioneSociale { get { return GetValore("OGGETTO_LabelRagioneSociale"); } }
        public static string OGGETTO_LabelTipologia { get { return GetValore("OGGETTO_LabelTipologia"); } }
        public static string OGGETTO_LabelStatoInstallazione { get { return GetValore("OGGETTO_LabelStatoInstallazione"); } }
        public static string OGGETTO_LabelAttivitaIPPC { get { return GetValore("OGGETTO_LabelAttivitaIPPC"); } }
        public static string OGGETTO_LabelCodiceFiscale { get { return GetValore("OGGETTO_LabelCodiceFiscale"); } }
        public static string OGGETTO_LabelLocalizzazione { get { return GetValore("OGGETTO_LabelLocalizzazione"); } }
        public static string OGGETTO_LabelSitoWeb { get { return GetValore("OGGETTO_LabelSitoWeb"); } }
        public static string OGGETTO_LabelCatAttivita { get { return GetValore("OGGETTO_LabelCatAttivita"); } }
        public static string Error_LabelNotFound { get { return GetValore("Error_LabelNotFound"); } }
        public static string Error_LabelError { get { return GetValore("Error_LabelError"); } }
        public static string GRIGLIA_LabelProv { get { return GetValore("GRIGLIA_LabelProv"); } }
        public static string GRIGLIA_LabelNumProvvedimento { get { return GetValore("GRIGLIA_LabelNumProvvedimento"); } }
        public static string PROCEDURE_Aia { get { return GetValore("PROCEDURE_Aia"); } }
        public static string EVENTI_TestoDocumentazioneNonDisponibile { get { return GetValore("EVENTI_TestoDocumentazioneNonDisponibile"); } }

        public static string RICERCA_LabelNormativa { get { return GetValore("RICERCA_LabelNormativa"); } }

        public static string RICERCA_LabelDataProvvedimento { get { return GetValore("RICERCA_LabelDataProvvedimento"); } }


        public static string RICERCA_LabelCompetenza { get { return GetValore("RICERCA_LabelCompetenza"); } }
    }

}

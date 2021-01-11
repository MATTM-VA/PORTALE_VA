using System;
using VALib.Domain.Common;
using VALib.Configuration;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;
using System.Data;
using VALib.Domain.Entities.DatiAmbientali;
using VALib.Domain.Services;

namespace VALib.Domain.Repositories.DatiAmbientali
{
    public sealed class StratoRepository : Repository
    {
        private static readonly StratoRepository _instance = new StratoRepository(Settings.DivaWebConnectionString);
        //private static readonly string _webCacheKey = "CategorieDocumentiCondivisione";

        private StratoRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static StratoRepository Instance
        {
            get { return _instance; }
        }

        public Strato RecuperaStrato(Guid id)
        {
            Strato strato = new Strato();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            SqlServerExecuteObject sseo2 = null;
            SqlDataReader dr2 = null;

            string sSql = @"SELECT Titolo, Descrizione, NomeTipoRisorsa, IdentificatoreUnico, IdentificatoreLivSuperiore, AltriDettagli, InformazioniSupplementari, FormatiPresentazione, NomeTipoRapprSpaziale, NomeLingua, IdStrato FROM vwMETAIdentificazione WHERE IdStrato = @id;
            SELECT Argomenti FROM vwMETAClassificazione WHERE IdStrato = @id;
            SELECT ParoleChiaveThesaurus, NomeThesaurus, NomeTipoData, DataThesaurus FROM vwMETAParoleChiave WHERE IdStrato = @id;
            SELECT DelimitazioneGeograficaLatNord, DelimitazioneGeograficaLatSud, DelimitazioneGeograficaLonEst, DelimitazioneGeograficaLonOvest FROM vwMETALocalizzazione WHERE IdStrato = @id;
            SELECT Nazioni, Regioni, Province, Comuni FROM vwMETATerritori WHERE IdStrato = @id;
            SELECT EstensioneTemporale, DataPubblicazione, DataRevisione, DataCreazione FROM vwMETARiferimentoTemporale WHERE IdStrato = @id;
            SELECT GenealogiaProcessoProduzione, Scala, DistanzaX, DistanzaY FROM vwMETAQualitaValidita WHERE IdStrato = @id;
            SELECT NomeSistemaRiferimento FROM vwMETASistemaRiferimento WHERE IdStrato = @id;
            SELECT TitoloSpecifica, DataSpecifica, NomeTipoData, ConformitaSpecifica FROM vwMETAConformita WHERE IdStrato = @id;
            SELECT LimitazioneAccesso, VincoliAccesso, VincoliFruibilita, AltriVincoli, NomeRestrizioneDati FROM vwMETAVincoliAccesso WHERE IdStrato = @id;
            SELECT NomeResponsabileDati, EmailResponsabileDati, TelefonoResponsabileDati, IndirizzoWebResponsabileDati, NomeRuoloResponsabile FROM vwMETAResponsabiliDati WHERE IdStrato = @id AND IdRuoloResponsabileDati <> 5;
            SELECT FormatoDati, DescVer FROM vwMETADistribuzione WHERE IdStrato = @id;
            SELECT NomeResponsabileDati, EmailResponsabileDati, TelefonoResponsabileDati, IndirizzoWebResponsabileDati, NomeRuoloResponsabile FROM vwMETAResponsabiliDati WHERE IdStrato = @id AND IdRuoloResponsabileDati = 5;
            SELECT DenominazioneResponsabileMetaDati, EmailResponsabileMetaDati, TelefonoResponsabileMetaDati, IndirizzoWebResponsabileMetaDati, NomeRuoloResponsabile FROM vwMETAResponsabiliMetaDati WHERE IdStrato = @id;
            SELECT DataMetadati, LinguaMetadati, IdentificatoreUnicoMetadati, IdentificatoreLivSuperioreMetadati FROM vwMETAMetadati WHERE IdStrato = @id;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;
            sseo.SqlParameters.AddWithValue("@id", id);

            sseo2 = new SqlServerExecuteObject();
            sseo2.CommandText = "SELECT * FROM vwMETARisorseOnline WHERE IdRisorsa = @id;";
            sseo2.CommandType = CommandType.Text;
            sseo2.SqlParameters.AddWithValue("@id", id);

            dr = SqlProvider.ExecuteReaderObject(sseo);
            dr2 = SqlProvider.ExecuteReaderObject(sseo2);

            while (dr.Read())
            {
                if (!dr.IsDBNull(0))
                {
                    strato.Identificazione.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelTitolo, dr.GetString(0)));
                    strato.Titolo = dr.GetString(0);
                }
                if (!dr.IsDBNull(1))
                    strato.Identificazione.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelDescrizione, dr.GetString(1)));

                if (!dr.IsDBNull(2))
                    strato.Identificazione.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelTipoRisorsa, dr.GetString(2)));

                if (dr2.HasRows)
                    strato.Identificazione.Add(new Tuple<string, string>("TabellaLink", ""));
                
                if (!dr.IsDBNull(3))
                    strato.Identificazione.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelIdentificatoreUnico, dr.GetString(3)));

                if (!dr.IsDBNull(4))
                    strato.Identificazione.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelIdentificatoreLivelloSuperiore, dr.GetString(4)));

                if (!dr.IsDBNull(5))
                    strato.Identificazione.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelAltriDettagli, dr.GetString(5)));

                if (!dr.IsDBNull(6))
                    strato.Identificazione.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelInformazioneSupplementari, dr.GetString(6)));

                if (!dr.IsDBNull(7))
                    strato.Identificazione.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelFormatoPresentazione, dr.GetString(7)));

                if (!dr.IsDBNull(8))
                    strato.Identificazione.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelTipoRappresentazioneSpaziale, dr.GetString(8)));

                if (!dr.IsDBNull(9))
                    strato.Identificazione.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelLingua, dr.GetString(9)));

                strato.Identificazione.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelIdentificatoreUnico, id.ToString()));
            }

            dr.NextResult();

            while (dr.Read())
            {
                if (!dr.IsDBNull(0))
                    strato.Classificazione.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelArgomenti, dr.GetString(0)));
            }

            dr.NextResult();

            while (dr.Read())
            {
                if (!dr.IsDBNull(0))
                    strato.ParoleChiave.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelParoleChiave, dr.GetString(0)));

                if (!dr.IsDBNull(1))
                    strato.ParoleChiave.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelThesaurus, dr.GetString(1)));

                if (!dr.IsDBNull(2))
                    strato.ParoleChiave.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelTipoData, dr.GetString(2)));

                if (!dr.IsDBNull(3))
                    strato.ParoleChiave.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelData, dr.GetString(3)));

            }

            dr.NextResult();

            while (dr.Read())
            {
                if (!dr.IsDBNull(0))
                    strato.Localizzazione.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelLatitudineNord, dr.GetDecimal(0).ToString()));

                if (!dr.IsDBNull(1))
                    strato.Localizzazione.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelLatitudineSud, dr.GetDecimal(1).ToString()));

                if (!dr.IsDBNull(2))
                    strato.Localizzazione.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelLongitudineEst, dr.GetDecimal(2).ToString()));

                if (!dr.IsDBNull(3))
                    strato.Localizzazione.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelLongitudineOvest, dr.GetDecimal(3).ToString()));

            }

            dr.NextResult();

            while (dr.Read())
            {
                if (!dr.IsDBNull(0))
                    strato.Localizzazione.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelNazioni, dr.GetString(0)));

                if (!dr.IsDBNull(1))
                    strato.Localizzazione.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelRegioni, dr.GetString(1)));

                if (!dr.IsDBNull(2))
                    strato.Localizzazione.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelProvince, dr.GetString(2)));

                if (!dr.IsDBNull(3))
                    strato.Localizzazione.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelComuni, dr.GetString(3)));

            }

            dr.NextResult();

            while (dr.Read())
            {
                if (!dr.IsDBNull(0))
                    strato.RiferimentoTemporale.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelEstensioneTemporale, dr.GetString(0)));

                if (!dr.IsDBNull(1))
                    strato.RiferimentoTemporale.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelDataPubblicazione, dr.GetString(1)));

                if (!dr.IsDBNull(2))
                    strato.RiferimentoTemporale.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelDataRevisione, dr.GetString(2)));

                if (!dr.IsDBNull(3))
                    strato.RiferimentoTemporale.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelDataCreazione, dr.GetString(3)));

            }

            dr.NextResult();

            while (dr.Read())
            {
                if (!dr.IsDBNull(0))
                    strato.Qualita.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelGenealogiaProcessoProduzione, dr.GetString(0)));

                if (!dr.IsDBNull(1))
                    strato.Qualita.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelScala, dr.GetString(1)));

                if (!dr.IsDBNull(2))
                    strato.Qualita.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelDistanzaX, dr.GetDecimal(2).ToString()));

                if (!dr.IsDBNull(3))
                    strato.Qualita.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelDistanzaY, dr.GetDecimal(3).ToString()));

            }

            dr.NextResult();

            while (dr.Read())
            {
                if (!dr.IsDBNull(0))
                    strato.SistemaRiferimento.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelSistemaRiferimento, dr.GetString(0)));
            }

            dr.NextResult();

            while (dr.Read())
            {
                if (!dr.IsDBNull(0))
                    strato.Conformita.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelTitoloSpecifiche, dr.GetString(0)));

                if (!dr.IsDBNull(1))
                    strato.Conformita.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelData, dr.GetString(1)));

                if (!dr.IsDBNull(2))
                    strato.Conformita.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelTipoData, dr.GetString(2)));

                if (!dr.IsDBNull(3))
                    strato.Conformita.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelConformitaSpecifica, dr.GetString(3)));

            }

            dr.NextResult();

            while (dr.Read())
            {
                if (!dr.IsDBNull(0))
                    strato.Vincoli.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelCondizioniApplicabili, dr.GetString(0)));

                if (!dr.IsDBNull(1))
                    strato.Vincoli.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelVincoliAccesso, dr.GetString(1)));

                if (!dr.IsDBNull(2))
                    strato.Vincoli.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelVincoliFruibilita, dr.GetString(2)));

                if (!dr.IsDBNull(3))
                    strato.Vincoli.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelAltriVincoli, dr.GetString(3)));

                if (!dr.IsDBNull(4))
                    strato.Vincoli.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelVincoliSicurezza, dr.GetString(4)));

            }

            dr.NextResult();

            while (dr.Read())
            {
                if (!dr.IsDBNull(0))
                    strato.Responsabili.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelEnte, dr.GetString(0)));

                if (!dr.IsDBNull(1))
                    strato.Responsabili.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelIndirizzoEmail, dr.GetString(1)));

                if (!dr.IsDBNull(2))
                    strato.Responsabili.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelNumeroTelefono, dr.GetString(2)));

                if (!dr.IsDBNull(3))
                    strato.Responsabili.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelIndirizzoWeb, dr.GetString(3)));

                if (!dr.IsDBNull(4))
                    strato.Responsabili.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelRuolo, dr.GetString(4)));

            }


            dr.NextResult();

            while (dr.Read())
            {
                if (!dr.IsDBNull(0))
                    strato.Distribuzione.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelFormatoDati, dr.GetString(0)));

                if (!dr.IsDBNull(1))
                    strato.Distribuzione.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelVersioneFormato, dr.GetString(1)));

            }

            dr.NextResult();

            while (dr.Read())
            {
                if (!dr.IsDBNull(0))
                    strato.Distribuzione.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelEnte, dr.GetString(0)));

                if (!dr.IsDBNull(1))
                    strato.Distribuzione.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelIndirizzoEmail, dr.GetString(1)));

                if (!dr.IsDBNull(2))
                    strato.Distribuzione.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelNumeroTelefono, dr.GetString(2)));

                if (!dr.IsDBNull(3))
                    strato.Distribuzione.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelIndirizzoWeb, dr.GetString(3)));

                if (!dr.IsDBNull(4))
                    strato.Distribuzione.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelRuolo, dr.GetString(4)));

            }

            dr.NextResult();

            while (dr.Read())
            {
                if (!dr.IsDBNull(0))
                    strato.Metadati.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelEnte, dr.GetString(0)));

                if (!dr.IsDBNull(1))
                    strato.Metadati.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelIndirizzoEmail, dr.GetString(1)));

                if (!dr.IsDBNull(2))
                    strato.Metadati.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelNumeroTelefono, dr.GetString(2)));

                if (!dr.IsDBNull(3))
                    strato.Metadati.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelIndirizzoWeb, dr.GetString(3)));

                if (!dr.IsDBNull(4))
                    strato.Metadati.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelRuolo, dr.GetString(4)));

            }
            
            dr.NextResult();

            while (dr.Read())
            {
                if (!dr.IsDBNull(0))
                    strato.Metadati.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelDataMetadati, dr.GetString(0)));

                if (!dr.IsDBNull(1))
                    strato.Metadati.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelLinguaMetadati, dr.GetString(1)));

                if (!dr.IsDBNull(2))
                    strato.Metadati.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelIdentificatoreUnicoMetadati, dr.GetString(2)));

                if (!dr.IsDBNull(3))
                    strato.Metadati.Add(new Tuple<string, string>(DizionarioService.METADATO_LabelIdentificatoreUnicoMetadatiSuperiore, dr.GetString(3)));

            }
            
            while (dr2.Read())
            {
                StratoMetadatiLink sml = new StratoMetadatiLink();

                sml.Denominazione = dr2.GetString(3);
                sml.LinkTesto = dr2.GetString(1);
                sml.LinkTooltip = dr2.GetString(4);
                sml.LinkUrl = dr2.GetString(2);
                sml.LinkTarget = "_blank";

                strato.Links.Add(sml);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            if (dr2 != null)
            {
                dr2.Close();
                dr2.Dispose();
            }

            return strato;
        }

    }
}

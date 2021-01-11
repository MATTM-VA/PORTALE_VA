using System;
using System.Collections.Generic;
using System.Linq;
using VALib.Domain.Common;
using VALib.Configuration;
using VALib.Domain.Entities.Contenuti;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;
using System.Data;
using VALib.Web;
using VALib.Domain.Entities.Contenuti.Base;

namespace VALib.Domain.Repositories.Contenuti
{
    public sealed class OggettoRepository : Repository
    {
        private static readonly OggettoRepository _instance = new OggettoRepository(Settings.VAConnectionString);

        private OggettoRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static OggettoRepository Instance
        {
            get { return _instance; }
        }

        private TipoOggettoEnum? RecuperaTipoOggetto(int oggettoID)
        {
            TipoOggettoEnum? tipoOggetto = null;
            SqlServerExecuteObject sseo = null;
            object result = null;

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = "SELECT TipoOggettoID FROM dbo.TBL_Oggetti WHERE OggettoID = @OggettoID";
            sseo.CommandType = CommandType.Text;
            sseo.SqlParameters.AddWithValue("@OggettoID", oggettoID);

            result = SqlProvider.ExecuteScalarObject(sseo);

            if (result != null)
                tipoOggetto = (TipoOggettoEnum?)((int)result);

            return tipoOggetto;
        }

        public object RecuperaOggettoInfo(int id)
        {
            TipoOggettoEnum? tipoOggetto = null;
            object oggetto = null;

            tipoOggetto = RecuperaTipoOggetto(id);

            if (tipoOggetto.HasValue && tipoOggetto.Value != TipoOggettoEnum.Nessuno)
            {
                switch (tipoOggetto.Value)
                {
                    case TipoOggettoEnum.Progetto:
                        oggetto = RecuperaOggettoInfoVia(id);
                        break;
                    case TipoOggettoEnum.Piano:
                    case TipoOggettoEnum.Programma:
                        oggetto = RecuperaOggettoInfoVas(id);
                        break;
                    case TipoOggettoEnum.Impianto:
                        oggetto = RecuperaOggettoInfoAIA(id);
                        break;
                    default:
                        break;
                }
            }

            return oggetto;
        }

        public object RecuperaOggettoInfoAiaRegionale(int id)
        {
            TipoOggettoEnum? tipoOggetto = null;
            object oggetto = null;

            tipoOggetto = RecuperaTipoOggetto(id);

            if (tipoOggetto.HasValue && tipoOggetto.Value != TipoOggettoEnum.Nessuno)
            {

                oggetto = RecuperaOggettoInfoAIARegionale(id);

            }

            return oggetto;
        }

        private OggettoInfoVia RecuperaOggettoInfoVia(int id)
        {
            OggettoInfoBase oggetto = null;
            OggettoInfoVia result = null;

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = "dbo.SP_RecuperaInfoOggettoVia";
            sseo.CommandType = CommandType.StoredProcedure;
            sseo.SqlParameters.AddWithValue("@OggettoID", id);

            dr = SqlProvider.ExecuteReaderObject(sseo);


            // Informazioni Comuni tra via e vas
            oggetto = RiempiIstanzaOggettoInfoBase(dr);

           
            if (oggetto != null)
            {
                result = new OggettoInfoVia();

                result.ID = oggetto.ID;
                result._nome_IT = oggetto._nome_IT;
                result._nome_EN = oggetto._nome_EN;
                result._descrizione_IT = oggetto._descrizione_IT;
                result._descrizione_EN = oggetto._descrizione_EN;
                result.TipoOggetto = oggetto.TipoOggetto;
                result.OggettoProceduraID = oggetto.OggettoProceduraID;

                result.ScadenzaPresentazioneOsservazioni = oggetto.ScadenzaPresentazioneOsservazioni;
                result.LinkLocalizzazione = oggetto.LinkLocalizzazione;
                result.EntitaCollegate = oggetto.EntitaCollegate;
                result.LinkCollegati = oggetto.LinkCollegati;
                result.Territori = oggetto.Territori;
                result.ProcedureCollegate = oggetto.ProcedureCollegate;
                result.DatiAmministrativi = oggetto.DatiAmministrativi;

                oggetto = null;
            }

            if (result != null)
            {
                dr.NextResult();
                dr.Read();
                // Opera, Cup
                Opera opera = new Opera();

                opera.ID = dr.GetInt32(0);
                opera.Tipologia = TipologiaRepository.Instance.RecuperaTipologia(dr.GetInt32(1));
                opera._nome_IT = dr.GetString(2);
                opera._nome_EN = dr.GetString(3);

                result.Opera = opera;
                result.Cup = dr.IsDBNull(4) ? "" : dr.GetString(4);

                dr.NextResult();

                result.AltriOggetti = new List<OggettoElenco>();

                while (dr.Read())
                {
                    OggettoElenco oggettoElenco = new OggettoElenco();
                    oggettoElenco.ID = dr.GetInt32(0);
                    oggettoElenco.Procedura = ProceduraRepository.Instance.RecuperaProcedura(dr.GetInt32(1));
                    oggettoElenco.TipoOggetto = TipoOggettoRepository.Instance.RecuperaTipoOggetto(dr.GetInt32(2));
                    oggettoElenco._nome_IT = dr.GetString(3);
                    oggettoElenco._nome_EN = dr.GetString(4);
                    oggettoElenco.Proponente = dr.GetString(5);
                    oggettoElenco.OggettoProceduraID = dr.GetInt32(6);

                    result.AltriOggetti.Add(oggettoElenco);
                }
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return result;
        }

        private OggettoInfoVas RecuperaOggettoInfoVas(int id)
        {
            OggettoInfoBase oggetto = null;
            OggettoInfoVas result = null;

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = "dbo.SP_RecuperaInfoOggettoVas";
            sseo.CommandType = CommandType.StoredProcedure;
            sseo.SqlParameters.AddWithValue("@OggettoID", id);

            dr = SqlProvider.ExecuteReaderObject(sseo);


            // Informazioni Comuni tra via e vas
            oggetto = RiempiIstanzaOggettoInfoBase(dr);
 
            if (oggetto != null)
            {
                result = new OggettoInfoVas();

                result.ID = oggetto.ID;
                result._nome_IT = oggetto._nome_IT;
                result._nome_EN = oggetto._nome_EN;
                result._descrizione_IT = oggetto._descrizione_IT;
                result._descrizione_EN = oggetto._descrizione_EN;
                result.TipoOggetto = oggetto.TipoOggetto;
                result.OggettoProceduraID = oggetto.OggettoProceduraID;

                result.ScadenzaPresentazioneOsservazioni = oggetto.ScadenzaPresentazioneOsservazioni;
                result.LinkLocalizzazione = oggetto.LinkLocalizzazione;
                result.EntitaCollegate = oggetto.EntitaCollegate;
                result.LinkCollegati = oggetto.LinkCollegati;
                result.Territori = oggetto.Territori;
                result.ProcedureCollegate = oggetto.ProcedureCollegate;
                result.DatiAmministrativi = oggetto.DatiAmministrativi;

                oggetto = null;
            }

            if (result != null)
            {
                dr.NextResult();
                dr.Read();
                // settore
                result.Settore = SettoreRepository.Instance.RecuperaSettore(dr.GetInt32(0));

            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return result;
        }

        private OggettoInfoAIA RecuperaOggettoInfoAIA(int id)
        {
            OggettoInfoBase oggetto = null;
            OggettoInfoAIA result = null;

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = "dbo.SP_RecuperaInfoOggettoAIA";
            sseo.CommandType = CommandType.StoredProcedure;
            sseo.SqlParameters.AddWithValue("@OggettoID", id);

            dr = SqlProvider.ExecuteReaderObject(sseo);


            // Informazioni Comuni tra via, vas e aia
            oggetto = RiempiIstanzaOggettoInfoBase(dr);
 
            if (oggetto != null)
            {
                result = new OggettoInfoAIA();

                result.ID = oggetto.ID;
                result._nome_IT = oggetto._nome_IT;
                result._nome_EN = oggetto._nome_EN;
                result._descrizione_IT = oggetto._descrizione_IT;
                result._descrizione_EN = oggetto._descrizione_EN;
                result.TipoOggetto = oggetto.TipoOggetto;
                result.OggettoProceduraID = oggetto.OggettoProceduraID;

                result.ScadenzaPresentazioneOsservazioni = oggetto.ScadenzaPresentazioneOsservazioni;
                result.LinkLocalizzazione = oggetto.LinkLocalizzazione;
                result.EntitaCollegate = oggetto.EntitaCollegate;
                result.LinkCollegati = oggetto.LinkCollegati;
                result.Territori = oggetto.Territori;
                result.ProcedureCollegate = oggetto.ProcedureCollegate;
                result.DatiAmministrativi = oggetto.DatiAmministrativi;

                oggetto = null;
            }

            if (result != null)
            {
                dr.NextResult();

                // Attivita IPPC
                List<AttivitaIPPC> listaIPPC = new List<AttivitaIPPC>();

                int i = 0;
                while (dr.Read())
                {
                    AttivitaIPPC IPPC = new AttivitaIPPC();
                    if (i == 0)
                    {
                        // Categoria impianto
                        result.CategoriaImpianto = !dr.IsDBNull(0) ? CategoriaImpiantoRepository.Instance.RecuperaCategoria(dr.GetInt32(0)) : null;
                        // Stato impianto
                        result.StatoImpianto = StatoImpiantoRepository.Instance.RecuperaStato(dr.GetInt32(1));
                        // Localizzazione impianto
                        result.IndirizzoImpianto = String.Format("{0} - {1}", dr.IsDBNull(2) ? "" : dr.GetString(2), dr.IsDBNull(3) ? "" : dr.GetString(3));

                    }
                    if (!dr.IsDBNull(4))
                    {
                        IPPC.ID = dr.GetInt32(4);
                        IPPC.Codice = dr.IsDBNull(5) ? "" : dr.GetString(5);
                        IPPC._nome_IT = dr.IsDBNull(6) ? "" : dr.GetString(6);
                        IPPC._nome_EN = dr.IsDBNull(7) ? "" : dr.GetString(7);
                    }

                    listaIPPC.Add(IPPC);
                    i++;
                }
                result.AttivitaIPPC = listaIPPC;
            }

 


            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return result;
        }

        private OggettoInfoAIA RecuperaOggettoInfoAIARegionale(int id)
        {
            OggettoInfoBase oggetto = null;
            OggettoInfoAIA result = null;

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = "dbo.SP_RecuperaInfoOggettoAIA";
            sseo.CommandType = CommandType.StoredProcedure;
            sseo.SqlParameters.AddWithValue("@OggettoID", id);
            dr = SqlProvider.ExecuteReaderObject(sseo);

            oggetto = RiempiIstanzaOggettoInfoAiaRegionale(dr);

            if (oggetto != null)
            {
                result = new OggettoInfoAIA();

                result.ID = oggetto.ID;
                result._nome_IT = oggetto._nome_IT;
                result._nome_EN = oggetto._nome_EN;
                result._descrizione_IT = oggetto._descrizione_IT;
                result._descrizione_EN = oggetto._descrizione_EN;
                result.TipoOggetto = oggetto.TipoOggetto;
                result.OggettoProceduraID = oggetto.OggettoProceduraID;

                result.ScadenzaPresentazioneOsservazioni = oggetto.ScadenzaPresentazioneOsservazioni;
                result.LinkLocalizzazione = oggetto.LinkLocalizzazione;
                result.EntitaCollegate = oggetto.EntitaCollegate;
                result.LinkCollegati = oggetto.LinkCollegati;
                result.Territori = oggetto.Territori;
              

                oggetto = null;
            }

            if (result != null)
            {
                // Avanzare sino al recordSet Categoria/Attivita IPPC
                dr.NextResult();
                dr.NextResult();
                dr.NextResult();
                dr.NextResult();


                // Attivita IPPC
                List<AttivitaIPPC> listaIPPC = new List<AttivitaIPPC>();

                int i = 0;
                while (dr.Read())
                {
                    AttivitaIPPC IPPC = new AttivitaIPPC();
                    if (i == 0)
                    {
                        // Categoria impianto
                        result.CategoriaImpianto = dr.IsDBNull(0) ? null : CategoriaImpiantoRepository.Instance.RecuperaCategoria(dr.GetInt32(0));
                        // Stato impianto
                        result.StatoImpianto = StatoImpiantoRepository.Instance.RecuperaStato(dr.GetInt32(1));
                        // Localizzazione impianto
                        result.IndirizzoImpianto = String.Format("{0} - {1}", dr.IsDBNull(2) ? "" : dr.GetString(2), dr.IsDBNull(3) ? "" : dr.GetString(3));

                    }
                    if (!dr.IsDBNull(4))
                    {
                        IPPC.ID = dr.GetInt32(4);
                        IPPC.Codice = dr.IsDBNull(5) ? "" : dr.GetString(5);
                        IPPC._nome_IT = dr.IsDBNull(6) ? "" : dr.GetString(6);
                        IPPC._nome_EN = dr.IsDBNull(7) ? "" : dr.GetString(7);
                    }

                    listaIPPC.Add(IPPC);
                    i++;
                }
                result.AttivitaIPPC = listaIPPC;
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return result;
        }

        private OggettoInfoBase RiempiIstanzaOggettoInfoBase(SqlDataReader dr)
        {
            // Informazioni Comuni tra via e vas
            OggettoInfoBase oggetto = null;

            List<RuoloEntita> ruoliEntita = RuoloEntitaRepository.Instance.RecuperaRuoliEntita();
            List<TipoLink> tipiLink = TipoLinkRepository.Instance.RecuperaTipiLink();
            List<TipologiaTerritorio> tipologieTerritorio = TipologiaTerritorioRepository.Instance.RecuperaTipologieTerritorio();
            IEnumerable<DatoAmministrativo> datiAmministrativi = DatoAmministrativoRepository.Instance.RecuperaDatiAmministrativi();
            List<Procedura> procedure = ProceduraRepository.Instance.RecuperaProcedure();
            List<StatoProcedura> statiProcedura;

            while (dr.Read())
            {
                oggetto = new OggettoInfoBase();

                oggetto.ID = dr.GetInt32(0);
                oggetto.TipoOggetto = TipoOggettoRepository.Instance.RecuperaTipoOggetto(dr.GetInt32(1));
                oggetto._nome_IT = dr.GetString(2);
                oggetto._nome_EN = dr.GetString(3);
                oggetto._descrizione_IT = dr.IsDBNull(4) ? "" : dr.GetString(4);
                oggetto._descrizione_EN = dr.IsDBNull(5) ? "" : dr.GetString(5);
                oggetto.LinkLocalizzazione = dr.IsDBNull(6) ? null : LinkUtility.LinkLocalizzazione(dr.GetString(6), oggetto.ID);
                oggetto.ScadenzaPresentazioneOsservazioni = dr.IsDBNull(7) ? null : (DateTime?)dr.GetDateTime(7);
                oggetto.OggettoProceduraID = dr.GetInt32(8);
            }

            if (oggetto.TipoOggetto.MacroTipoOggetto.Enum.Equals(MacroTipoOggettoEnum.Aia))
            {
                statiProcedura = StatoProceduraAIARepository.Instance.RecuperaStatiProceduraAIA();
            }
            else
            {
                statiProcedura = StatoProceduraVIPERARepository.Instance.RecuperaStatiProceduraVIPERA();
            }

            if (oggetto != null)
            {
                // ENTITA
                dr.NextResult();

                while (dr.Read())
                {
                    EntitaCollegata entitaCollegata = new EntitaCollegata();

                    entitaCollegata.Entita = new Entita(dr.GetInt32(0), dr.GetString(1), dr.IsDBNull(3) ? "" : dr.GetString(3),
                          dr.IsDBNull(4) ? "" : dr.GetString(4), dr.IsDBNull(5) ? "" : dr.GetString(5),
                          dr.IsDBNull(6) ? "" : dr.GetString(6), dr.IsDBNull(7) ? "" : dr.GetString(7),
                          dr.IsDBNull(8) ? "" : dr.GetString(8));

                    entitaCollegata.Ruolo = ruoliEntita.Single(x => x.ID == dr.GetInt32(2));

                    oggetto.EntitaCollegate.Add(entitaCollegata);
                }

                // LINK
                dr.NextResult();

                while (dr.Read())
                {
                    LinkCollegato linkCollegato = new LinkCollegato();

                    linkCollegato.Link = new Link(dr.GetInt32(0), dr.GetString(1), dr.GetString(2), dr.GetString(3));
                    linkCollegato.Tipo = tipiLink.Single(x => x.ID == dr.GetInt32(4));

                    oggetto.LinkCollegati.Add(linkCollegato);
                }

                // TERRITORI
                dr.NextResult();

                while (dr.Read())
                {
                    Territorio territorio = new Territorio();

                    territorio.ID = dr.GetGuid(0);
                    territorio.GenitoreID = dr.IsDBNull(1) ? null : (Guid?)dr.GetGuid(1);
                    territorio.Tipologia = tipologieTerritorio.Single(x => x.ID == dr.GetInt32(2));
                    territorio.Nome = dr.GetString(3);
                    territorio.CodiceIstat = dr.IsDBNull(4) ? "" : dr.GetString(4);

                    oggetto.Territori.Add(territorio);
                }

                // PROCEDURE COLLEGATE
                dr.NextResult();

                while (dr.Read())
                {
                    ProceduraCollegata proceduraCollegata = new ProceduraCollegata();
                    StatoProcedura statoProcedura = null;
                    if (!dr.IsDBNull(2))
                        statoProcedura = statiProcedura.FirstOrDefault(x => x.ID == dr.GetInt32(2));

                    proceduraCollegata.OggettoProceduraID = dr.GetInt32(0);
                    proceduraCollegata.Procedura = procedure.FirstOrDefault(x => x.ID == dr.GetInt32(1));
                    proceduraCollegata.StatoProcedura = statoProcedura;
                    proceduraCollegata.Data = dr.IsDBNull(3) ? null : (DateTime?)dr.GetDateTime(3);
                    proceduraCollegata.NumeroDocumenti = dr.GetInt32(4);
                    proceduraCollegata.ViperaAiaID = dr.IsDBNull(5) ? null : dr.GetString(5);

                    oggetto.ProcedureCollegate.Add(proceduraCollegata);
                }

                // DATI AMMINISTRATIVI
                dr.NextResult();

                while (dr.Read())
                {
                    ValoreDatoAmministrativo valoreDatoAmministrativo = new ValoreDatoAmministrativo();

                    valoreDatoAmministrativo.OggettoProceduraID = dr.GetInt32(0);
                    valoreDatoAmministrativo.Procedura = procedure.Single(x => x.ID == dr.GetInt32(1));
                    valoreDatoAmministrativo._vBool = dr.IsDBNull(2) ? null : (bool?)dr.GetBoolean(2);
                    valoreDatoAmministrativo._vDatetime = dr.IsDBNull(3) ? null : (DateTime?)dr.GetDateTime(3);
                    valoreDatoAmministrativo._vDouble = dr.IsDBNull(4) ? null : (double?)dr.GetDouble(4);
                    valoreDatoAmministrativo._vString = dr.IsDBNull(5) ? "" : dr.GetString(5);
                    valoreDatoAmministrativo.DatoAmministrativo = datiAmministrativi.Single(x => x.ID == dr.GetInt32(6));
                    valoreDatoAmministrativo.ViperaAiaID = dr.IsDBNull(7) ? null : dr.GetString(7);

                    oggetto.DatiAmministrativi.Add(valoreDatoAmministrativo);
                }

                 dr.NextResult();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        StatoProcedura statoProcedura = null;
                        if (!dr.IsDBNull(3))
                            statoProcedura = statiProcedura.FirstOrDefault(x => x.ID == dr.GetInt32(3));

                        ValoreDatoAmministrativo valoreDatoAmministrativo = new ValoreDatoAmministrativo();
                        valoreDatoAmministrativo.OggettoProceduraID = dr.GetInt32(0);
                        valoreDatoAmministrativo.Procedura = procedure.Single(x => x.ID == dr.GetInt32(1));
                        valoreDatoAmministrativo._vBool = null;
                        valoreDatoAmministrativo._vDatetime = null;
                        valoreDatoAmministrativo._vDouble = null;
                        valoreDatoAmministrativo._vString = statoProcedura != null ? statoProcedura.GetNome() : "";
                        valoreDatoAmministrativo.DatoAmministrativo = datiAmministrativi.FirstOrDefault(x => x.ID == dr.GetInt32(2));
                        valoreDatoAmministrativo.ViperaAiaID = dr.IsDBNull(4) ? null : dr.GetString(4);

                        oggetto.DatiAmministrativi.Add(valoreDatoAmministrativo);
                    }
                }
            }

            return oggetto;
        }


        private OggettoInfoBase RiempiIstanzaOggettoInfoAiaRegionale(SqlDataReader dr)
        {
            // Informazioni Comuni tra via e vas
            OggettoInfoBase oggetto = null;

            List<RuoloEntita> ruoliEntita = RuoloEntitaRepository.Instance.RecuperaRuoliEntita();
            List<TipoLink> tipiLink = TipoLinkRepository.Instance.RecuperaTipiLink();
            List<TipologiaTerritorio> tipologieTerritorio = TipologiaTerritorioRepository.Instance.RecuperaTipologieTerritorio();
            List<StatoProcedura> statiProcedura;

            while (dr.Read())
            {
                oggetto = new OggettoInfoBase();

                oggetto.ID = dr.GetInt32(0);
                oggetto.TipoOggetto = TipoOggettoRepository.Instance.RecuperaTipoOggetto(dr.GetInt32(1));
                oggetto._nome_IT = dr.GetString(2);
                oggetto._nome_EN = dr.GetString(3);
                oggetto._descrizione_IT = dr.IsDBNull(4) ? "" : dr.GetString(4);
                oggetto._descrizione_EN = dr.IsDBNull(5) ? "" : dr.GetString(5);
                oggetto.LinkLocalizzazione = dr.IsDBNull(6) ? null : LinkUtility.LinkLocalizzazione(dr.GetString(6), oggetto.ID);
                oggetto.ScadenzaPresentazioneOsservazioni = dr.IsDBNull(7) ? null : (DateTime?)dr.GetDateTime(7);
                oggetto.OggettoProceduraID = dr.GetInt32(8);
            }

            statiProcedura = StatoProceduraAIARepository.Instance.RecuperaStatiProceduraAIA();

            if (oggetto != null)
            {
                // ENTITA
                dr.NextResult();

                while (dr.Read())
                {
                    EntitaCollegata entitaCollegata = new EntitaCollegata();

                    entitaCollegata.Entita = new Entita(dr.GetInt32(0), dr.GetString(1), dr.IsDBNull(3) ? "" : dr.GetString(3),
                          dr.IsDBNull(4) ? "" : dr.GetString(4), dr.IsDBNull(5) ? "" : dr.GetString(5),
                          dr.IsDBNull(6) ? "" : dr.GetString(6), dr.IsDBNull(7) ? "" : dr.GetString(7),
                          dr.IsDBNull(8) ? "" : dr.GetString(8));

                    entitaCollegata.Ruolo = ruoliEntita.Single(x => x.ID == dr.GetInt32(2));

                    oggetto.EntitaCollegate.Add(entitaCollegata);
                }

                // LINK
                dr.NextResult();

                while (dr.Read())
                {
                    LinkCollegato linkCollegato = new LinkCollegato();

                    linkCollegato.Link = new Link(dr.GetInt32(0), dr.GetString(1), dr.GetString(2), dr.GetString(3));
                    linkCollegato.Tipo = tipiLink.Single(x => x.ID == dr.GetInt32(4));

                    oggetto.LinkCollegati.Add(linkCollegato);
                }

                // TERRITORI
                dr.NextResult();

                while (dr.Read())
                {
                    Territorio territorio = new Territorio();

                    territorio.ID = dr.GetGuid(0);
                    territorio.GenitoreID = dr.IsDBNull(1) ? null : (Guid?)dr.GetGuid(1);
                    territorio.Tipologia = tipologieTerritorio.Single(x => x.ID == dr.GetInt32(2));
                    territorio.Nome = dr.GetString(3);
                    territorio.CodiceIstat = dr.IsDBNull(4) ? "" : dr.GetString(4);

                    oggetto.Territori.Add(territorio);
                }
            }

            return oggetto;
        }


        public OggettoDocumentazioneBase RecuperaOggettoDocumentazione(int oggettoID, int oggettoProceduraID)
        {
            OggettoDocumentazioneBase oggetto = null;
          
            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = "dbo.SP_RecuperaDocumentazioneOggettoBase";
            sseo.CommandType = CommandType.StoredProcedure;
            sseo.SqlParameters.AddWithValue("@OggettoID", oggettoID);
            sseo.SqlParameters.AddWithValue("@OggettoProceduraID", oggettoProceduraID);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            if (dr != null)
            {
                List<TipoOggetto> tipiOggetto = TipoOggettoRepository.Instance.RecuperaTipiOggetto();
                IEnumerable<DatoAmministrativo> datiAmministrativi = DatoAmministrativoRepository.Instance.RecuperaDatiAmministrativi();
                List<Procedura> procedure = ProceduraRepository.Instance.RecuperaProcedure();
                List<StatoProcedura> statiProceduraVIPERA = StatoProceduraVIPERARepository.Instance.RecuperaStatiProceduraVIPERA();
                List<StatoProcedura> statiProceduraAIA = StatoProceduraAIARepository.Instance.RecuperaStatiProceduraAIA();

                if (dr.Read())
                {
                    oggetto = new OggettoDocumentazioneBase();
                    oggetto.ID = oggettoID;
                    oggetto.OggettoProceduraID = oggettoProceduraID;

                    oggetto.TipoOggetto = tipiOggetto.FirstOrDefault(x => x.ID == dr.GetInt32(1));
                    oggetto._nome_IT = dr.GetString(2);
                    oggetto._nome_EN = dr.GetString(3);
                }

                dr.NextResult();

                if (dr.Read())
                {
                    ProceduraCollegata proceduraCollegata = new ProceduraCollegata();
                    StatoProcedura statoProcedura = null;
                    if (!dr.IsDBNull(2))
                    {

                        if (oggetto.TipoOggetto.MacroTipoOggetto.Enum.Equals(MacroTipoOggettoEnum.Aia))
                        {
                            statoProcedura = statiProceduraAIA.FirstOrDefault(x => x.ID == dr.GetInt32(2));
                        }
                        else
                        {
                            statoProcedura = statiProceduraVIPERA.FirstOrDefault(x => x.ID == dr.GetInt32(2));
                        }
                    }



                    proceduraCollegata.OggettoProceduraID = dr.GetInt32(0);
                    proceduraCollegata.Procedura = procedure.FirstOrDefault(x => x.ID == dr.GetInt32(1));
                    proceduraCollegata.StatoProcedura = statoProcedura;
                    proceduraCollegata.Data = dr.IsDBNull(3) ? null : (DateTime?)dr.GetDateTime(3);
                    proceduraCollegata.NumeroDocumenti = dr.GetInt32(4);
                    proceduraCollegata.ViperaAiaID = dr.IsDBNull(5) ? null : dr.GetString(5);

                    if (oggetto != null)
                        oggetto.ProceduraCollegata = proceduraCollegata;
                }

                dr.NextResult();

                while (dr.Read())
                {
                    ValoreDatoAmministrativo valoreDatoAmministrativo = new ValoreDatoAmministrativo();

                    valoreDatoAmministrativo.OggettoProceduraID = dr.GetInt32(0);
                    valoreDatoAmministrativo.Procedura = procedure.Single(x => x.ID == dr.GetInt32(1));
                    valoreDatoAmministrativo._vBool = dr.IsDBNull(2) ? null : (bool?)dr.GetBoolean(2);
                    valoreDatoAmministrativo._vDatetime = dr.IsDBNull(3) ? null : (DateTime?)dr.GetDateTime(3);
                    valoreDatoAmministrativo._vDouble = dr.IsDBNull(4) ? null : (double?)dr.GetDouble(4);
                    valoreDatoAmministrativo._vString = dr.IsDBNull(5) ? "" : dr.GetString(5);
                    valoreDatoAmministrativo.DatoAmministrativo = datiAmministrativi.Single(x => x.ID == dr.GetInt32(6));
                    valoreDatoAmministrativo.ViperaAiaID = dr.IsDBNull(7) ? null : dr.GetString(7);

                    if (oggetto != null)
                        oggetto.DatiAmministrativi.Add(valoreDatoAmministrativo);
                }

                    dr.NextResult();

                while (dr.Read())
                {
                    StatoProcedura statoProcedura = null;

                    if (!dr.IsDBNull(3))
                    {
                        if (oggetto.TipoOggetto.MacroTipoOggetto.Enum.Equals(MacroTipoOggettoEnum.Aia))
                        {
                            statoProcedura = statiProceduraAIA.FirstOrDefault(x => x.ID == dr.GetInt32(3));
                        }
                        else
                        {
                            statoProcedura = statiProceduraVIPERA.FirstOrDefault(x => x.ID == dr.GetInt32(3));
                        }
                    }

                    ValoreDatoAmministrativo valoreDatoAmministrativo = new ValoreDatoAmministrativo();
                    valoreDatoAmministrativo.OggettoProceduraID = dr.GetInt32(0);
                    valoreDatoAmministrativo.Procedura = procedure.Single(x => x.ID == dr.GetInt32(1));
                    valoreDatoAmministrativo._vBool = null;
                    valoreDatoAmministrativo._vDatetime = null;
                    valoreDatoAmministrativo._vDouble = null;
                    valoreDatoAmministrativo._vString = statoProcedura != null ? statoProcedura.GetNome() : "";
                    valoreDatoAmministrativo.DatoAmministrativo = datiAmministrativi.FirstOrDefault(x => x.ID == dr.GetInt32(2));
                    valoreDatoAmministrativo.ViperaAiaID = dr.IsDBNull(4) ? null : dr.GetString(4);

                    if (oggetto != null)
                        oggetto.DatiAmministrativi.Add(valoreDatoAmministrativo);
                }
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return oggetto;
        }

       

        internal int? RecuperaOggettoProceduraIDCorrente(int id)
        {
            int? oggettoProceduraID = null;
            SqlServerExecuteObject sseo = null;
            object result = null;

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = "SELECT TOP(1) OggettoProceduraID FROM dbo.TBL_OggettiProcedure WHERE OggettoID = @OggettoID AND UltimaProcedura = 1 ORDER BY DataInserimento DESC";
            sseo.CommandType = CommandType.Text;
            sseo.SqlParameters.AddWithValue("@OggettoID", id);

            result = SqlProvider.ExecuteScalarObject(sseo);

            if (result != null)
                oggettoProceduraID = (int?)result;

            return oggettoProceduraID;
        }

        public List<int> RecuperaOggettoIDPerViperaAiaID(string viperaID, int? macroTipoOggettoID)
        {

            List<int> oggettoID = new List<int>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = @"SELECT OP.OggettoID FROM dbo.TBL_OggettiProcedure AS OP 
                            INNER JOIN dbo.TBL_Oggetti AS O ON OP.OggettoID = O.OggettoID 
                            INNER JOIN dbo.TBL_TipiOggetto AS TTO ON O.TipoOggettoID = TTO.TipoOggettoID
                            WHERE (Cast(OP.ViperaID as varchar(10)) = @ViperaID) 
                                OR ( OP.AIAID = @ViperaID AND ((@MacroTipoOggettoID IS NULL) OR (TTO.MacroTipoOggettoID = @MacroTipoOggettoID)));";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;

            sseo.SqlParameters.Add("@ViperaID", SqlDbType.VarChar).Value = viperaID;
            sseo.SqlParameters.AddWithValue("@MacroTipoOggettoID", macroTipoOggettoID.HasValue ? (object)macroTipoOggettoID.Value : DBNull.Value);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    oggettoID.Add(dr.GetInt32(0));
                }
            }
            else
            {

                oggettoID.Add(0);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return oggettoID;
        }

        public int RecuperaOggettoIDPerViperaID(int ViperaID, int? macroTipoOggettoID)
        {
            int oggettoID = 0;

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = @"SELECT OP.OggettoID FROM dbo.TBL_OggettiProcedure AS OP 
                            INNER JOIN dbo.TBL_Oggetti AS O ON OP.OggettoID = O.OggettoID 
                            INNER JOIN dbo.TBL_TipiOggetto AS TTO ON O.TipoOggettoID = TTO.TipoOggettoID
                            WHERE (OP.ViperaID = @ViperaID) AND ((@MacroTipoOggettoID IS NULL) OR (TTO.MacroTipoOggettoID = @MacroTipoOggettoID));";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;
            sseo.SqlParameters.AddWithValue("@ViperaID", ViperaID);
            sseo.SqlParameters.AddWithValue("@MacroTipoOggettoID", macroTipoOggettoID.HasValue ? (object)macroTipoOggettoID.Value : DBNull.Value);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            if (dr.HasRows)
            {
                dr.Read();
                oggettoID = dr.GetInt32(0);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return oggettoID;
        }

        public int RecuperaOggettoIDPerAiaID(string AiaID, int? macroTipoOggettoID)
        {
            int oggettoID = 0;

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;
 
            string sSql = @"SELECT OP.OggettoID FROM dbo.TBL_OggettiProcedure AS OP 
                            INNER JOIN dbo.TBL_Oggetti AS O ON OP.OggettoID = O.OggettoID 
                            INNER JOIN dbo.TBL_TipiOggetto AS TTO ON O.TipoOggettoID = TTO.TipoOggettoID
                            WHERE (op.AIAID = @AiaID) AND ((@MacroTipoOggettoID IS NULL) OR (TTO.MacroTipoOggettoID = @MacroTipoOggettoID));";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;
            sseo.SqlParameters.Add("@AiaID", SqlDbType.VarChar).Value = AiaID;
            sseo.SqlParameters.AddWithValue("@MacroTipoOggettoID", macroTipoOggettoID.HasValue ? (object)macroTipoOggettoID.Value : DBNull.Value);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            if (dr.HasRows)
            {
                dr.Read();
                oggettoID = dr.GetInt32(0);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return oggettoID;
        }
    }
}

using System;
using System.Collections.Generic;
using VALib.Domain.Common;
using VALib.Configuration;
using VALib.Domain.Entities.Contenuti;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;
using System.Data;

namespace VALib.Domain.Repositories.Contenuti
{
    public sealed class OggettoElencoRepository : Repository
    {
        private static readonly OggettoElencoRepository _instance = new OggettoElencoRepository(Settings.VAConnectionString);
        //private static readonly string _webCacheKey = typeof(OggettoElencoRepository).FullName;

        private OggettoElencoRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static OggettoElencoRepository Instance
        {
            get { return _instance; }
        }

        public List<OggettoElenco> RecuperaOggettiElencoVia(int? proceduraID, int? tipologiaID, string lingua, string testoRicerca, string orderBy, string orderDirection, int startrowNum, int endRowNum, out int rows)
        {
            return RecuperaOggettiElencoVia(proceduraID, tipologiaID, null, lingua, testoRicerca, orderBy, orderDirection, startrowNum, endRowNum, out rows);
        }

        public List<OggettoElenco> RecuperaOggettiElencoVia(int? proceduraID, int? tipologiaID, int? attributoID, string lingua, string testoRicerca, string orderBy, string orderDirection, int startrowNum, int endRowNum, out int rows)
        {
            List<OggettoElenco> oggettiElenco = new List<OggettoElenco>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;
            rows = 0;

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = "dbo.SP_RecuperaOggettiVia";
            sseo.CommandType = CommandType.StoredProcedure;
            sseo.SqlParameters.AddWithValue("@ProceduraID", proceduraID.HasValue ? (object)proceduraID.Value : DBNull.Value);
            sseo.SqlParameters.AddWithValue("@TipologiaID", tipologiaID.HasValue ? (object)tipologiaID.Value : DBNull.Value);
            sseo.SqlParameters.AddWithValue("@attributoID", attributoID.HasValue ? (object)attributoID.Value : DBNull.Value);
            sseo.SqlParameters.AddWithValue("@Lingua", lingua);
            sseo.SqlParameters.AddWithValue("@TestoRicerca", testoRicerca);
            sseo.SqlParameters.AddWithValue("@OrderBy", orderBy);
            sseo.SqlParameters.AddWithValue("@OrderDirection", orderDirection);
            sseo.SqlParameters.AddWithValue("@StartRowNum", startrowNum);
            sseo.SqlParameters.AddWithValue("@EndRowNum", endRowNum);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                OggettoElenco oggetto = RiempiIstanza(dr);
                oggettiElenco.Add(oggetto);
            }

            if (dr.NextResult() && dr.Read())
                rows = dr.GetInt32(0);

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return oggettiElenco;
        }

        public List<OggettoElenco> RecuperaOggettiElencoTerritorio(int macroTipoOggettoID, int? tipologiaTerritorioID, string testoRicerca, string orderBy, string orderDirection, int startrowNum, int endRowNum, out int rows)
        {
            List<OggettoElenco> oggettiElenco = new List<OggettoElenco>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;
            rows = 0;

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = "dbo.SP_RecuperaOggettiTerritorio";
            sseo.CommandType = CommandType.StoredProcedure;
            sseo.SqlParameters.AddWithValue("@MacroTipoOggettoID", macroTipoOggettoID);
            sseo.SqlParameters.AddWithValue("@TipologiaTerritorioID", tipologiaTerritorioID.HasValue ? (object)tipologiaTerritorioID.Value : DBNull.Value);
            sseo.SqlParameters.AddWithValue("@TestoRicerca", testoRicerca);
            sseo.SqlParameters.AddWithValue("@OrderBy", orderBy);
            sseo.SqlParameters.AddWithValue("@OrderDirection", orderDirection);
            sseo.SqlParameters.AddWithValue("@StartRowNum", startrowNum);
            sseo.SqlParameters.AddWithValue("@EndRowNum", endRowNum);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                OggettoElenco oggetto = RiempiIstanza(dr);
                oggettiElenco.Add(oggetto);
            }

            if (dr.NextResult() && dr.Read())
                rows = dr.GetInt32(0);

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return oggettiElenco;
        }

        public List<OggettoElenco> RecuperaOggettiElencoVas(int? proceduraID, int? settoreID, string lingua, string testoRicerca, string orderBy, string orderDirection, int startrowNum, int endRowNum, out int rows)
        {
            return RecuperaOggettiElencoVas(proceduraID, settoreID, null, lingua, testoRicerca, orderBy, orderDirection, startrowNum, endRowNum, out rows);
        }

        public List<OggettoElenco> RecuperaOggettiElencoVas(int? proceduraID, int? settoreID, int? attributoID, string lingua, string testoRicerca, string orderBy, string orderDirection, int startrowNum, int endRowNum, out int rows)
        {
            List<OggettoElenco> oggettiElenco = new List<OggettoElenco>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;
            rows = 0;

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = "dbo.SP_RecuperaOggettiVas";
            sseo.CommandType = CommandType.StoredProcedure;
            sseo.SqlParameters.AddWithValue("@ProceduraID", proceduraID.HasValue ? (object)proceduraID.Value : DBNull.Value);
            sseo.SqlParameters.AddWithValue("@SettoreID", settoreID.HasValue ? (object)settoreID.Value : DBNull.Value);
            sseo.SqlParameters.AddWithValue("@AttributoID", attributoID.HasValue ? (object)attributoID.Value : DBNull.Value);
            sseo.SqlParameters.AddWithValue("@Lingua", lingua);
            sseo.SqlParameters.AddWithValue("@TestoRicerca", testoRicerca);
            sseo.SqlParameters.AddWithValue("@OrderBy", orderBy);
            sseo.SqlParameters.AddWithValue("@OrderDirection", orderDirection);
            sseo.SqlParameters.AddWithValue("@StartRowNum", startrowNum);
            sseo.SqlParameters.AddWithValue("@EndRowNum", endRowNum);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                OggettoElenco oggetto = RiempiIstanza(dr);
                oggettiElenco.Add(oggetto);
            }

            if (dr.NextResult() && dr.Read())
                rows = dr.GetInt32(0);

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return oggettiElenco;
        }

     
        public List<OggettoElenco> RecuperaOggettiElencoAia(int? proceduraID, int? tipologiaID, string lingua, string testoRicerca, string orderBy, string orderDirection, int startrowNum, int endRowNum, out int rows)
        {
            return RecuperaOggettiElencoAia(proceduraID, tipologiaID, null, lingua, testoRicerca, orderBy, orderDirection, startrowNum, endRowNum, out rows);
        }

        public List<OggettoElenco> RecuperaOggettiElencoAia(int? proceduraID, int? tipologiaID, int? attributoID, string lingua, string testoRicerca, string orderBy, string orderDirection, int startrowNum, int endRowNum, out int rows)
        {
            List<OggettoElenco> oggettiElenco = new List<OggettoElenco>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;
            rows = 0;

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = "dbo.SP_RecuperaOggettiAia";
            sseo.CommandType = CommandType.StoredProcedure;
            sseo.SqlParameters.AddWithValue("@ProceduraID", proceduraID.HasValue ? (object)proceduraID.Value : DBNull.Value);
            sseo.SqlParameters.AddWithValue("@TipologiaID", tipologiaID.HasValue ? (object)tipologiaID.Value : DBNull.Value);
            sseo.SqlParameters.AddWithValue("@attributoID", attributoID.HasValue ? (object)attributoID.Value : DBNull.Value);
            sseo.SqlParameters.AddWithValue("@Lingua", lingua);
            sseo.SqlParameters.AddWithValue("@TestoRicerca", testoRicerca);
            sseo.SqlParameters.AddWithValue("@OrderBy", orderBy);
            sseo.SqlParameters.AddWithValue("@OrderDirection", orderDirection);
            sseo.SqlParameters.AddWithValue("@StartRowNum", startrowNum);
            sseo.SqlParameters.AddWithValue("@EndRowNum", endRowNum);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                OggettoElenco oggetto = RiempiIstanza(dr);
                oggettiElenco.Add(oggetto);
            }

            if (dr.NextResult() && dr.Read())
                rows = dr.GetInt32(0);

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return oggettiElenco;
        }

        public List<OggettoElenco> RecuperaOggettiSpaziali(MacroTipoOggettoEnum macroTipoOggetto, double xMax, double yMax, double xMin, double yMin, string orderBy, string orderDirection, int startrowNum, int endRowNum, out int rows)
        {
            List<OggettoElenco> oggettiElenco = new List<OggettoElenco>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;
            rows = 0;

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = "dbo.SP_RecuperaOggettiViaSpaziale";
            sseo.CommandType = CommandType.StoredProcedure;
            sseo.SqlParameters.AddWithValue("@xMax", xMax);
            sseo.SqlParameters.AddWithValue("@yMax", yMax);
            sseo.SqlParameters.AddWithValue("@xMin", xMin);
            sseo.SqlParameters.AddWithValue("@yMin", yMin);
            sseo.SqlParameters.AddWithValue("@OrderBy", orderBy);
            sseo.SqlParameters.AddWithValue("@OrderDirection", orderDirection);
            sseo.SqlParameters.AddWithValue("@StartRowNum", startrowNum);
            sseo.SqlParameters.AddWithValue("@EndRowNum", endRowNum);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                OggettoElenco oggetto = RiempiIstanza(dr);
                oggettiElenco.Add(oggetto);
            }

            if (dr.NextResult() && dr.Read())
                rows = dr.GetInt32(0);

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return oggettiElenco;
        }

        public List<OggettoElenco> RecuperaOggettiElenco(string lingua, string testoRicerca, string orderBy, string orderDirection, int startrowNum, int endRowNum, out int rows, string elencoVipAiaID = null)
        {
            List<OggettoElenco> oggettiElenco = new List<OggettoElenco>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;
            rows = 0;

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = "dbo.SP_RecuperaOggetti";
            sseo.CommandType = CommandType.StoredProcedure;
            sseo.SqlParameters.AddWithValue("@Lingua", lingua);
            sseo.SqlParameters.AddWithValue("@TestoRicerca", testoRicerca);
            sseo.SqlParameters.AddWithValue("@OrderBy", orderBy);
            sseo.SqlParameters.AddWithValue("@OrderDirection", orderDirection);
            sseo.SqlParameters.AddWithValue("@StartRowNum", startrowNum);
            sseo.SqlParameters.AddWithValue("@EndRowNum", endRowNum);
            sseo.SqlParameters.AddWithValue("@ElencoViperaAiaID", elencoVipAiaID == null ? "" : elencoVipAiaID.TrimEnd(','));

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                OggettoElenco oggetto = RiempiIstanzaViaVasAia(dr);
                oggettiElenco.Add(oggetto);
            }

            if (dr.NextResult() && dr.Read())
                rows = dr.GetInt32(0);

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return oggettiElenco;
        }

        private OggettoElenco RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            OggettoElenco oggetto = new OggettoElenco();

            oggetto.ID = dr.GetInt32(1);
            oggetto.TipoOggetto = TipoOggettoRepository.Instance.RecuperaTipoOggetto(dr.GetInt32(2));
            oggetto.Procedura = ProceduraRepository.Instance.RecuperaProcedura(dr.GetInt32(3));
            oggetto._nome_IT = dr.GetString(4);
            oggetto._nome_EN = dr.GetString(5);
            oggetto._descrizione_IT = dr.GetString(6);
            oggetto._descrizione_EN = dr.GetString(7);
            oggetto.Proponente = dr.GetString(8);
            oggetto.OggettoProceduraID = dr.GetInt32(9);            
            return oggetto;
        }

        private OggettoElenco RiempiIstanzaAia(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            OggettoElenco oggetto = new OggettoElenco();

            oggetto.ID = dr.GetInt32(1);
            oggetto.TipoOggetto = TipoOggettoRepository.Instance.RecuperaTipoOggetto(dr.GetInt32(2));
            oggetto.Procedura = ProceduraRepository.Instance.RecuperaProcedura(dr.GetInt32(3));
            oggetto._nome_IT = dr.GetString(4);
            oggetto._nome_EN = dr.GetString(5);
            oggetto._descrizione_IT = dr.GetString(6);
            oggetto._descrizione_EN = dr.GetString(7);
            oggetto.Proponente = dr.GetString(8);
            oggetto.OggettoProceduraID = dr.GetInt32(9);
            oggetto.StatoImpianto = dr.GetString(10);
            oggetto.AttivitaIPPC = dr.GetString(11);
            oggetto.Territorio = dr.GetString(12);
            oggetto.Competenza = dr.GetInt32(13);

            return oggetto;
        }

        private OggettoElenco RiempiIstanzaViaVasAia(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            OggettoElenco oggetto = new OggettoElenco();

            oggetto.ID = dr.GetInt32(1);
            oggetto.TipoOggetto = TipoOggettoRepository.Instance.RecuperaTipoOggetto(dr.GetInt32(2));
            oggetto.Procedura = ProceduraRepository.Instance.RecuperaProcedura(dr.GetInt32(3));
            oggetto._nome_IT = dr.GetString(4);
            oggetto._nome_EN = dr.GetString(5);
            oggetto._descrizione_IT = dr.GetString(6);
            oggetto._descrizione_EN = dr.GetString(7);
            oggetto.Proponente = dr.GetString(8);
            oggetto.OggettoProceduraID = dr.GetInt32(9);
            oggetto.ViperaAiaID = dr.IsDBNull(10) ? "" : dr.GetString(10);
            return oggetto;
        }

        public List<OggettoElencoProcedura> RecuperaOggettiElencoProcedura(MacroTipoOggettoEnum macroTipoOggetto, int parametro, string lingua, string testoRicerca, string orderBy, string orderDirection, int startrowNum, int endRowNum, out int rows)
        {
            List<OggettoElencoProcedura> oggettiElenco = new List<OggettoElencoProcedura>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;
            rows = 0;

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = "dbo.SP_RecuperaOggettiPerProceduraInCorso";
            sseo.CommandType = CommandType.StoredProcedure;
            sseo.SqlParameters.AddWithValue("@MacroTipoOggettoID", (int)macroTipoOggetto);
            sseo.SqlParameters.AddWithValue("@Parametro", parametro);
            sseo.SqlParameters.AddWithValue("@Lingua", lingua);
            sseo.SqlParameters.AddWithValue("@TestoRicerca", testoRicerca);
            sseo.SqlParameters.AddWithValue("@OrderBy", orderBy);
            sseo.SqlParameters.AddWithValue("@OrderDirection", orderDirection);
            sseo.SqlParameters.AddWithValue("@StartRowNum", startrowNum);
            sseo.SqlParameters.AddWithValue("@EndRowNum", endRowNum);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                OggettoElencoProcedura oggetto = new OggettoElencoProcedura();

                oggetto.ID = dr.GetInt32(1);
                oggetto._nome_IT = dr.GetString(2);
                oggetto._nome_EN = dr.GetString(3);
                oggetto.Proponente = dr.GetString(4);
                oggetto.OggettoProceduraID = dr.GetInt32(7);
                oggetto.TipoOggetto = TipoOggettoRepository.Instance.RecuperaTipoOggetto(dr.GetInt32(8));

                if (!dr.IsDBNull(9)) { 
            
                    if (macroTipoOggetto == MacroTipoOggettoEnum.Aia)
                        oggetto.StatoProcedura = StatoProceduraAIARepository.Instance.RecuperaStatoProceduraAIA(dr.GetInt32(9));
                    else
                        oggetto.StatoProcedura = StatoProceduraVIPERARepository.Instance.RecuperaStatoProceduraVIPERA(dr.GetInt32(9));
                }

                oggetto.Data = dr.GetDateTime(5);
                oggettiElenco.Add(oggetto);
            }

            if (dr.NextResult() && dr.Read())
                rows = dr.GetInt32(0);

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            //rows = oggettiElenco.Count();

            return oggettiElenco;
        }
    }
}

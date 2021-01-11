using System;
using System.Collections.Generic;
using VALib.Domain.Common;
using VALib.Configuration;
using VALib.Domain.Entities.Contenuti;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;


namespace VALib.Domain.Repositories.Contenuti
{
    public sealed class ProvvedimentoRepository : Repository
    {
        private static readonly ProvvedimentoRepository _instance = new ProvvedimentoRepository(Settings.VAConnectionString);
       

        private ProvvedimentoRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static ProvvedimentoRepository Instance
        {
            get { return _instance; }
        }
       
        public List<Provvedimento> RecuperaProvvedimenti(string lang, string testo, DateTime? dataDa, DateTime? dataA, int tipoProvvedimentoID, int startrowNum, int endRowNum, out int rows)
        {
            List<Provvedimento> provvedimenti = new List<Provvedimento>();
            rows = 0;

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;
            string sSql = "";
            string nomeCampoOggetto = "";
            string nomeCampoOggetto2 = "";

            switch (lang.ToLower())
            {
                case "it":
                    nomeCampoOggetto = "Nome_IT";
                    nomeCampoOggetto2 = "Oggetto_IT";
                    break;
                case "en":
                    nomeCampoOggetto = "Nome_EN";
                    nomeCampoOggetto2 = "Oggetto_EN";
                    break;
                default:
                    nomeCampoOggetto = "Nome_IT";
                    nomeCampoOggetto2 = "Oggetto_IT";
                    break;
            }

            sSql = @"SELECT * FROM 
                (SELECT *, ROW_NUMBER() OVER(ORDER BY Data DESC) 
                    ROWNUM  FROM 
                (
                SELECT P.ProvvedimentoID, P.TipoProvvedimentoID, O.OggettoID, P.Data, P.NumeroProtocollo, 
                O.Nome_IT, O.Nome_EN, E.Nome, P.Esito, O.TipoOggettoID, P.Oggetto_IT, P.Oggetto_EN, P.OggettoProceduraID 
                FROM dbo.TBL_Provvedimenti AS P 
                INNER JOIN .TBL_OggettiProcedure AS OP ON OP.OggettoProceduraID = P.OggettoProceduraID 
                INNER JOIN dbo.TBL_Oggetti AS O ON O.OggettoID = OP.OggettoID 
                INNER JOIN dbo.TBL_Entita AS E ON E.EntitaID = P.EntitaID 
                INNER JOIN dbo.TBL_TipiOggetto T ON T.TipoOggettoID = O.TipoOggettoID
                WHERE (P.TipoProvvedimentoID = @tipoProvvedimentoID) AND (T.MacroTipoOggettoID <> 3 OR OP.AIAID IS NOT NULL)
                AND (((P.Data >= @dataDa) OR (@dataDa IS NULL)) AND ((P.Data <= @dataA) OR (@dataA IS NULL)))
                AND ((O.{0} LIKE @testo) OR (P.{1} LIKE @testo) OR (E.Nome LIKE @testo) OR (P.NumeroProtocollo LIKE @testo) OR (P.Esito LIKE @testo))
                GROUP BY P.ProvvedimentoID, P.TipoProvvedimentoID, O.OggettoID, P.Data, P.NumeroProtocollo, 
                O.Nome_IT, O.Nome_EN, E.Nome, P.Esito, O.TipoOggettoID, P.Oggetto_IT, P.Oggetto_EN, P.OggettoProceduraID  
                )T) R WHERE R.ROWNUM > @StartRowNum AND R.ROWNUM <= @EndRowNum;
                SELECT COUNT(*)
                FROM
                (
                SELECT P.ProvvedimentoID FROM dbo.TBL_Provvedimenti AS P 
                INNER JOIN dbo.TBL_OggettiProcedure AS OP ON OP.OggettoProceduraID = P.OggettoProceduraID 
                INNER JOIN dbo.TBL_Oggetti AS O ON O.OggettoID = OP.OggettoID 
                INNER JOIN dbo.TBL_Entita AS E ON E.EntitaID = P.EntitaID 
                INNER JOIN dbo.TBL_TipiOggetto T ON T.TipoOggettoID = O.TipoOggettoID
                WHERE (P.TipoProvvedimentoID = @tipoProvvedimentoID) AND (T.MacroTipoOggettoID <> 3 OR OP.AIAID IS NOT NULL)
                AND (((P.Data >= @dataDa) OR (@dataDa IS NULL)) AND ((P.Data <= @dataA) OR (@dataA IS NULL)))
                AND ((O.{0} LIKE @testo) OR (P.{1} LIKE @testo) OR (E.Nome LIKE @testo) OR (P.NumeroProtocollo LIKE @testo) OR (P.Esito LIKE @testo))
                GROUP BY P.ProvvedimentoID, P.TipoProvvedimentoID, O.OggettoID, P.Data, P.NumeroProtocollo, O.Nome_IT, 
                O.Nome_EN, E.Nome, P.Esito, O.TipoOggettoID, P.Oggetto_IT, P.Oggetto_EN, P.OggettoProceduraID ) T;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = string.Format(sSql, nomeCampoOggetto, nomeCampoOggetto2);
            sseo.SqlParameters.AddWithValue("@StartRowNum", startrowNum);
            sseo.SqlParameters.AddWithValue("@EndRowNum", endRowNum);
            sseo.SqlParameters.AddWithValue("@tipoProvvedimentoID", tipoProvvedimentoID);
            sseo.SqlParameters.AddWithValue("@dataDa", dataDa.HasValue ? (object)dataDa.Value : DBNull.Value);
            sseo.SqlParameters.AddWithValue("@dataA", dataA.HasValue ? (object)dataA.Value : DBNull.Value);
            sseo.SqlParameters.AddWithValue("@testo", string.Format("%{0}%", testo));

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                Provvedimento provvedimento = RiempiIstanza(dr);
                provvedimenti.Add(provvedimento);
            }

            if (dr.NextResult() && dr.Read())
                rows = dr.GetInt32(0);

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return provvedimenti;
        }

        public List<Provvedimento> RecuperaProvvedimentiRegionali(string lang, string testo, DateTime? dataDa, 
                                                                    DateTime? dataA, int proceduraID, int TipologiaID,
                                                                    int startrowNum, int endRowNum, out int rows)
        {
            List<Provvedimento> provvedimenti = new List<Provvedimento>();
            rows = 0;

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;
            string sSql = "";
            string nomeCampoOggetto = "";
            string nomeCampoOggetto2 = "";

            switch (lang.ToLower())
            {
                case "it":
                    nomeCampoOggetto = "Nome_IT";
                    nomeCampoOggetto2 = "Oggetto_IT";
                    break;
                case "en":
                    nomeCampoOggetto = "Nome_EN";
                    nomeCampoOggetto2 = "Oggetto_EN";
                    break;
                default:
                    nomeCampoOggetto = "Nome_IT";
                    nomeCampoOggetto2 = "Oggetto_IT";
                    break;
            }

            string qProceduraID = "AND (OP.ProceduraID = @proceduraID)";
            string qCategoriaImpiantoID = "AND (CI.CategoriaImpiantoID = @TipologiaID)";

            sSql = @"SELECT * FROM 
                (SELECT *, ROW_NUMBER() OVER(ORDER BY Data DESC) 
                    ROWNUM  FROM 
                    (
                        SELECT P.ProvvedimentoID, P.TipoProvvedimentoID, O.OggettoID, P.Data, P.NumeroProtocollo, 
	                        O.Nome_IT, O.Nome_EN, E.Nome, P.Esito, O.TipoOggettoID, P.Oggetto_IT, P.Oggetto_EN, 
	                        P.OggettoProceduraID,CI.CategoriaImpiantoID, OP.ProceduraID,TR.Nome as PROV
                        FROM dbo.TBL_Provvedimenti AS P 
	                        INNER JOIN dbo.TBL_OggettiProcedure AS OP ON OP.OggettoProceduraID = P.OggettoProceduraID 
	                        INNER JOIN dbo.TBL_Oggetti AS O ON O.OggettoID = OP.OggettoID 
	                        INNER JOIN dbo.TBL_Entita AS E ON E.EntitaID = P.EntitaID 
	                        INNER JOIN dbo.TBL_TipiOggetto T ON T.TipoOggettoID = O.TipoOggettoID                
	                        LEFT JOIN  dbo.TBL_ExtraOggettiImpianto AS EO ON EO.OggettoID = O.OggettoID 
	                        LEFT JOIN  dbo.TBL_CategorieImpianti AS CI ON CI.CategoriaImpiantoID = EO.CategoriaImpiantoID
	                        LEFT JOIN (
		                        SELECT T.Nome, SOT.OggettoID FROM TBL_Territori T
		                        INNER JOIN STG_OggettiTerritori SOT on SOT.TerritorioID = T.TerritorioID
		                        WHERE TipologiaTerritorioID = 3
	                        ) TR on TR.OggettoID = O.OggettoID
                        WHERE 
	                        (T.MacroTipoOggettoID = {0} AND OP.AIAID IS NULL)
	                        AND (((P.Data >= @dataDa) OR (@dataDa IS NULL)) AND ((P.Data <= @dataA) OR (@dataA IS NULL)))
	                        {1}
	                        {2}
	                        AND ((O.{3} LIKE @testo) OR (P.{4} LIKE @testo) OR (E.Nome LIKE @testo) OR (P.NumeroProtocollo LIKE @testo) OR (P.Esito LIKE @testo))
                    )
                T) R 
                WHERE R.ROWNUM > @StartRowNum AND R.ROWNUM <= @EndRowNum;
                SELECT COUNT(*)
                FROM
                (
                        SELECT P.ProvvedimentoID, P.TipoProvvedimentoID, O.OggettoID, P.Data, P.NumeroProtocollo, 
	                        O.Nome_IT, O.Nome_EN, E.Nome, P.Esito, O.TipoOggettoID, P.Oggetto_IT, P.Oggetto_EN, 
	                        P.OggettoProceduraID,CI.CategoriaImpiantoID, OP.ProceduraID,TR.Nome as PROV
                        FROM dbo.TBL_Provvedimenti AS P 
	                        INNER JOIN dbo.TBL_OggettiProcedure AS OP ON OP.OggettoProceduraID = P.OggettoProceduraID 
	                        INNER JOIN dbo.TBL_Oggetti AS O ON O.OggettoID = OP.OggettoID 
	                        INNER JOIN dbo.TBL_Entita AS E ON E.EntitaID = P.EntitaID 
	                        INNER JOIN dbo.TBL_TipiOggetto T ON T.TipoOggettoID = O.TipoOggettoID                
	                        LEFT JOIN  dbo.TBL_ExtraOggettiImpianto AS EO ON EO.OggettoID = O.OggettoID 
	                        LEFT JOIN  dbo.TBL_CategorieImpianti AS CI ON CI.CategoriaImpiantoID = EO.CategoriaImpiantoID
	                        LEFT JOIN (
		                        SELECT T.Nome, SOT.OggettoID FROM TBL_Territori T
		                        INNER JOIN STG_OggettiTerritori SOT on SOT.TerritorioID = T.TerritorioID
		                        WHERE TipologiaTerritorioID = 3
	                        ) TR on TR.OggettoID = O.OggettoID
                        WHERE 
	                        (T.MacroTipoOggettoID = {0} AND OP.AIAID IS NULL)
	                        AND (((P.Data >= @dataDa) OR (@dataDa IS NULL)) AND ((P.Data <= @dataA) OR (@dataA IS NULL)))
	                        {1}
	                        {2}
	                        AND ((O.{3} LIKE @testo) OR (P.{4} LIKE @testo) OR (E.Nome LIKE @testo) OR (P.NumeroProtocollo LIKE @testo) OR (P.Esito LIKE @testo))
                ) T;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = string.Format(sSql, (int)MacroTipoOggettoEnum.Aia, proceduraID.ToString().Equals("0") ? "" : qProceduraID,
                                            TipologiaID.ToString().Equals("0") ? "" : qCategoriaImpiantoID,
                                            nomeCampoOggetto, nomeCampoOggetto2);
            sseo.SqlParameters.AddWithValue("@StartRowNum", startrowNum);
            sseo.SqlParameters.AddWithValue("@EndRowNum", endRowNum);
            sseo.SqlParameters.AddWithValue("@proceduraID", proceduraID);
            sseo.SqlParameters.AddWithValue("@TipologiaID", TipologiaID);
            sseo.SqlParameters.AddWithValue("@dataDa", dataDa.HasValue ? (object)dataDa.Value : DBNull.Value);
            sseo.SqlParameters.AddWithValue("@dataA", dataA.HasValue ? (object)dataA.Value : DBNull.Value);
            sseo.SqlParameters.AddWithValue("@testo", string.Format("%{0}%", testo));

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                Provvedimento provvedimento = RiempiIstanzaAiaRegionale(dr);
                provvedimenti.Add(provvedimento);
            }

            if (dr.NextResult() && dr.Read())
                rows = dr.GetInt32(0);

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return provvedimenti;
        }
        

        private Provvedimento RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            Provvedimento provvedimento = new Provvedimento();

            provvedimento.ID = dr.GetInt32(0);
            provvedimento.Tipo = TipoProvvedimentoRepository.Instance.RecuperaTipoProvvedimento(dr.GetInt32(1));
            provvedimento.OggettoID = dr.GetInt32(2);
            provvedimento.Data = dr.GetDateTime(3);
            provvedimento.NumeroProtocollo = dr.GetString(4);
            provvedimento._nomeProgetto_IT = dr.GetString(5);
            provvedimento._nomeProgetto_EN = dr.GetString(6);
            provvedimento.Proponente = dr.GetString(7);
            provvedimento.Esito = dr.GetString(8);
            provvedimento.TipoOggetto = TipoOggettoRepository.Instance.RecuperaTipoOggetto(dr.GetInt32(9));
            provvedimento._oggetto_IT = dr.IsDBNull(10) ? "" : dr.GetString(10);
            provvedimento._oggetto_EN = dr.IsDBNull(11) ? "" : dr.GetString(11);
            provvedimento.OggettoProceduraID = dr.GetInt32(12);

            return provvedimento;
        }

        private Provvedimento RiempiIstanzaAiaRegionale(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            Provvedimento provvedimento = new Provvedimento();

            provvedimento.ID = dr.GetInt32(0);
            provvedimento.Tipo = dr.IsDBNull(1) ? null :  TipoProvvedimentoRepository.Instance.RecuperaTipoProvvedimento(dr.GetInt32(1));
            provvedimento.OggettoID = dr.GetInt32(2);
            if (!dr.IsDBNull(3))
            {
                provvedimento.Data = dr.GetDateTime(3);
            }
            else {
                provvedimento.Data = null;
            }
            
            provvedimento.NumeroProtocollo = dr.GetString(4);
            provvedimento._nomeProgetto_IT = dr.GetString(5);
            provvedimento._nomeProgetto_EN = dr.GetString(6);
            provvedimento.Proponente = dr.GetString(7);
            provvedimento.TipoOggetto = TipoOggettoRepository.Instance.RecuperaTipoOggetto(dr.GetInt32(9));
            provvedimento._oggetto_IT = dr.IsDBNull(10) ? "" : dr.GetString(10);
            provvedimento._oggetto_EN = dr.IsDBNull(11) ? "" : dr.GetString(11);
            provvedimento.OggettoProceduraID = dr.GetInt32(12);
            provvedimento.CategoriaImpianto = CategoriaImpiantoRepository.Instance.RecuperaCategoria(dr.IsDBNull(13) ? 0 : dr.GetInt32(13));
            provvedimento.Procedura = ProceduraRepository.Instance.RecuperaProcedura(dr.IsDBNull(14) ? 0 : dr.GetInt32(14));
            provvedimento.Prov = dr.IsDBNull(15) ? "" : dr.GetString(15);
            return provvedimento;
        }
    }
}

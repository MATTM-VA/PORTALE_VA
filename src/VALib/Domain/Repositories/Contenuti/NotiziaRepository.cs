using System;
using System.Collections.Generic;
using VALib.Domain.Common;
using VALib.Configuration;
using VALib.Domain.Entities.Contenuti;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;
using System.Data;
using ElogToolkit.Web;
using VALib.Domain.Repositories.UI;

namespace VALib.Domain.Repositories.Contenuti
{
    public sealed class NotiziaRepository : Repository
    {
        private static readonly NotiziaRepository _instance = new NotiziaRepository(Settings.VAConnectionString);
        //private static readonly string _webCacheKey = "CategorieNotizie";

        private NotiziaRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static NotiziaRepository Instance
        {
            get { return _instance; }
        }

        public List<Notizia> RecuperaNotizie(string testo, int? categoriaNotiziaID, bool? pubblicata, StatoNotiziaEnum? stato, int startrowNum, int endRowNum, out int rows)
        {
            List<Notizia> notizie = new List<Notizia>();
            rows = 0;

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;
            string sSql = "";

            sSql = @"SELECT * FROM (
                SELECT N.NotiziaID, N.CategoriaNotiziaID, N.ImmagineID, N.Data, N.Titolo_IT, N.Titolo_EN, N.TitoloBreve_IT, N.TitoloBreve_EN, N.Abstract_IT, N.Abstract_EN, N.Testo_IT, N.Testo_EN, N.Pubblicata, N.DataInserimento, N.DataUltimaModifica, N.Stato, ROW_NUMBER() 
                OVER(ORDER BY Data DESC) 
                ROWNUM 
                FROM dbo.TBL_Notizie AS N WHERE (N.CategoriaNotiziaID = @CategoriaNotiziaID OR @CategoriaNotiziaID IS NULL) 
                                            AND (N.Pubblicata = @Pubblicata OR @Pubblicata IS NULL)
                                            AND (N.Stato = @Stato OR @Stato IS NULL)
                                            AND (N.Titolo_IT LIKE @Testo)
                ) 
                R WHERE R.ROWNUM > @StartRowNum AND R.ROWNUM <= @EndRowNum;
                SELECT COUNT(*) FROM dbo.TBL_Notizie AS N WHERE (N.CategoriaNotiziaID = @CategoriaNotiziaID OR @CategoriaNotiziaID IS NULL) 
                                            AND (N.Pubblicata = @Pubblicata OR @Pubblicata IS NULL)
                                            AND (N.Stato = @Stato OR @Stato IS NULL)
                                            AND (N.Titolo_IT LIKE @Testo);";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@StartRowNum", startrowNum);
            sseo.SqlParameters.AddWithValue("@EndRowNum", endRowNum);
            sseo.SqlParameters.AddWithValue("@CategoriaNotiziaID", categoriaNotiziaID.HasValue ? (object)categoriaNotiziaID.Value : DBNull.Value);
            sseo.SqlParameters.AddWithValue("@Pubblicata", pubblicata.HasValue ? (object)pubblicata.Value : DBNull.Value);
            sseo.SqlParameters.AddWithValue("@Stato", stato.HasValue ? (object)stato.Value : DBNull.Value);
            sseo.SqlParameters.AddWithValue("@Testo", string.Format("%{0}%", testo));

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                Notizia notizia = RiempiIstanza(dr);
                notizie.Add(notizia);
            }

            if (dr.NextResult() && dr.Read())
                rows = dr.GetInt32(0);

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return notizie;
        }

        public List<Notizia> RecuperaNotizie(string lingua, string testo, bool? cercaAnnoCorrente, int? annoCorrente, int? categoriaNotiziaID, bool? pubblicata, StatoNotiziaEnum? stato, int startrowNum, int endRowNum, out int rows)
        {
            List<Notizia> notizie = new List<Notizia>();
            rows = 0;

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;
            string sSql = "";

            sSql = @"dbo.SP_RecuperaNotizie";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.StoredProcedure;
            sseo.SqlParameters.AddWithValue("@StartRowNum", startrowNum);
            sseo.SqlParameters.AddWithValue("@EndRowNum", endRowNum);
            sseo.SqlParameters.AddWithValue("@OrderBy", "");
            sseo.SqlParameters.AddWithValue("@OrderDirection", "");
            sseo.SqlParameters.AddWithValue("@CategoriaNotiziaID", categoriaNotiziaID.HasValue ? (object)categoriaNotiziaID.Value : DBNull.Value);
            sseo.SqlParameters.AddWithValue("@Pubblicata", pubblicata.HasValue ? (object)pubblicata.Value : DBNull.Value);
            sseo.SqlParameters.AddWithValue("@Stato", stato.HasValue ? (object)stato.Value : DBNull.Value);
            sseo.SqlParameters.AddWithValue("@TestoRicerca", string.IsNullOrWhiteSpace(testo) ? "" : testo);
            sseo.SqlParameters.AddWithValue("@Lingua", lingua);
            sseo.SqlParameters.AddWithValue("@AnnoCorrente", annoCorrente.HasValue ? (object)annoCorrente.Value : DBNull.Value);
            sseo.SqlParameters.AddWithValue("@CercaAnnoCorrente", cercaAnnoCorrente.HasValue ? (object)cercaAnnoCorrente.Value : DBNull.Value);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                Notizia notizia = RiempiIstanza(dr);
                notizie.Add(notizia);
            }

            if (dr.NextResult() && dr.Read())
                rows = dr.GetInt32(0);

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return notizie;
        }

        public Notizia RecuperaNotizia(int id)
        {
            Notizia notizia = null;

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SELECT N.NotiziaID, N.CategoriaNotiziaID, N.ImmagineID, N.Data, N.Titolo_IT, N.Titolo_EN, N.TitoloBreve_IT, N.TitoloBreve_EN, N.Abstract_IT, N.Abstract_EN, N.Testo_IT, N.Testo_EN, N.Pubblicata, N.DataInserimento, N.DataUltimaModifica, N.Stato FROM dbo.TBL_Notizie AS N WHERE NotiziaID = @NotiziaID;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;
            sseo.SqlParameters.AddWithValue("@NotiziaID", id);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                notizia = RiempiIstanza(dr);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return notizia;
        }

        internal int SalvaNotizia(Notizia notizia)
        {
            int result = 0;

            if (notizia.IsNew)
                result = InserisciNotizia(notizia);
            else
                result = ModificaNotizia(notizia);

            if (result > 0)
                this.CacheRemove(OggettoCaroselloRepository._webCacheKey);


            return result;
        }

        private int ModificaNotizia(Notizia notizia)
        {
            int result = 0;

            SqlServerExecuteObject sseo = null;
            string sSql = "";
            string testo_ITNoHtml = "";
            string testo_ENNoHtml = "";

            testo_ITNoHtml = HtmlUtility.HtmlStrip(notizia.Testo_IT).Trim();
            testo_ENNoHtml = HtmlUtility.HtmlStrip(notizia.Testo_EN).Trim();

            sSql = @"UPDATE dbo.TBL_Notizie SET CategoriaNotiziaID = @CategoriaNotiziaID, ImmagineID = @ImmagineID, Data = @Data, Titolo_IT = @Titolo_IT, 
                            Titolo_EN = @Titolo_EN, TitoloBreve_IT = @TitoloBreve_IT, TitoloBreve_EN = @TitoloBreve_EN, Abstract_IT = @Abstract_IT, Abstract_EN = @Abstract_EN, 
                            Testo_IT = @Testo_IT, Testo_EN = @Testo_EN, DataUltimaModifica = @DataUltimaModifica, Pubblicata = @Pubblicata, Stato = @Stato 
                            WHERE NotiziaID = @NotiziaID;
                    MERGE dbo.FTL_Notizie as FTL
                    USING (SELECT @NotiziaID, @Titolo_IT, @Titolo_EN, @Abstract_IT, @Abstract_EN, @Testo_IT_NoHTML, @Testo_EN_NoHTML) AS S 
                            (NotiziaID, Titolo_IT, Titolo_EN, Abstract_IT, Abstract_EN, Testo_IT_NoHTML, Testo_EN_NoHTML) 
                            ON (FTL.NotiziaID = S.NotiziaID)
                    WHEN MATCHED THEN
                        UPDATE
                        SET Titolo_IT = S.Titolo_IT, Titolo_EN = S.Titolo_EN, 
                            Abstract_IT = S.Abstract_IT, Abstract_EN = S.Abstract_EN, 
                            Testo_IT = S.Testo_IT_NoHTML, Testo_EN = S.Testo_EN_NoHTML
                    WHEN NOT MATCHED THEN
                        INSERT (NotiziaID, Titolo_IT, Titolo_EN, Abstract_IT, Abstract_EN, Testo_IT, Testo_EN)
                        values (S.NotiziaID, S.Titolo_IT, S.Titolo_EN, S.Abstract_IT, S.Abstract_EN, S.Testo_IT_NoHTML, S.Testo_EN_NoHTML);";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@CategoriaNotiziaID", notizia.Categoria.ID);
            sseo.SqlParameters.AddWithValue("@ImmagineID", notizia.ImmagineID);
            sseo.SqlParameters.AddWithValue("@Data", notizia.Data);
            sseo.SqlParameters.AddWithValue("@Titolo_IT", notizia.Titolo_IT);
            sseo.SqlParameters.AddWithValue("@Titolo_EN", notizia.Titolo_EN);
            sseo.SqlParameters.AddWithValue("@TitoloBreve_IT", notizia.TitoloBreve_IT);
            sseo.SqlParameters.AddWithValue("@TitoloBreve_EN", notizia.TitoloBreve_EN);
            sseo.SqlParameters.AddWithValue("@Abstract_IT", notizia.Abstract_IT);
            sseo.SqlParameters.AddWithValue("@Abstract_EN", notizia.Abstract_EN);
            sseo.SqlParameters.AddWithValue("@Testo_IT", notizia.Testo_IT);
            sseo.SqlParameters.AddWithValue("@Testo_EN", notizia.Testo_EN);
            sseo.SqlParameters.AddWithValue("@Testo_IT_NoHTML", testo_ITNoHtml);
            sseo.SqlParameters.AddWithValue("@Testo_EN_NoHTML", testo_ENNoHtml);
            sseo.SqlParameters.AddWithValue("@DataUltimaModifica", notizia.DataUltimaModifica);
            sseo.SqlParameters.AddWithValue("@Pubblicata", notizia.Pubblicata);
            sseo.SqlParameters.AddWithValue("@Stato", notizia.Stato);
            sseo.SqlParameters.AddWithValue("@NotiziaID", notizia.ID);

            SqlProvider.ExecuteNonQueryObject(sseo);

            result = notizia.ID;

            return result;
        }

        private int InserisciNotizia(Notizia notizia)
        {
            int result = 0;
            //SqlProvider provider = new SqlServerProvider(Settings.VAConnectionString);
            ElogToolkit.Data.SqlServer.SqlServerProvider.SqlServerTransactionObject tran = SqlProvider.CreateTransactionObject();

            SqlServerExecuteObject sseo = null;
            string sSql = "";

            sSql = "INSERT INTO dbo.TBL_Notizie (CategoriaNotiziaID, ImmagineID, Data, Titolo_IT, Titolo_EN, TitoloBreve_IT, TitoloBreve_EN, Abstract_IT, Abstract_EN, Testo_IT, Testo_EN, Pubblicata, DataInserimento, DataUltimaModifica, Stato) VALUES " +
                            "(@CategoriaNotiziaID, @ImmagineID, @Data, @Titolo_IT, @Titolo_EN, @TitoloBreve_IT, @TitoloBreve_EN, @Abstract_IT, @Abstract_EN, @Testo_IT, @Testo_EN, @Pubblicata, @DataInserimento, @DataUltimaModifica, @Stato);" +
                    "SELECT @@IDENTITY;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@CategoriaNotiziaID", notizia.Categoria.ID);
            sseo.SqlParameters.AddWithValue("@ImmagineID", notizia.ImmagineID);
            sseo.SqlParameters.AddWithValue("@Data", notizia.Data);
            sseo.SqlParameters.AddWithValue("@Titolo_IT", notizia.Titolo_IT);
            sseo.SqlParameters.AddWithValue("@Titolo_EN", notizia.Titolo_EN);
            sseo.SqlParameters.AddWithValue("@TitoloBreve_IT", notizia.TitoloBreve_IT);
            sseo.SqlParameters.AddWithValue("@TitoloBreve_EN", notizia.TitoloBreve_EN);
            sseo.SqlParameters.AddWithValue("@Abstract_IT", notizia.Abstract_IT);
            sseo.SqlParameters.AddWithValue("@Abstract_EN", notizia.Abstract_EN);
            sseo.SqlParameters.AddWithValue("@Testo_IT", notizia.Testo_IT);
            sseo.SqlParameters.AddWithValue("@Testo_EN", notizia.Testo_EN);
            sseo.SqlParameters.AddWithValue("@DataInserimento", notizia.DataInserimento);
            sseo.SqlParameters.AddWithValue("@DataUltimaModifica", notizia.DataUltimaModifica);
            sseo.SqlParameters.AddWithValue("@Pubblicata", notizia.Pubblicata);
            sseo.SqlParameters.AddWithValue("@Stato", notizia.Stato);

            SqlServerExecuteObject sseoFTL = null;
            string sSqlFTL = "";

            string testo_ITNoHtml = "";
            string testo_ENNoHtml = "";

            testo_ITNoHtml = HtmlUtility.HtmlStrip(notizia.Testo_IT).Trim();
            testo_ENNoHtml = HtmlUtility.HtmlStrip(notizia.Testo_EN).Trim();

            sSqlFTL = "INSERT INTO dbo.FTL_Notizie (NotiziaID, Titolo_IT, Titolo_EN, Abstract_IT, Abstract_EN, Testo_IT, Testo_EN) VALUES " +
                            "(@NotiziaID, @Titolo_IT, @Titolo_EN, @Abstract_IT, @Abstract_EN, @Testo_IT_NoHTML, @Testo_EN_NoHTML);";

            sseoFTL = new SqlServerExecuteObject();
            sseoFTL.CommandText = sSqlFTL;
            sseoFTL.SqlParameters.AddWithValue("@Titolo_IT", notizia.Titolo_IT);
            sseoFTL.SqlParameters.AddWithValue("@Titolo_EN", notizia.Titolo_EN);
            sseoFTL.SqlParameters.AddWithValue("@Abstract_IT", notizia.Abstract_IT);
            sseoFTL.SqlParameters.AddWithValue("@Abstract_EN", notizia.Abstract_EN);
            sseoFTL.SqlParameters.AddWithValue("@Testo_IT_NoHTML", testo_ITNoHtml);
            sseoFTL.SqlParameters.AddWithValue("@Testo_EN_NoHTML", testo_ENNoHtml);
            
            try
            {
                tran.Begin();

                result = int.Parse(tran.ExecuteScalarObject(sseo).ToString());

                sseoFTL.SqlParameters.AddWithValue("@NotiziaID", result);

                tran.ExecuteNonQueryObject(sseoFTL);

                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
            }
            finally
            {
                sseoFTL = null;
                sseo = null;

                tran.Dispose();
            }

            return result;
        }

        private Notizia RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            Notizia notizia = new Notizia();

            notizia.ID = dr.GetInt32(0);
            notizia.Categoria = CategoriaNotiziaRepository.Instance.RecuperaCategoriaNotizia(dr.GetInt32(1));
            notizia.ImmagineID = dr.GetInt32(2);
            notizia.Data = dr.GetDateTime(3);
            notizia.Titolo_IT = dr.GetString(4);
            notizia.Titolo_EN = dr.GetString(5);
            notizia.TitoloBreve_IT = dr.GetString(6);
            notizia.TitoloBreve_EN = dr.GetString(7);
            notizia.Abstract_IT = dr.GetString(8);
            notizia.Abstract_EN = dr.GetString(9);
            notizia.Testo_IT = dr.GetString(10);
            notizia.Testo_EN = dr.GetString(11);
            notizia.Pubblicata = dr.GetBoolean(12);
            notizia.DataInserimento = dr.GetDateTime(13);
            notizia.DataUltimaModifica = dr.GetDateTime(14);
            notizia.Stato = (StatoNotiziaEnum)dr.GetInt32(15);
            notizia.Immagini = ImmagineRepository.Instance.RecuperaImmaginiFiglio(notizia.ImmagineID);

            return notizia;
        }

        public void Elimina(int id)
        {
            SqlServerExecuteObject sseo = null;
            string sSql = "";

            sSql = "DELETE FROM dbo.TBL_Notizie WHERE NotiziaID = @NotiziaID; DELETE FROM dbo.FTL_Notizie WHERE NotiziaID = @NotiziaID;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@NotiziaID", id);

            SqlProvider.ExecuteNonQueryObject(sseo);
        }
    }
}

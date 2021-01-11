using System;
using System.Collections.Generic;
using VALib.Domain.Common;
using VALib.Configuration;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;
using System.Data;
using VALib.Domain.Entities.UI;
using VALib.Domain.Repositories.Contenuti;
using System.Web;
using System.Web.Caching;

namespace VALib.Domain.Repositories.UI
{
    public sealed class DatoAmbientaleHomeRepository : Repository
    {
        private static readonly DatoAmbientaleHomeRepository _instance = new DatoAmbientaleHomeRepository(Settings.VAConnectionString);
        private static readonly string _webCacheKey = "DatiAmbientaliHome";

        private DatoAmbientaleHomeRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static DatoAmbientaleHomeRepository Instance
        {
            get { return _instance; }
        }

        public List<DatoAmbientaleHome> RecuperaDatiAmbientaliHome(string testo, bool? pubblicato, int startrowNum, int endRowNum, out int rows)
        {
            List<DatoAmbientaleHome> datiAmbientaliHome = new List<DatoAmbientaleHome>();
            rows = 0;

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;
            string sSql = "";
            
            sSql = @"SELECT * FROM (
                SELECT D.DatoAmbientaleHomeID, D.ImmagineID, D.Titolo_IT, D.Titolo_EN, D.Link, D.Pubblicato, D.DataInserimento, D.DataUltimaModifica, ROW_NUMBER() 
                OVER(ORDER BY D.DataUltimaModifica DESC) 
                ROWNUM 
                FROM dbo.TBL_UI_DatiAmbientaliHome AS D WHERE (D.Pubblicato = @Pubblicato OR @Pubblicato IS NULL)
                                            AND (D.Titolo_IT LIKE @Testo)
                ) 
                R WHERE R.ROWNUM > @StartRowNum AND R.ROWNUM <= @EndRowNum;
                SELECT COUNT(*) FROM dbo.TBL_UI_DatiAmbientaliHome AS D WHERE (D.Pubblicato = @Pubblicato OR @Pubblicato IS NULL)
                                            AND (D.Titolo_IT LIKE @Testo);";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@StartRowNum", startrowNum);
            sseo.SqlParameters.AddWithValue("@EndRowNum", endRowNum);
            sseo.SqlParameters.AddWithValue("@Pubblicato", pubblicato.HasValue ? (object)pubblicato.Value : DBNull.Value);
            sseo.SqlParameters.AddWithValue("@Testo", string.Format("%{0}%", testo));

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                DatoAmbientaleHome datoAmbientaleHome = RiempiIstanza(dr);
                datiAmbientaliHome.Add(datoAmbientaleHome);
            }

            if (dr.NextResult() && dr.Read())
                rows = dr.GetInt32(0);

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return datiAmbientaliHome;
        }

        public List<DatoAmbientaleHome> RecuperaDatiAmbientaliHomeIndex(int num)
        {
            List<DatoAmbientaleHome> datiAmbientaliHome = new List<DatoAmbientaleHome>();

            datiAmbientaliHome = this.CacheGet(_webCacheKey) as List<DatoAmbientaleHome>;
            int c = 0;
            if (datiAmbientaliHome == null)
            {
                datiAmbientaliHome = RecuperaDatiAmbientaliHome("", true, 0, num, out c);
                //HttpContext.Current.Cache.Insert(_webCacheKey, datiAmbientaliHome, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 0, Settings.DurataCacheSecondi));

                this.CacheInsert(_webCacheKey, datiAmbientaliHome, TimeSpan.FromMinutes(15));
            }

            return datiAmbientaliHome;
        }

        public DatoAmbientaleHome RecuperaDatoAmbientaleHome(int id)
        {
            DatoAmbientaleHome datoAmbientaleHome = null;

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SELECT D.DatoAmbientaleHomeID, D.ImmagineID, D.Titolo_IT, D.Titolo_EN, D.Link, D.Pubblicato, D.DataInserimento, D.DataUltimaModifica FROM dbo.TBL_UI_DatiAmbientaliHome AS D WHERE DatoAmbientaleHomeID = @DatoAmbientaleHomeID;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;
            sseo.SqlParameters.AddWithValue("@DatoAmbientaleHomeID", id);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                datoAmbientaleHome = RiempiIstanza(dr);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return datoAmbientaleHome;
        }

        internal int SalvaDatoAmbientaleHome(DatoAmbientaleHome datoAmbientaleHome)
        {
            int result = 0;

            if (datoAmbientaleHome.IsNew)
                result = InserisciDatoAmbientaleHome(datoAmbientaleHome);
            else
                result = ModificaDatoAmbientaleHome(datoAmbientaleHome);

            return result;
        }
        
        private int ModificaDatoAmbientaleHome(DatoAmbientaleHome datoAmbientaleHome)
        {
            int result = 0;

            SqlServerExecuteObject sseo = null;
            string sSql = "";

            sSql = "UPDATE dbo.TBL_UI_DatiAmbientaliHome SET ImmagineID = @ImmagineID, Titolo_IT = @Titolo_IT, " +
                            "Titolo_EN = @Titolo_EN, Link = @Link, DataUltimaModifica = @DataUltimaModifica, Pubblicato = @Pubblicato " +
                            "WHERE DatoAmbientaleHomeID = @DatoAmbientaleHomeID;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@ImmagineID", datoAmbientaleHome.ImmagineID);
            sseo.SqlParameters.AddWithValue("@Titolo_IT", datoAmbientaleHome.Titolo_IT);
            sseo.SqlParameters.AddWithValue("@Titolo_EN", datoAmbientaleHome.Titolo_EN);
            sseo.SqlParameters.AddWithValue("@Link", datoAmbientaleHome.Link);
            sseo.SqlParameters.AddWithValue("@DataUltimaModifica", datoAmbientaleHome.DataUltimaModifica);
            sseo.SqlParameters.AddWithValue("@Pubblicato", datoAmbientaleHome.Pubblicato);
            sseo.SqlParameters.AddWithValue("@DatoAmbientaleHomeID", datoAmbientaleHome.ID);

            SqlProvider.ExecuteNonQueryObject(sseo);

            result = datoAmbientaleHome.ID;

            return result;
        }
        
        private int InserisciDatoAmbientaleHome(DatoAmbientaleHome datoAmbientaleHome)
        {
            int result = 0;

            SqlServerExecuteObject sseo = null;
            string sSql = "";

            sSql = "INSERT INTO dbo.TBL_UI_DatiAmbientaliHome (ImmagineID, Titolo_IT, Titolo_EN, Link, Pubblicato, DataInserimento, DataUltimaModifica) VALUES " +
                            "(@ImmagineID, @Titolo_IT, @Titolo_EN, @Link, @Pubblicato, @DataInserimento, @DataUltimaModifica);" +
                    "SELECT @@IDENTITY;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@ImmagineID", datoAmbientaleHome.ImmagineID);
            sseo.SqlParameters.AddWithValue("@Titolo_IT", datoAmbientaleHome.Titolo_IT);
            sseo.SqlParameters.AddWithValue("@Titolo_EN", datoAmbientaleHome.Titolo_EN);
            sseo.SqlParameters.AddWithValue("@Link", datoAmbientaleHome.Link);
            sseo.SqlParameters.AddWithValue("@DataInserimento", datoAmbientaleHome.DataInserimento);
            sseo.SqlParameters.AddWithValue("@DataUltimaModifica", datoAmbientaleHome.DataUltimaModifica);
            sseo.SqlParameters.AddWithValue("@Pubblicato", datoAmbientaleHome.Pubblicato);

            result = int.Parse(SqlProvider.ExecuteScalarObject(sseo).ToString());

            return result;
        }

        private DatoAmbientaleHome RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            DatoAmbientaleHome datoAmbientaleHome = new DatoAmbientaleHome();

            datoAmbientaleHome.ID = dr.GetInt32(0);
            datoAmbientaleHome.ImmagineID = dr.GetInt32(1);
            datoAmbientaleHome.Titolo_IT = dr.GetString(2);
            datoAmbientaleHome.Titolo_EN = dr.GetString(3);
            datoAmbientaleHome.Link = dr.GetString(4);
            datoAmbientaleHome.Pubblicato = dr.GetBoolean(5);
            datoAmbientaleHome.DataInserimento = dr.GetDateTime(6);
            datoAmbientaleHome.DataUltimaModifica = dr.GetDateTime(7);
            datoAmbientaleHome.Immagine = ImmagineRepository.Instance.RecuperaImmagine(datoAmbientaleHome.ImmagineID);

            return datoAmbientaleHome;
        }

        public void Elimina(int id)
        {
            SqlServerExecuteObject sseo = null;
            string sSql = "";

            sSql = "DELETE FROM TBL_UI_DatiAmbientaliHome WHERE DatoAmbientaleHomeID = @DatoAmbientaleHomeID;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@DatoAmbientaleHomeID", id);

            SqlProvider.ExecuteNonQueryObject(sseo);
        }
    }
}

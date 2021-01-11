using System;
using VALib.Domain.Common;
using VALib.Configuration;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;
using VALib.Domain.Entities.UI;
using ElogToolkit.Web;

namespace VALib.Domain.Repositories.UI
{
    public sealed class PaginaStaticaRepository : Repository
    {
        private static readonly PaginaStaticaRepository _instance = new PaginaStaticaRepository(Settings.VAConnectionString);
        //private static readonly string _webCacheKey = "PaginaStatica";

        private PaginaStaticaRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static PaginaStaticaRepository Instance
        {
            get { return _instance; }
        }

        public PaginaStatica RecuperaPaginaStatica(int voceMenuID)
        {
            PaginaStatica paginaStatica = null;

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SELECT PaginaStaticaID, VoceMenuID, Testo_IT, Testo_EN, DataInserimento, DataUltimaModifica, Visibile FROM dbo.TBL_UI_PagineStatiche WHERE VoceMenuID = @VoceMenuID;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = System.Data.CommandType.Text;
            sseo.SqlParameters.AddWithValue("@VoceMenuID", voceMenuID);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                paginaStatica = RiempiIstanza(dr);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return paginaStatica;
        }

        internal int SalvaPaginaStatica(PaginaStatica paginaStatica)
        {
            int result = 0;

            if (paginaStatica.IsNew)
                result = InserisciPaginaStatica(paginaStatica);
            else
                result = ModificaPaginaStatica(paginaStatica);

            return result;
        }

        private int ModificaPaginaStatica(PaginaStatica paginaStatica)
        {
            int result = 0;

            SqlServerExecuteObject sseo = null;
            string sSql = "";
            string testo_ITNoHtml = "";
            string testo_ENNoHtml = "";

            testo_ITNoHtml = HtmlUtility.HtmlStrip(paginaStatica.Testo_IT).Trim();
            testo_ENNoHtml = HtmlUtility.HtmlStrip(paginaStatica.Testo_EN).Trim();

            if (testo_ITNoHtml.Length > 4000)
                testo_ITNoHtml = testo_ITNoHtml.Substring(0, 4000);

            if (testo_ENNoHtml.Length > 4000)
                testo_ENNoHtml = testo_ENNoHtml.Substring(0, 4000);

            sSql = @"UPDATE dbo.TBL_UI_PagineStatiche SET 
                      Testo_IT = @Testo_IT, Testo_EN = @Testo_EN, DataUltimaModifica = @DataUltimaModifica
                      WHERE VoceMenuID = @VoceMenu;
                    UPDATE dbo.FTL_PagineStatiche SET 
                      Nome_IT = @Nome_IT, Nome_EN = @Nome_EN, Descrizione_IT = @Descrizione_IT, Descrizione_EN = @Descrizione_EN, Testo_IT = @Testo_IT_NoHTML, Testo_EN = @Testo_EN_NoHTML 
                      WHERE VoceMenuID = @VoceMenu;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@Testo_IT", paginaStatica.Testo_IT);
            sseo.SqlParameters.AddWithValue("@Testo_EN", paginaStatica.Testo_EN);
            sseo.SqlParameters.AddWithValue("@Nome_IT", paginaStatica.VoceMenu._nome_IT);
            sseo.SqlParameters.AddWithValue("@Nome_EN", paginaStatica.VoceMenu._nome_EN);
            sseo.SqlParameters.AddWithValue("@Descrizione_IT", paginaStatica.VoceMenu._descrizione_IT);
            sseo.SqlParameters.AddWithValue("@Descrizione_EN", paginaStatica.VoceMenu._descrizione_EN);
            sseo.SqlParameters.AddWithValue("@Testo_IT_NoHTML", testo_ITNoHtml);
            sseo.SqlParameters.AddWithValue("@Testo_EN_NoHTML", testo_ENNoHtml);
            sseo.SqlParameters.AddWithValue("@DataUltimaModifica", paginaStatica.DataUltimaModifica);
            sseo.SqlParameters.AddWithValue("@VoceMenu", paginaStatica.VoceMenu.ID);

            SqlProvider.ExecuteNonQueryObject(sseo);

            result = paginaStatica.ID;

            return result;
        }

        private int InserisciPaginaStatica(PaginaStatica paginaStatica)
        {
            int result = 0;

            SqlServerExecuteObject sseo = null;
            string sSql = "";

            sSql = "INSERT INTO dbo.TBL_UI_PagineStatiche (VoceMenuID, Testo_IT, Testo_EN, DataInserimento, DataUltimaModifica) VALUES " +
                            "(@VoceMenuID, @Testo_IT, @Testo_EN, @DataInserimento, @DataUltimaModifica);" +
                    "SELECT @@IDENTITY;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@VoceMenuID", paginaStatica.VoceMenu.ID);
            sseo.SqlParameters.AddWithValue("@Testo_IT", paginaStatica.Testo_IT);
            sseo.SqlParameters.AddWithValue("@Testo_EN", paginaStatica.Testo_EN);
            sseo.SqlParameters.AddWithValue("@DataInserimento", paginaStatica.DataInserimento);
            sseo.SqlParameters.AddWithValue("@DataUltimaModifica", paginaStatica.DataUltimaModifica);

            result = int.Parse(SqlProvider.ExecuteScalarObject(sseo).ToString());

            return result;
        }

        private PaginaStatica RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            PaginaStatica paginaStatica = new PaginaStatica();

            paginaStatica.ID = dr.GetInt32(0);
            paginaStatica.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu(dr.GetInt32(1));
            paginaStatica.Testo_IT = dr.IsDBNull(2) ? "" : dr.GetString(2);
            paginaStatica.Testo_EN = dr.IsDBNull(2) ? "" : dr.GetString(3);
            paginaStatica.DataInserimento = dr.GetDateTime(4);
            paginaStatica.DataUltimaModifica = dr.GetDateTime(5);
            //paginaStatica.Visibile = dr.GetBoolean(8);

            return paginaStatica;
        }

    }
}

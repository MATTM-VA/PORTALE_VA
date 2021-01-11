using System.Collections.Generic;
using VALib.Domain.Common;
using VALib.Configuration;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;
using System.Data;
using VALib.Domain.Entities.UI;

namespace VALib.Domain.Repositories.UI
{
    public sealed class PaginaStaticaElencoRepository : Repository
    {
        private static readonly PaginaStaticaElencoRepository _instance = new PaginaStaticaElencoRepository(Settings.VAConnectionString);
        //private static readonly string _webCacheKey = typeof(PaginaStaticaElencoRepository).FullName;

        private PaginaStaticaElencoRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static PaginaStaticaElencoRepository Instance
        {
            get { return _instance; }
        }


        public List<int> RecuperaPagineStaticheVoceMenuIdElenco(bool visibile = true)
        {
            List<int> VociMenuID = new List<int>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = " SELECT VoceMenuID FROM TBL_UI_PagineStatiche WHERE Visibile = @Visibile ";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.SqlParameters.AddWithValue("@Visibile", visibile);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                VociMenuID.Add(dr.GetInt32(0));
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return VociMenuID;
        }

        public List<PaginaStaticaElenco> RecuperaPagineStaticheElenco(string lingua, string testoRicerca, string orderBy, string orderDirection, int startrowNum, int endRowNum, out int rows)
        {
            List<PaginaStaticaElenco> pagineStaticheElenco = new List<PaginaStaticaElenco>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;
            rows = 0;

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = "dbo.SP_RecuperaPagineStatiche";
            sseo.CommandType = CommandType.StoredProcedure;
            sseo.SqlParameters.AddWithValue("@Lingua", lingua);
            sseo.SqlParameters.AddWithValue("@TestoRicerca", testoRicerca);
            sseo.SqlParameters.AddWithValue("@OrderBy", orderBy);
            sseo.SqlParameters.AddWithValue("@OrderDirection", orderDirection);
            sseo.SqlParameters.AddWithValue("@StartRowNum", startrowNum);
            sseo.SqlParameters.AddWithValue("@EndRowNum", endRowNum);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                PaginaStaticaElenco pagina = new PaginaStaticaElenco();

                pagina.ID = 0;
                pagina.VoceMenu = VoceMenuRepository.Instance.RecuperaVoceMenu(dr.GetInt32(1));
                pagina._nome_IT = dr.GetString(2);
                pagina._nome_EN = dr.GetString(3);

                pagineStaticheElenco.Add(pagina);
            }

            if (dr.NextResult() && dr.Read())
                rows = dr.GetInt32(0);

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return pagineStaticheElenco;
        }

    }
}

using System.Collections.Generic;
using VALib.Domain.Common;
using VALib.Configuration;
using VALib.Domain.Entities.Contenuti;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;
using System.Data;

namespace VALib.Domain.Repositories.Contenuti
{
    public sealed class ConteggioProceduraRepository : Repository
    {
        private static readonly ConteggioProceduraRepository _instance = new ConteggioProceduraRepository(Settings.VAConnectionString);
        //private static readonly string _webCacheKey = "CategorieNotizie";

        private ConteggioProceduraRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static ConteggioProceduraRepository Instance
        {
            get { return _instance; }
        }

        public IEnumerable<ConteggioProcedura> RecuperaConteggiProcedure(MacroTipoOggettoEnum macroTipoOggetto, bool concluse)
        {
            List<ConteggioProcedura> conteggiProcedure = new List<ConteggioProcedura>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SP_RecuperaStatoProcedure";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.StoredProcedure;
            sseo.SqlParameters.AddWithValue("@Concluse", concluse);
            sseo.SqlParameters.AddWithValue("@MacroTipoOggettoID", macroTipoOggetto);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                ConteggioProcedura conteggioProcedura = new ConteggioProcedura();

                conteggioProcedura.Conteggio = dr.GetInt32(0);
                conteggioProcedura.Procedura = ProceduraRepository.Instance.RecuperaProcedura(dr.GetInt32(1));
                conteggioProcedura.Parametro = dr.GetInt32(2);

                conteggiProcedure.Add(conteggioProcedura);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return conteggiProcedure;
        }

    }
}

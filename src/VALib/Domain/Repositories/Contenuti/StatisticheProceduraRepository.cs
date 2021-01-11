using System.Collections.Generic;
using VALib.Domain.Common;
using VALib.Configuration;
using VALib.Domain.Entities.Contenuti;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;
using System.Data;

namespace VALib.Domain.Repositories.Contenuti
{
    public sealed class StatisticheProceduraRepository : Repository
    {
        private static readonly StatisticheProceduraRepository _instance = new StatisticheProceduraRepository(Settings.VAConnectionString);
        //private static readonly string _webCacheKey = "CategorieNotizie";

        private StatisticheProceduraRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static StatisticheProceduraRepository Instance
        {
            get { return _instance; }
        }

        public IEnumerable<StatisticheProcedura> RecuperaStatisticheProcedure(int anno)
        {
            List<StatisticheProcedura> conteggiProcedure = new List<StatisticheProcedura>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SP_RecuperaStatisticheProcedure";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.StoredProcedure;
            sseo.SqlParameters.AddWithValue("@Anno", anno);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                StatisticheProcedura statisticheProcedura = new StatisticheProcedura();

                statisticheProcedura.Procedura = ProceduraRepository.Instance.RecuperaProcedura(dr.GetInt32(0));
                statisticheProcedura.InCorso = dr.GetInt32(1);
                statisticheProcedura.Avviate = dr.GetInt32(2);
                statisticheProcedura.Concluse = dr.GetInt32(3);

                conteggiProcedure.Add(statisticheProcedura);
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

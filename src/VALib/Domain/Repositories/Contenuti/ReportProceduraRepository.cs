using System.Collections.Generic;
using VALib.Domain.Common;
using VALib.Configuration;
using VALib.Domain.Entities.Contenuti;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;
using System.Data;

namespace VALib.Domain.Repositories.Contenuti
{
    public sealed class ReportProceduraRepository : Repository
    {
        private static readonly ReportProceduraRepository _instance = new ReportProceduraRepository(Settings.VAConnectionString);
        //private static readonly string _webCacheKey = "CategorieNotizie";

        private ReportProceduraRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static ReportProceduraRepository Instance
        {
            get { return _instance; }
        }

        public IEnumerable<ReportProcedura> RecuperaReportProcedure(int anno, int proceduraID, bool mostraInCorso, bool mostraAvviate, bool mostraConcluse)
        {
            List<ReportProcedura> reportProcedure = new List<ReportProcedura>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "dbo.SP_RecuperaReportProcedure";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.StoredProcedure;
            sseo.SqlParameters.AddWithValue("@Anno", anno);
            sseo.SqlParameters.AddWithValue("@proceduraID", proceduraID);
            sseo.SqlParameters.AddWithValue("@mostraInCorso", mostraInCorso);
            sseo.SqlParameters.AddWithValue("@mostraAvviate", mostraAvviate);
            sseo.SqlParameters.AddWithValue("@mostraConcluse", mostraConcluse);

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                ReportProcedura reportProcedura = new ReportProcedura();

                // TODO: riempire la classe
                reportProcedura.MacroTipoOggetto = (MacroTipoOggettoEnum)dr.GetInt32(0);
                reportProcedura.OggettoProceduraID = dr.GetInt32(1);
                if (!dr.IsDBNull(2))
                    reportProcedura.IDVIP = dr.GetString(2);
                reportProcedura._nome_IT = dr.GetString(3);
                reportProcedura._nome_EN = dr.GetString(4);
                reportProcedura.Proponente = dr.GetString(5);
                reportProcedura.DataAvvio = dr.GetDateTime(6);

                if ((int)reportProcedura.MacroTipoOggetto==(int)TipoOggettoEnum.Impianto)
                {
                    reportProcedura.StatoProcedura = StatoProceduraAIARepository.Instance.RecuperaStatoProceduraAIA(dr.GetInt32(7));
                }
                else
                {
                    reportProcedura.StatoProcedura = StatoProceduraVIPERARepository.Instance.RecuperaStatoProceduraVIPERA(dr.GetInt32(7));
                }
                
                if (!dr.IsDBNull(8))
                    reportProcedura.DataProvvedimento = dr.GetDateTime(8);
                if (!dr.IsDBNull(9))
                    reportProcedura.NumeroProvvedimento = dr.GetString(9);
                else
                    reportProcedura.NumeroProvvedimento = "-";
                if (!dr.IsDBNull(10))
                    reportProcedura.Esito = dr.GetString(10);
                else
                    reportProcedura.Esito = "-";
                if (!dr.IsDBNull(11))
                    reportProcedura.Settore = SettoreRepository.Instance.RecuperaSettore(dr.GetInt32(11));
                if (!dr.IsDBNull(12))
                    reportProcedura.Tipologia = TipologiaRepository.Instance.RecuperaTipologia(dr.GetInt32(12));
                if (!dr.IsDBNull(13))
                    reportProcedura.CategoriaImpianto = CategoriaImpiantoRepository.Instance.RecuperaCategoria(dr.GetInt32(13));
                reportProcedure.Add(reportProcedura);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return reportProcedure;
        }

    }
}

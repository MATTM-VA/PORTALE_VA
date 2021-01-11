using System.Collections.Generic;
using VALib.Domain.Common;
using VALib.Configuration;
using VALib.Domain.Entities.Contenuti;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;
using System.Data;

namespace VALib.Domain.Repositories.Contenuti
{
    public sealed class ConteggioTipoProvvedimentoRepository : Repository
    {
        private static readonly ConteggioTipoProvvedimentoRepository _instance = new ConteggioTipoProvvedimentoRepository(Settings.VAConnectionString);
        //private static readonly string _webCacheKey = "CategorieNotizie";

        private ConteggioTipoProvvedimentoRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static ConteggioTipoProvvedimentoRepository Instance
        {
            get { return _instance; }
        }

        public IEnumerable<ConteggioTipoProvvedimento> RecuperaConteggiTipiProvvedimenti()
        {
            List<ConteggioTipoProvvedimento> conteggiTipiProvvedimenti = new List<ConteggioTipoProvvedimento>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = @"SELECT COUNT(*) AS Conteggio, P.TipoProvvedimentoID FROM dbo.TBL_Provvedimenti AS P
                INNER JOIN dbo.TBL_OggettiProcedure OP ON OP.OggettoProceduraID = P.OggettoProceduraID
                INNER JOIN dbo.TBL_Oggetti O ON O.OggettoID = OP.OggettoID
                INNER JOIN dbo.TBL_TipiOggetto T ON T.TipoOggettoID = O.TipoOggettoID
                WHERE T.MacroTipoOggettoID <> {0} OR OP.AIAID IS NOT NULL
                GROUP BY P.TipoProvvedimentoID";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = string.Format(sSql, (int)MacroTipoOggettoEnum.Aia);
            sseo.CommandType = CommandType.Text;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                ConteggioTipoProvvedimento conteggioTipoProvvedimento = new ConteggioTipoProvvedimento();

                conteggioTipoProvvedimento.Conteggio = dr.GetInt32(0);
                conteggioTipoProvvedimento.TipoProvvedimento = TipoProvvedimentoRepository.Instance.RecuperaTipoProvvedimento(dr.GetInt32(1));

                conteggiTipiProvvedimenti.Add(conteggioTipoProvvedimento);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return conteggiTipiProvvedimenti;
        }

    }
}

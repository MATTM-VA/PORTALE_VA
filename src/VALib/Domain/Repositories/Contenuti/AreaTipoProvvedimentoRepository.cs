using System;
using System.Collections.Generic;
using System.Linq;
using VALib.Domain.Common;
using VALib.Configuration;
using VALib.Domain.Entities.Contenuti;
using ElogToolkit.Data.SqlServer;
using System.Data.SqlClient;
using System.Data;
using System.Web;

namespace VALib.Domain.Repositories.Contenuti
{
    public sealed class AreaTipoProvvedimentoRepository : Repository
    {
        private static readonly AreaTipoProvvedimentoRepository _instance = new AreaTipoProvvedimentoRepository(Settings.VAConnectionString);
        private static readonly string _webCacheKey = "AreeTipiProvvedimenti";

        private AreaTipoProvvedimentoRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static AreaTipoProvvedimentoRepository Instance
        {
            get { return _instance; }
        }

        public IEnumerable<AreaTipoProvvedimento> RecuperaAreeTipiProvvedimenti()
        {
            IEnumerable<AreaTipoProvvedimento> areaTipoProvvedimento = new List<AreaTipoProvvedimento>();

            areaTipoProvvedimento = this.CacheGet(_webCacheKey) as List<AreaTipoProvvedimento>;

            if (areaTipoProvvedimento == null)
            {
                areaTipoProvvedimento = RecuperaAreeTipiProvvedimentiPrivate();
                //HttpContext.Current.Cache.Insert(_webCacheKey, areaTipoProvvedimento, CreateCacheDependency(_webCacheKey));
                this.CacheInsert(_webCacheKey, areaTipoProvvedimento, TimeSpan.FromMinutes(15));
            }

            return areaTipoProvvedimento;
        }

        public AreaTipoProvvedimento RecuperaAreaTipoProvvedimento(int id)
        {
            return RecuperaAreeTipiProvvedimenti().FirstOrDefault(x => (int)x.ID == id);
        }

        private IEnumerable<AreaTipoProvvedimento> RecuperaAreeTipiProvvedimentiPrivate()
        {
            List<AreaTipoProvvedimento> areeTipiProvvedimenti = new List<AreaTipoProvvedimento>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SELECT AreaTipoProvvedimentoID, Nome_IT, Nome_EN, Ordine FROM dbo.TBL_AreeTipiProvvedimenti;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                AreaTipoProvvedimento tipoProvvedimento = RiempiIstanza(dr);
                areeTipiProvvedimenti.Add(tipoProvvedimento);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return areeTipiProvvedimenti;
        }

        private AreaTipoProvvedimento RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            AreaTipoProvvedimento areaTipoProvvedimento = new AreaTipoProvvedimento();

            areaTipoProvvedimento.ID = dr.GetInt32(0);
            areaTipoProvvedimento._nome_IT = dr.GetString(1);
            areaTipoProvvedimento._nome_EN = dr.GetString(2);
            areaTipoProvvedimento.Ordine = dr.GetInt32(3);

            return areaTipoProvvedimento;
        }
    }
}

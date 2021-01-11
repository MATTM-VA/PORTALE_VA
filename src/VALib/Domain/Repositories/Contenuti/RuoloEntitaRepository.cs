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
    public sealed class RuoloEntitaRepository : Repository
    {
        private static readonly RuoloEntitaRepository _instance = new RuoloEntitaRepository(Settings.VAConnectionString);
        private static readonly string _webCacheKey = "RuoliEntita";

        private RuoloEntitaRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static RuoloEntitaRepository Instance
        {
            get { return _instance; }
        }

        public List<RuoloEntita> RecuperaRuoliEntita()
        {
            List<RuoloEntita> ruoliEntita = new List<RuoloEntita>();

            ruoliEntita = this.CacheGet(_webCacheKey) as List<RuoloEntita>;

            if (ruoliEntita == null)
            {
                ruoliEntita = RecuperaRuoliEntitaPrivate();

                //HttpContext.Current.Cache.Insert(_webCacheKey, ruoliEntita, CreateCacheDependency(_webCacheKey));
                this.CacheInsert(_webCacheKey, ruoliEntita, TimeSpan.FromMinutes(15));
            }

            return ruoliEntita;
        }

        private List<RuoloEntita> RecuperaRuoliEntitaPrivate()
        {
            List<RuoloEntita> ruoliEntita = new List<RuoloEntita>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SELECT RuoloEntitaID, Nome_IT, Nome_EN  FROM dbo.TBL_RuoliEntita;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                RuoloEntita ruoloEntita = RiempiIstanza(dr);
                ruoliEntita.Add(ruoloEntita);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return ruoliEntita;
        }

        public RuoloEntita RecuperaRuoloEntita(int id)
        {
            return RecuperaRuoliEntita().FirstOrDefault(x => (int)x.ID == id);
        }

        private RuoloEntita RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            RuoloEntita ruoloEntita = new RuoloEntita();

            ruoloEntita.ID = dr.GetInt32(0);
            ruoloEntita._nome_IT = dr.GetString(1);
            ruoloEntita._nome_EN = dr.GetString(2);
            ruoloEntita.Enum = (RuoloEntitaEnum)dr.GetInt32(0);

            return ruoloEntita;
        }
    }
}

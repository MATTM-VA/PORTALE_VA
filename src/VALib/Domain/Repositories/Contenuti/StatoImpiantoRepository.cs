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
    public sealed class StatoImpiantoRepository : Repository
    {
        private static readonly StatoImpiantoRepository _instance = new StatoImpiantoRepository(Settings.VAConnectionString);
        private static readonly string _webCacheKey = "StatoImpianti";

        private StatoImpiantoRepository(string connectionString)
            : base(connectionString)
        {

        }

        public static StatoImpiantoRepository Instance
        {
            get { return _instance; }
        }

        public List<StatoImpianto> RecuperaStati()
        {
            List<StatoImpianto> stati = new List<StatoImpianto>();

            stati = this.CacheGet(_webCacheKey) as List<StatoImpianto>;

            if (stati == null)
            {
                stati = RecuperaStatiPrivate();

                //HttpContext.Current.Cache.Insert(_webCacheKey, settori, CreateCacheDependency(_webCacheKey));
                this.CacheInsert(_webCacheKey, stati, TimeSpan.FromMinutes(15));
            }

            return stati;
        }

        public StatoImpianto RecuperaStato(int id)
        {
            return RecuperaStati().FirstOrDefault(x => (int)x.ID == id);
        }

        private List<StatoImpianto> RecuperaStatiPrivate()
        {
            List<StatoImpianto> stati = new List<StatoImpianto>();

            SqlServerExecuteObject sseo = null;
            SqlDataReader dr = null;

            string sSql = "SELECT StatoImpiantiID, Nome_IT, Nome_EN FROM dbo.TBL_StatoImpianti;";

            sseo = new SqlServerExecuteObject();
            sseo.CommandText = sSql;
            sseo.CommandType = CommandType.Text;

            dr = SqlProvider.ExecuteReaderObject(sseo);

            while (dr.Read())
            {
                StatoImpianto stato = RiempiIstanza(dr);
                stati.Add(stato);
            }

            if (dr != null)
            {
                dr.Close();
                dr.Dispose();
            }

            return stati;
        }

        private StatoImpianto RiempiIstanza(SqlDataReader dr)
        {
            if (dr == null)
                throw new ArgumentNullException("dr");

            StatoImpianto stato = new StatoImpianto();

            stato.ID = dr.GetInt32(0);
            stato._nome_IT = dr.GetString(1);
            stato._nome_EN = dr.GetString(2);

            return stato;
        }
    }
}
